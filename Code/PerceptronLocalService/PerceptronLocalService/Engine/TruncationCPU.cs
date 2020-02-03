using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;


namespace PerceptronLocalService.Engine
{
    public class TruncationCPU : ITruncation
    {
        //double AcetylationWeight = MassAdjustment.AcetylationWeight;
        ////private const double MethionineWeight = 42.0106;
        //double MethionineWeight = 131.04049; //RECEIVING so for the #TIMEBEING//= AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;

        public void PreTruncation(SearchParametersDto parameters, List<ProteinDto> proteinList, List<ProteinDto> proteinListLeft, List<ProteinDto> proteinListRight, List<newMsPeaksDto> peakData2DList)
        {
            var proteinExperimentalMw = peakData2DList[0].Mass;

            for (var i = 0; i < proteinList.Count; i++)
            {
                //if (proteinList[i].Header == "P31689")
                //{
                /////////////////////////////////////////////////////////// #J4TDM  ///////////////////////////////////////////////////////////
                //if (proteinList[i].Header == "P04439")
                //if (proteinList[i].Header == "P61574" || proteinList[i].Header == "Q8IVY1" || proteinList[i].Header == "O00507" || proteinList[i].Header == "Q9HC73" || proteinList[i].Header == "Q9Y6D6" || proteinList[i].Header == "Q5JY77" || proteinList[i].Header == "Q9Y6D6")

                ///////////////////////////////////////////////////////////
                //if (proteinList[i].Header == "A0A1B0GUU1" || proteinList[i].Header == "A6NNS2" || proteinList[i].Header == "Q9NQR1" || proteinList[i].Header == "Q7Z7E8" || proteinList[i].Header == "P31930" || proteinList[i].Header == "Q8IWU6" || proteinList[i].Header == "Q9NX05" ||
                //proteinList[i].Header == "Q9BWD1" || proteinList[i].Header == "Q9H8M9" || proteinList[i].Header == "Q5BKX8" || proteinList[i].Header == "Q496J9" || proteinList[i].Header == "P35250" || proteinList[i].Header == "P17661" || proteinList[i].Header == "A6H8Y1" || proteinList[i].Header == "P53801")
                //{
                /////////////////////////////////////////////////////////// #J4TDM  ///////////////////////////////////////////////////////////


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

                // GIVE BELOW CODE IS JUST FOR COMPENSATING ZERO BASED INDEXING
                int factor = 0; /// factor is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                int minusfactorPreTruncationIndex = 0; /// minusfactorPreTruncationIndex is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                int minusfactorTruncationIndex = 0; /// minusfactorTruncationIndex is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM


                if (preTruncationIndex != prtLength)  //#TESTING: 2 CONDITIONS 
                {
                    factor = 1;
                }
                else
                {
                    minusfactorPreTruncationIndex = -1;/// minusfactorPreTruncationIndex is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                    minusfactorTruncationIndex = 1; /// minusfactorTruncationIndex is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                }
                // GIVE ABOVE CODE IS JUST FOR COMPENSATING ZERO BASED INDEXING


                protein.TruncationIndex = preTruncationIndex + minusfactorPreTruncationIndex;
                var newProtein = new ProteinDto(protein);

                var tmpSeq = sequence.Substring(0, preTruncationIndex + factor);
                var tmpSeqLength = tmpSeq.Length;
                newProtein.Sequence = tmpSeq;

                leftIons = leftIons.GetRange(0, preTruncationIndex + factor);

                var molW = leftIons[tmpSeqLength - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;  // newProtein.Mw    //#FreeTimeTesting

                // Right Ion Stuff

                var truncationIndex = prtLength - (preTruncationIndex + factor);
                if (truncationIndex != 0)
                {
                    var insilicoTruncationIdxMass = rightIons[truncationIndex - 1];
                    rightIons = rightIons.GetRange(truncationIndex, prtLength - truncationIndex);
                    rightIons = rightIons.Select(x => x - insilicoTruncationIdxMass).ToList();
                }
               
                int FlagSet = 0; // FlagSet is a vairable for differentiating the some calculations of Simple Terminal Modification to Terminal Modification(Truncation)

                TerminalModificationsCPU.TerminalModifications(FlagSet, molW, leftIons, rightIons, tmpSeq, tmpSeqLength, parameters, protein, proteinListRight);


                //if (IndividualModifications[0] == "None")
                //{
                //    newProtein.TerminalModification = "None";
                //    newProtein.Mw = molW;
                //    newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                //    newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                //    proteinListRight.Add(newProtein);
                //}

                //if (tmpSeq[0] == 'M')
                //{
                //    if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but not in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation
                //    {
                //        newProtein = new ProteinDto(protein)
                //        {
                //            TerminalModification = "NME",
                //            Mw = molW - MethionineWeight,
                //            InsilicoDetails =
                //            {
                //                InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList(),
                //                InsilicoMassRight = rightIons.ToList()
                //            },
                //            Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                //        };
                //        proteinListRight.Add(newProtein);
                //    }

                //    if (parameters.TerminalModification.Contains("NME_Acetylation"))
                //    {

                //        newProtein = new ProteinDto(protein)
                //        {
                //            TerminalModification = "NME_Acetylation",
                //            Mw = molW - MethionineWeight + AcetylationWeight,
                //            InsilicoDetails =
                //            {
                //                InsilicoMassLeft =
                //                    leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList(),
                //                InsilicoMassRight = rightIons.ToList()
                //            },
                //            Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                //        };
                //        proteinListRight.Add(newProtein);
                //    }
                //    if (parameters.TerminalModification.Contains("M_Acetylation"))
                //    {
                //        newProtein = new ProteinDto(protein)
                //        {
                //            TerminalModification = "M_Acetylation",
                //            Sequence = tmpSeq,
                //            Mw = molW + AcetylationWeight,
                //            InsilicoDetails =
                //            {
                //                InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList(),
                //                InsilicoMassRight = rightIons.ToList()
                //            }
                //        };
                //        proteinListRight.Add(newProtein);
                //    }
                //}

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

                protein.TruncationIndex = truncationIndex + minusfactorTruncationIndex;
                newProtein = new ProteinDto(protein);



                if (truncationIndex + minusfactorTruncationIndex > 0) //#TESTING: 3 CONDITIONS 
                {
                    newProtein.TruncationIndex = truncationIndex;
                    tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
                    newProtein.Sequence = tmpSeq;
                    rightIons = rightIons.GetRange(0, preTruncationIndex + 1).ToList();

                    newProtein.Mw = rightIons[rightIons.Count - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;
                    newProtein.TerminalModification = "None";
                    leftIons = Clone.Decrypt<List<double>>(leftString);

                    double insilicoTruncationIdxMass1 = leftIons[truncationIndex - 1];
                    leftIons = leftIons.GetRange(truncationIndex, prtLength - truncationIndex);

                    leftIons = leftIons.Select(x => x - insilicoTruncationIdxMass1).ToList();
                    newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                    newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                    proteinListLeft.Add(newProtein);
                }
                //}  //COMMENT ME
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


        public void TruncationLeft(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncatedLeft, List<ProteinDto> CandidateListTruncationLeftProcessed, List<ProteinDto> RemainingProteinsLeft, List<newMsPeaksDto> peakData2DList)
        {
            var IntactProteinMass = peakData2DList[0].Mass;
            const int tol = 2;
            int NEEDTOBEDECIDED; int factor;
            if (parameters.PtmAllow == 1)  // if PtmAllow is just only BlindPTM otherwise make separate BlindPTM...
            {
                // HERE WHEN BE BLIND PTM...
                NEEDTOBEDECIDED = 128;
                factor = 0;
                subTruncationLeft(parameters, CandidateProteinListTruncatedLeft, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationLeftProcessed, RemainingProteinsLeft);
            }
            else
            {
                NEEDTOBEDECIDED = 256;
                factor = 1;
                subTruncationLeft(parameters, CandidateProteinListTruncatedLeft, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationLeftProcessed, RemainingProteinsLeft);
            }

        }

        public void subTruncationLeft(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncatedLeft, double IntactProteinMass, int tol, int NEEDTOBEDECIDED, int factor, List<ProteinDto> CandidateListTruncationLeftProcessed, List<ProteinDto> RemainingProteinsLeft)
        {
            var isLeftRightIonEqualTrunLeft = new List<ProteinDto>();  // #J4TDM
            for (var index = 0; index < CandidateProteinListTruncatedLeft.Count; index++)
            {
                //if (CandidateProteinListTruncatedLeft[index].Header == "Q92544" || CandidateProteinListTruncatedLeft[index].Header == "P61574" || CandidateProteinListTruncatedLeft[index].Header == "P04439" || CandidateProteinListTruncatedLeft[index].Header == "Q8NBJ4" || CandidateProteinListTruncatedLeft[index].Header == "A0A087WT01")
                ////if (CandidateProteinListTruncatedLeft[index].Header == "O95298")
                //{
                var tempprotein = CandidateProteinListTruncatedLeft[index];
                var protein = new ProteinDto(tempprotein);
                var prtLength = protein.Sequence.Length;



                // shift experimental Mass by truncation mass
                var truncationMass = protein.Mw - IntactProteinMass;
                var start = Convert.ToInt32(Math.Ceiling(truncationMass / NEEDTOBEDECIDED) - 1); // "-1" is Added Just for ZERO INDEXING...  // NEEDTOBEDECIDED  = 256 OR 128
                if (truncationMass > 0)
                {
                    // Find Truncation Location and Type
                    var leftIndex = -1;

                    for (var m = start; m < prtLength; m++)
                    {
                        var diffLeft = protein.InsilicoDetails.InsilicoMassLeft[m] - truncationMass;

                        if (Math.Abs(diffLeft) <= tol) // No need to worry about Terminal Modifications!)
                            leftIndex = m; // This index correspond to the New truncation site located at N-Terminous

                        if (diffLeft > tol)
                            break;
                    }
                    if (leftIndex == -1)  ////////IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!
                    {
                        if (parameters.PtmAllow == 1)
                        {
                            //proteinListRemaining.Add(protein);    //  IN SPECTRUM!!!!    -- IS IT BUG OR NOT....!!!
                            RemainingProteinsLeft.Add(protein);
                        }
                    }
                    else
                    {
                        protein.Truncation = "Left";

                        protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.Select(x => x - protein.InsilicoDetails.InsilicoMassLeft[leftIndex]).ToList();

                        protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(leftIndex + 1, protein.InsilicoDetails.InsilicoMassLeft.Count - leftIndex - 1);

                        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(0, prtLength - leftIndex - 1); // as this will be the MW of protein - Water

                        var sequence = protein.Sequence.Substring(leftIndex + 1, prtLength - leftIndex - 1);

                        protein.Sequence = sequence;

                        if (sequence.Length < 5)
                            continue;

                        protein.TruncationIndex = protein.TruncationIndex + leftIndex;
                        protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;

                        CandidateListTruncationLeftProcessed.Add(protein);

                        if (protein.InsilicoDetails.InsilicoMassLeft.Count != protein.InsilicoDetails.InsilicoMassRight.Count)
                        {
                            isLeftRightIonEqualTrunLeft.Add(protein);
                        }
                    }
                }
                //}
            }
        }

        public void TruncationRight(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncatedRight, List<ProteinDto> CandidateListTruncationRightProcessed, List<ProteinDto> RemainingProteinsRight, List<newMsPeaksDto> peakData2DList)
        {
            var IntactProteinMass = peakData2DList[0].Mass;
            const int tol = 2;
            int NEEDTOBEDECIDED; int factor;
            if (parameters.PtmAllow == 1)  // if PtmAllow is just only BlindPTM otherwise make separate BlindPTM...
            {
                // HERE THERE WILL BE BLIND PTM...
                NEEDTOBEDECIDED = 256; factor = 0;
                subTruncationRight(parameters, CandidateProteinListTruncatedRight, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationRightProcessed, RemainingProteinsRight);
            }
            else
            {
                NEEDTOBEDECIDED = 168; factor = -1;
                subTruncationRight(parameters, CandidateProteinListTruncatedRight, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationRightProcessed, RemainingProteinsRight);
            }
        }

        public void subTruncationRight(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncatedRight, double IntactProteinMass, int tol, int NEEDTOBEDECIDED, int factor, List<ProteinDto> CandidateListTruncationRightProcessed, List<ProteinDto> RemainingProteinsRight)
        {
            var isLeftRightIonEqualTrunRight = new List<ProteinDto>();

            for (var index = 1; index < CandidateProteinListTruncatedRight.Count; index++)
            {
                //if (CandidateProteinListTruncatedRight[index].Header == "P31689")
                //{

                var tempprotein = CandidateProteinListTruncatedRight[index];
                var protein = new ProteinDto(tempprotein);

                var prtLength = protein.Sequence.Length;

                // shift experimental Mass by truncation mass
                var truncationMass = protein.Mw - IntactProteinMass;

                //truncationMass = AdjustProteinForTruncation(truncationMass, parameters);
                //Just one Amino Acid can't be Proteform so """start == 1 OR <-1""" is obseleted
                var start = Convert.ToInt32(Math.Ceiling(truncationMass / NEEDTOBEDECIDED)) - 1; // +factor;  // "-1" is Added for ZeroIndexing.   ////NEEDTOBEDECIDED = 168 OR 256

                if (truncationMass > 0)
                {

                    // Find Truncation Location and Type
                    var rightIndex = -1;

                    for (var m = start; m < prtLength; m++)
                    {
                        var diff = protein.InsilicoDetails.InsilicoMassRight[m] - truncationMass;
                        if (Math.Abs(diff) <= tol) // No need to worry about Terminal Modifications!)
                            rightIndex = m; // This index correspond to the New truncation site located at N-Terminous
                        if (diff > tol)
                            break;
                    }
                    int isPtmAllow = 1;
                    if (rightIndex == -1)////////IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!
                    {
                        if (isPtmAllow != 1)
                        {
                            //proteinListRemaining.Add(protein);    //  IN SPECTRUM!!!!    -- IS IT BUG OR NOT....!!!
                            RemainingProteinsRight.Add(protein);
                        }

                    }

                    else
                    {
                        var truncationIndex = prtLength - rightIndex;

                        protein.Truncation = "Right";

                        protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(0, truncationIndex - 1);

                        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.Select(x => x - protein.InsilicoDetails.InsilicoMassRight[rightIndex]).ToList(); // as this will be the MW of protein - Water

                        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(rightIndex + 1, protein.InsilicoDetails.InsilicoMassRight.Count - rightIndex - 1);

                        var sequence = protein.Sequence.Substring(0, truncationIndex - 1);

                        protein.Sequence = sequence;

                        if (sequence.Length < 5) continue;



                        protein.TruncationIndex = truncationIndex - 2;  // "-1" is Added for Zero Indexing of C#   #ASK

                        protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;

                        CandidateListTruncationRightProcessed.Add(protein);

                        if (protein.InsilicoDetails.InsilicoMassLeft.Count != protein.InsilicoDetails.InsilicoMassRight.Count)
                        {
                            isLeftRightIonEqualTrunRight.Add(protein);
                        }
                    }

                }
                //}
            }
        }

        public List<ProteinDto> FilterTruncatedProteins(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListInput, List<PstTagList> PstTags)
        {
            var CandidateProteinsListFinal = new List<ProteinDto>();

            string tag;
            if (parameters.DenovoAllow == 1)
            {
                for (int iterationOnProteinList = 0; iterationOnProteinList < CandidateProteinListInput.Count; iterationOnProteinList++)
                {

                    var tempprotein = CandidateProteinListInput[iterationOnProteinList];
                    var protein = new ProteinDto(tempprotein);

                    var LeftIons = protein.InsilicoDetails.InsilicoMassLeft;
                    var RightIons = protein.InsilicoDetails.InsilicoMassRight;

                    for (int iteration = 0; iteration < PstTags.Count; iteration++)
                    {
                        tag = PstTags[iteration].PstTags;
                        if (protein.Sequence.Contains(tag))
                        {
                            LeftIons = LeftIons.GetRange(0, LeftIons.Count - 1);   // For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O
                            RightIons = RightIons.GetRange(0, RightIons.Count - 1); // For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O

                            CandidateProteinsListFinal.Add(protein);
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int iterationOnProteinList = 0; iterationOnProteinList < CandidateProteinListInput.Count; iterationOnProteinList++)
                {
                    var tempprotein = CandidateProteinListInput[iterationOnProteinList];
                    var protein = new ProteinDto(tempprotein);

                    var LeftIons = protein.InsilicoDetails.InsilicoMassLeft;
                    var RightIons = protein.InsilicoDetails.InsilicoMassRight;

                    LeftIons = LeftIons.GetRange(0, LeftIons.Count - 1); // For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O
                    RightIons = RightIons.GetRange(0, RightIons.Count - 1);// For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O

                    CandidateProteinsListFinal.Add(protein);
                }
            }
            return CandidateProteinsListFinal;
        }
    }
}
