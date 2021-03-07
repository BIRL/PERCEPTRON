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


__device__ int dev_data[50];
__device__ int dev_count = 0;
__device__ int dev_wind_count = 0;
__device__ int dev_pst_count = 0;
__device__ int multipleLengthPstCounter = 0;
__device__ int charArrayCounter = 0;

struct ParametersToCpp
{
	double MwTolerance;
	double NeutralLoss;
	double SliderValue;
	double HopThreshhold;
	int Autotune;
	int DenovoAllow;
	int MinimumPstLength;
	int MaximumPstLength;
};

typedef struct _WindowCapturedElementsStruct
{
	double TunedMass;
	int elementCount;
} windowcapturedelementsstruct;

typedef struct _ShortlistedMassSumsAndIntensities
{
	double massSum;
	double AvgIntensity;
	bool operator() (_ShortlistedMassSumsAndIntensities i, _ShortlistedMassSumsAndIntensities j) { return (i.massSum < j.massSum); }
} ShortlistedMassSumsAndIntensities;

typedef struct _DataForPsts
{
	int startIndex;
	int endIndex;
	double startIndexMass;
	double endIndexMass;
	double massDifferenceBetweenPeaks;
	char AminoAcidSymbol;
	double TagError;
	double averageIntensity;
} dataforpsts;

typedef struct _PeptideSequenceTags
{
	char PstTag[8];
	int PstTagLength;
	double PstErrorScore;
	double PstFrequency;
	double IntensitySum;
	int startIndex;
	int endIndex;
	double ErrorSum;
	double RMSE;
} peptidesequencetags;

__device__ void my_push_back(double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, double summationOfMasses, double averageOfIntensities)
{
	int insert_pt = atomicAdd(&dev_count, 1);
	dev_PeakListMassesSum[insert_pt] = summationOfMasses;
	dev_PeakListIntensitiesAverage[insert_pt] = averageOfIntensities;
	return;
}

__device__ void PST_push_back(_DataForPsts *SingleLengthPSTs_ptr, int tid, int i, double StartIndMass, double EndIndMass, double differenceOfMasses, char aminoAcidSymbol, double TagError, double averageOfIntensities)
{
	int insert_ptr = atomicAdd(&dev_pst_count, 1);
	SingleLengthPSTs_ptr[insert_ptr].startIndex = tid;
	SingleLengthPSTs_ptr[insert_ptr].endIndex = i;
	SingleLengthPSTs_ptr[insert_ptr].startIndexMass = StartIndMass;
	SingleLengthPSTs_ptr[insert_ptr].endIndexMass = EndIndMass;
	SingleLengthPSTs_ptr[insert_ptr].massDifferenceBetweenPeaks = differenceOfMasses;
	SingleLengthPSTs_ptr[insert_ptr].AminoAcidSymbol = aminoAcidSymbol;
	SingleLengthPSTs_ptr[insert_ptr].TagError = TagError;
	SingleLengthPSTs_ptr[insert_ptr].averageIntensity = averageOfIntensities;
	return;
}

__device__ void window_push_back(_WindowCapturedElementsStruct *windowcapturedelements, double a, int b)
{
	int insert_ptr = atomicAdd(&dev_wind_count, 1);
	windowcapturedelements[insert_ptr].TunedMass = a;
	windowcapturedelements[insert_ptr].elementCount = b;
	return;
}

__device__ char * my_strcpy(char *dest, const char *src) {
	int i = 0;
	do {
		dest[i] = src[i];
	} while (src[i++] != 0);
	return dest;
}

__global__ void CalculatingTupleSumsAndSingleLengthPsts(double *raw_ptr_masses, double *raw_ptr_intensities, double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, _DataForPsts *SingleLengthPSTs_ptr, double *dev_aminoAcidMassesList, char *dev_aminoAcidSymbolList, double MwTolerance, double NeutralLoss, double HopThreshold, int N, int AutoTune, int DenovoAllow) {
	int tid = blockIdx.x * blockDim.x + threadIdx.x;

	if (tid < N)
	{
		for (int i = tid + 1; i < N; i++)
		{
			double averageOfIntensities = (raw_ptr_intensities[tid] + raw_ptr_intensities[i]) / 2;
			if (AutoTune == 1)
			{
				double summationOfMasses = raw_ptr_masses[tid] + raw_ptr_masses[i] + NeutralLoss;
				my_push_back(dev_PeakListMassesSum, dev_PeakListIntensitiesAverage, summationOfMasses, averageOfIntensities);
			}
			if (DenovoAllow == 1)
			{
				double differenceOfMasses = fabs(raw_ptr_masses[tid] - raw_ptr_masses[i]);
				for (int j = 0; j < 21; j++)
				{
					double TagError = pow(fabs(dev_aminoAcidMassesList[j] - differenceOfMasses), 2);
					if (fabs(dev_aminoAcidMassesList[j] - differenceOfMasses) <= HopThreshold)
					{
						PST_push_back(SingleLengthPSTs_ptr, tid, i, raw_ptr_masses[tid], raw_ptr_masses[i], differenceOfMasses, dev_aminoAcidSymbolList[j], TagError, averageOfIntensities);
					}
				}
			}
		}
	}
	else
		return;
}

