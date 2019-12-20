using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using ICSharpCode.ILSpy;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;

namespace PerceptronLocalService.Engine
{
    class InsilicoFilterGpu : IInsilicoFilter
    {
        private const int ThreadsPerBlock = 1024;
        public void ComputeInsilicoScore(List<ProteinDto> proteinList, List<double> peakList, double tol)
        {
            var gpu = CudafyHost.GetDevice(CudafyModes.Target);
            var architecture = gpu.GetArchitecture();
            var cudafyModule = CudafyTranslator.Cudafy(architecture);
            gpu.LoadModule(cudafyModule);

            var pepUnit = "Da";

            var peakListArray = peakList.ToArray();
            double[] toleranceArray = { tol };
            var arraySize = new int[1];
            var numberofMatches = new int[1];

            var peakListArrayDevice = gpu.Allocate(peakListArray);
            var toleranceDevice = gpu.Allocate(toleranceArray);
            var numberofMatchesDevice = gpu.Allocate(numberofMatches);
            var arraySizeDevice = gpu.Allocate(arraySize);

            gpu.CopyToDevice(peakListArray, peakListArrayDevice);
            gpu.CopyToDevice(toleranceArray, toleranceDevice);

            foreach (var protein in proteinList)
            {
                var insilicoMassLeftArray = protein.InsilicoDetails.InsilicoMassLeft.ToArray();
                var insilicoMassRightArray = protein.InsilicoDetails.InsilicoMassRight.ToArray();

                var modifiedIonsArray = GetModifiedIonsArray(protein);

                arraySize[0] = (peakListArray.Length * insilicoMassLeftArray.Length);
                arraySize[0] += (peakListArray.Length * insilicoMassRightArray.Length);
                arraySize[0] += modifiedIonsArray.Length * peakListArray.Length;

                var differencesArray = new double[arraySize[0]];
                var matchesFound = new int[arraySize[0]];

                var insilicoMassLeftArrayDevice = gpu.Allocate(insilicoMassLeftArray);
                var insilicoMassRightArrayDevice = gpu.Allocate(insilicoMassRightArray);
                var differencesArrayDevice = gpu.Allocate(differencesArray);
                var matchesFoundDevice = gpu.Allocate(matchesFound);
                var modifiedIonsArrayDevice = gpu.Allocate(modifiedIonsArray);

                gpu.CopyToDevice(insilicoMassLeftArray, insilicoMassLeftArrayDevice);
                gpu.CopyToDevice(insilicoMassRightArray, insilicoMassRightArrayDevice);
                gpu.CopyToDevice(arraySize, arraySizeDevice);
                gpu.CopyToDevice(modifiedIonsArray, modifiedIonsArrayDevice);

                var numBlocks = (int)Math.Ceiling((double)arraySize[0] / ThreadsPerBlock);

                switch (pepUnit)
                {
                    case "Da":
                    case "mmu":
                        gpu.Launch(numBlocks, ThreadsPerBlock).PeaksCompare_Da(insilicoMassLeftArrayDevice, insilicoMassRightArrayDevice, peakListArrayDevice, differencesArrayDevice, matchesFoundDevice, arraySizeDevice, numberofMatchesDevice, toleranceDevice, modifiedIonsArrayDevice);
                        break;
                    case "ppm":
                        gpu.Launch(numBlocks, ThreadsPerBlock).PeaksCompare_ppm(insilicoMassLeftArrayDevice, insilicoMassRightArrayDevice, peakListArrayDevice, differencesArrayDevice, matchesFoundDevice, arraySizeDevice, numberofMatchesDevice, toleranceDevice, modifiedIonsArrayDevice);
                        break;
                    default:
                        break;
                }

                gpu.CopyFromDevice(differencesArrayDevice, differencesArray);
                gpu.CopyFromDevice(matchesFoundDevice, matchesFound);

                var sum = differencesArray.Count(t => t < tol);

                var sum2 = matchesFound.Sum();

                var score = (double)sum / peakList.Count;
                score = score * Math.Pow(2, 1 - score);
                protein.InsilicoScore = score;
            }
        }

