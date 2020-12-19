using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Dapper;
//using Mono.CSharp;
using System.Text.RegularExpressions;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

using System.Diagnostics;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositorySql : IProteinRepository
    {

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        /* BELOW IS A WORKING FOR COMMON TRUNCATED CANDIDATE PROTEIN LIST */
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        ////public List<ProteinDto> FetchingSqlDatabaseProteins(string DatabaseName, double IntactMass, double UserDefinedTolerance, string CandidateList)
        ////{
        ////    Stopwatch ProteinFetchingTime = new Stopwatch();         // DELME Execution Time Working
        ////    Stopwatch OneCallTime = new Stopwatch();         // DELME Execution Time Working
        ////    ProteinFetchingTime.Start();              // DELME Execution Time Working
        ////    var query = GetQuery(DatabaseName, IntactMass, UserDefinedTolerance, CandidateList);

        ////    var connectionString = GetConnectionString(DatabaseName);
        ////    List<SerializedProteinDataDto> FetchedSqlProteins = new List<SerializedProteinDataDto>();  //Updated 20201208 - Initialized
        ////    OneCallTime.Start();              // DELME Execution Time Working

        ////    //////for (int i = 0; i < somevaluehere; i++)               //MyNotes - PERCEPTRON NOTES
        ////    //////{
        ////    //////    using (var connection = new SqlConnection(connectionString))
        ////    //////    {
        ////    //////        FetchedSqlProteins.AddRange(connection.Query<SerializedProteinDataDto>(query).ToList());
        ////    //////    }
        ////    //////}
        ////    using (var connection = new SqlConnection(connectionString))
        ////    {
        ////        FetchedSqlProteins = connection.Query<SerializedProteinDataDto>(query).ToList();
        ////    }
        ////    OneCallTime.Stop();              // DELME Execution Time Working
        ////    var SqlDatabaseProteins = new List<ProteinDto>();

        ////    foreach (var proteinInfo in FetchedSqlProteins)
        ////    {
        ////        var insilico = new InsilicoObjectDto()
        ////        {
        ////            InsilicoMassLeft = proteinInfo.Insilico.Split(',').Select(double.Parse).ToList(),
        ////            InsilicoMassRight = proteinInfo.InsilicoR.Split(',').Select(double.Parse).ToList() // InsilicoR
        ////        };
        ////        // Description Updated 20200917 --- Last Elements of InsilicoMassLeft & InsilicoMassRight are the "MW of Protein - Water" so, it will be removed in TerminalModificationsCPU.cs (Method: EachProteinTerminalModifications)


        ////        var protein = new ProteinDto()
        ////        {
        ////            Header = proteinInfo.ID,
        ////            InsilicoDetails = insilico,
        ////            Mw = proteinInfo.MW,
        ////            Sequence = proteinInfo.Seq,
        ////            OriginalSequence = proteinInfo.Seq

        ////        };

        ////        SqlDatabaseProteins.Add(protein);   //  MyNotes - Assign Capacity of SqlDatabaseProteins

        ////    }

        ////    ProteinFetchingTime.Stop();       // DELME Execution Time Working
        ////    return SqlDatabaseProteins;
        ////}

        //////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
        /////* ABOVE IS A WORKING FOR COMMON TRUNCATED CANDIDATE PROTEIN LIST */
        //////~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//





        ////public CandidateProteinListsDto ExtractTruncatedProteins(double IntactMass, SearchParametersDto parameters, List<PstTagList> PstTags, List<ProteinDto> SqlDatabaseProteins)
        ////{
        ////    CandidateProteinListsDto CandidateProteinListsInfo = new CandidateProteinListsDto();
        ////    var CandidateList = CandidateProteinListsInfo.CandidateProteinList;
        ////    var CandidateListTruncated = CandidateProteinListsInfo.CandidateProteinListTruncated;

        ////    for (int index = 0; index < SqlDatabaseProteins.Count; index++)
        ////    {
        ////        var protein = SqlDatabaseProteins[index];


        ////        if (parameters.FilterDb == "True")
        ////        {
        ////            double TotalFixedWeight = 0.0;
        ////            double VariableWeight = 0.0;

        ////            if (parameters.FixedModifications.Count != 0 && parameters.VariableModifications.Count != 0)
        ////            {
        ////                if (parameters.FixedModifications.Count != 0)
        ////                {
        ////                    double ChemicalModMass = GetChemicalModMass(parameters, protein.Sequence);
        ////                    double FixedWeight = GetPTMModMassShift(parameters.FixedModifications, protein.Sequence);
        ////                    TotalFixedWeight = ChemicalModMass + FixedWeight;
        ////                }
        ////                if (parameters.VariableModifications.Count != 0)
        ////                {
        ////                    VariableWeight = GetPTMModMassShift(parameters.VariableModifications, protein.Sequence);
        ////                }
        ////            }
        ////            if (parameters.Truncation == "False")//#Placeholder  // #POTENTIALBUG  IN SPECTRUM {isempty(truncation)} ///   
        ////            {
        ////                if (Math.Abs(protein.Mw + TotalFixedWeight - IntactMass) <= parameters.MwTolerance + VariableWeight)
        ////                {
        ////                    CandidateList.Add(protein);
        ////                }
        ////            }

        ////            else
        ////            {
        ////                if (Math.Abs(protein.Mw + TotalFixedWeight - IntactMass) <= parameters.MwTolerance + VariableWeight)
        ////                {
        ////                    CandidateList.Add(protein);
        ////                }
        ////                else if (protein.Mw - IntactMass > parameters.MwTolerance)
        ////                {
        ////                    if (parameters.DenovoAllow == "True")
        ////                    {
        ////                        for (int i = 0; i < PstTags.Count; i++)
        ////                        {
        ////                            string Tag = PstTags[i].PstTags;
        ////                            if (protein.Sequence.Contains(Tag))
        ////                            {
        ////                                CandidateListTruncated.Add(protein);
        ////                                break;
        ////                            }
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        CandidateListTruncated.Add(protein);
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            CandidateList.Add(protein);
        ////            CandidateListTruncated.Add(protein);
        ////        }
        ////    }

        ////    return CandidateProteinListsInfo;
        ////}


        private string GetQuery(string DatabaseName)//, double IntactMass, double UserDefinedTolerance, string CandidateList)
        {
            string query = "";
            if (1 == 1)//CandidateList == "User Defined Range List")
            {
                string subquery1 = "Select * From  [";
                string subquery2 = "].[dbo].[ProteinInfoes]";
                query = subquery1 + DatabaseName + subquery2;  //Query to select all proteins in specified Database
            }
            else   // Query For Truncated Candidate Protein List
            {
                string subquery1 = "Select * From  [";
                string subquery2 = "].[dbo].[ProteinInfoes]";
                query = subquery1 + DatabaseName + subquery2;  //Query to select all proteins in specified Database
            }


            return query;
        }







        public List<List<ProteinDto>> FetchingSqlDatabaseProteins(SearchParametersDto parameters)
        {
            ConvertStringListToDoubleList _ConvertStringListToDoubleList = new ConvertStringListToDoubleList();

            List<List<ProteinDto>> SqlDatabases = new List<List<ProteinDto>>(2) { new List<ProteinDto>(), new List<ProteinDto>() };


            Stopwatch ProteinFetchingTime = new Stopwatch();         // DELME Execution Time Working
            Stopwatch OneCallTime = new Stopwatch();         // DELME Execution Time Working
            ProteinFetchingTime.Start();              // DELME Execution Time Working

            int iterate = 1;
            if (parameters.FDRCutOff != "0.0" && parameters.FDRCutOff != "0")
            {
                iterate = 2;
            }

            string query;
            string connectionString;

            for (int iterations = 0; iterations < iterate; iterations++)
            {

                if (iterations == 0)
                {
                    query = GetQuery(parameters.ProtDb);
                    connectionString = GetConnectionString(parameters.ProtDb);
                }
                else
                {
                    string DbName = parameters.ProtDb + "Decoy";
                    query = GetQuery(DbName);
                    connectionString = GetConnectionString(DbName);
                }


                List<SerializedProteinDataDto> FetchedSqlProteins = new List<SerializedProteinDataDto>();  //Updated 20201208 - Initialized
                OneCallTime.Start();              // DELME Execution Time Working

                using (var connection = new SqlConnection(connectionString))
                {
                    FetchedSqlProteins = connection.Query<SerializedProteinDataDto>(query).ToList();
                }
                OneCallTime.Stop();              // DELME Execution Time Working
                int Capacity = FetchedSqlProteins.Count;    //Updated 20201210
                var SqlDatabaseProteins = new List<ProteinDto>(Capacity);  //Updated 20201210

                for (int iter = 0; iter < Capacity; iter++)   //Updated 20201210  //   MyNotes - apply here for loop not a foreach loop
                {                                                   ///  MyNotes - PERCEPTRON NOTES
                    var proteinInfo = FetchedSqlProteins[iter];  //Updated 20201210

                    var insilico = new InsilicoObjectDto()
                    {
                        InsilicoMassLeft = _ConvertStringListToDoubleList.ConvertStringToDouble(proteinInfo.Insilico.Split(',')),
                        InsilicoMassRight = _ConvertStringListToDoubleList.ConvertStringToDouble(proteinInfo.InsilicoR.Split(','))   // InsilicoR
                    };
                    // Description Updated 20200917 --- Last Elements of InsilicoMassLeft & InsilicoMassRight are the "MW of Protein - Water" so, it will be removed in TerminalModificationsCPU.cs (Method: EachProteinTerminalModifications)


                    var protein = new ProteinDto()
                    {
                        Header = proteinInfo.ID,
                        InsilicoDetails = insilico,
                        Mw = proteinInfo.MW,
                        Sequence = proteinInfo.Seq,
                        OriginalSequence = proteinInfo.Seq

                    };

                    SqlDatabaseProteins.Add(protein);   //  MyNotes - Assign Capacity of SqlDatabaseProteins
                }
                SqlDatabases[iterations] = SqlDatabaseProteins;
            }
            ProteinFetchingTime.Stop();       // DELME Execution Time Working
            return SqlDatabases;
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




        private string GetConnectionString(string db)
        {
            switch (db)
            {
                case "Ecoli":
                    return ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
                case "EcoliDecoy":
                    return ConfigurationManager.ConnectionStrings["EcoliDecoyConnectionStringName"].ConnectionString;
                case "Human":
                    return ConfigurationManager.ConnectionStrings["HumanConnectionStringName"].ConnectionString;
                case "HumanDecoy":
                    return ConfigurationManager.ConnectionStrings["HumanDecoyConnectionStringName"].ConnectionString;
                default:  // Default is also Human
                    return ConfigurationManager.ConnectionStrings["HumanConnectionStringName"].ConnectionString;
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