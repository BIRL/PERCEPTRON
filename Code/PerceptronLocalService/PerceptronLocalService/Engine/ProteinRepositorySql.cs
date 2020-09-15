using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Mono.CSharp;
using System.Text.RegularExpressions;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositorySql : IProteinRepository
    {

        public List<ProteinDto> FetchingSqlDatabaseProteins(SearchParametersDto parameters)
        {
            var query = GetQuery(parameters.ProtDb);

            var connectionString = GetConnectionString(parameters.ProtDb);
            List<SerializedProteinDataDto> FetchedSqlProteins;
            using (var connection = new SqlConnection(connectionString))
            {
                FetchedSqlProteins = connection.Query<SerializedProteinDataDto>(query).ToList();
            }

            var SqlDatabaseProteins = new List<ProteinDto>();

            foreach (var proteinInfo in FetchedSqlProteins)
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //if ((proteinInfo.ID == "P60624"))///***      P62805      // || (proteinInfo.ID == "A0A0B4J280"))// || (proteinInfo.ID != "P04439"))
                ////(proteinInfo.ID != "A0A087WTH1")// || proteinInfo.ID != "A0A0B4J280" || proteinInfo.ID != "P57738")   //   A6NHS1    for ptm insilico generator
                //{
                //    int wait = 1;
                //}
                //else
                //{
                //    continue;
                //}

                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

                SqlDatabaseProteins.Add(protein);

            }
            return SqlDatabaseProteins;
        }


        public CandidateProteinListsDto ExtractProteins(double IntactMass, SearchParametersDto parameters, List<PstTagList> PstTags, List<ProteinDto> SqlDatabaseProteins)
        {
            CandidateProteinListsDto CandidateProteinListsInfo = new CandidateProteinListsDto();
            var CandidateList = CandidateProteinListsInfo.CandidateProteinList;
            var CandidateListTruncated = CandidateProteinListsInfo.CandidateProteinListTruncated;

            for (int index = 0; index < SqlDatabaseProteins.Count; index++)
            {
                var protein = SqlDatabaseProteins[index];


                if (parameters.FilterDb == "True")
                {
                    double TotalFixedWeight = 0.0;
                    double VariableWeight = 0.0;

                    if (parameters.FixedModifications.Count != 0 && parameters.VariableModifications.Count != 0)
                    {
                        if (parameters.FixedModifications.Count != 0)
                        {
                            double ChemicalModMass = GetChemicalModMass(parameters, protein.Sequence);
                            double FixedWeight = GetPTMModMassShift(parameters.FixedModifications, protein.Sequence);
                            TotalFixedWeight = ChemicalModMass + FixedWeight;
                        }
                        if (parameters.VariableModifications.Count != 0)
                        {
                            VariableWeight = GetPTMModMassShift(parameters.VariableModifications, protein.Sequence);
                        }
                    }
                    if (parameters.Truncation == "False")//#Placeholder  // #POTENTIALBUG  IN SPECTRUM {isempty(truncation)} ///   
                    {
                        if (Math.Abs(protein.Mw + TotalFixedWeight - IntactMass) <= parameters.MwTolerance + VariableWeight)
                        {
                            CandidateList.Add(protein);
                        }
                    }

                    else
                    {
                        if (Math.Abs(protein.Mw + TotalFixedWeight - IntactMass) <= parameters.MwTolerance + VariableWeight)
                        {
                            CandidateList.Add(protein);
                        }
                        else if (protein.Mw - IntactMass > parameters.MwTolerance)
                        {
                            if (parameters.DenovoAllow == "True")
                            {
                                for (int i = 0; i < PstTags.Count; i++)
                                {
                                    string Tag = PstTags[i].PstTags;
                                    if (protein.Sequence.Contains(Tag))
                                    {
                                        CandidateListTruncated.Add(protein);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                CandidateListTruncated.Add(protein);
                            }
                        }
                    }
                }
                else
                {
                    CandidateList.Add(protein);
                    CandidateListTruncated.Add(protein);
                }
            }

            return CandidateProteinListsInfo;
        }


        private string GetQuery(string DatabaseName)
        {

            string subquery1 = "Select * From  [";
            string subquery2 = "].[dbo].[ProteinInfoes]";
            string query = subquery1 + DatabaseName + subquery2;  //Query to select all proteins in specified Database

            return query;
        }

        private string GetConnectionString(string db)
        {
            switch (db)
            {
                case "Ecoli":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                case "Bacteria":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                case "TrEMBL":
                    return ConfigurationManager.ConnectionStrings["TrEMBLConnectionStringName"].ConnectionString;
                case "Human":
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;
                default:  // Default is also Human
                    return ConfigurationManager.ConnectionStrings["ProteinConnectionStringName"].ConnectionString;
            }
        }


        public double GetChemicalModMass(SearchParametersDto parameters, string Sequence)
        {
            double ChemicalModMass = 0.0;
            PTMModificationsInfoDto PTMModificationsInfo = new PTMModificationsInfoDto();
            var ListOfModInfo = PTMModificationsInfo.ListPTMModificationsInfo;

            if (parameters.CysteineChemicalModification != "None")
            {
                int NoofOccurrence = Regex.Matches(Sequence, "C").Count;
                if (NoofOccurrence > 0)
                {
                    var ModificationTableClass = new ModificationMWShift();
                    double MolecularWeight = ModificationTableClass.ModificationMWShiftTable(parameters.CysteineChemicalModification);

                    ListOfModInfo.Add(new PTMModificationsInfoDto(NoofOccurrence, "C", MolecularWeight));

                }
            }
            if (parameters.MethionineChemicalModification != "None")
            {
                int NoofOccurrence = Regex.Matches(Sequence, "M").Count;
                if (NoofOccurrence > 0)
                {
                    var ModificationTableClass = new ModificationMWShift();
                    double MolecularWeight = ModificationTableClass.ModificationMWShiftTable(parameters.MethionineChemicalModification);
                    ListOfModInfo.Add(new PTMModificationsInfoDto(NoofOccurrence, "M", MolecularWeight));

                }
            }

            for (int i = 0; i < ListOfModInfo.Count; i++)
            {
                ChemicalModMass = ChemicalModMass + (ListOfModInfo[i].Occurrences * ListOfModInfo[i].MolecularWeight);

            }
            return Math.Round(ChemicalModMass, 4);

        }

        public double GetPTMModMassShift(List<string> TypeOfModication, string Sequence)
        {
            double MolecularWeightShift = 0.0;
            PTMModificationsInfoDto PTMModificationsInfo = new PTMModificationsInfoDto();
            var ListOfModInfo = PTMModificationsInfo.ListPTMModificationsInfo;


            for (int i = 0; i < TypeOfModication.Count; i++)
            {
                var ModSite = TypeOfModication[i];
                var SiteIndex = ModSite.IndexOf("_");
                string Site = ModSite.Substring(SiteIndex + 1); //Extracting Site from FixedModifications
                int NoofOccurrence = Regex.Matches(Sequence, Site).Count;

                if (NoofOccurrence > 0)
                {
                    var ModificationTableClass = new ModificationMWShift();
                    string ModificationName = ModSite.Substring(0, ModSite.Length - 2);
                    double MolecularWeight = ModificationTableClass.ModificationMWShiftTable(ModificationName);

                    ListOfModInfo.Add(new PTMModificationsInfoDto(NoofOccurrence, Site, MolecularWeight));

                }
            }

            for (int i = 0; i < ListOfModInfo.Count; i++)
            {
                MolecularWeightShift = MolecularWeightShift + (ListOfModInfo[i].Occurrences * ListOfModInfo[i].MolecularWeight);

            }

            return Math.Round(MolecularWeightShift, 4);
        }

    }
}