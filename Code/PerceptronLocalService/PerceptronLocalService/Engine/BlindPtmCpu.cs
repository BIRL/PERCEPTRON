using System;
using System.Collections.Generic;
using System.Diagnostics;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;
using PerceptronLocalService.Interfaces;
using System.Linq;

namespace PerceptronLocalService.Engine
{
    public class BlindPtmCpu 
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();

        public BlindPTMDto BlindPTMExtraction(MsPeaksDto peakData, SearchParametersDto parameters)
        {
            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            var peakDatalist = new List<peakData2Dlist>();
            for (int row = 0; row <= peakData.Mass.Count - 1; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);
            }
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakDatalist.OrderBy(n => n.Mass).ToList();
            var MolW = peakData.Mass[peakData.Mass.Count - 1];    // Molar weight that is the last row of the peak list
            var UserHopThreshold = 1;

            // peaks has first index as 0, followed by all the peaklist except MolW, followed by MolW - all the peaks from peaklist and then at the last index, there is MolW
            var peaks = new List<double>();
            peaks.Add(0);
            for (int row = 0; row <= peakData.Mass.Count - 2; row++)
            {
                peaks.Add(peakData.Mass[row]);
            }
            for (int row = 0; row <= peakData.Mass.Count - 2; row++)
            {
                peaks.Add(MolW - peakData.Mass[row]);
            }
            peaks.Add(MolW);
            peaks = peaks.OrderBy(n => n).ToList(); // Sorting of peaks

            // InfoTable data
            double[] InfoModMass = { 70.0055, 99.0321, 111.0321, 113.0477, 113.0477, 117.0248, 129.0426, 129.0426, 131.9994, 139.0634, 142.0742, 142.1106, 143.0583, 144.0899, 147.0354, 156.1263, 159.0354, 160.0848, 163.0303, 166.9983, 170.1056, 170.1167, 173.0147, 173.0324, 173.0511, 181.0140, 184.1324, 194.9932, 198.1117, 208.0484, 217.0252, 243.0296, 290.1114, 304.1271, 341.2392, 408.0772, 431.1649 };
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
                    double PeakDiff = peaks[ExpI] - peaks[ExpJ];
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

        public List<ProteinDto> BlindPTMGeneral(List<ProteinDto> CandidateProtList, MsPeaksDto peakData, double UserHopThreshold, BlindPTMDto BlindPTMExtractionInfo, SearchParametersDto parameters, string TypeOfFunction)
        //A general function for BlindPTM, BlindPTM_Truncation_Left and BlindPTM_Truncation_Right
        {
            int sizeHopInfo = BlindPTMExtractionInfo.sizeHopInfo;
            List<string> HopInfoName = BlindPTMExtractionInfo.HopInfoName;
            List<char> HopInfoAA = BlindPTMExtractionInfo.HopInfoAA;
            List<double> HopInfoEnd = BlindPTMExtractionInfo.HopInfoEnd;
            List<double> HopInfoStart = BlindPTMExtractionInfo.HopInfoStart;

            //Variable initialization
            var PeptideTolerance = parameters.PeptideTolerance;
            var PeptideToleranceUnit = parameters.PeptideToleranceUnit;
            List<ProteinDto> CandidateProtListModified = new List<ProteinDto>();
            var proteinIndex = 0;

            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            var peakDatalist = new List<peakData2Dlist>();
            for (int row = 0; row < peakData.Mass.Count; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);
            }
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakDatalist.OrderBy(n => n.Mass).ToList();
            var MolW = peakData.Mass[peakData.Mass.Count - 1];    // Molar weight that is the last row of the peak list
            double tolConv = 0;