        public void ComputeInsilicoScore1(List<ProteinDto> proteinList, List<double> peakList, double tol)
        {
            var gpu = CudafyHost.GetDevice(CudafyModes.Target);
            var architecture = gpu.GetArchitecture();
            var cudafyModule = CudafyTranslator.Cudafy(architecture);
            gpu.LoadModule(cudafyModule);

            var pepUnit = "Da";

            var peakListArray = peakList.ToArray();
            double[] toleranceArray = { tol };
            //var arraySize = new int[1];
            //var numberofMatches = new int[1];

            var peakListArrayDevice = gpu.Allocate(peakListArray);
            var toleranceDevice = gpu.Allocate(toleranceArray);
            //var numberofMatchesDevice = gpu.Allocate(numberofMatches);
            //var arraySizeDevice = gpu.Allocate(arraySize);

            gpu.CopyToDevice(peakListArray, peakListArrayDevice);
            gpu.CopyToDevice(toleranceArray, toleranceDevice);

            foreach (var protein in proteinList)
            {
               
                var insilicoMassLeftArray = protein.InsilicoDetails.InsilicoMassLeft.ToArray();
                var insilicoMassRightArray = protein.InsilicoDetails.InsilicoMassRight.ToArray();
                var insilicoMassLeftAoLeftArray = protein.InsilicoDetails.InsilicoMassLeftAo.ToArray();
                var insilicoMassLeftAstarArray = protein.InsilicoDetails.InsilicoMassLeftAstar.ToArray();
                var insilicoMassLeftBoArray = protein.InsilicoDetails.InsilicoMassLeftBo.ToArray();
                var insilicoMassLeftBstarArray = protein.InsilicoDetails.InsilicoMassLeftBstar.ToArray();
                var insilicoMassRightYoArray = protein.InsilicoDetails.InsilicoMassRightYo.ToArray();
                var insilicoMassRightYstarArray = protein.InsilicoDetails.InsilicoMassRightYstar.ToArray();
                var insilicoMassRightZoArray = protein.InsilicoDetails.InsilicoMassRightZo.ToArray();
                var insilicoMassRightZooArray = protein.InsilicoDetails.InsilicoMassRightZoo.ToArray();
                            
                var differenceArraySize = peakList.Count*protein.InsilicoDetails.InsilicoMassLeft.Count*10;
                var differenceArraySizeArray = new int[differenceArraySize];
                var differencesArray = new double[differenceArraySize];
                var matchesFound = new int[differenceArraySize];

                var insilicoMassLeftArrayDevice = gpu.Allocate(insilicoMassLeftArray);
                var insilicoMassRightArrayDevice = gpu.Allocate(insilicoMassRightArray);
                var insilicoMassLeftAoLeftArrayDevice = gpu.Allocate(insilicoMassLeftAoLeftArray);
                var insilicoMassLeftAstarArrayDevice = gpu.Allocate(insilicoMassLeftAstarArray);
                var insilicoMassLeftBoArrayDevice = gpu.Allocate(insilicoMassLeftBoArray);
                var insilicoMassLeftBstarArrayDevice = gpu.Allocate(insilicoMassLeftBstarArray);
                var insilicoMassRightYoArrayDevice = gpu.Allocate(insilicoMassRightYoArray);
                var insilicoMassRightYstarArrayDevice = gpu.Allocate(insilicoMassRightYstarArray);
                var insilicoMassRightZoArrayDevice = gpu.Allocate(insilicoMassRightZoArray);
                var insilicoMassRightZooArrayDevice = gpu.Allocate(insilicoMassRightZooArray);

                var differenceArraySizeDevice = gpu.Allocate(differenceArraySizeArray);
                var differencesArrayDevice = gpu.Allocate(differencesArray);
                var matchesFoundDevice = gpu.Allocate(matchesFound);
     

                gpu.CopyToDevice(insilicoMassLeftArray, insilicoMassLeftArrayDevice);
                gpu.CopyToDevice(insilicoMassRightArray, insilicoMassRightArrayDevice);
                gpu.CopyToDevice(insilicoMassLeftAoLeftArray, insilicoMassLeftAoLeftArrayDevice);
                gpu.CopyToDevice(insilicoMassLeftAstarArray, insilicoMassLeftAstarArrayDevice);
                gpu.CopyToDevice(insilicoMassLeftBoArray, insilicoMassLeftBoArrayDevice);
                gpu.CopyToDevice(insilicoMassLeftBstarArray, insilicoMassLeftBstarArrayDevice);
                gpu.CopyToDevice(insilicoMassRightYoArray, insilicoMassRightYoArrayDevice);
                gpu.CopyToDevice(insilicoMassRightYstarArray, insilicoMassRightYstarArrayDevice);
                gpu.CopyToDevice(insilicoMassRightZoArray, insilicoMassRightZoArrayDevice);
                gpu.CopyToDevice(insilicoMassRightZooArray, insilicoMassRightZooArrayDevice);

                gpu.CopyToDevice(differenceArraySizeArray, differenceArraySizeDevice);


                var numBlocks = (int)Math.Ceiling((double)differenceArraySize / ThreadsPerBlock);

                gpu.Launch(numBlocks, ThreadsPerBlock)
                    .PeaksCompare_Da(insilicoMassLeftArrayDevice, insilicoMassRightArrayDevice,
                        insilicoMassLeftAoLeftArrayDevice,
                        insilicoMassLeftAstarArrayDevice,
                        insilicoMassLeftBoArrayDevice,
                        insilicoMassLeftBstarArrayDevice,
                        insilicoMassRightYoArrayDevice,
                        insilicoMassRightYstarArrayDevice,
                        insilicoMassRightZoArrayDevice,
                        insilicoMassRightZooArrayDevice);//,
                    
                    //peakListArrayDevice, differencesArrayDevice, matchesFoundDevice, arraySizeDevice, numberofMatchesDevice, toleranceDevice, modifiedIonsArrayDevice);
                
                gpu.CopyFromDevice(differencesArrayDevice, differencesArray);
                gpu.CopyFromDevice(matchesFoundDevice, matchesFound);

                var sum = differencesArray.Count(t => t < tol);

                var sum2 = matchesFound.Sum();

                var score = (double)sum / peakList.Count;
                score = score * Math.Pow(2, 1 - score);
                protein.InsilicoScore = score;
            }
        }


