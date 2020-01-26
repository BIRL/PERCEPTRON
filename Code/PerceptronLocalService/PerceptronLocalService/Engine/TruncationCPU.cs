using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;


namespace PerceptronLocalService.Engine
{
    public class Truncation : ITruncation
    {
        private const double AcetylationWeight = 42.0106;
        //private const double MethionineWeight = 42.0106;
        double MethionineWeight = 131.04049; //RECEIVING so for the #TIMEBEING//= AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;

        public void PreTruncation(SearchParametersDto parameters, List<ProteinDto> proteinList, List<ProteinDto> proteinListLeft, List<ProteinDto> proteinListRight, List<newMsPeaksDto> peakData2DList)
        {
            string[] IndividualModifications = parameters.TerminalModification.Split(',');
            var proteinExperimentalMw = peakData2DList[0].Mass;

            for (var i = 0; i < proteinList.Count; i++)
            {
                //if (proteinList[i].Header == "Q8NBJ4")
                //{


                var protein1 = proteinList[i];

                var protein = new ProteinDto(protein1);

                var prtLength = protein.Sequence.Length;
                var preTruncationIndex = prtLength;
                var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + parameters.MwTolerance) / 168) - 1);

                var leftString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassLeft);
                var leftIons = Clone.Decrypt<List<double>>(leftString);

                var seqString = Clone.CloneObject(protein.Sequence);
                var sequence = Clone.Decrypt<string>(seqString);

                var rightString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassRight);
                var rightIons = Clone.Decrypt<List<double>>(rightString);


                preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, parameters.MwTolerance, leftIons, prtLength);

                int factor = 0; /// factor is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                if (preTruncationIndex != prtLength)  //#TESTING: 2 CONDITIONS 
                {
                    factor = 1;
                }


                protein.TruncationIndex = preTruncationIndex;
                var newProtein = new ProteinDto(protein);

                var tmpSeq = sequence.Substring(0, preTruncationIndex + factor);
                var tmpSeqLength = tmpSeq.Length;
                newProtein.Sequence = tmpSeq;

                leftIons = leftIons.GetRange(0, preTruncationIndex + factor);

                var molW = leftIons[tmpSeqLength - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;

                // Right Ion Stuff

                var truncationIndex = prtLength - (preTruncationIndex + factor);
                if (truncationIndex != 0)
                {
                    var insilicoTruncationIdxMass = rightIons[truncationIndex - 1];
                    rightIons = rightIons.GetRange(truncationIndex, prtLength - truncationIndex);
                    rightIons = rightIons.Select(x => x - insilicoTruncationIdxMass).ToList();
                }

                if (IndividualModifications[0] == "None")
                {



                    newProtein.TerminalModification = "None";
                    newProtein.Mw = molW;
                    newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                    newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                    proteinListRight.Add(newProtein);
                }

                if (tmpSeq[0] == 'M')
                {


                    if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but not in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation
                    {
                        newProtein = new ProteinDto(protein)
                        {
                            TerminalModification = "NME",
                            Mw = molW - MethionineWeight,
                            InsilicoDetails =
                            {
                                InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList(),
                                InsilicoMassRight = rightIons.ToList()
                            },
                            Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                        };
                        proteinListRight.Add(newProtein);
                    }

                    if (parameters.TerminalModification.Contains("NME_Acetylation"))
                    {

                        newProtein = new ProteinDto(protein)
                        {
                            TerminalModification = "NME_Acetylation",
                            Mw = molW - MethionineWeight + AcetylationWeight,
                            InsilicoDetails =
                            {
                                InsilicoMassLeft =
                                    leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList(),
                                InsilicoMassRight = rightIons.ToList()
                            },
                            Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                        };
                        proteinListRight.Add(newProtein);
                    }
                    if (parameters.TerminalModification.Contains("M_Acetylation"))
                    {
                        newProtein = new ProteinDto(protein)
                        {
                            TerminalModification = "M_Acetylation",
                            Sequence = tmpSeq,
                            Mw = molW + AcetylationWeight,
                            InsilicoDetails =
                            {
                                InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList(),
                                InsilicoMassRight = rightIons.ToList()
                            }
                        };
                        proteinListRight.Add(newProtein);
                    }
                }

                //Save Copy for Left Truncation
                //protein = proteinList[i];
                //newProtein = new ProteinDto(protein);



                protein = new ProteinDto(protein1);




                sequence = Clone.Decrypt<string>(seqString);
                prtLength = sequence.Length;
                preTruncationIndex = prtLength;
                rightIons = Clone.Decrypt<List<double>>(rightString);

                preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, parameters.MwTolerance, rightIons, prtLength);

                truncationIndex = prtLength - (preTruncationIndex + 1);

                protein.TruncationIndex = truncationIndex;
                newProtein = new ProteinDto(protein);



                if (truncationIndex > 0) //#TESTING: 3 CONDITIONS 
                {
                    newProtein.TruncationIndex = truncationIndex;
                    tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
                    newProtein.Sequence = tmpSeq;
                    rightIons = rightIons.GetRange(0, preTruncationIndex + 1).ToList();

                    newProtein.Mw = rightIons[rightIons.Count - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;
                    newProtein.TerminalModification = "None";
                    leftIons = Clone.Decrypt<List<double>>(leftString);

                    double insilicoTruncationIdxMass1 = leftIons[truncationIndex - 1];
                    leftIons = leftIons.GetRange(truncationIndex, prtLength - truncationIndex);

                    leftIons = leftIons.Select(x => x - insilicoTruncationIdxMass1).ToList();
                    newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                    newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                    proteinListLeft.Add(newProtein);
                }
                //}
            }
        }

        public int FindPreTruncationIndex(double proteinExperimentalMw, double MwTolerance, List<double> Ions, int ProteinSequenceLength)
        {
            int preTruncationIndex = ProteinSequenceLength;
            var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + MwTolerance) / 168)) - 1;
            for (var m = start; m < ProteinSequenceLength; m++)
            {
                if (!(Ions[m] - proteinExperimentalMw > MwTolerance)) continue;
                preTruncationIndex = m;
                break;
            }
            return preTruncationIndex;
        }
        public List<double> ElementwiseListOperation(List<double> Ions, double Offset)
        {
            var newList = new List<double>();
            for (int iter = 0; iter < Ions.Count; iter++)
            {

                newList.Add(Ions[iter] + Offset);
            }
            return newList;

        }



    }
}
