#include <iostream>
#include <stdio.h>
#include <cublas.h>
#include <vector>
#include <assert.h>
#include <stdlib.h>
#include <cuda.h>
#include <cuda_runtime.h>
#include <cuda_runtime_api.h>
#include <device_launch_parameters.h>
#include  <device_atomic_functions.h>
#include <thrust/host_vector.h>
#include <thrust/device_vector.h>
#include <string>
#include <iterator>
#include<algorithm>
#include <chrono>
#include <math.h>
//#include "CudaHeaderFile.cuh"

__global__ void GeneratingMultipleLengthPsts(_PeptideSequenceTags *MultipleLengthPst_ptr, _DataForPsts *SingleLengthPSTs, int N)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	if (tid < N)
	{
		int HomePeak = SingleLengthPSTs[tid].endIndex;
		char HomePeakAA = SingleLengthPSTs[tid].AminoAcidSymbol;
		int StartIndex = SingleLengthPSTs[tid].startIndex;
		for (int i = 0; i < N; i++)
		{
			int HopPeak = SingleLengthPSTs[i].startIndex;
			char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
			int EndIndex = SingleLengthPSTs[i].endIndex;
			if (HomePeak == HopPeak)
			{
				int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
				MultipleLengthPst_ptr[insert_ptr].PstTag[0] = HomePeakAA;
				MultipleLengthPst_ptr[insert_ptr].PstTag[1] = HopPeakAA;
				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
				MultipleLengthPst_ptr[insert_ptr].PstTagLength = 2;
				MultipleLengthPst_ptr[insert_ptr].IntensitySum = SingleLengthPSTs[tid].averageIntensity + SingleLengthPSTs[i].averageIntensity;
				MultipleLengthPst_ptr[insert_ptr].PstFrequency = ((SingleLengthPSTs[tid].averageIntensity + SingleLengthPSTs[i].averageIntensity) / 2)*(2 * 2);
				MultipleLengthPst_ptr[insert_ptr].ErrorSum = SingleLengthPSTs[tid].TagError + SingleLengthPSTs[i].TagError;
				MultipleLengthPst_ptr[insert_ptr].RMSE = MultipleLengthPst_ptr[insert_ptr].ErrorSum / 2;
				double RMSE = (sqrt(MultipleLengthPst_ptr[insert_ptr].ErrorSum) / 2) * 10;
				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = exp(-RMSE * 2);
				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = (MultipleLengthPst_ptr[insert_ptr].PstTagLength * MultipleLengthPst_ptr[insert_ptr].PstFrequency) / RMSE;
			}
		}
	}
}

__global__ void GeneratingMultipleLengthPsts2(_PeptideSequenceTags *MultipleLengthPst_ptr, _PeptideSequenceTags *DupletPSTs, _DataForPsts *SingleLengthPSTs, int N, int num, int SizeOfPst)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	if (tid < N)
	{
		int HomePeak = DupletPSTs[tid].endIndex;
		char *HomePeakAA = DupletPSTs[tid].PstTag;
		int StartIndex = DupletPSTs[tid].startIndex;
		for (int i = 0; i < num; i++)
		{
			int HopPeak = SingleLengthPSTs[i].startIndex;
			char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
			int EndIndex = SingleLengthPSTs[i].endIndex;
			if (HomePeak == HopPeak)
			{
				int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
				my_strcpy(MultipleLengthPst_ptr[insert_ptr].PstTag, HomePeakAA);
				MultipleLengthPst_ptr[insert_ptr].PstTag[SizeOfPst - 1] = HopPeakAA;
				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
				MultipleLengthPst_ptr[insert_ptr].PstTagLength = SizeOfPst;
				MultipleLengthPst_ptr[insert_ptr].IntensitySum = DupletPSTs[tid].IntensitySum + SingleLengthPSTs[i].averageIntensity;
				MultipleLengthPst_ptr[insert_ptr].PstFrequency = ((DupletPSTs[tid].IntensitySum + SingleLengthPSTs[i].averageIntensity) / SizeOfPst)*(SizeOfPst*SizeOfPst);
				MultipleLengthPst_ptr[insert_ptr].ErrorSum = DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError;
				MultipleLengthPst_ptr[insert_ptr].RMSE = (DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError) / SizeOfPst;
				double RMSE = (sqrt(DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError) / SizeOfPst) * 10;
				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = exp(-RMSE * 2);
				//MultipleLengthPst_ptr[insert_ptr].PstErrorScore = (MultipleLengthPst_ptr[insert_ptr].PstTagLength * MultipleLengthPst_ptr[insert_ptr].PstFrequency) / RMSE;
			}
		}
	}
}