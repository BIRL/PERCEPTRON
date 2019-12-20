using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    public class InsilicoFilterCpu : IInsilicoFilter

    {
        
        public  void Insilico_filter1(List<ProteinDto> proteinList, List<double> peakList, double tol)
        {
            tol = 15;
            var pepUnit = "ppm";

            foreach (var protein in proteinList)
            {
                double insilicoScore = 0;
                var insilico = protein.InsilicoDetails;
                foreach (var experimentalPeakMass in peakList)
                {
                    var peakDifferenceTolerance = ComputeDifferenceThreshold(tol, pepUnit, experimentalPeakMass);

                    for (var insilicoIndex = 0; insilicoIndex < insilico.InsilicoMassLeft.Count; insilicoIndex++)
                    {
                        //left
                        var diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassLeft[insilicoIndex]);
                        if (diff <= peakDifferenceTolerance)
                        insilicoScore++;

                        //Right
                        diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassRight[insilicoIndex]);
                         if (diff <= peakDifferenceTolerance)
                            insilicoScore++;

                        //ao
                         if (insilico.InsilicoMassLeftAo.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassLeftAo[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //bo
                        if (insilico.InsilicoMassLeftBo.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassLeftBo[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //yo
                        if (insilico.InsilicoMassRightYo.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassRightYo[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //zo
                        if (insilico.InsilicoMassRightZo.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassRightZo[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //zoo
                        if (insilico.InsilicoMassRightZoo.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassRightZoo[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //a*
                        if (insilico.InsilicoMassLeftAstar.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassLeftAstar[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //b*
                        if (insilico.InsilicoMassLeftBstar.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassLeftBstar[insilicoIndex]);
                             if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }

                        //y*
                        if (insilico.InsilicoMassRightYstar.Any())
                        {
                            diff = Math.Abs(experimentalPeakMass - insilico.InsilicoMassRightYstar[insilicoIndex]);
                            if (diff <= peakDifferenceTolerance)
                                insilicoScore++;
                        }
                    }
                }


                insilicoScore = insilicoScore/peakList.Count;
                insilicoScore = insilicoScore*Math.Pow(2, 1 - insilicoScore);
                protein.InsilicoScore = insilicoScore;
                protein.InsilicoDetails = insilico;

            }
        }

        public void ComputeInsilicoScore(List<ProteinDto> proteinList, List<double> peakList, double tol)
        {
            tol = 15;
            var pepUnit = "ppm";

            foreach (var protein in proteinList)
            {
                double insilicoScore = 0;
                var insilico = protein.InsilicoDetails;

                var experimentalPeakIndex = 0;
                var theoreticalPeakIndex = 0;
                var peakDifferenceTolerance = ComputeDifferenceThreshold(tol, pepUnit, peakList[0]);

                while (experimentalPeakIndex < peakList.Count && theoreticalPeakIndex < insilico.InsilicoMassLeft.Count)
                {
                    //left
                    var diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeft[theoreticalPeakIndex]);
                    if (diff <= peakDifferenceTolerance)
                    {
                        insilicoScore++;
                        //experimentalPeakIndex++;
                        break;
                    }

                    //ao
                    if (insilico.InsilicoMassLeftAo.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeftAo[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    //bo
                    if (insilico.InsilicoMassLeftBo.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeftBo[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    //a*
                    if (insilico.InsilicoMassLeftAstar.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeftAstar[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    //b*
                    if (insilico.InsilicoMassLeftBstar.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeftBstar[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    if (peakList[experimentalPeakIndex]  >
                        insilico.InsilicoMassLeft[theoreticalPeakIndex] + peakDifferenceTolerance)
                    {
                        theoreticalPeakIndex++;
                    }
                    else
                    {
                        experimentalPeakIndex++;
                    }
                }

                experimentalPeakIndex = 0;
                theoreticalPeakIndex = insilico.InsilicoMassRight.Count-1;
                peakDifferenceTolerance = ComputeDifferenceThreshold(tol, pepUnit, peakList[0]);

                while (experimentalPeakIndex < peakList.Count && theoreticalPeakIndex >= 0)
                {
                    //Right
                    var diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassLeft[theoreticalPeakIndex]);
                    if (diff <= peakDifferenceTolerance)
                        insilicoScore++;

                    //yo
                    if (insilico.InsilicoMassRightYo.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassRightYo[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    //zo
                    if (insilico.InsilicoMassRightZo.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassRightZo[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    //zoo
                    if (insilico.InsilicoMassRightZoo.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassRightZoo[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }               

                    //y*
                    if (insilico.InsilicoMassRightYstar.Any())
                    {
                        diff = Math.Abs(peakList[experimentalPeakIndex] - insilico.InsilicoMassRightYstar[theoreticalPeakIndex]);
                        if (diff <= peakDifferenceTolerance)
                            insilicoScore++;
                    }

                    if (peakList[experimentalPeakIndex] >
                        insilico.InsilicoMassLeft[theoreticalPeakIndex] + peakDifferenceTolerance)
                    {
                        theoreticalPeakIndex--;
                    }
                    else
                    {
                        experimentalPeakIndex++;
                    }
                }               

                insilicoScore = insilicoScore / peakList.Count;
                insilicoScore = insilicoScore * Math.Pow(2, 1 - insilicoScore);
                protein.InsilicoScore = insilicoScore;
                protein.InsilicoDetails = insilico;

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
                default:
                    return (tol * peak) / 100;
            }

        }
    }
}