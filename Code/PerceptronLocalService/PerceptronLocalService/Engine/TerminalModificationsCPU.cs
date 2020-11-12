using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.Engine;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public class TerminalModificationsCPU : ITerminalModifications
    {

        public List<ProteinDto> EachProteinTerminalModifications(SearchParametersDto parameters, List<ProteinDto> candidateProteins)
        {
            //int FlagSet = 1; // FlagSet is a vairable for differentiating the some calculations of Simple Terminal Modification to Terminal Modification(Truncation) //Updated 20201112

            var tempCandidateProteins = new List<ProteinDto>();
            for (int index = 0; index < candidateProteins.Count; index++)
            {
                if (parameters.DenovoAllow == "True" && candidateProteins[index].PstScore == 0)
                {
                    continue;
                }


                //if (candidateProteins[index].Header == "A6NDN8" || candidateProteins[index].Header ==  "Q99525")
                //{
                //countDELME = 1;

                //Preparing Protein Info
                var protein = candidateProteins[index];
                var tempprotein = new ProteinDto(protein);  // Lists are referenced based. Cloned a copy of protein

                var sequence = tempprotein.Sequence;
                var leftIons = tempprotein.InsilicoDetails.InsilicoMassLeft;
                var rightIons = tempprotein.InsilicoDetails.InsilicoMassRight;

                //Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O
                leftIons.RemoveAt(leftIons.Count - 1);
                rightIons.RemoveAt(rightIons.Count - 1);

                

                double molW = tempprotein.Mw; //InsilicoDetails.InsilicoMassLeft[tempprotein.InsilicoDetails.InsilicoMassLeft.Count - 1];
                int tmpSeqLength = sequence.Length;

                //TerminalModifications(FlagSet, molW, leftIons, rightIons, sequence, tmpSeqLength, parameters, protein, tempCandidateProteins); //Updated 20201112
                TerminalModifications(molW, leftIons, rightIons, sequence, tmpSeqLength, parameters, protein, tempCandidateProteins); //Updated 20201112

            }

            return tempCandidateProteins;
            
        }

        //public static void TerminalModifications(int FlagSet, double molW, List<double> leftIons, List<double> rightIons, string tempseq, int tmpSeqLength, SearchParametersDto parameters, ProteinDto tempprotein, List<ProteinDto> tempCandidateProteins)  //Updated 20201112
        public static void TerminalModifications(double molW, List<double> leftIons, List<double> rightIons, string tempseq, int tmpSeqLength, SearchParametersDto parameters, ProteinDto tempprotein, List<ProteinDto> tempCandidateProteins)  //Updated 20201112
        {
            double AcetylationWeight = MassAdjustment.AcetylationWeight;
            double MethionineWeight = AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight; 

            string[] IndividualModifications = parameters.TerminalModification.Split(',');

            var DELMECandiList = new List<ProteinDto>();

            if (IndividualModifications.Contains("None"))
            {
                var newProtein = new ProteinDto(tempprotein);
                newProtein.TerminalModification = "None";
                newProtein.Mw = molW;
                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                tempCandidateProteins.Add(newProtein);
            }
            if (tempseq[0] == 'M')
            {
                if (IndividualModifications.Contains("NME")) // Used this (IndividualModifications) instead of this (parameters.TerminalModification) to avoid conflict between "NME" & "NME_Acetylation"
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME",
                        Mw = molW - MethionineWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        },
                        Sequence = tempseq.Substring(1, tmpSeqLength - 1), // "-1" Added
                        //PstScore = tempprotein.PstScore * tempseq.Length / (tempseq.Length - 1)
                    };

                    /*      //Updated 20201112
                     * if (FlagSet == 1)
                    {
                        newProtein.PstScore = newProtein.PstScore * tempseq.Length / (tempseq.Length - 1);
                        newProtein.InsilicoDetails.InsilicoMassLeft.RemoveAt(0);
                        newProtein.InsilicoDetails.InsilicoMassRight.RemoveAt(newProtein.InsilicoDetails.InsilicoMassRight.Count - 1);
                    }
                    */

                    newProtein.PstScore = newProtein.PstScore * tempseq.Length / (tempseq.Length - 1); //Updated 20201112
                    newProtein.InsilicoDetails.InsilicoMassLeft.RemoveAt(0); //Updated 20201112
                    newProtein.InsilicoDetails.InsilicoMassRight.RemoveAt(newProtein.InsilicoDetails.InsilicoMassRight.Count - 1); //Updated 20201112
                    tempCandidateProteins.Add(newProtein);
                }

                if (parameters.TerminalModification.Contains("NME_Acetylation"))
                {

                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME_Acetylation",
                        Mw = molW - MethionineWeight + AcetylationWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft =
                                leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        },
                        Sequence = tempseq.Substring(1, tmpSeqLength - 1), // "-1" Added
                        //PstScore = tempprotein.PstScore * tempseq.Length / (tempseq.Length - 1)
                    };
                    /*      //Updated 20201112
                     * if (FlagSet == 1)
                    {
                        newProtein.PstScore = newProtein.PstScore * tempseq.Length / (tempseq.Length - 1);
                        newProtein.InsilicoDetails.InsilicoMassLeft.RemoveAt(0);
                        newProtein.InsilicoDetails.InsilicoMassRight.RemoveAt(newProtein.InsilicoDetails.InsilicoMassRight.Count - 1);
                    }
                    */

                    newProtein.PstScore = newProtein.PstScore * tempseq.Length / (tempseq.Length - 1); //Updated 20201112
                    newProtein.InsilicoDetails.InsilicoMassLeft.RemoveAt(0); //Updated 20201112
                    newProtein.InsilicoDetails.InsilicoMassRight.RemoveAt(newProtein.InsilicoDetails.InsilicoMassRight.Count - 1); //Updated 20201112

                    tempCandidateProteins.Add(newProtein);
                }
                if (parameters.TerminalModification.Contains("M_Acetylation"))
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "M_Acetylation",
                        Sequence = tempseq,
                        Mw = molW + AcetylationWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        }
                    };
                    tempCandidateProteins.Add(newProtein);
                }
            }
        }
    }
}