__global__ void WindowLaunchKernel(int NumOfThreadsToLaunch, double minSum, double maxSum, _ShortlistedMassSumsAndIntensities *shortListedData, int sizeOfShortlistedData, _WindowCapturedElementsStruct *windowcapturedelements, double SliderValue)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;
	if (tid < NumOfThreadsToLaunch)
	{
		double WindowStart = minSum + (tid * SliderValue);
		double WindowEnd = WindowStart + 1.00727647;
		double sumoftunedmassesandintensities = 0;
		double sumoftunedintensities = 0;
		int Count = 0;
		for (int i = 0; i < sizeOfShortlistedData; i++)
		{
			if (WindowStart <= shortListedData[i].massSum && shortListedData[i].massSum < WindowEnd)//#DISCUSSION
			{
				double data = shortListedData[i].massSum * shortListedData[i].AvgIntensity;
				sumoftunedmassesandintensities = sumoftunedmassesandintensities + data;
				sumoftunedintensities = sumoftunedintensities + shortListedData[i].AvgIntensity;
				Count = Count + 1;
			}
			else if (shortListedData[i].massSum >= WindowEnd)
			{
				break;
			}
		}
		double TunedMass = sumoftunedmassesandintensities / sumoftunedintensities;
		int elementCount = Count;
		window_push_back(windowcapturedelements, TunedMass, elementCount);
	}
}

//
//
//__device__ void massTuner_push_back(double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, double summationOfMasses, double averageOfIntensities)
//{
//	int insert_pt = atomicAdd(&dev_count, 1);
//	dev_PeakListMassesSum[insert_pt] = summationOfMasses;
//	dev_PeakListIntensitiesAverage[insert_pt] = averageOfIntensities;
//	return;
//}
//
//__device__ void PST_push_back(_DataForPsts *SingleLengthPSTs_ptr, int tid, int i, double StartIndMass, double EndIndMass, double differenceOfMasses, char aminoAcidSymbol, double TagError, double averageOfIntensities)
//{
//	int insert_ptr = atomicAdd(&dev_pst_count, 1);
//	SingleLengthPSTs_ptr[insert_ptr].startIndex = tid;
//	SingleLengthPSTs_ptr[insert_ptr].endIndex = i;
//	SingleLengthPSTs_ptr[insert_ptr].startIndexMass = StartIndMass;
//	SingleLengthPSTs_ptr[insert_ptr].endIndexMass = EndIndMass;
//	SingleLengthPSTs_ptr[insert_ptr].massDifferenceBetweenPeaks = differenceOfMasses;
//	SingleLengthPSTs_ptr[insert_ptr].AminoAcidSymbol = aminoAcidSymbol;
//	SingleLengthPSTs_ptr[insert_ptr].TagError = TagError;
//	SingleLengthPSTs_ptr[insert_ptr].averageIntensity = averageOfIntensities;
//	return;
//}
//
//__device__ void window_push_back(_WindowCapturedElementsStruct *windowcapturedelements, double a, int b)
//{
//	int insert_ptr = atomicAdd(&dev_wind_count, 1);
//	windowcapturedelements[insert_ptr].TunedMass = a;
//	windowcapturedelements[insert_ptr].elementCount = b;
//	return;
//}
//
//__device__ char * my_strcpy(char *dest, const char *src) {
//	int i = 0;
//	do {
//		dest[i] = src[i];
//	} while (src[i++] != 0);
//	return dest;
//}
//
////void AccomodateIsoforms(vector< _PeptideSequenceTags> &Final_MultipleLengthPsts, ParametersToCpp Parameters);
//
//__global__ void CalculatingTupleSumsAndSingleLengthPsts(double *raw_ptr_masses, double *raw_ptr_intensities, double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, _DataForPsts *SingleLengthPSTs_ptr, double *dev_aminoAcidMassesList, char *dev_aminoAcidSymbolList, double MwTolerance, double NeutralLoss, double HopThreshold, int N, int AutoTune, int DenovoAllow);
////
//__global__ void GeneratingMultipleLengthPsts(_PeptideSequenceTags *MultipleLengthPst_ptr, _DataForPsts *SingleLengthPSTs, int N)