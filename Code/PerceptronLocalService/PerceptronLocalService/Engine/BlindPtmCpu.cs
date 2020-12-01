using System;
using System.Collections.Generic;
using System.Diagnostics;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;
using PerceptronLocalService.Interfaces;
using System.Linq;

namespace PerceptronLocalService.Engine
{
    public class BlindPtmCpu : IBlindPTMModule
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();

        public BlindPTMDto BlindPTMExtraction(List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters)
        {
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakData2DList.OrderBy(n => n.Mass).ToList();
            var MolW = ExperimentalSpectrum[ExperimentalSpectrum.Count - 1].Mass;// Molar weight that is the last row of the peak list
            var UserHopThreshold = 1;

            // Creation of "peaks" first index as 0 followed by all the peaklist except MolW and then, MolW - all the peaks from peaklist and then at the last index, there is MolW
            var peaks = new List<double>();
            peaks.Add(0);
            for (int row = 0; row <= ExperimentalSpectrum.Count - 2; row++)     // "-1" is for Excluding Intact Mass & other "-1" is for Zero Indexing
            {
                peaks.Add(ExperimentalSpectrum[row].Mass);                          
            }
            for (int row = 0; row <= ExperimentalSpectrum.Count - 2; row++)     /// *R  peakData.Mass.Count   *W   peakData2DList.Count
            {
                peaks.Add(MolW - ExperimentalSpectrum[row].Mass);
            }
            peaks.Add(MolW);
            peaks = peaks.OrderBy(n => n).ToList(); // Sorting of peaks

            // InfoTable data
            double[] InfoModMass = { 70.0055000000000, 99.0321000000000, 111.032100000000, 113.047700000000, 113.047700000000, 117.024800000000, 129.042600000000, 129.042600000000, 131.999400000000, 139.063400000000, 142.074200000000, 142.110600000000, 143.058300000000, 144.089900000000, 147.035400000000, 156.126300000000, 159.035400000000, 160.084800000000, 163.030300000000, 166.998300000000, 170.105600000000, 170.116700000000, 173.014700000000, 173.032400000000, 173.051100000000, 181.014000000000, 184.132400000000, 194.993200000000, 198.111700000000, 208.048400000000, 217.025200000000, 243.029600000000, 290.111400000000, 304.127100000000, 341.239200000000, 408.077200000000, 431.164900000000 };
            char[] InfoModAminoAcids = { 'S', 'G', 'Q', 'A', 'P', 'C', 'S', 'P', 'C', 'P', 'G', 'K', 'T', 'K', 'M', 'K', 'M', 'K', 'M', 'S', 'K', 'R', 'C', 'E', 'M', 'T', 'R', 'D', 'R', 'Y', 'H', 'Y', 'S', 'T', 'C', 'C', 'N' };
            string[] InfoModName = { "Pyruvate-S", "Acetylation", "Pyrrolidone-Aarboxylic-Acid", "Acetylation", "Hydroxylation", "Methylation", "Acetylation", "DiHydroxylation", "S-Nitrosylation", "Acetylation", "Methylation", "Methylation", "Acetylation", "Hydroxylation", "Sulfoxide", "DiMethylation", "Formylation", "DiHydroxylation", "Sulfone", "Phosphorylation", "Acetylation", "Methylation", "Pyruvate-C", "Gamma-Carboxyglutamic-Acid", "Acetylation", "Phosphorylation", "DiMethylation", "Phosphorylation", "Acetylation", "Nitration", "Phosphorylation", "Phosphorylation", "O-linked-Glycosylation", "O-linked-Glycosylation", "Palmitoylation", "Glutathionylation", "N-linked-Glycosylation" };

            // Initialization
            var HopInfoName = new List<string>();
            var HopInfoAA = new List<char>();
            var HopInfoEnd = new List<double>();
            var HopInfoStart = new List<double>();
            var LadderIndex = 0;

            // Extraction of Modified and Unmodified tags
            for (int ExpI = 1; ExpI <= peaks.Count - 2; ExpI++)
            {
                for (int ExpJ = ExpI + 1; ExpJ <= peaks.Count - 1; ExpJ++)
                {
                    double PeakDiff = peaks[ExpJ] - peaks[ExpI];
                    for (int AAIndex = 0; AAIndex <= InfoModMass.Length - 1; AAIndex++)
                    {
                        double Error = PeakDiff - InfoModMass[AAIndex];
                        double Abs_Error = Math.Abs(Error);
                        if (Abs_Error <= UserHopThreshold)
                        {
                            LadderIndex = LadderIndex + 1;
                            HopInfoName.Add(InfoModName[AAIndex]);
                            HopInfoAA.Add(InfoModAminoAcids[AAIndex]);
                            HopInfoStart.Add(peaks[ExpI]);
                            HopInfoEnd.Add(peaks[ExpJ]);
                        }
                        else if (Error < -UserHopThreshold)
                        {
                            break;
                        }
                    }
                }
            }
            var sizeHopInfo = HopInfoAA.Count;

            var BlindPTMExtractionInfo = new BlindPTMDto(sizeHopInfo, HopInfoName, HopInfoAA, HopInfoEnd, HopInfoStart);

            // Function for BlindPTM should be called here

            return BlindPTMExtractionInfo;
        }

