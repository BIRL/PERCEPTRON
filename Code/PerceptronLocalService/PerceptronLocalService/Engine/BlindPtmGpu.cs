using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public static class BlindPtmGpu
    {
        private const int N = 1024;
        private static readonly double[] ModificationMass = BlindPtmUtility.ReturnModificationMass();
        private static readonly string[] ModificationName = BlindPtmUtility.ReturnModificationNames();
        private static readonly char[] ModificationAminoAcids = BlindPtmUtility.ReturnModificationTags();

        public static void BlindPTM(List<double> experimentalSpectrum, double molW, List<ProteinDto> candidateProteinsList,
            double pepTol, double userHopThreshold, string pepUnit)
        {
            var stopwatch = new Stopwatch();
            
            // Data Preperation and Loading GPU Module
            stopwatch.Start();
            var peaks = new List<double>();
            var aminoAcidList = new List<string>();
            var modificationList = new List<string>();
            var startList = new List<double>();
            var endList = new List<double>();
            foreach (var peak in experimentalSpectrum)
            {
                peaks.Add(peak + 1.00727647);
                peaks.Add(molW - (peak + 1.00727647));
                //peaks.Add(peak);
                //peaks.Add(molW - (peak));
            }
            peaks.Sort();
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy();
                km.Serialize();
            }
            gpu.LoadModule(km);
            stopwatch.Stop();
            Console.WriteLine("Data Preperation: " + stopwatch.Elapsed);

            // GPU Module
            stopwatch.Restart();
            var lengthSquared = peaks.Count*peaks.Count;
            var peaksArray = peaks.ToArray();
            var lengthOfPeakList = new int[1];
            lengthOfPeakList[0] = peaks.Count;
            var outputArray = new char[peaks.Count, peaks.Count, 37];
            var errorArray = new double[peaks.Count, peaks.Count, 37];
            var modMassList = ModificationMass;
            char[,,] outputArrayDevice = gpu.Allocate(outputArray);
            double[,,] errorArrayDevice = gpu.Allocate(errorArray);
            double[] peaksDevice = gpu.Allocate<double>(peaksArray.Length);
            int[] lengthOfPeakListDevice = gpu.Allocate<int>(lengthOfPeakList.Length);
            double[] ptmMassListDevice = gpu.Allocate<double>(modMassList.Length);
            gpu.CopyToDevice(peaksArray, peaksDevice);
            gpu.CopyToDevice(lengthOfPeakList, lengthOfPeakListDevice);
            gpu.CopyToDevice(ModificationMass, ptmMassListDevice);
            int block = (int) Math.Ceiling((double) lengthSquared*37/N);
            gpu.Launch(block, N).PtmExtractor(peaksDevice, lengthOfPeakListDevice, ptmMassListDevice, outputArrayDevice,
                    errorArrayDevice);
            gpu.CopyFromDevice(outputArrayDevice, outputArray);
            gpu.CopyFromDevice(errorArrayDevice, errorArray);
            gpu.FreeAll();

            for (var i = 0; i < peaks.Count; i++)
            {
                for (var j = 0; j < peaks.Count; j++)
                {
                    for (var k = 0; k < 37; k++)
                    {
                        if (outputArray[i, j, k] == '\0') continue;
                        aminoAcidList.Add(ModificationAminoAcids[outputArray[i, j, k]].ToString());
                        modificationList.Add(ModificationName[outputArray[i, j, k]]);
                        startList.Add(peaks[i]);
                        endList.Add(peaks[j]);
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("GPU Generation: " + stopwatch.Elapsed);

            // PTM Shortlisting
            stopwatch.Restart();
            foreach (var protein in candidateProteinsList)
            {
                var sequence = protein.Sequence.ToCharArray();
                var hopI = 0;
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
                                if (shortlistedEnd[shortlistedEnd.Count - 1] > startList[hopI])
                                {
                                    hopI = hopI + 1;
                                    if (hopI == startList.Count)
                                    {
                                        break;
                                    }
                                    continue;
                                }
                            var diff = startList[hopI] - protein.InsilicoDetails.InsilicoMassLeft[thrI];
                            if (diff <= userHopThreshold && diff >= -userHopThreshold)
                            {
                                if (aminoAcidList[hopI] == sequence[thrI + 2].ToString())
                                {
                                    var temproray = modificationList[hopI].Split('_');
                                    var modMass = AminoAcids.ModificationTable(temproray[0]);
                                    //var modMass = AminoAcids.ModTable(modificationList[hopI]);
                                    diff =
                                        Math.Abs(endList[hopI] -
                                                 (protein.InsilicoDetails.InsilicoMassLeft[thrI + 1
                                                     ] +
                                                  modMass));
                                    if (string.Compare(pepUnit, "ppm", StringComparison.Ordinal) == 0)
                                        diff = (diff/molW)*1000000;
                                    else if (string.Compare(pepUnit, "%", StringComparison.Ordinal) == 0)
                                        diff = (diff/molW)*100;
                                    if (diff < pepTol)
                                    {
                                        for (var i = thrI + 1;
                                            i < protein.InsilicoDetails.InsilicoMassLeft.Count;
                                            i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] =
                                                protein.InsilicoDetails.InsilicoMassLeft[i] +
                                                modMass;
                                        protein.Mw = protein.Mw + modMass;
                                        shortlistedAminoAcid.Add(aminoAcidList[hopI]);
                                        shortlistedModification.Add(modificationList[hopI]);
                                        shortlistedEnd.Add(endList[hopI]);
                                        shortlistedStart.Add(startList[hopI]);
                                        shortlistedIndex.Add(thrI);
                                    }
                                }
                            }
                            else if (diff > userHopThreshold)
                            {
                                thrI = thrI + 1;
                                if (thrI == protein.InsilicoDetails.InsilicoMassLeft.Count - 1)
                                    break;
                                continue;
                            }
                            else if (diff < -userHopThreshold)
                            {
                                hopI = hopI + 1;
                                if (hopI == startList.Count)
                                    break;
                                continue;
                            }
                            hopI = hopI + 1;
                            if (hopI == startList.Count)
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.Message);
                    }
                }
                for (var hopIndex = 0; hopIndex < shortlistedStart.Count; hopIndex++)
                {
                    var site = new PostTranslationModificationsSiteDto
                    {
                        Index = shortlistedIndex[hopIndex],
                        ModName = shortlistedModification[hopIndex],
                        ModWeight = AminoAcids.ModificationTable(shortlistedModification[hopIndex]),
                        Site = Convert.ToChar(shortlistedAminoAcid[hopIndex])
                    };
                    protein.PtmParticulars.Add(site);
                }
                var massError = Math.Abs(molW - protein.Mw);
                protein.MwScore = Math.Abs(massError) < 0 ? 1 : Math.Pow(massError, 0.5);
            }
            stopwatch.Stop();
            Console.WriteLine("Shortlisting :" + stopwatch.Elapsed);
        }

        [Cudafy]
        // ReSharper disable once UnusedMember.Local
        private static void PtmExtractor(GThread thread, double[] peaks, int[] lengthOfPeakList, double[] ptmMassList,
            char[,,] outputArray, double[,,] errorsArray)
        {
            int tid = thread.threadIdx.x + thread.blockIdx.x*thread.blockDim.x;
            int s = tid%lengthOfPeakList[0];
            int e = s + 1 + tid/(lengthOfPeakList[0]*37);
            int d = (tid/lengthOfPeakList[0])%37;
            if (e < lengthOfPeakList[0])
            {
                double peakDifference = peaks[e] - peaks[s];
                if (peakDifference < 0)
                    peakDifference *= -1;
                double error = peakDifference - ptmMassList[d];
                if (error < 0)
                    error *= -1;
                if (!(error < 1)) return;
                outputArray[s, e, d] = (char) d;
                errorsArray[s, e, d] = error;
            }
        }
    }
}
