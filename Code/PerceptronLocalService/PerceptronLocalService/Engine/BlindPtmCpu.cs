using System;
using System.Collections.Generic;
using System.Diagnostics;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public static class BlindPtmCpu
    {
        public static void BlindPTM(List<double> experimentalSpectrum, double molW, List<ProteinDto> candidateProteinsList, double pepTol, double userHopThreshold, string pepUnit)
        {
            var stopwatch = new Stopwatch();

            // Data Preperation
            stopwatch.Start();
            var peaks = new List<double>();
            foreach (var peak in experimentalSpectrum)
            {
                peaks.Add(peak + 1.00727647);
                peaks.Add(molW - (peak + 1.00727647));
                //peaks.Add(peak);
                //peaks.Add(molW - (peak));
            }
            peaks.Sort();
            stopwatch.Stop();
            Console.WriteLine("Data Preperation: "+ stopwatch.Elapsed);
            
            // PTM Extraction
            stopwatch.Restart();
            var aminoAcidList = new List<string>();
            var modificationList = new List<string>();
            var startList = new List<double>();
            var endList = new List<double>();
            for (var expI = 0; expI < peaks.Count; expI++)
            {
                for (var expJ = expI + 1; expJ < peaks.Count; expJ++)
                {
                    var peakDiff = peaks[expJ] - peaks[expI];
                    var modification = AminoAcids.GetModifiedAminoAcid(peakDiff, userHopThreshold);
                    foreach (var mod in modification)
                    {
                        var temproray = mod.Split('_');
                        if (temproray.Length <= 1) continue;
                        aminoAcidList.Add(temproray[1]);
                        modificationList.Add(temproray[0]);
                        startList.Add(peaks[expI]);
                        endList.Add(peaks[expJ]);
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Generation: " + stopwatch.Elapsed);
            
            // PTM Shortlisting
            stopwatch.Restart();
            foreach (var protein in candidateProteinsList)
            {
                var sequence = protein.Sequence;
                var hopIndex = 0;
                var thrI = 0;
                var shortlistedAminoAcid = new List<string>();
                var shortlistedModification = new List<string>();
                var shortlistedEnd = new List<double>();
                var shortlistedStart = new List<double>();
                var shortlistedIndex = new List<int>();
                
                while (true)
                {
                    try
                    {
                        if (startList.Count > 0)
                        {
                            if (shortlistedStart.Count > 0)
                                if (shortlistedEnd[shortlistedEnd.Count-1] > startList[hopIndex])
                                {
                                    hopIndex = hopIndex + 1;
                                    if (hopIndex == startList.Count)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                            var diff = startList[hopIndex] - protein.InsilicoDetails.InsilicoMassLeft[thrI];
                            if (diff <= userHopThreshold && diff >= -userHopThreshold)
                            {
                                if (aminoAcidList[hopIndex] == sequence[thrI+2].ToString())
                                {
                                    var modMass = AminoAcids.ModificationTable(modificationList[hopIndex]);
                                    diff =
                                        Math.Abs(endList[hopIndex] -
                                                 (protein.InsilicoDetails.InsilicoMassLeft[thrI + 1
                                                     ] +
                                                  modMass));
                                    if (string.Compare(pepUnit, "ppm", StringComparison.Ordinal) == 0)
                                        diff = (diff / molW) * 1000000;
                                    else if (string.Compare(pepUnit, "%", StringComparison.Ordinal) == 0)
                                        diff = (diff / molW) * 100;
                                    if (diff < pepTol)
                                    {
                                        for (var i = thrI + 1;
                                            i < protein.InsilicoDetails.InsilicoMassLeft.Count;
                                            i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] =
                                                protein.InsilicoDetails.InsilicoMassLeft[i] +
                                                modMass;
                                        protein.Mw = protein.Mw + modMass;
                                        shortlistedAminoAcid.Add(aminoAcidList[hopIndex]);
                                        shortlistedModification.Add(modificationList[hopIndex]);
                                        shortlistedEnd.Add(endList[hopIndex]);
                                        shortlistedStart.Add(startList[hopIndex]);
                                        shortlistedIndex.Add(thrI);
                                    }
                                }
                            }
                            else if (diff > userHopThreshold)
                            {
                                thrI = thrI + 1;
                                if (thrI == protein.InsilicoDetails.InsilicoMassLeft.Count - 1 )
                                    break;
                                continue;
                            }
                            else if (diff < -userHopThreshold)
                            {
                                hopIndex = hopIndex + 1;
                                if (hopIndex == startList.Count)
                                    break;
                                continue;
                            }
                            hopIndex = hopIndex + 1;
                            if (hopIndex == startList.Count)
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                }
                for (var hopIter = 0; hopIter < shortlistedStart.Count; hopIter++)
                {
                    var site = new PostTranslationModificationsSiteDto
                    {
                        Index = shortlistedIndex[hopIter],
                        ModName = shortlistedModification[hopIter],
                        ModWeight = AminoAcids.ModificationTable(shortlistedModification[hopIter]),
                        Site = Convert.ToChar(shortlistedAminoAcid[hopIter])
                    };
                    protein.PtmParticulars.Add(site);
                }
                var massError = Math.Abs(molW - protein.Mw);
                protein.MwScore = Math.Abs(massError) < 0 ? 1 : Math.Pow(massError, 0.5);
            }
            stopwatch.Stop();
            Console.WriteLine("Shortlisting :" + stopwatch.Elapsed);
        }

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
