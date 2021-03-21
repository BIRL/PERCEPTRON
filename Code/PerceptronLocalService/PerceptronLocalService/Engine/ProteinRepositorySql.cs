using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositorySql : IProteinRepository
    {
        public CandidateProteinListsDto ExtractProteins(double IntactMass, SearchParametersDto parameters, List<PstTagList> PstTags, List<ProteinDto> SqlDatabaseProteins)
        {
            CandidateProteinListsDto CandidateProteinListsInfo = new CandidateProteinListsDto();
            var CandidateList = CandidateProteinListsInfo.CandidateProteinList;
            var CandidateListTruncated = CandidateProteinListsInfo.CandidateProteinListTruncated;

            for (int index = 0; index < SqlDatabaseProteins.Count; index++)
            {
                var protein = SqlDatabaseProteins[index];

                //if (!(protein.Header == "P32162"))   //  Q9BTM9
                //{
                //    continue;
                //}

                //if (!(protein.Header == "P31689"))
                //{
                //    continue;
                //}

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