using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

using System.Diagnostics;


namespace PerceptronLocalService.Engine
{
    public class TruncationCPU : ITruncation
    {
        //double AcetylationWeight = MassAdjustment.AcetylationWeight;
        ////private const double MethionineWeight = 42.0106;
        //double MethionineWeight = 131.04049; //RECEIVING so for the #TIMEBEING//= AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;

        public void PreTruncation(double IntactProteinMass, double MwTolerance, List<string> IndividualModifications, 
            List<ProteinDto> proteinList, List<ProteinDto> proteinListLeft, List<ProteinDto> proteinListRight, List<newMsPeaksDto> peakData2DList)
        {
            var proteinExperimentalMw = IntactProteinMass;

            /* (Below) Updated 20201130  -- For Time Efficiancy  */
            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy 
            ModificationMWShift ModificationTableClass = new ModificationMWShift();
            //double MassOfHydrogen = MassAdjustment.H;
            //double MassOfOxygen = MassAdjustment.O;
            double MassOfWater = MassAdjustment.H2O;
            double AcetylationWeight = ModificationTableClass.ModificationMWShiftTable("Acetylation");   ///MassAdjustment.AcetylationWeight;  // Updated 20201201
            double MethionineWeight = AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;
            //double SumOfAcetylationMethionineWeight = AcetylationWeight + MethionineWeight;
            double AcetylationMinusMethionine = AcetylationWeight - MethionineWeight;   //Overall will give -ve value...   // Updated 20201217 

            //TerminalModificationsList _TerminalModifications = new TerminalModificationsList();
            //var IndividualModifications = _TerminalModifications.TerminalModifications(parameters.TerminalModification);
            /* (Above) Updated 20201130  -- For Time Efficiancy  */


            Stopwatch OnlyPreTruncation = new Stopwatch();    // DELME Execution Time Working

            Stopwatch OneCallTime = new Stopwatch();         // DELME Execution Time Working
            Stopwatch OnlyInMemoryCopyTime = new Stopwatch();   // DELME Execution Time Working
            Stopwatch OnlyPreTruncationRemain = new Stopwatch();    // DELME Execution Time Working
            //OnlyPreTruncationFirstHalf.Start();     // DELME Execution Time Working
            Stopwatch OnlyTerminalModification = new Stopwatch();    // DELME Execution Time Working

            Stopwatch TimeUsedBySubstring = new Stopwatch();    // DELME Execution Time Working
            Stopwatch TimeUsedByGetRange = new Stopwatch();    // DELME Execution Time Working

            Stopwatch TimeUsedBySelect = new Stopwatch();   // DELME Execution Time Working
            Stopwatch TimeUsedByFindPreTruncation = new Stopwatch();   // DELME Execution Time Working
            Stopwatch AnotherClassCalling = new Stopwatch();    // DELME Execution Time Working

            Stopwatch ProteinSequenceLengthTime = new Stopwatch();    // DELME Execution Time Working

            OnlyPreTruncation.Start();     // DELME Execution Time Working

            for (var i = 0; i < proteinList.Count; i++)
            {
                //if (proteinList[i].Header == "A6NC98")

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
                OneCallTime.Start();
                //OnlyInMemoryCopyTime.Start();     // DELME Execution Time Working
                var protein1 = proteinList[i];
                //OnlyInMemoryCopyTime.Stop();
                // DELME Execution Time Working
                var protein = new ProteinDto(protein1); //ProteinDto.GetCopy(protein1);
                OneCallTime.Stop();     // DELME Execution Time Working
                                        // DELME Execution Time Working
                                        //continue;  //  #HardCode


                OnlyPreTruncationRemain.Start();        // DELME Execution Time Working
                var prtLength = protein.Sequence.Length;
                var preTruncationIndex = prtLength;
                var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + MwTolerance) / 168) - 1);

                var leftIons = protein.InsilicoDetails.InsilicoMassLeft;
                var sequence = protein.Sequence;
                var rightIons = protein.InsilicoDetails.InsilicoMassRight;
                OnlyPreTruncationRemain.Stop();          // DELME Execution Time Working



