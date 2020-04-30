//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using PerceptronLocalService.DTO;
//using System.Text;
//using Cudafy;
//using Cudafy.Host;
//using Cudafy.Translator;
//using System.Diagnostics;
//using PerceptronLocalService.Interfaces;
//using PerceptronLocalService.Utility;

//namespace PerceptronLocalService.Engine
//{
//    public class PstFilterGpu : IPeptideSequenceTagScoring
//    {
//        private const int N = 1024;
//        private const int Size = 14524302;
//        private const string Transitions = "ABCDEFGHI*KLMN*PQRST*VWXYZ";

//        private void ScoreProteins(char[] pstMatchVector, List<PstTagList> pstList, List<int> proteinLengths, List<ProteinDto> proteins)
//        {
//            //var temp = 0;
//            //var counter = 0;
//            //for (var i = 0; i < proteinLengths.Count; i++)
//            //{
//            //    double score = 0;
//            //    temp += proteinLengths[i];
//            //    while (counter < temp)
//            //    {
//            //        if (Convert.ToInt16(pstMatchVector[counter]) > 0)
//            //        {
//            //            score += (pstList[pstMatchVector[counter] - 1].ErrorScore + pstList[pstMatchVector[counter] - 1].FrequencyScore);
//            //        }
//            //        counter++;
//            //    }
//            //    score = score / Convert.ToDouble(proteins[i].Sequence.Length);
//            //    if (score > 1)
//            //        score = 1;
//            //    proteins[i].PstScore = score;

//            //}

//        }

//        [Cudafy]
//        private static void DeviceSearchPstInSuperString(GThread thread, int[,] stateTransitionMatrix, int[] stateSuccessVector, char[] pstMatchVector, char[] superString, int[] superStringSizeArray)
//        {
//        //    var tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
//        //    var superStringIndex = tid;
//        //    var superStringSize = superStringSizeArray[0];

//        //    while (superStringIndex < superStringSize)
//        //    {
//        //        thread.SyncThreads();
//        //        var start = superStringIndex;
//        //        var state = 0;
//        //        while ((state != -1) && (superStringIndex < superStringSize))
//        //        {
//        //            var aminoAcid = superString[superStringIndex];
//        //            thread.SyncThreads();
//        //            int nextState = stateTransitionMatrix[state, (int)(aminoAcid) - (int)('A')];
//        //            superStringIndex += 1;
//        //            if (nextState != -1)
//        //            {
//        //                var pstNumber = stateSuccessVector[nextState];
//        //                if (pstNumber > 0)
//        //                {
//        //                    pstMatchVector[start] = (char)pstNumber;
//        //                }
//        //                thread.SyncThreads();
//        //            }
//        //            state = nextState;
//        //        }
//        //        superStringIndex = start + N * thread.gridDim.x;
//        //    }
//        //}

//        //private static char[] SearchPstInSuperString(string[] pstArray, string sequenceSuperString)
//        //{
//        //    GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
//        //    eArchitecture architecture = gpu.GetArchitecture();
//        //    CudafyModule cudafyModule = CudafyTranslator.Cudafy(architecture);
//        //    gpu.LoadModule(cudafyModule);

//        //    var finiteStateMachineBuilder = new StringSearch(pstArray);
//        //    var stateTransitionMatrix = finiteStateMachineBuilder.GenerateStateTransitionMatrix(Transitions);
//        //    var superStringArray = sequenceSuperString.ToCharArray();
//        //    var superStringSize = sequenceSuperString.Length;
//        //    var stateSuccessVector = finiteStateMachineBuilder.GenerateStateSuccessVector();
//        //    var pstMatchVector = new char[superStringSize];
//        //    int[] superStringSizeArray = { superStringSize };

//        //    gpu.SetCurrentContext();

//        //    int[,] stateTransitionMatrixD = gpu.Allocate(stateTransitionMatrix);
//        //    int[] stateSuccessVectorD = gpu.Allocate(stateSuccessVector);
//        //    char[] pstMatchVectorD = gpu.Allocate<char>(pstMatchVector);
//        //    char[] superStringArrayD = gpu.Allocate<char>(superStringArray);
//        //    int[] superStringSizeArrayD = gpu.Allocate<int>(superStringSizeArray);

//        //    gpu.CopyToDevice(stateTransitionMatrix, stateTransitionMatrixD);
//        //    gpu.CopyToDevice(stateSuccessVector, stateSuccessVectorD);
//        //    gpu.CopyToDevice(pstMatchVector, pstMatchVectorD);
//        //    gpu.CopyToDevice(superStringArray, superStringArrayD);
//        //    gpu.CopyToDevice(superStringSizeArray, superStringSizeArrayD);

//        //    int block = (int)Math.Ceiling((double)superStringSize / N);
//        //    gpu.Launch(block, N).DeviceSearchPstInSuperString(stateTransitionMatrixD, stateSuccessVectorD, pstMatchVectorD, superStringArrayD, superStringSizeArrayD);

//        //    gpu.CopyFromDevice(pstMatchVectorD, pstMatchVector);
//        //    gpu.FreeAll();
//        //    return pstMatchVector;

//        }

//        public void ScoreProteinsByPst(List<PstTagsDto> pstList, List<ProteinDto> proteinList)
//        {
//            //var sb = new StringBuilder();
//            //var proteinLengths = new List<int>();
//            //foreach (var protein in proteinList)
//            //{
//            //    sb.Append(protein.Sequence);
//            //    proteinLengths.Add(protein.Sequence.Length);
//            //}
//            //var superString = sb.ToString();
//            //sb.Clear();

//            //var pstArray = pstList.Select(x => x.AminoAcidTag.ToUpper()).ToArray();
//            //var pstMatchVector = SearchPstInSuperString(pstArray, superString);
//            //ScoreProteins(pstMatchVector, pstList, proteinLengths, proteinList);

//        }
//    }
//}
