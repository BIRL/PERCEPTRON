using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

using System.Diagnostics;

namespace PerceptronLocalService.Engine
{
    public class TerminalModificationsCPU : ITerminalModifications
    {

        public List<ProteinDto> EachProteinTerminalModifications(SearchParametersDto parameters, List<ProteinDto> candidateProteins)
        {
            //int FlagSet = 1; // FlagSet is a vairable for differentiating the some calculations of Simple Terminal Modification to Terminal Modification(Truncation) //Updated 20201112

            /* (Below) Updated 20201207  -- For Time Efficiancy  */
            ModificationMWShift ModificationTableClass = new ModificationMWShift();
            double AcetylationWeight = ModificationTableClass.ModificationMWShiftTable("Acetylation");   ///MassAdjustment.AcetylationWeight;  // Updated 20201201
            double MethionineWeight = AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;
            double AcetylationMinusMethionine =  AcetylationWeight - MethionineWeight;   //Overall will give -ve value...   // Updated 20201217 

            TerminalModificationsList _TerminalModifications = new TerminalModificationsList();
            var IndividualModifications = _TerminalModifications.TerminalModifications(parameters.TerminalModification);
            
            int Capacity = candidateProteins.Count * IndividualModifications.Count;
            /* (Above) Updated 20201130  -- For Time Efficiancy  */

            var tempCandidateProteins = new List<ProteinDto>(Capacity);
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
                TerminalModifications(molW, AcetylationWeight, MethionineWeight, AcetylationMinusMethionine, leftIons, rightIons, sequence, tmpSeqLength, IndividualModifications, tempprotein, tempCandidateProteins); //Updated 20201221 Bug Fix

            }

            return tempCandidateProteins;
            
        }

        //public static void TerminalModifications(int FlagSet, double molW, List<double> leftIons, List<double> rightIons, string tempseq, int tmpSeqLength, SearchParametersDto parameters, ProteinDto tempprotein, List<ProteinDto> tempCandidateProteins)  //Updated 20201112
        public static void TerminalModifications(double molW, double AcetylationWeight, double MethionineWeight, double AcetylationMinusMethionine,  List<double> leftIons, List<double> rightIons, string tempseq, int tmpSeqLength, List<string> IndividualModifications, ProteinDto tempprotein, List<ProteinDto> tempCandidateProteins)  //Updated 20201112
        {
            //double AcetylationWeight = MassAdjustment.AcetylationWeight;
            //double MethionineWeight = AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight; 

            /*      Time Efficiency         BELOW       */
            Stopwatch TimeTerminalModification = new Stopwatch();
            Stopwatch InMemoryCopy = new Stopwatch();
            TimeTerminalModification.Start();     // DELME Execution Time Working

            /*      Time Efficiency         ABOVE       */

            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy
            
            if (IndividualModifications[0] == "None")
            {
                InMemoryCopy.Start();   //DELME
                var newProtein = new ProteinDto(tempprotein);
                InMemoryCopy.Stop();    //DELME


                newProtein.TerminalModification = "None";
                newProtein.Mw = molW;
                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                tempCandidateProteins.Add(newProtein);
            }
            if (tempseq[0] == 'M')
            {
                if (IndividualModifications[1] == "NME") // Used this (IndividualModifications) instead of this (parameters.TerminalModification) to avoid conflict between "NME" & "NME_Acetylation"
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME",
                        Mw = molW - MethionineWeight,
                        InsilicoDetails =
                            {
                                ////InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList(),   /// Updated 20201201 Removed Because of its Runtime cost
                                InsilicoMassLeft = _MassRemove.MassRemoval(leftIons, MethionineWeight),       // Added for Time Efficiency /// Updated 20201201
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

                if (IndividualModifications[2] == "NME_Acetylation")   //Updated 20201207  //Time Complexity
                {

                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME_Acetylation",
                        Mw = molW + AcetylationMinusMethionine,   //Plus Sign (+) is only because we adding AcetylationMinusMethionine i.e. in minus.  Ref SPECTRUM:  ""- AA(double('M')-64) + AcetylationWeight ""    //Updated 20201217
                        InsilicoDetails =
                            {
                                ////InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList(),   /// Updated 20201201 Removed Because of its Runtime cost
                                InsilicoMassLeft = _MassRemove.MassRemoval(leftIons, - AcetylationMinusMethionine),  // -ve sign is only because we adding AcetylationMinusMethionine i.e. in minus. so minus * minus = plus and in _MassRemove.MassRemoval we already have -ve sign /// Updated 20201217
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
                if (IndividualModifications[3] == "M_Acetylation")   //Updated 20201207  //Time Complexity
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "M_Acetylation",
                        Sequence = tempseq,
                        Mw = molW + AcetylationWeight,
                        InsilicoDetails =
                            {
                                ////InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList(),   /// Updated 20201201 Removed Because of its Runtime cost
                                InsilicoMassLeft = _MassRemove.MassRemoval(leftIons, -AcetylationWeight),// (Simple Algebra) -AcetylationWeight will be - * - = + in MassRemoval method // Added for Time Efficiency /// Updated 20201201
                                InsilicoMassRight = rightIons.ToList()
                            }
                    };
                    tempCandidateProteins.Add(newProtein);
                }
            }

            TimeTerminalModification.Stop();
        }
    }
}