                TimeUsedByFindPreTruncation.Start();      // DELME Execution Time Working
                preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, MwTolerance, leftIons, prtLength);
                TimeUsedByFindPreTruncation.Stop();      // DELME Execution Time Working


                OnlyPreTruncationRemain.Start();        // DELME Execution Time Working

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
                OnlyPreTruncationRemain.Stop();        // DELME Execution Time Working


                OnlyInMemoryCopyTime.Start();     // DELME Execution Time Working
                var newProtein = new ProteinDto(protein); //ProteinDto.GetCopy(protein);
                OnlyInMemoryCopyTime.Stop();     // DELME Execution Time Working



                TimeUsedBySubstring.Start();      // DELME Execution Time Working
                var tmpSeq = sequence.Substring(0, preTruncationIndex + factor);
                TimeUsedBySubstring.Stop();      // DELME Execution Time Working


                var tmpSeqLength = tmpSeq.Length;
                newProtein.Sequence = tmpSeq;

                TimeUsedByGetRange.Start();      // DELME Execution Time Working
                leftIons = leftIons.GetRange(0, preTruncationIndex + factor);
                TimeUsedByGetRange.Stop();      // DELME Execution Time Working


                var molW = leftIons[tmpSeqLength - 1] + MassOfWater;   //Updated 20201130  -- For Time Efficiancy 

                // Right Ion Stuff

                var truncationIndex = prtLength - (preTruncationIndex + factor);
                if (truncationIndex != 0)
                {
                    var insilicoTruncationIdxMass = rightIons[truncationIndex - 1];

                    TimeUsedByGetRange.Start();      // DELME Execution Time Working
                    rightIons = rightIons.GetRange(truncationIndex, prtLength - truncationIndex);
                    TimeUsedByGetRange.Stop();      // DELME Execution Time Working


                    TimeUsedBySelect.Start();      // DELME Execution Time Working
                    ////rightIons = rightIons.Select(x => x - insilicoTruncationIdxMass).ToList();   /// Updated 20201201 Removed Because of its Runtime cost
                    rightIons = _MassRemove.MassRemoval(rightIons, insilicoTruncationIdxMass);    // Added for Time Efficiency /// Updated 20201201
                    TimeUsedBySelect.Stop();      // DELME Execution Time Working
                }

                //int FlagSet = 0; // FlagSet is a vairable for differentiating the some calculations of Simple Terminal Modification to Terminal Modification(Truncation) //Updated 20201112

                //TerminalModificationsCPU.TerminalModifications(FlagSet, molW, leftIons, rightIons, tmpSeq, tmpSeqLength, parameters, newProtein, proteinListRight); //Updated 20201112

                OnlyTerminalModification.Start();     // DELME Execution Time Working
                TerminalModificationsCPU.TerminalModifications(molW, AcetylationWeight, MethionineWeight, AcetylationMinusMethionine, leftIons, rightIons, tmpSeq, tmpSeqLength, IndividualModifications, newProtein, proteinListRight); //Updated 20201112   //#EnhancementOfCode In PreTruncation: newProtein should contains all information irrespective to individual molW, leftIons, rightIons, tmpSeq, tmpSeqLength etc.
                OnlyTerminalModification.Stop();     // DELME Execution Time Working

                //Save Copy for Left Truncation
                //protein = proteinList[i];
                //newProtein = new ProteinDto(protein);
                //protein = new ProteinDto(protein1);  //Updated 20200805
                sequence = protein.Sequence;
                prtLength = sequence.Length;
                preTruncationIndex = prtLength;
                rightIons = protein.InsilicoDetails.InsilicoMassRight;

                TimeUsedByFindPreTruncation.Start();      // DELME Execution Time Working
                preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, MwTolerance, rightIons, prtLength);
                TimeUsedByFindPreTruncation.Stop();      // DELME Execution Time Working

                truncationIndex = prtLength - (preTruncationIndex + 1);

                protein.TruncationIndex = truncationIndex + minusfactorTruncationIndex;

                OnlyInMemoryCopyTime.Start();     // DELME Execution Time Working
                newProtein = new ProteinDto(protein); //ProteinDto.GetCopy(protein);
                OnlyInMemoryCopyTime.Stop();     // DELME Execution Time Working


                if (truncationIndex + minusfactorTruncationIndex > 0) //#TESTING: 3 CONDITIONS 
                {
                    newProtein.TruncationIndex = truncationIndex;

                    TimeUsedBySubstring.Start();      // DELME Execution Time Working
                    tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
                    TimeUsedBySubstring.Stop();      // DELME Execution Time Working

                    newProtein.Sequence = tmpSeq;

                    TimeUsedByGetRange.Start();      // DELME Execution Time Working
                    rightIons = rightIons.GetRange(0, preTruncationIndex + 1).ToList();
                    TimeUsedByGetRange.Stop();      // DELME Execution Time Working


                    newProtein.Mw = rightIons[rightIons.Count - 1] + MassOfWater;
                    newProtein.TerminalModification = "None";
                    leftIons = protein.InsilicoDetails.InsilicoMassLeft;

                    double insilicoTruncationIdxMass1 = leftIons[truncationIndex - 1];

                    TimeUsedByGetRange.Start();      // DELME Execution Time Working
                    leftIons = leftIons.GetRange(truncationIndex, prtLength - truncationIndex);
                    TimeUsedByGetRange.Stop();      // DELME Execution Time Working

                    TimeUsedBySelect.Start();      // DELME Execution Time Working
                    ////leftIons = leftIons.Select(x => x - insilicoTruncationIdxMass1).ToList();   /// Updated 20201201 Removed Because of its Runtime cost
                    leftIons = _MassRemove.MassRemoval(leftIons, insilicoTruncationIdxMass1);    // Added for Time Efficiency /// Updated 20201201
                    TimeUsedBySelect.Stop();      // DELME Execution Time Working

                    newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                    newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                    proteinListLeft.Add(newProtein);
                }
                //}  //COMMENT ME

            }


            OnlyPreTruncation.Stop();     // DELME Execution Time Working
        }

        //ITS HEALTHY.... FindPreTruncationIndex [ORIGINAL]
        public int FindPreTruncationIndex(double proteinExperimentalMw, double MwTolerance, List<double> Ions, int ProteinSequenceLength)
        {
            /*  Updated 20201127    Below*/
            ////int preTruncationIndex = ProteinSequenceLength;
            ////var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + MwTolerance) / 168)) - 1;
            ////for (var m = start; m < ProteinSequenceLength; m++)
            ////{
            ////    if (!(Ions[m] - proteinExperimentalMw > MwTolerance)) continue;
            ////    preTruncationIndex = m;
            ////    break;
            ////}
            ////return preTruncationIndex;
            var preTruncationIndex = ProteinSequenceLength;
            var start = 0;
            var mid = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(ProteinSequenceLength) / 2));
            var end = ProteinSequenceLength - 1;

            MergeSort(proteinExperimentalMw, MwTolerance, Ions, ProteinSequenceLength, ref start, ref mid, ref end, ref preTruncationIndex);
            return preTruncationIndex;
            /*  Updated 20201127    Above*/
        }


        public void MergeSort(double proteinExperimentalMw, double MwTolerance, List<double> Ions, int ProteinSequenceLength, ref int start, ref int mid, ref int end, ref int preTruncationIndex) //Updated 20201112
        {
            if (Ions[end] - proteinExperimentalMw < MwTolerance)
            {
                preTruncationIndex = end;
                return;
            }
            else if (Ions[mid] - proteinExperimentalMw > MwTolerance && Ions[mid - 1] - proteinExperimentalMw <= MwTolerance)
            {
                preTruncationIndex = mid;
                return;
            }
            else
            {
                if (Ions[mid] - proteinExperimentalMw > MwTolerance && Ions[mid - 1] - proteinExperimentalMw > MwTolerance)
                {
                    end = mid;
                    mid = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mid + start) / 2));
                    MergeSort(proteinExperimentalMw, MwTolerance, Ions, ProteinSequenceLength, ref start, ref mid, ref end, ref preTruncationIndex);
                }
                else if (Ions[mid] - proteinExperimentalMw < MwTolerance && Ions[mid - 1] - proteinExperimentalMw < MwTolerance)
                {
                    start = mid;
                    mid = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(end + mid) / 2));
                    MergeSort(proteinExperimentalMw, MwTolerance, Ions, ProteinSequenceLength, ref start, ref mid, ref end, ref preTruncationIndex);
                }
            }
        }

        public void TruncationLeft(double IntactProteinMass, string PtmAllow, List<ProteinDto> CandidateProteinListTruncatedLeft, 
            List<ProteinDto> CandidateListTruncationLeftProcessed, List<ProteinDto> RemainingProteinsLeft, List<newMsPeaksDto> peakData2DList)
        {
            
            const int tol = 2;
            int NEEDTOBEDECIDED; int factor;
            if (PtmAllow == "True")  // if PtmAllow is just only BlindPTM otherwise make separate BlindPTM...
            {
                // HERE WHEN BE BLIND PTM...
                NEEDTOBEDECIDED = 128;
                factor = 0;
                subTruncationLeft(PtmAllow, CandidateProteinListTruncatedLeft, IntactProteinMass, tol, NEEDTOBEDECIDED, factor,
                    CandidateListTruncationLeftProcessed, RemainingProteinsLeft);
            }
            else
            {
                NEEDTOBEDECIDED = 256;
                factor = 1;
                subTruncationLeft(PtmAllow, CandidateProteinListTruncatedLeft, IntactProteinMass, tol, NEEDTOBEDECIDED, factor,
                    CandidateListTruncationLeftProcessed, RemainingProteinsLeft);
            }

        }

        public void subTruncationLeft(string PtmAllow, List<ProteinDto> CandidateProteinListTruncatedLeft, double IntactProteinMass,
            int tol, int NEEDTOBEDECIDED, int factor, List<ProteinDto> CandidateListTruncationLeftProcessed, List<ProteinDto> RemainingProteinsLeft)
        {
            Stopwatch subTruncationLeftTime = new Stopwatch();        // DELME Execution Time Working
            Stopwatch subTruncationLeftInMemory = new Stopwatch();        // DELME Execution Time Working
            subTruncationLeftTime.Start();     // DELME Execution Time Working;

            double MassOfHydrogen = MassAdjustment.H;   //Updated 20201130  -- For Time Efficiancy 
            double MassOfOxygen = MassAdjustment.O;   //Updated 20201130  -- For Time Efficiancy 
            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy

            var isLeftRightIonEqualTrunLeft = new List<ProteinDto>();  // #J4TDM
            for (var index = 0; index < CandidateProteinListTruncatedLeft.Count; index++)
            {
                //if (CandidateProteinListTruncatedLeft[index].Header == "Q92544" || CandidateProteinListTruncatedLeft[index].Header == "P61574" || CandidateProteinListTruncatedLeft[index].Header == "P04439" || CandidateProteinListTruncatedLeft[index].Header == "Q8NBJ4" || CandidateProteinListTruncatedLeft[index].Header == "A0A087WT01")
                ////if (CandidateProteinListTruncatedLeft[index].Header == "O95298")
                //{
                var tempprotein = CandidateProteinListTruncatedLeft[index];

                subTruncationLeftInMemory.Start();         // DELME Execution Time Working
                var protein = new ProteinDto(tempprotein); //ProteinDto.GetCopy(tempprotein);
                subTruncationLeftInMemory.Stop();         // DELME Execution Time Working

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
                        if (PtmAllow == "True")
                        {
                            //proteinListRemaining.Add(protein);    //  IN SPECTRUM!!!!    -- IS IT BUG OR NOT....!!!
                            RemainingProteinsLeft.Add(protein);
                        }
                    }
                    else
                    {
                        protein.Truncation = "Left";

                        ////protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.Select(x => x - protein.InsilicoDetails.InsilicoMassLeft[leftIndex]).ToList();   /// Updated 20201201 Removed Because of its Runtime cost
                        protein.InsilicoDetails.InsilicoMassLeft = _MassRemove.MassRemoval(protein.InsilicoDetails.InsilicoMassLeft, protein.InsilicoDetails.InsilicoMassLeft[leftIndex]);    // Added for Time Efficiency /// Updated 20201201
                        protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(leftIndex + 1, protein.InsilicoDetails.InsilicoMassLeft.Count - leftIndex - 1);

                        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(0, prtLength - leftIndex - 1); // as this will be the MW of protein - Water

                        var sequence = protein.Sequence.Substring(leftIndex + 1, prtLength - leftIndex - 1);

                        protein.Sequence = sequence;

                        if (sequence.Length < 5)
                            continue;

                        protein.TruncationIndex = protein.TruncationIndex + leftIndex;
                        protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + (2 * MassOfHydrogen) + MassOfOxygen;

                        CandidateListTruncationLeftProcessed.Add(protein);

                        if (protein.InsilicoDetails.InsilicoMassLeft.Count != protein.InsilicoDetails.InsilicoMassRight.Count)
                        {
                            isLeftRightIonEqualTrunLeft.Add(protein);
                        }
                    }
                }
                //}
            }
            subTruncationLeftTime.Stop();     // DELME Execution Time Working;
        }

        public void TruncationRight(double IntactProteinMass, string PtmAllow, List<ProteinDto> CandidateProteinListTruncatedRight, 
            List<ProteinDto> CandidateListTruncationRightProcessed, List<ProteinDto> RemainingProteinsRight, List<newMsPeaksDto> peakData2DList)
        {
            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy 
            const int tol = 2;
            int NEEDTOBEDECIDED; int factor;
            if (PtmAllow == "True") //parameters.PtmAllow  == BlindPtm
            {
                // HERE THERE WILL BE BLIND PTM...
                NEEDTOBEDECIDED = 256; factor = 0;
                subTruncationRight(PtmAllow, CandidateProteinListTruncatedRight, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationRightProcessed, RemainingProteinsRight);
            }
            else
            {
                NEEDTOBEDECIDED = 168; factor = -1;
                subTruncationRight(PtmAllow, CandidateProteinListTruncatedRight, IntactProteinMass, tol, NEEDTOBEDECIDED, factor, CandidateListTruncationRightProcessed, RemainingProteinsRight);
            }
        }

        public void subTruncationRight(string PtmAllow, List<ProteinDto> CandidateProteinListTruncatedRight, double IntactProteinMass,
            int tol, int NEEDTOBEDECIDED, int factor, List<ProteinDto> CandidateListTruncationRightProcessed, List<ProteinDto> RemainingProteinsRight)
        {
            double MassOfHydrogen = MassAdjustment.H;   //Updated 20201130  -- For Time Efficiancy 
            double MassOfOxygen = MassAdjustment.O;   //Updated 20201130  -- For Time Efficiancy 
            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy 

            var isLeftRightIonEqualTrunRight = new List<ProteinDto>();//DELME

            for (var index = 0; index < CandidateProteinListTruncatedRight.Count; index++) // 0 //index = 1
            {
                //if (CandidateProteinListTruncatedRight[index].Header == "P31689")
                //{

                var tempprotein = CandidateProteinListTruncatedRight[index];
                var protein = new ProteinDto(tempprotein); //ProteinDto.GetCopy(tempprotein);

                var prtLength = protein.Sequence.Length;

                // shift experimental Mass by truncation mass
                var truncationMass = protein.Mw - IntactProteinMass;

                //truncationMass = AdjustProteinForTruncation(truncationMass, parameters);
                //Just one Amino Acid can't be Proteform so """start == 1 OR <-1""" is obseleted


                var start = -1; //Just for Initialization
                if (factor == -1)  //Updated 20210122
                    start = Convert.ToInt32(Math.Ceiling(truncationMass / NEEDTOBEDECIDED)) - 2; //Updated 20210119 // "-1" is Added for ZeroIndexing.   ////NEEDTOBEDECIDED = 168 OR 256
                else if (factor == 0)
                    start = Convert.ToInt32(Math.Ceiling(truncationMass / NEEDTOBEDECIDED)) - 1; //Updated 20210122 // "-1" is Added for ZeroIndexing.   ////NEEDTOBEDECIDED = 168 OR 256


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

                    if (rightIndex == -1)////////IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!IS IT BUG OR NOT....!!!
                    {
                        if (PtmAllow == "True")
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

                        ////protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.Select(x => x - protein.InsilicoDetails.InsilicoMassRight[rightIndex]).ToList();    /// Updated 20201201 Removed Because of its Runtime cost

                        protein.InsilicoDetails.InsilicoMassRight = _MassRemove.MassRemoval(protein.InsilicoDetails.InsilicoMassRight, protein.InsilicoDetails.InsilicoMassRight[rightIndex]);// as this will be the MW of protein - Water    // Added for Time Efficiency /// Updated 20201201
                        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(rightIndex + 1, protein.InsilicoDetails.InsilicoMassRight.Count - rightIndex - 1);

                        var sequence = protein.Sequence.Substring(0, truncationIndex - 1);

                        protein.Sequence = sequence;

                        if (sequence.Length < 5) continue;



                        protein.TruncationIndex = truncationIndex - 2;  // "-1" is Added for Zero Indexing of C#   #ASK

                        protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + (2 * MassOfHydrogen) + MassOfOxygen;

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
            if (parameters.DenovoAllow == "True")
            {
                for (int iterationOnProteinList = 0; iterationOnProteinList < CandidateProteinListInput.Count; iterationOnProteinList++)
                {

                    var tempprotein = CandidateProteinListInput[iterationOnProteinList];
                    var protein = new ProteinDto(tempprotein); //ProteinDto.GetCopy(tempprotein);

                    var LeftIons = protein.InsilicoDetails.InsilicoMassLeft;
                    var RightIons = protein.InsilicoDetails.InsilicoMassRight;
                    var PstTagsExists = new List<string>();

                    for (int iteration = 0; iteration < PstTags.Count; iteration++)
                    {

                        tag = PstTags[iteration].PstTags;
                        if (protein.Sequence.Contains(tag))// && protein.Header == "A6H8Y1")
                        {
                            //BELOW - For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O
                            protein.InsilicoDetails.InsilicoMassLeft = LeftIons.GetRange(0, LeftIons.Count - 1);     // Updated 20200920   -- Assigned to protein.InsilicoDetails.InsilicoMassLeft
                            protein.InsilicoDetails.InsilicoMassRight = RightIons.GetRange(0, RightIons.Count - 1);     // Updated 20200920   -- Assigned to protein.InsilicoDetails.InsilicoMassRight

                            PstTagsExists.Add(PstTags[iteration].PstTags);
                            break;     // Updated 20200920   -- Added

                        }
                    }
                    if (PstTagsExists.Count != 0)
                    {
                        protein.PstTagsWithComma = string.Join(",", PstTagsExists); //Joining Pst Tags with comma in a One string
                        CandidateProteinsListFinal.Add(protein);
                    }
                }
            }
            else
            {
                for (int iterationOnProteinList = 0; iterationOnProteinList < CandidateProteinListInput.Count; iterationOnProteinList++)
                {
                    var tempprotein = CandidateProteinListInput[iterationOnProteinList];
                    var protein = new ProteinDto(tempprotein); //ProteinDto.GetCopy(tempprotein);

                    var LeftIons = protein.InsilicoDetails.InsilicoMassLeft;
                    var RightIons = protein.InsilicoDetails.InsilicoMassRight;

                    //BELOW - For Fragmentation Ions: Therefore, last positioned Ions Removed as its the Mass of protein -H2O
                    protein.InsilicoDetails.InsilicoMassLeft = LeftIons.GetRange(0, LeftIons.Count - 1);     // Updated 20200920   -- Assigned to protein.InsilicoDetails.InsilicoMassLeft
                    protein.InsilicoDetails.InsilicoMassRight = RightIons.GetRange(0, RightIons.Count - 1);     // Updated 20200920   -- Assigned to protein.InsilicoDetails.InsilicoMassRight

                    CandidateProteinsListFinal.Add(protein);
                }
            }
            return CandidateProteinsListFinal;
        }
    }
}
