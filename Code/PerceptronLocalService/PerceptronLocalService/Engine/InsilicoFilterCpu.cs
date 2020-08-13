using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    public class InsilicoFilterCpu : IInsilicoFilter
    {

        public List<ProteinDto> ComputeInsilicoScore(List<ProteinDto> proteinList, List<newMsPeaksDto> OriginalpeakData2DList, double tol, string pepUnit)
        {
            var CandidateProteinswithInsilicoScores = new List<ProteinDto>();
            var peakData2DList = new List<newMsPeaksDto>();     // Lists are referenced based therefore, making a copy of new Peak List
            //tol = 15;
            //var pepUnit = "ppm";
            //int delme; var delmeList = new List<ProteinDto>();

            for (int index = 0; index < OriginalpeakData2DList.Count; index++)
            {
                if (OriginalpeakData2DList[index].Intensity < 0.000092)
                {
                    var temp = new newMsPeaksDto(OriginalpeakData2DList[index].Mass, 0.001);
                    peakData2DList.Add(temp);    // Lists are referenced based therefore, making a copy of new Peak List with modified intensities that will be used in this algo only
                }

                else
                {
                    var temp = new newMsPeaksDto(OriginalpeakData2DList[index].Mass, 1);
                    peakData2DList.Add(temp);    // Lists are referenced based therefore, making a copy of new Peak List with modified intensities that will be used in this algo only
                }
                    
            }
            if (peakData2DList.Count > 16)//16) //Just to avoid small data. Because its hardly possible to get Spectral Matches with small peak Count
            {
                for (int indexProteinList = 0; indexProteinList < proteinList.Count; indexProteinList++)//foreach (var protein in proteinList) //Run Loop on Candidate Protein List (proteinList)
                {
                    //if (proteinList[indexProteinList].Header == "A6NDN8")
                    {


                        //    delme = 0;

                        var insilico = proteinList[indexProteinList].InsilicoDetails; //Insilico Detail of Specific Protein(according to indexProteinList)

                        double Matches_Score = 0;  // Variable is Reference Type //double[] Matches_Score = new double[] { 0 };
                        int MatchCounter = 0; // Variable is Reference Type // int[] MatchCounter = new int[] { 0 };  
                        var LeftMatched_Index = new List<int>();
                        var RightMatched_Index = new List<int>();
                        var LeftPeak_Index = new List<int>();
                        var RightPeak_Index = new List<int>();

                        var Left = new List<string>();
                        var Right = new List<string>();

                        int IdxL = 0;
                        int IdxR = 0;
                        var LeftType = new List<string>();
                        var RightType = new List<string>();

                        string Type = "";

                        int SpecialLeftFragments = insilico.InsilicoMassLeftAo.Count + insilico.InsilicoMassLeftBo.Count + insilico.InsilicoMassLeftAstar.Count + insilico.InsilicoMassLeftBstar.Count;
                        int SpecialRightFragments = insilico.InsilicoMassRightYo.Count + insilico.InsilicoMassRightYstar.Count + insilico.InsilicoMassRightZo.Count + insilico.InsilicoMassRightZoo.Count;

                        //For Finding consecutive region & Variables are Reference Type
                        int Counter = 0; // 
                        int OldConsec = 0;
                        int OldConsec2 = 0;
                        int ConsecutiveRegion = 0;

                        for (int indexPeakList = 1; indexPeakList < peakData2DList.Count; indexPeakList++)//Starts from One!!! Do not interested in Intact Protein Mass  /////EXPERIMENTAL PEAK LIST
                        {
                            double peakDifferenceTolerance = ComputeDifferenceThreshold(tol, pepUnit, peakData2DList[indexPeakList].Mass); //peakList[0]
                            //if (proteinList[indexProteinList].Header == "P02652" && indexPeakList == 31)
                            //    delme = 0;

                            int Consecutive = indexPeakList;
                            ////#FORTHETIMEBEING: Updated 20200115 COMMENTED: PREVIOUSLY Removing Last Entry(MW of Protein - Water). So, now Just For Fragments Now Added: -1
                            ////#Update: Updated 20200202:  "-1" is Removed and now its just ".Count"
                            for (int indexLeftSide = IdxL; indexLeftSide < insilico.InsilicoMassLeft.Count; indexLeftSide++)
                            {
                                Type = "Left";
                                double difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassLeft[indexLeftSide];
                                //Check in Left Ions
                                SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType);

                                if (SpecialLeftFragments > 0)
                                {
                                    if (insilico.InsilicoMassLeftAo.Count > 0)
                                    {
                                        Type = "A'";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassLeftAo[indexLeftSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType);
                                    }
                                    if (insilico.InsilicoMassLeftBo.Count > 0)
                                    {
                                        Type = "B'";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassLeftBo[indexLeftSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType);
                                    }
                                    if (insilico.InsilicoMassLeftAstar.Count > 0)
                                    {
                                        Type = "A*";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassLeftAstar[indexLeftSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType);
                                    }
                                    if (insilico.InsilicoMassLeftBstar.Count > 0)
                                    {
                                        Type = "B*";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassLeftBstar[indexLeftSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType);
                                    }
                                }
                                if (difference < -peakDifferenceTolerance && indexLeftSide > 1)
                                {
                                    IdxL = indexLeftSide - 1;
                                    break;
                                }
                            }
                            //#FORTHETIMEBEING: Updated 20200115 COMMENTED: PREVIOUSLY Removing Last Entry(MW of Protein - Water). So, now Just For Fragments Now Added: -1
                            ////#Update: Updated 20200202:  "-1" is Removed and now its just ".Count"
                            for (int indexRightSide = IdxR; indexRightSide < insilico.InsilicoMassRight.Count; indexRightSide++)
                            {
                                ///Check in Right Ions
                                double difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassRight[indexRightSide];
                                Type = "Right";
                                SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType);
                                if (SpecialRightFragments > 0)
                                {
                                    if (insilico.InsilicoMassRightYo.Count > 0)
                                    {
                                        Type = "Y'";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassRightYo[indexRightSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType);
                                    }
                                    if (insilico.InsilicoMassRightZo.Count > 0)
                                    {
                                        Type = "Z'";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassRightZo[indexRightSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType);
                                    }
                                    if (insilico.InsilicoMassRightZoo.Count > 0)
                                    {
                                        Type = "Z''";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassRightZoo[indexRightSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType);
                                    }
                                    if (insilico.InsilicoMassRightYstar.Count > 0)
                                    {
                                        Type = "Y*";
                                        difference = peakData2DList[indexPeakList].Mass - insilico.InsilicoMassRightYstar[indexRightSide];
                                        SpectralComparison(difference, peakData2DList[indexPeakList], indexPeakList, peakDifferenceTolerance, ref Consecutive, ref Counter, ref OldConsec, ref OldConsec2, ref ConsecutiveRegion, ref Matches_Score, ref MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType);
                                    }
                                }
                                if (difference < -peakDifferenceTolerance && indexRightSide > 1)
                                {
                                    IdxR = indexRightSide - 1;
                                    break;
                                }
                            }
                        }


                        proteinList[indexProteinList].InsilicoScore = Matches_Score / peakData2DList.Count;
                        proteinList[indexProteinList].MatchCounter = MatchCounter;

                        proteinList[indexProteinList].LeftMatchedIndex = LeftMatched_Index;
                        proteinList[indexProteinList].LeftPeakIndex = LeftPeak_Index;
                        proteinList[indexProteinList].LeftType = LeftType;

                        proteinList[indexProteinList].RightMatchedIndex = RightMatched_Index;
                        proteinList[indexProteinList].RightPeakIndex = RightPeak_Index;
                        proteinList[indexProteinList].RightType = RightType;
                    }
                    //experimentalPeakIndex = 0;
                    //theoreticalPeakIndex = insilico.InsilicoMassRight.Count - 1;
                    //peakDifferenceTolerance = ComputeDifferenceThreshold(tol, pepUnit, peakData2DList[0].Mass);
                }
            }
            for (int MatchIndex = 0; MatchIndex < proteinList.Count; MatchIndex++)
            {
                if (proteinList[MatchIndex].MatchCounter > 0)
                {
                    CandidateProteinswithInsilicoScores.Add(proteinList[MatchIndex]);
                }
            }

            return CandidateProteinswithInsilicoScores;
        }

        private void SpectralComparison(double difference, newMsPeaksDto OnepeakData2DList, int indexPeakList, double peakDifferenceTolerance, ref int Consecutive, ref int Counter, ref int OldConsec, ref int OldConsec2, ref int ConsecutiveRegion, ref double Matches_Score, ref int MatchCounter, List<int> Matched_IndexList, List<int> Peak_IndexList, int indexSide, string Type, List<string> TypeList) // Matched_Index == LeftMatched_Index OR RightMatched_Index  /// Peak_IndexList ==  Peak List Index
        {
            double absdifference = Math.Abs(difference);  //Taking Absoulte difference {Doesn't matter}

            if (absdifference <= peakDifferenceTolerance)
            {
                if (Consecutive == OldConsec + 1 && OldConsec == OldConsec2 + 1)
                {
                    if (Counter == 0)
                    {
                        ConsecutiveRegion = ConsecutiveRegion + 1;
                    }

                    Counter = Counter + 1;
                    Matches_Score = Matches_Score + 1.5;
                    OldConsec2 = OldConsec;
                    OldConsec = Consecutive;
                }
                else if (Consecutive == OldConsec && OldConsec == OldConsec2 + 1)
                {
                    Counter = Counter + 1;
                    Matches_Score = Matches_Score + 1.5;
                }
                else
                {
                    Counter = 0;
                    Matches_Score = Matches_Score + OnepeakData2DList.Intensity;
                    OldConsec2 = OldConsec;
                    OldConsec = Consecutive;
                }
                // Only Update Score
                Matched_IndexList.Add(indexSide);     // Left or Right Peak Index 
                Peak_IndexList.Add(indexPeakList);   // Peak List Index
                TypeList.Add(Type);
                //Matched_IndexList.Add(indexPeakList);
                /////////LeftType = [LeftType; {'Left'} ];
                MatchCounter = MatchCounter + 1;
            }
        }

        private static double ComputeDifferenceThreshold(double tol, string pepUnit, double peak)
        {
            switch (pepUnit)
            {
                case "Da":
                case "mmu":
                    return tol;
                case "ppm":
                    return (tol * peak) / 1000000;
                default: //No Need
                    return (tol * peak) / 100;
            }

        }
    }
}