        public List<ProteinDto> BlindPTMGeneral(List<ProteinDto> CandidateProtList, List<newMsPeaksDto> peakData2DList, double UserHopThreshold, BlindPTMDto BlindPTMExtractionInfo, SearchParametersDto parameters, string TypeOfFunction)
        //A general function for BlindPTM, BlindPTM_Truncation_Left and BlindPTM_Truncation_Right
        {
            //Variable initialization
            var sizeHopInfo = BlindPTMExtractionInfo.sizeHopInfo;
            var HopInfoName = BlindPTMExtractionInfo.HopInfoName;
            var HopInfoAA = BlindPTMExtractionInfo.HopInfoAA;
            var HopInfoEnd = BlindPTMExtractionInfo.HopInfoEnd;
            var HopInfoStart = BlindPTMExtractionInfo.HopInfoStart;
            var PeptideTolerance = parameters.PeptideTolerance;
            var PeptideToleranceUnit = parameters.PeptideToleranceUnit;
            List<ProteinDto> CandidateProtListModified = new List<ProteinDto>();

            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakData2DList.OrderBy(n => n.Mass).ToList();
            var MolW = ExperimentalSpectrum[ExperimentalSpectrum.Count - 1].Mass;    // Molar weight that is the last row of the peak list   /// *R peakData.Mass[peakData.Mass.Count - 1]  *W peakData2DList[peakData2DList.Count - 1].Mass
            double tolConv = 0;

            // if size of peakData is 1, then tolConv is equal to that one mass value, else it is the second-last mass value from the sorted peakData list
            if (ExperimentalSpectrum.Count == 1)
            {
                tolConv = ExperimentalSpectrum[ExperimentalSpectrum.Count - 1].Mass;
            }
            else
            {
                tolConv = ExperimentalSpectrum[ExperimentalSpectrum.Count - 2].Mass;
            }
            List<PTMDataDto> ShortlistedHops = new List<PTMDataDto>();
            if (sizeHopInfo > 0)
            {
                for (int row = 0; row < CandidateProtList.Count; row++)
                {
                    ProteinDto protein = new ProteinDto(CandidateProtList[row]); //Protein at index row is being processed
                    var sequence = protein.Sequence;
                    //If the function is BlindPTM_Truncation_Left, the protein sequence is flipped, otherwise the original sequence is processed
                    if (TypeOfFunction == "BlindPTM_Truncation_Left")
                    {
                        char[] array = sequence.ToCharArray();
                        Array.Reverse(array);
                        sequence = new String(array);
                    }
                    //Variable initialization
                    var HopI = 0;
                    var ThrI = 0;
                    var loop = 1;
                    var Ladder_Index = -1;

                    while (loop == 1)
                    {
                        var Start = HopInfoStart[HopI];
                        if (Ladder_Index != -1 && ShortlistedHops.ElementAt(Ladder_Index).End > Start) //Ladder_Index should be accessible i.e. it should be from 0  or greater and to check if the size of ShortlistedHop is non-zero, we check that end should be greater than start
                        {
                            HopI = HopI + 1;
                            if (HopI >= sizeHopInfo - 1) //if Hop index exceeds the size of Hop, break the loop
                                break;
                            else
                                continue;
                        }
                        var diff = 0.0;
                        if (TypeOfFunction == "BlindPTM_Truncation_Left")
                        {
                            diff = Start - protein.InsilicoDetails.InsilicoMassRight[ThrI];
                        }
                        else
                        {
                            diff = Start - protein.InsilicoDetails.InsilicoMassLeft[ThrI];
                        }
                        if (diff <= UserHopThreshold && diff >= -UserHopThreshold)
                        {
                            string AA = HopInfoAA[HopI].ToString();
                            if (AA[0] == sequence[ThrI + 1])
                            {
                                var Mod = HopInfoName[HopI];
                                var ModWeight = ModificationTableClass.ModificationMWShiftTable(Mod);  //Function ModTable is defined below BlindPTMGeneral
                                var End = HopInfoEnd[HopI];
                                if (TypeOfFunction == "BlindPTM_Truncation_Left")
                                    diff = Math.Abs(End - (protein.InsilicoDetails.InsilicoMassRight[ThrI + 1] + ModWeight));
                                else
                                    diff = Math.Abs(End - (protein.InsilicoDetails.InsilicoMassLeft[ThrI + 1] + ModWeight));
                                if (PeptideToleranceUnit == "ppm")
                                    diff = (diff / tolConv) * 1000000;
                                else if (PeptideToleranceUnit == "%")
                                    diff = (diff / tolConv) * 100;
                                if (diff < PeptideTolerance)
                                {
                                    //InsilicoMassLeft and InsilicoMassRight are being updated by the addition of the mass of modification
                                    if (TypeOfFunction == "BlindPTM_Truncation_Left")
                                    {
                                        for (int i = ThrI + 1; i < protein.InsilicoDetails.InsilicoMassRight.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassRight[i] = protein.InsilicoDetails.InsilicoMassRight[i] + ModWeight;

                                        for (int i = protein.Sequence.Length - ThrI - 2; i < protein.InsilicoDetails.InsilicoMassLeft.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] = protein.InsilicoDetails.InsilicoMassLeft[i] + ModWeight;
                                    }
                                    else
                                    {
                                        for (int i = ThrI + 1; i < protein.InsilicoDetails.InsilicoMassLeft.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] = protein.InsilicoDetails.InsilicoMassLeft[i] + ModWeight;

                                        for (int i = protein.Sequence.Length - ThrI - 2; i < protein.InsilicoDetails.InsilicoMassRight.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassRight[i] = protein.InsilicoDetails.InsilicoMassRight[i] + ModWeight;
                                    }

                                    protein.Mw = protein.Mw + ModWeight;
                                    Ladder_Index = Ladder_Index + 1;

                                    //Protein components are being updated 
                                    var tempList = new PTMDataDto(Mod, AA, End, Start, ThrI);
                                    ShortlistedHops.Add(tempList);
                                    //ShortlistedHops.ElementAt(Ladder_Index).ModName = Mod;
                                    //ShortlistedHops.ElementAt(Ladder_Index).AminoAcidName = (AA);
                                    //ShortlistedHops.ElementAt(Ladder_Index).Start = Start;
                                    //ShortlistedHops.ElementAt(Ladder_Index).End = End;
                                    //ShortlistedHops.ElementAt(Ladder_Index).ThrI = ThrI;
                                }
                            }
                        }
                        else if (diff > UserHopThreshold)
                        {
                            ThrI = ThrI + 1;
                            if (TypeOfFunction == "BlindPTM_Truncation_Left")
                            {
                                if (ThrI >= protein.InsilicoDetails.InsilicoMassRight.Count - 1) //If index exceeds the length of insilico right list, break the loop
                                    break;
                                else
                                    continue;
                            }
                            else
                            {
                                if (ThrI >= protein.InsilicoDetails.InsilicoMassLeft.Count - 1)
                                    break;
                                else
                                    continue;
                            }
                        }
                        else if (diff < -UserHopThreshold)
                        {
                            HopI = HopI + 1;
                            if (HopI >= sizeHopInfo - 1) //if Hop index exceeds the size of Hop, break the loop
                                break;
                            else
                                continue;
                        }
                        HopI = HopI + 1;
                        if (HopI >= sizeHopInfo - 1)
                            break;
                    }

                    for (int HopIndex = 0; HopIndex < ShortlistedHops.Count; HopIndex++)
                    {
                        int index = 0;
                        // Updating the protein
                        
                        if (TypeOfFunction == "BlindPTM_Truncation_Left")
                            index = sequence.Length - ShortlistedHops[HopIndex].ThrI;
                        else
                            index = ShortlistedHops[HopIndex].ThrI + 1;

                        protein.PtmParticulars.Add(new PostTranslationModificationsSiteDto(index, ShortlistedHops[HopIndex].ModName,
                                                                                        Convert.ToChar(ShortlistedHops[HopIndex].AminoAcidName)));
                        
                    }
                    // Protein molar weight scoring
                    if (TypeOfFunction == "BlindPTM")
                    {
                        var error = Math.Abs(MolW - protein.Mw);
                        if (error == 0)
                            protein.MwScore = 1;
                        else
                            protein.MwScore = 1 / Math.Pow(2, error);
                    }
                    // All the updated proteins are being stored in a new list CandidateProtListModified
                    if (ShortlistedHops.Count > 0)
                    {
                        ProteinDto tempProtein = new ProteinDto(protein);      // Just for safety against referenced based property of list...
                        CandidateProtListModified.Add(tempProtein);
                    }
                }
            }
            return CandidateProtListModified;
        }

        public List<ProteinDto> BlindPTMLocalization(List<ProteinDto> Matches, double IntactMass, SearchParametersDto parameters)
        {
            for (int index = 0; index < Matches.Count; index++)
            {
                // Ptm parameters initialization
                ProteinDto protein = Matches[index];

                var MassDiff = IntactMass - protein.Mw;  // peakData2DList[0].Mass = Intact Mass of Protein

                if (parameters.PtmAllow == "True")
                {
                    if (MassDiff > 13 && MassDiff < 951.3660) //Arbitrary number less then weight of methyl group && 3*N-linked-Glycosylation
                    {
                        var left = 0;
                        var right = 0;
                        if (protein.LeftMatchedIndex.Count > 0)
                        {
                            left = protein.LeftMatchedIndex[protein.LeftMatchedIndex.Count - 1] + 1; //The last LeftMatchedIndex + 1 gives the start site of modification
                        }
                        else
                        {
                            left = 1;
                        }
                        if (protein.RightMatchedIndex.Count > 0)
                        {
                            right = protein.InsilicoDetails.InsilicoMassLeft.Count - protein.RightMatchedIndex[protein.RightMatchedIndex.Count - 1] + 1; //Num of LeftIons - last LeftMatchedIndex + 1 gives the end site of modification
                        }
                        else
                        {
                            right = protein.InsilicoDetails.InsilicoMassLeft.Count; //Num of LeftIons (i.e. the index right next to where LeftIons end) gives the end site of modification
                        }
                        if (left < right && left > 1 && right < protein.InsilicoDetails.InsilicoMassLeft.Count)
                        {
                            // Blind PTM Localization Information are beig updated
                            protein.BlindPtmLocalizationInfo = new BlindPtmInfo(left, right, MassDiff);
                            protein.Mw = protein.Mw + MassDiff;
                        }
                    }
                }
                // Scoring
                var error = Math.Abs(MassDiff);
                if (error == 0)
                {
                    protein.MwScore = 1;
                }
                else
                {
                    protein.MwScore = 1 / Math.Pow(2, error);
                }
            }
            return Matches;
        }

        public List<ProteinDto> PTMTruncation_Modification(List<ProteinDto> CandidateProtListInput, List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters, string FunctionType)
        {
            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            //var peakDatalist = new List<peakData2Dlist>();
            List<ProteinDto> CandidateProtListOutput = new List<ProteinDto>();
            RemoveMass _MassRemove = new RemoveMass();   //Added 20201201  -- For Time Efficiancy
            //for (int row = 0; row < peakData.Mass.Count; row++)
            //{
            //    var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
            //    peakDatalist.Add(dataforpeakDatalist);
            //}
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakData2DList.OrderBy(n => n.Mass).ToList();
            var MolW = ExperimentalSpectrum[ExperimentalSpectrum.Count - 1].Mass;    // Molar weight that is the last row of the peak list
            double tolConv = 0;
            string cleavageType = parameters.InsilicoFragType;

            // if size of peakData is 1, then tolConv is equal to that one mass value, else it is the second-last mass value from the sorted peakData list
            if (peakData2DList.Count == 1)
            {
                tolConv = peakData2DList[peakData2DList.Count -1 ].Mass;
            }
            else
            {
                tolConv = peakData2DList[peakData2DList.Count - 2].Mass;
            }
            var PeptideTolerance = parameters.PeptideTolerance;
            var PeptideToleranceUnit = parameters.PeptideToleranceUnit;
            double tol = 0;
            if (PeptideToleranceUnit == "ppm")
                tol = (PeptideTolerance / tolConv) * 1000000;
            else if (PeptideToleranceUnit == "%")
                tol = (PeptideTolerance / tolConv) * 100;

            for (int index = 0; index < CandidateProtListInput.Count; index++)
            {
                var protein = new ProteinDto(CandidateProtListInput[index]);
                var TruncationMass = protein.Mw - MolW;
                if (cleavageType == "CID" || cleavageType == "IMD" || cleavageType == "BIRD" || cleavageType == "SID" || cleavageType == "HCD")
                {
                    if (FunctionType == "Truncation_Left_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.Proton;
                    else if (FunctionType == "Truncation_Right_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.OH + 2 * MassAdjustment.H;
                }
                else if (cleavageType == "ECD" || cleavageType == "ETD")
                {
                    if (FunctionType == "Truncation_Left_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.Proton + MassAdjustment.N + 3 * MassAdjustment.H;
                    else if (FunctionType == "Truncation_Right_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.OH - MassAdjustment.NH;
                }
                else if (cleavageType == "EDD" || cleavageType == "NETD")
                {
                    if (FunctionType == "Truncation_Left_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.Proton - MassAdjustment.CO;
                    else if (FunctionType == "Truncation_Right_Modification")
                        TruncationMass = TruncationMass + MassAdjustment.OH + MassAdjustment.CO;
                }

                if (TruncationMass > 0)
                {
                    var Index = -1;
                    for (int ind = 0; ind < protein.Sequence.Length; ind++)
                    {
                        if (FunctionType == "Truncation_Left_Modification")
                        {
                            var diff_left = protein.InsilicoDetails.InsilicoMassLeft[ind] - TruncationMass;
                            if (Math.Abs(diff_left) <= tol)
                                Index = ind;
                            else if (Math.Abs(diff_left) > tol)
                                break;
                        }
                        else if (FunctionType == "Truncation_Right_Modification")
                        {
                            var diff_right = protein.InsilicoDetails.InsilicoMassRight[ind] - TruncationMass;
                            if (Math.Abs(diff_right) <= tol)
                                Index = ind;
                            else if (Math.Abs(diff_right) > tol)
                                break;
                        }
                    }
                    if (Index != -1)
                    {
                        if (FunctionType == "Truncation_Left_Modification")
                        {
                            protein.Truncation = "Left";

                            ////protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.Select(x => x - protein.InsilicoDetails.InsilicoMassLeft[Index]).ToList();     //Updated 20201201 Removed Because of its Runtime cost

                            protein.InsilicoDetails.InsilicoMassLeft = _MassRemove.MassRemoval(protein.InsilicoDetails.InsilicoMassLeft, protein.InsilicoDetails.InsilicoMassLeft[Index]);     // Added for Time Efficiency /// Updated 20201201

                            protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(Index + 1, protein.InsilicoDetails.InsilicoMassLeft.Count - Index - 1);

                            protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(0, protein.Sequence.Length - Index - 1); // as this will be the MW of protein - Water

                            var sequence = protein.Sequence.Substring(Index + 1, protein.Sequence.Length - Index - 1);
                            protein.Sequence = sequence;
                            if (sequence.Length < 5)
                                continue;

                            protein.TruncationIndex = protein.TruncationIndex + Index;
                            protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;

                            CandidateProtListOutput.Add(protein);
                        }
                        else if (FunctionType == "Truncation_Right_Modification")
                        {
                            var truncationIndex = protein.Sequence.Length - Index;
                            protein.Truncation = "Right";
                            protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(0, truncationIndex - 1);
                            
                            ////protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.Select(x => x - protein.InsilicoDetails.InsilicoMassRight[Index]).ToList();     //Updated 20201201 Removed Because of its Runtime cost // as this will be the MW of protein - Water

                            protein.InsilicoDetails.InsilicoMassRight = _MassRemove.MassRemoval(protein.InsilicoDetails.InsilicoMassRight, protein.InsilicoDetails.InsilicoMassRight[Index]);     // Added for Time Efficiency /// Updated 20201201


                            protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(Index + 1, protein.InsilicoDetails.InsilicoMassRight.Count - Index - 1);

                            var sequence = protein.Sequence.Substring(0, truncationIndex - 1);
                            protein.Sequence = sequence;

                            if (sequence.Length < 5)
                                continue;

                            protein.TruncationIndex = truncationIndex - 2;  // "-1" is Added for Zero Indexing of C#
                            protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;
                            CandidateProtListOutput.Add(protein);
                        }
                    }
                }
            }
            return CandidateProtListOutput;
        }
    }
}