        private double[] GetModifiedIonsArray(ProteinDto protein)
        {
            var modifiedIonsArray = protein.InsilicoDetails.InsilicoMassLeftAo.ToArray();
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassLeftAstar);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassLeftBo);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassLeftBstar);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassRightYo);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassRightYstar);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassRightZo);
            modifiedIonsArray.AddRange(protein.InsilicoDetails.InsilicoMassRightZoo);
            return modifiedIonsArray;
        }

        [Cudafy]
        public static void PeaksCompare_Dalton(GThread thread, double[] leftInsilicoMasses, double[] rightInsilicoMasses, 
            double[] insilicoMassLeftAo, double[] insilicoMassLeftAstar, double[] insilicoMassLeftBo, double[] insilicoMassLeftBstar,
            double[] insilicoMassRightYo, double[] insilicoMassRightYstar, double[] insilicoMassRightZo, double[] insilicoMassRightZoo,
            double[] peakListMasses, double[] differencesArray, int[] matchesFound, int[] arraySize, int[] numberOfMatches, 
            double[] Tolerance, double[] modifiedIonsArray)
        {
            int tidx = thread.blockIdx.x * thread.blockDim.x + thread.threadIdx.x;

            int Iindex = tidx % leftInsilicoMasses.Length;
            int Pindex = (tidx / leftInsilicoMasses.Length) % peakListMasses.Length;
            double peak = peakListMasses[Pindex];

            int planeSize = leftInsilicoMasses.Length*peakListMasses.Length;
            // modified array index ***

            if (tidx < planeSize)
            {
                differencesArray[tidx] = leftInsilicoMasses[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = rightInsilicoMasses[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassLeftAo[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassLeftAstar[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassLeftBo[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassLeftBstar[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassRightYo[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassRightYstar[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassRightZo[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

                thread.SyncThreads();
                tidx += planeSize;

                differencesArray[tidx] = insilicoMassRightZoo[Iindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx]*-1;

                if (differencesArray[tidx] < Tolerance[0])
                    matchesFound[tidx] = 1;
                else
                    matchesFound[tidx] = 0;

            }
            

           
        }


        [Cudafy]
        public static void PeaksCompare_Da(GThread thread, double[] LeftInsilicoMasses, double[] RightInsilicoMasses, double[] peakListMasses, double[] differencesArray, int[] MatchesFound, int[] arraySize, int[] numberOfMatches, double[] Tolerance, double[] modifiedIonsArray)
        {
            int tidx = thread.blockIdx.x * thread.blockDim.x + thread.threadIdx.x;

            int ILindex = tidx % LeftInsilicoMasses.Length;
            int IRindex = tidx % RightInsilicoMasses.Length;
            int MIindex = tidx % modifiedIonsArray.Length;
            int Pindex = tidx / LeftInsilicoMasses.Length;
            double peak = peakListMasses[Pindex];

            // modified array index ***

            if (tidx < LeftInsilicoMasses.Length * peakListMasses.Length)
            {
                differencesArray[tidx] = LeftInsilicoMasses[ILindex] - peak;
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx] * -1;

                thread.SyncThreads();

                differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] = RightInsilicoMasses[IRindex] - peak;
                if (differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] < 0)
                    differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] = differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] * -1;
            }
            thread.SyncThreads();

            if (tidx < modifiedIonsArray.Length * peakListMasses.Length)
            {
                differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] = modifiedIonsArray[MIindex] - peak;
                //thread.SyncThreads();
                if (differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] < 0)
                    differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] = differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] * -1;
            }
            thread.SyncThreads();

            if (differencesArray[tidx] < Tolerance[0] )
                MatchesFound[tidx] = 1;
            else
                MatchesFound[tidx] = 0;

            thread.SyncThreads();
        }

        [Cudafy]
        public static void PeaksCompare_ppm(GThread thread, double[] LeftInsilicoMasses, double[] RightInsilicoMasses, double[] peakListMasses, double[] differencesArray, int[] MatchesFound, int[] arraySize, int[] numberOfMatches, double[] Tolerance, double[] modifiedIonsArray)
        {
            int tidx = thread.blockIdx.x * thread.blockDim.x + thread.threadIdx.x;

            int ILindex = tidx % LeftInsilicoMasses.Length;
            int IRindex = tidx % RightInsilicoMasses.Length;
            int MIindex = tidx % modifiedIonsArray.Length;
            int Pindex = tidx / LeftInsilicoMasses.Length;

            if (tidx < LeftInsilicoMasses.Length * peakListMasses.Length)
            {
                differencesArray[tidx] = ((LeftInsilicoMasses[ILindex] - peakListMasses[Pindex]) * 1000000) / peakListMasses[Pindex];
                if (differencesArray[tidx] < 0)
                    differencesArray[tidx] = differencesArray[tidx] * -1;
            }
            thread.SyncThreads();

            if (tidx < RightInsilicoMasses.Length * peakListMasses.Length)
            {
                differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] = ((RightInsilicoMasses[IRindex] - peakListMasses[Pindex]) * 1000000) / peakListMasses[Pindex];
                if (differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] < 0)
                    differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] = differencesArray[tidx + (LeftInsilicoMasses.Length * peakListMasses.Length)] * -1;
            }
            thread.SyncThreads();

            if (tidx < modifiedIonsArray.Length * peakListMasses.Length)
            {
                differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] = ((modifiedIonsArray[MIindex] - peakListMasses[Pindex]) * 1000000) / peakListMasses[Pindex];
                if (differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] < 0)
                    differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] = differencesArray[tidx + ((LeftInsilicoMasses.Length * peakListMasses.Length) + (RightInsilicoMasses.Length * peakListMasses.Length))] * -1;
            }
            thread.SyncThreads();

            if (differencesArray[tidx] < Tolerance[0] && differencesArray[tidx] > 0.0)
                MatchesFound[tidx] = 1;
            else
                MatchesFound[tidx] = 0;
        }
    }
}
