using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mono.CSharp;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositorySql : IProteinRepository
    {
        public void Dummy()
        {

        }

        public List<ProteinDto> ExtractProteins(double IntactMass, SearchParametersDto parameters, List<PstTagList> PstTags, int CandidateList) // Added "int CandidateList". 20200112
        {
            var query = GetQuery(IntactMass, parameters, CandidateList);

            var connectionString = GetConnectionString(parameters.ProtDb);
            List<SerializedProteinDataDto> prot;
            using (var connection = new SqlConnection(connectionString))
            {
                prot = connection.Query<SerializedProteinDataDto>(query).ToList();

            }

            var proteins = new List<ProteinDto>();

            foreach (var proteinInfo in prot)
            {
                //if (proteinInfo.ID == "P04439")  // Was for Testing
                //{
                //    int wait;
                //}
                var insilico = new InsilicoObjectDto()
                {
                    InsilicoMassLeft = proteinInfo.Insilico.Split(',').Select(double.Parse).ToList(),
                    InsilicoMassRight = proteinInfo.InsilicoR.Split(',').Select(double.Parse).ToList() // InsilicoR
                };

                //#FORTHETIMEBEING: Updated 20200115 COMMENTED: PREVIOUSLY Removing Last Entry(MW of Protein - Water)
                //insilico.InsilicoMassLeft.RemoveAt(insilico.InsilicoMassLeft.Count - 1); // JUST IN CASE::: as this will be the MW of protein - water
                //insilico.InsilicoMassRight.RemoveAt(insilico.InsilicoMassRight.Count - 1);

                var protein = new ProteinDto()
                {
                    Header = proteinInfo.ID,
                    InsilicoDetails = insilico,
                    Mw = proteinInfo.MW,
                    Sequence = proteinInfo.Seq,
                    OriginalSequence = proteinInfo.Seq

                };

                if (parameters.FilterDb == 1)
                {
                    if (CandidateList == 0)  // Simple Candidate Protein List According to given tolerance with no Truncation
                    {
                        proteins.Add(protein);
                    }
                    else if (CandidateList == 1)  // Candidate Protein List with Truncation     // parameters.Truncation == 1 && 
                    {
                        if (parameters.DenovoAllow == 1)
                        {
                            for (int indexPstTags = 0; indexPstTags < PstTags.Count; indexPstTags++)
                            {
                                if (proteinInfo.Seq.Contains(PstTags[indexPstTags].PstTags))
                                {
                                    proteins.Add(protein);
                                    break;  //Any Tag is Reported....??? Good!  BREAK THE LOOP AND KEEP THE PROTEIN
                                }
                            }
                        }
                        else
                        {
                            proteins.Add(protein);
                        }
                        
                    }
                }
                else if (parameters.FilterDb == 0 && CandidateList == 0)
                {
                    proteins.Add(protein);
                }
                else if (parameters.FilterDb == 0 && CandidateList == 1)
                {
                    proteins.Add(protein);
                }

            }
            return proteins;
        }

        //private FilteringDatabase()

        //private List<ProteinDto


        private string GetQuery(double IntactMass, SearchParametersDto parameters, int CandidateList) // Added "int CandidateList". 20200112
        {
            string DatabaseName = parameters.ProtDb;

            if (parameters.ProtDb == "Bacteria") // Was Needed: When FE also showing Bacteria but same ConnectionStrings {So, Doesn't Matter}
            {
                parameters.ProtDb = "Ecoli";
                DatabaseName = parameters.ProtDb;
            }

            string subquery1 = "Select * From  [";
            string subquery2 = "].[dbo].[ProteinInfoes]";


            string query = subquery1 + DatabaseName + subquery2;   //"Select * From  [ProteinDB].[dbo].[ProteinInfoes]";  // If FilterDb == 0 then, we will take Whole Protein Database. So, query will be enough for this purpose
            
            
            if (parameters.FilterDb == 1)
            {
                if (CandidateList == 0) //For Simple Candidate Protein List >>>> CandidateList == 0
                {
                    query += " Where MW >= " + (IntactMass - parameters.MwTolerance) + " AND MW <= " + (IntactMass + parameters.MwTolerance);    // SAME AS abs(DBProtein_MW + MWeight - Protein_ExperimentalMW)<= MWTolerance + TolExt
                }
                else if (CandidateList == 1)  //For Truncated Candidate Protein List >>>> CandidateList == 1      // parameters.Truncation == 1 && 
                {
                    query += " Where MW -" + IntactMass + ">" + parameters.MwTolerance;  //MW is Protein Mass  // (DBProtein_MW - Protein_ExperimentalMW) > MWTolerance
                }
            }
            else
            {
                return query;
            }
            return query;
        }



        private string GetConnectionString(string db)
        {
            switch (db)
            {
                case "Ecoli":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                //break;
                case "Bacteria":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                //break;
                case "TrEMBL":// FARHAN's TESTING
                    return ConfigurationManager.ConnectionStrings["TrEMBLConnectionStringName"].ConnectionString;
                //break;
                case "Human":
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;
                //break;
                default:  // Default is also Human
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;
            }
        }
    }
}
