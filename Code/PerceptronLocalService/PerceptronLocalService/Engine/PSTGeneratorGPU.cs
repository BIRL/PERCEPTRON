using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public class PstGeneratorGpu : IPeptideSequenceTagGenerator
    {
        private const int ThreadsPerBlock = 1024;
        private const int AminoAcidAlphabetSize = 26;
        private static char[] _aminoAcids = new char[AminoAcidAlphabetSize];
        private static char[,] _pstMatrix;
        private static double[,] _pstErrorMatrix;
        private static double[] _peakIntensities;
        private static double[,] _pstIntensityMatix;
        private static int _peakCount;

        public List<PstTagList> GeneratePeptideSequenceTags(SearchParametersDto parameters, MsPeaksDto peakData)
        {
            //Converting Peak List Data into 2D List
            var peakDatalist = new List<peakData2Dlist>();  //Making a 2D list(peakDatalist) in which Mass & Intensity includes {{FARHAN!! ITS NOT AN EFFICIENT WAY}} NOt NEEDED THIS IF ONCE MsPeaksDto.cs is modified
            for (int row = 0; row <= peakData.Mass.Count - 1; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);

            }
            var peakDatalistsort = peakDatalist.OrderBy(n => n.Mass).ToArray();
            double[] MassPeakData = peakDatalistsort.Select(n => n.Mass).ToArray();
            double[] IntensityPeakData = peakDatalistsort.Select(n => n.Intensity).ToArray();
            var pstList = new List<NewPstTagsDtoGpu>();
            GeneratePstGpu(MassPeakData, pstList, parameters.HopThreshhold, IntensityPeakData);
            //if (IsFilterPstByLength(parameters))
            //    pstList = pstList.Where(t => t.AminoAcidTag.Length >= parameters.MinimumPstLength && t.AminoAcidTag.Length <= parameters.MaximumPstLength).ToList();

            //FilterTagsWithMultipleOccurences(pstList);


            ////////////var PstTagListGpu = AccomodateIsoforms(pstList, parameters);
            //AccomodateIsoforms(pstList);


            //return pstList;
            var pstList2 = new List<PstTagList>();
            return pstList2;
        }
        private static void GeneratePstGpu(double[] MassPeakData, List<NewPstTagsDtoGpu> pstList, double pstHopTolerance, double[] IntensityPeakData)
        {
            _aminoAcids = GetAminoAcidCharacters();
            _peakCount = MassPeakData.Length;
            
            // For Finding Pst Tags and Error Matrix
            // GPU Initializations
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            eArchitecture architecture = gpu.GetArchitecture();
            CudafyModule cudafyModule = CudafyTranslator.Cudafy(architecture);
            gpu.LoadModule(cudafyModule);

            var aminoAcidMasses = GetAminoAcidMasses();// Does not contain modifications !!!!!!!!!!!!!!!!!

            // GPU variable initalizations
            int numberOfThreads = MassPeakData.Length * MassPeakData.Length * AminoAcidAlphabetSize;
            double[] peaksHost = MassPeakData;                                      //Mass
            //double[] IntensityDataHost = IntensityPeakData;                         //Intensity
            

            int[] peakCountHost = { MassPeakData.Length };
            _pstMatrix = new char[MassPeakData.Length, MassPeakData.Length];
            _pstErrorMatrix = new double[MassPeakData.Length, MassPeakData.Length];
            _pstIntensityMatix = new double[MassPeakData.Length, MassPeakData.Length];
            
            double[] pstHopToleranceHost = { pstHopTolerance };
            double[] peaksDevice = gpu.Allocate<double>(peaksHost);
            //double[] PeakIntensityDevice = gpu.Allocate<double>(IntensityDataHost);
            
            int[] peakCountDevice = gpu.Allocate<int>(peakCountHost);  //Peak Count
            double[] aminoAcidMassesDevice = gpu.Allocate<double>(aminoAcidMasses);
            double[] pstHopToleranceDevice = gpu.Allocate<double>(pstHopToleranceHost);
            char[,] pstMatrixDevice = gpu.Allocate<char>(_pstMatrix);
            double[,] pstErrorMatrixDevice = gpu.Allocate<double>(_pstErrorMatrix);
            //double[,] pstIntensityDevice = gpu.Allocate<double>(_pstIntensityMatix);
                   ///// Intensity
            // GPU variable data Preperation
            gpu.CopyToDevice(peaksHost, peaksDevice);
            gpu.CopyToDevice(peakCountHost, peakCountDevice);
            gpu.CopyToDevice(aminoAcidMasses, aminoAcidMassesDevice);
            gpu.CopyToDevice(pstHopToleranceHost, pstHopToleranceDevice);
            gpu.CopyToDevice(_pstMatrix, pstMatrixDevice);
            gpu.CopyToDevice(_pstErrorMatrix, pstErrorMatrixDevice);
            //gpu.CopyToDevice(_pstIntensityMatix, pstIntensityDevice);
            
            // Launching GPU Code
            int block = (int)Math.Ceiling((double)numberOfThreads / ThreadsPerBlock);
            //gpu.Launch(block, ThreadsPerBlock).PstGenerationKernel(peaksDevice, peakCountDevice, aminoAcidMassesDevice, pstHopToleranceDevice, pstMatrixDevice, pstErrorMatrixDevice); //, pstIntensityDevice
            gpu.Launch(block, ThreadsPerBlock).PstGenerationKernel(peaksDevice, peakCountDevice, aminoAcidMassesDevice, pstHopToleranceDevice, pstMatrixDevice, pstErrorMatrixDevice);  //    , PeakIntensityDevice, pstIntensityDevice
            gpu.CopyFromDevice(pstMatrixDevice, _pstMatrix);
            gpu.CopyFromDevice(pstErrorMatrixDevice, _pstErrorMatrix);
            //gpu.CopyFromDevice(pstIntensityDevice, _pstIntensityMatix);
               ///// Intensity
            gpu.FreeAll();


            ////EQUATION6: Average Intensity(intensity of pst) = intensityhomepeak + intensityhoppeak)/2
            //So, want to add Intensities till now didn't know how to use this GPU function (PstGenerationKernel) so, that I will also get Average Intensites
            //Therefore, separately doing this.. I think so, its some issue regarding GPU ALLOCATION
            // GPU variable initalizations
            var monoisotopicIntensityPeaks = IntensityPeakData;
            var intensitypeakListLength = new int[1];
            intensitypeakListLength[0] = IntensityPeakData.Length;
            var PeakIntensityMatrix = new double[IntensityPeakData.Length, IntensityPeakData.Length];
            
            // GPU variable data Preperation
            var monoisotopicIntensityPeaksDevice = gpu.Allocate(IntensityPeakData);
            var intensitypeakListLengthDevice = gpu.Allocate(intensitypeakListLength); //intensitypeakListLengthDevice
            var PeakIntensityDevice = gpu.Allocate(PeakIntensityMatrix); //intensityoutputDevice
            gpu.CopyToDevice(IntensityPeakData, monoisotopicIntensityPeaksDevice);
            gpu.CopyToDevice(intensitypeakListLength, intensitypeakListLengthDevice);
            gpu.CopyToDevice(PeakIntensityMatrix, PeakIntensityDevice);

            // Launching GPU Code
            var numOfBlocksforintensity = (int)Math.Ceiling((double)numberOfThreads / ThreadsPerBlock);
            //gpu.Launch(numOfBlocksforintensity, ThreadsPerBlock).DeviceGenerateFragmentPairMatrix(monoisotopicIntensityPeaksDevice, intensitypeakListLengthDevice, intensityoutputDevice);

            gpu.Launch(numOfBlocksforintensity, ThreadsPerBlock).GpuGeneratedAverageIntensities(monoisotopicIntensityPeaksDevice, intensitypeakListLengthDevice, PeakIntensityDevice);
            gpu.CopyFromDevice(PeakIntensityDevice, PeakIntensityMatrix);
            gpu.FreeAll();

            var asd = _pstErrorMatrix;
            var qwe = _pstMatrix;
            var wqeer = PeakIntensityMatrix;

            for (var i = 0; i < MassPeakData.Length; i++)
            {
                //List<double> ErrorList = new List<double>();
                //List<double> PstTagIntensityList = new List<double>();
                int a = i;
                GPU_recursor(pstList, 0, 0, "", i);
            }

        }

        private static void GPU_recursor(List<NewPstTagsDtoGpu> pstList, double Error, double PstTagIntensity, string PstTag, int row)
        {
            //sumIntensity += _peakIntensities[row];
            if (PstTag.Length >= 1)
                pstList.Add(new NewPstTagsDtoGpu(PstTag,PstTag.Length, Error, PstTagIntensity));
            for (int j = 0; j < _peakCount; j++)
            {
                if (_pstMatrix[row, j] != '\0')// startIndex ==  EndIndex
                {
                    GPU_recursor(pstList, (Error + _pstErrorMatrix[row, j]), PstTagIntensity, PstTag + _aminoAcids[_pstMatrix[row, j]], j); //Math.Pow(_pstErrorMatrix[row, j],2)
                }
            }
        }

        [Cudafy]
        private static void PstGenerationKernel(GThread thread, double[] peaks, int[] peakListCountArray, double[] aminoAcidMasses, double[] hopTolerance, char[,] pstMatchMatrix, double[,] errors, double[,] AveragePstIntensity) //, double[] PeakIntensityDevice, double[,] AveragePstIntensity
        {
            int peakListCount = peakListCountArray[0];
            int tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int startIndex = tid % peakListCount;
            int endIndex = tid / (peakListCount * 26);
            int aminoAcidIndex = (tid / peakListCount) % 26;

            if (endIndex < peakListCount && endIndex > startIndex)
            {
                double peakDifference = peaks[endIndex] - peaks[startIndex];   /// 5 - 3   = 2    /// 3 - 5
                if (peakDifference < 0)
                    peakDifference *= -1;
                double error = peakDifference - aminoAcidMasses[aminoAcidIndex];
                if (error < 0)
                    error *= -1;
                if (error < hopTolerance[0])
                {
                    pstMatchMatrix[startIndex, endIndex] = (char)aminoAcidIndex;
                    errors[startIndex, endIndex] = error;   // Here we have absolute error which does not matter {Because we will take Square in coming steps (like Eq4)}
                }
            }
        }
        //private static void GpuGeneratedAverageIntensities(GThread thread, double[] peaks, int[] peakListCountArray, double[] aminoAcidMasses, double[] hopTolerance, char[,] pstMatchMatrix, double[,] errors, double[,] AveragePstIntensity)
        [Cudafy]
        private static void GpuGeneratedAverageIntensities(GThread thread, double[] IntensityPeakDataDevice, int[] intensitypeakListLengthDevice, double[,] GpuIntensityPeakMatrix)
        {
            var tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            var StartIndex = tid % intensitypeakListLengthDevice[0];
            var EndIndex = StartIndex + (tid / intensitypeakListLengthDevice[0]);

            ///if (EndIndex >= intensitypeakListLengthDevice[0]) return;

            if (EndIndex < intensitypeakListLengthDevice[0] && EndIndex > StartIndex)
            {
                var AverageOfIntensities = (IntensityPeakDataDevice[StartIndex] + IntensityPeakDataDevice[EndIndex]) / 2;
                //GpuIntensityPeakMatrix[StartIndex, EndIndex] = SumOfIntensities;
                GpuIntensityPeakMatrix[StartIndex, EndIndex] = AverageOfIntensities;
            }
            
        }

        private void AccomodateIsoforms(List<PstTagsDtoGpu> pstTags)
        {
            var tagsContainingIsoformIL = pstTags.Select(x => x).Where(x => x.AminoAcidTag.Contains('I')).ToList();
            foreach (var tag in tagsContainingIsoformIL)
            {
                tag.AminoAcidTag = tag.AminoAcidTag.Replace('I', 'L');
            }
            pstTags.AddRange(tagsContainingIsoformIL);
        }


        private static char[] GetAminoAcidCharacters()
        {//{{PREVIOUS CASES}"L"---- "A" "C""D""E" "F""G""H""K""M""N""O""P""Q""R""S""T""U""V""W""Y"  ---- ""I"" is here but will be Accommodated in Acco.. function}
            var aminoAcids = new char[AminoAcidAlphabetSize];
            aminoAcids[0] = 'X';
            aminoAcids[1] = 'A';
            aminoAcids[2] = 'C';
            aminoAcids[3] = 'D';
            aminoAcids[4] = 'E';
            aminoAcids[5] = 'F';
            aminoAcids[6] = 'G';
            aminoAcids[7] = 'H';
            aminoAcids[8] = 'I';
            aminoAcids[9] = 'X';
            aminoAcids[10] = 'K';
            aminoAcids[11] = 'X';
            aminoAcids[12] = 'M';
            aminoAcids[13] = 'N';
            aminoAcids[14] = 'O';
            aminoAcids[15] = 'P';
            aminoAcids[16] = 'Q';
            aminoAcids[17] = 'R';
            aminoAcids[18] = 'S';
            aminoAcids[19] = 'T';
            aminoAcids[20] = 'U';
            aminoAcids[21] = 'V';
            aminoAcids[22] = 'W';
            aminoAcids[23] = 'X';
            aminoAcids[24] = 'Y';
            aminoAcids[25] = 'X';

            return aminoAcids;
        }

        private static double[] GetAminoAcidMasses()
        {
            double[] difference = new double[AminoAcidAlphabetSize];// Does not contain modifications !!!!!!!!!!!!!!!!!
            difference[0] = -1;
            difference[1] = 71.037114; //A
            difference[2] = 103.009185; //C
            difference[3] = 115.026943; //D
            difference[4] = 129.042593; //E
            difference[5] = 147.068414; //F
            difference[6] = 57.021464; //G
            difference[7] = 137.058912; //H
            difference[8] = 113.084064; //I
            difference[9] = -1;
            difference[10] = 128.094963; //K
            difference[11] = -1; //L
            difference[12] = 131.040485; //M
            difference[13] = 114.042927; //N
            difference[14] = 255.158295;
            difference[15] = 97.052764; //P
            difference[16] = 128.058578; //Q
            difference[17] = 156.101111; //R
            difference[18] = 87.032028; //S
            difference[19] = 101.047679; //T
            difference[20] = 168.964203;
            difference[21] = 99.068414; //V
            difference[22] = 186.079313; //W
            difference[23] = -1;
            difference[24] = 163.06332; //Y
            difference[25] = -1;


            return difference;
        }

        //[Cudafy]
        //private static void NOTUSEDPstGenerationKernelO(GThread thread, double[] peaks, int[] peakListCountArray, double[] aminoAcidMasses, double[] hopTolerance, char[,] pstMatchMatrix, double[,] errors)
        //{
        //    int peakListCount = peakListCountArray[0];
        //    int tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
        //    int startIndex = tid % peakListCount;
        //    int endIndex = startIndex + (tid / (peakListCount * 26)) + 1;
        //    int d = (tid / peakListCount) % 26;

        //    if (endIndex < peakListCount)
        //    {
        //        double dif = peaks[endIndex] - peaks[startIndex];
        //        if (dif < 0)
        //            dif *= -1;
        //        double error = dif - aminoAcidMasses[d];
        //        if (error < 0)
        //            error *= -1;
        //        if (error < 0.5 / 16)
        //        {
        //            pstMatchMatrix[startIndex, endIndex] = (char)d;
        //            errors[startIndex, endIndex] = error;
        //        }
        //    }
        //}
    }
}