            // if size of peakData is 1, then tolConv is equal to that one mass value, else it is the second-last mass value from the sorted peakData list
            if (peakData.Mass.Count == 1)
            {
                tolConv = peakData.Mass[peakData.Mass.Count - 1];
            }
            else
            {
                tolConv = peakData.Mass[peakData.Mass.Count - 2];
            }
            List<PTMDataDto> ShortlistedHops = new List<PTMDataDto>();
            if (sizeHopInfo > 0)
            {
                for (int row = 0; row < CandidateProtList.Count; row++)
                {
                    var protein = CandidateProtList.ElementAt(row); //Protein at index row is being processed
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
                                var ModWeight = ModificationTableClass.ModificationMWShiftTable(Mod); //Function ModTable is defined below BlindPTMGeneral
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
                                    ShortlistedHops.ElementAt(Ladder_Index).ModName = Mod;
                                    ShortlistedHops.ElementAt(Ladder_Index).AminoAcidName = (AA);
                                    ShortlistedHops.ElementAt(Ladder_Index).Start = Start;
                                    ShortlistedHops.ElementAt(Ladder_Index).End = End;
                                    ShortlistedHops.ElementAt(Ladder_Index).ThrI = ThrI;
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
                        // Updating the protein
                        protein.PtmParticulars[HopIndex].ModName = ShortlistedHops.ElementAt(HopIndex).ModName;
                        protein.PtmParticulars[HopIndex].Site = Convert.ToChar(ShortlistedHops.ElementAt(HopIndex).AminoAcidName);
                        if (TypeOfFunction == "BlindPTM_Truncation_Left")
                            protein.PtmParticulars[HopIndex].Index = sequence.Length - ShortlistedHops.ElementAt(HopIndex).ThrI;
                        else
                            protein.PtmParticulars[HopIndex].Index = ShortlistedHops.ElementAt(HopIndex).ThrI + 1;
                    }
                    // Protein molar weight scoring
                    if (TypeOfFunction == "BlindPTM")
                    {
                        var error = Math.Abs(MolW - protein.Mw);
                        if (error == 0)
                            protein.MwScore = 1;
                        else
                            protein.MwScore = Math.Pow((1 / 2), error);
                    }
                    // All the updated proteins are being stored in a new list CandidateProtListModified
                    if (ShortlistedHops.Count > 0)
                    {
                        proteinIndex = proteinIndex + 1;
                        CandidateProtListModified[proteinIndex] = protein;
                    }
                }
            }
            return CandidateProtListModified;
        }


        public List<ProteinDto> BlindPTMLocalization(List<ProteinDto> Matches, MsPeaksDto peakData, SearchParametersDto parameters)
        {
            for (int index = 0; index <= Matches.Count; index++)
            {
                // Ptm parameters initialization
                var protein = Matches.ElementAt(index);
                protein.PtmParticulars[index].ModStartSite = -1;
                protein.PtmParticulars[index].ModEndSite = -1;
                protein.PtmParticulars[index].ModWeight = -1;
                var MassDiff = peakData.Mass[0] - protein.Mw;

                if (parameters.PtmAllow == 1)
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
                        if (left < right && left > 1 && right < protein.LeftMatchedIndex.Count)
                        {
                            // Ptm parameters are beig updated
                            protein.PtmParticulars[index].ModStartSite = left;
                            protein.PtmParticulars[index].ModEndSite = right;
                            protein.PtmParticulars[index].ModWeight = MassDiff;
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
                    protein.MwScore = Math.Pow((1 / 2), error);
                }
            }
            return Matches;
        }



        //public static void BlindPTM(List<double> experimentalSpectrum, double molW, List<ProteinDto> candidateProteinsList, double pepTol, double userHopThreshold, string pepUnit)
        //{
        //    var stopwatch = new Stopwatch();

        //    // Data Preperation
        //    stopwatch.Start();
        //    var peaks = new List<double>();
        //    foreach (var peak in experimentalSpectrum)
        //    {
        //        peaks.Add(peak + 1.00727647);
        //        peaks.Add(molW - (peak + 1.00727647));
        //        //peaks.Add(peak);
        //        //peaks.Add(molW - (peak));
        //    }
        //    peaks.Sort();
        //    stopwatch.Stop();
        //    Console.WriteLine("Data Preperation: "+ stopwatch.Elapsed);
            
        //    // PTM Extraction
        //    stopwatch.Restart();
        //    var aminoAcidList = new List<string>();
        //    var modificationList = new List<string>();
        //    var startList = new List<double>();
        //    var endList = new List<double>();
        //    for (var expI = 0; expI < peaks.Count; expI++)
        //    {
        //        for (var expJ = expI + 1; expJ < peaks.Count; expJ++)
        //        {
        //            var peakDiff = peaks[expJ] - peaks[expI];
        //            var modification = AminoAcids.GetModifiedAminoAcid(peakDiff, userHopThreshold);
        //            foreach (var mod in modification)
        //            {
        //                var temproray = mod.Split('_');
        //                if (temproray.Length <= 1) continue;
        //                aminoAcidList.Add(temproray[1]);
        //                modificationList.Add(temproray[0]);
        //                startList.Add(peaks[expI]);
        //                endList.Add(peaks[expJ]);
        //            }
        //        }
        //    }
        //    stopwatch.Stop();
        //    Console.WriteLine("Generation: " + stopwatch.Elapsed);
            
        //    // PTM Shortlisting
        //    stopwatch.Restart();
        //    foreach (var protein in candidateProteinsList)
        //    {
        //        var sequence = protein.Sequence;
        //        var hopIndex = 0;
        //        var thrI = 0;
        //        var shortlistedAminoAcid = new List<string>();
        //        var shortlistedModification = new List<string>();
        //        var shortlistedEnd = new List<double>();
        //        var shortlistedStart = new List<double>();
        //        var shortlistedIndex = new List<int>();
                
        //        while (true)
        //        {
        //            try
        //            {
        //                if (startList.Count > 0)
        //                {
        //                    if (shortlistedStart.Count > 0)
        //                        if (shortlistedEnd[shortlistedEnd.Count-1] > startList[hopIndex])
        //                        {
        //                            hopIndex = hopIndex + 1;
        //                            if (hopIndex == startList.Count)
        //                            {
        //                                break;
        //                            }
        //                            continue;
        //                        }
        //                    var diff = startList[hopIndex] - protein.InsilicoDetails.InsilicoMassLeft[thrI];
        //                    if (diff <= userHopThreshold && diff >= -userHopThreshold)
        //                    {
        //                        if (aminoAcidList[hopIndex] == sequence[thrI+2].ToString())
        //                        {
        //                            var modMass = AminoAcids.ModificationTable(modificationList[hopIndex]);
        //                            diff =
        //                                Math.Abs(endList[hopIndex] -
        //                                         (protein.InsilicoDetails.InsilicoMassLeft[thrI + 1
        //                                             ] +
        //                                          modMass));
        //                            if (string.Compare(pepUnit, "ppm", StringComparison.Ordinal) == 0)
        //                                diff = (diff / molW) * 1000000;
        //                            else if (string.Compare(pepUnit, "%", StringComparison.Ordinal) == 0)
        //                                diff = (diff / molW) * 100;
        //                            if (diff < pepTol)
        //                            {
        //                                for (var i = thrI + 1;
        //                                    i < protein.InsilicoDetails.InsilicoMassLeft.Count;
        //                                    i++)
        //                                    protein.InsilicoDetails.InsilicoMassLeft[i] =
        //                                        protein.InsilicoDetails.InsilicoMassLeft[i] +
        //                                        modMass;
        //                                protein.Mw = protein.Mw + modMass;
        //                                shortlistedAminoAcid.Add(aminoAcidList[hopIndex]);
        //                                shortlistedModification.Add(modificationList[hopIndex]);
        //                                shortlistedEnd.Add(endList[hopIndex]);
        //                                shortlistedStart.Add(startList[hopIndex]);
        //                                shortlistedIndex.Add(thrI);
        //                            }
        //                        }
        //                    }
        //                    else if (diff > userHopThreshold)
        //                    {
        //                        thrI = thrI + 1;
        //                        if (thrI == protein.InsilicoDetails.InsilicoMassLeft.Count - 1 )
        //                            break;
        //                        continue;
        //                    }
        //                    else if (diff < -userHopThreshold)
        //                    {
        //                        hopIndex = hopIndex + 1;
        //                        if (hopIndex == startList.Count)
        //                            break;
        //                        continue;
        //                    }
        //                    hopIndex = hopIndex + 1;
        //                    if (hopIndex == startList.Count)
        //                        break;
        //                }
        //            }
        //            catch (Exception exception)
        //            {
        //                Debug.WriteLine(exception.Message);
        //            }
        //        }
        //        for (var hopIter = 0; hopIter < shortlistedStart.Count; hopIter++)
        //        {
        //            var site = new PostTranslationModificationsSiteDto
        //            {
        //                Index = shortlistedIndex[hopIter],
        //                ModName = shortlistedModification[hopIter],
        //                ModWeight = AminoAcids.ModificationTable(shortlistedModification[hopIter]),
        //                Site = Convert.ToChar(shortlistedAminoAcid[hopIter])
        //            };
        //            protein.PtmParticulars.Add(site);
        //        }
        //        var massError = Math.Abs(molW - protein.Mw);
        //        protein.MwScore = Math.Abs(massError) < 0 ? 1 : Math.Pow(massError, 0.5);
        //    }
        //    stopwatch.Stop();
        //    Console.WriteLine("Shortlisting :" + stopwatch.Elapsed);
        //}

        //public static void BlindPTM_Localization(List<Protein> matches, double experimentalProteinMass)
        //{
        //    for (var index = 0; index < matches.Count; index++)
        //    {
        //        /*  Matches{index,1}.Blind.Start = -1;
        //            Matches{index,1}.Blind.End = -1;
        //            Matches{index,1}.Blind.Mass = -1;
        //            massDiff = (Experimental_Protein_Mass - Matches{index,1}.MolW);
        //            if massDiff > 13
        //                if numel(Matches{index,1}.LeftMatched_Index) > 0
        //                    left = Matches{index,1}.LeftMatched_Index(numel(Matches{index,1}.LeftMatched_Index))+1;
        //                else
        //                    left = 1;
        //                end
        //                if numel(Matches{index,1}.RightMatched_Index) >0
        //                    right = numel(Matches{index,1}.LeftIons)-Matches{index,1}.RightMatched_Index(numel(Matches{index,1}.RightMatched_Index))+1;
        //                else
        //                    right = numel(Matches{index,1}.LeftIons);
        //                end
        //    %             sequence = Matches{index,1}.Sequence;
        //                Matches{index,1}.Blind.Start = left;
        //                Matches{index,1}.Blind.End = right;
        //                Matches{index,1}.Blind.Mass = massDiff;
            
        //                Matches{index}.MolW = Matches{index}.MolW + massDiff;
            
        //                ERROR=abs(Experimental_Protein_Mass-Matches{index}.MolW);
        //                if(ERROR==0)
        //                    MW_score=1;   %diff=0 mw_score=1
        //                else
        //                    MW_score=1/2^(ERROR);% compute mol_weight Score for proteins via mw_score=10/|diff|
        //                end
        //                Matches{index}.MWScore = MW_score;%store protein's
        //            else
        //                ERROR=abs(Experimental_Protein_Mass-Matches{index}.MolW);
        //                if(ERROR==0)
        //                    MW_score=1;   %diff=0 mw_score=1
        //                else
        //                    MW_score=1/2^(ERROR);% compute mol_weight Score for proteins via mw_score=10/|diff|
        //                end
        //                Matches{index}.MWScore = MW_score;%store protein's
        //            end
        //   */
        //    }
        //}

    }
}
