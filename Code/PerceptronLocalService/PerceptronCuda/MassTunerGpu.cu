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

#define SIZE	1024 
using namespace std;

//	--- Updated: 20210223 ---
__device__ int dev_data[50];
__device__ int dev_count = 0;
__device__ int dev_wind_count = 0;

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

__device__ void my_push_back(double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, double summationOfMasses, double averageOfIntensities)
{
	int insert_pt = atomicAdd(&dev_count, 1);
	dev_PeakListMassesSum[insert_pt] = summationOfMasses;
	dev_PeakListIntensitiesAverage[insert_pt] = averageOfIntensities;
	return;
}

__global__ void vectorAdd(double *raw_ptr_masses, double *raw_ptr_intensities, double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, double MwTolerance, double NeutralLoss, int N) {
	int tid = blockIdx.x * blockDim.x + threadIdx.x;

	if (tid < N)
	{
		for (int i = tid + 1; i < N; i++)
		{
			double summationOfMasses = raw_ptr_masses[tid] + raw_ptr_masses[i] + NeutralLoss;
			double averageOfIntensities = (raw_ptr_intensities[tid] + raw_ptr_intensities[i]) / 2;
			my_push_back(dev_PeakListMassesSum, dev_PeakListIntensitiesAverage, summationOfMasses, averageOfIntensities);
		}
	}
	else
		return;
}

__device__ void window_push_back(_WindowCapturedElementsStruct *windowcapturedelements, double a, int b)
{
	int insert_ptr = atomicAdd(&dev_wind_count, 1);
	windowcapturedelements[insert_ptr].TunedMass = a;
	windowcapturedelements[insert_ptr].elementCount = b;
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

extern "C" __declspec(dllexport) double __cdecl
wholeproteinmasstuner(double PeakListMasses[], double PeakListIntensities[], int PeakListLength, double MwTolerance, double NeutralLoss, double Slider_Value)
{
	double WholeProteinMass = PeakListMasses[0];

	const int N = PeakListLength;
	const int zN = (floor(PeakListLength*PeakListLength) / 2) - (floor(PeakListLength / 2));
	double *dev_masses, *dev_intensities, *dev_PeakListMassesSum, *dev_PeakListIntensitiesAverage;

	cudaMalloc((void**)&dev_masses, sizeof(double)*N);
	cudaMalloc((void**)&dev_intensities, sizeof(double)*N);

	thrust::device_vector<double> DevicePeakListMassesSum(zN);
	double *raw_ptr00 = thrust::raw_pointer_cast(DevicePeakListMassesSum.data());

	thrust::device_vector<double> DevicePeakListAvgIntensities(zN);
	double *raw_ptr01 = thrust::raw_pointer_cast(DevicePeakListAvgIntensities.data());

	cudaMemcpy(dev_masses, PeakListMasses, N * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_intensities, PeakListIntensities, N * sizeof(double), cudaMemcpyHostToDevice);

	int THREADS = 256;
	int BLOCKS = (N + THREADS - 1);
	vectorAdd << < BLOCKS, THREADS >> > (dev_masses, dev_intensities, raw_ptr00, raw_ptr01, MwTolerance, NeutralLoss, N);

	thrust::host_vector<double> PeakListMassesSum = DevicePeakListMassesSum;
	thrust::host_vector<double> PeakListIntensitiesAverage = DevicePeakListAvgIntensities;
	thrust::host_vector<_ShortlistedMassSumsAndIntensities> shortlistedMassSumAndIntensities;
	for (int i = 0; i < zN; i++)
	{
		double a = PeakListMassesSum[i];
		if (PeakListMasses[0] - MwTolerance <= PeakListMassesSum[i] && PeakListMassesSum[i] <= PeakListMasses[0] + MwTolerance)
		{
			_ShortlistedMassSumsAndIntensities data;
			data.massSum = PeakListMassesSum[i];
			data.AvgIntensity = PeakListIntensitiesAverage[i];
			shortlistedMassSumAndIntensities.push_back(data);
		}
	}

	std::sort(shortlistedMassSumAndIntensities.begin(), shortlistedMassSumAndIntensities.end(),
		[](const _ShortlistedMassSumsAndIntensities &mass, const _ShortlistedMassSumsAndIntensities &mass2)
	{ return (mass.massSum < mass2.massSum); });

	int x = 0;
	double minSum = shortlistedMassSumAndIntensities[0].massSum;
	double maxSum = shortlistedMassSumAndIntensities[shortlistedMassSumAndIntensities.size() - 1].massSum;

	int sizeOfShortlistedData = shortlistedMassSumAndIntensities.size();
	double SliderValue = (WholeProteinMass * Slider_Value) / (pow(10.0, 6.0));
	int NumOfThreadsToLaunch = floor((maxSum - minSum) * (1 / SliderValue));

	thrust::device_vector<_ShortlistedMassSumsAndIntensities> device_shortlistedMassSumAndIntensities = shortlistedMassSumAndIntensities;
	_ShortlistedMassSumsAndIntensities *raw_ptr = thrust::raw_pointer_cast(device_shortlistedMassSumAndIntensities.data());

	thrust::device_vector<_WindowCapturedElementsStruct> device_windowcapturedelements(NumOfThreadsToLaunch);
	_WindowCapturedElementsStruct *raw_ptr2 = thrust::raw_pointer_cast(device_windowcapturedelements.data());

	int THREADS2 = 256;
	int BLOCKS2 = (NumOfThreadsToLaunch / THREADS + 1);

	WindowLaunchKernel << <BLOCKS2, THREADS2 >> > (NumOfThreadsToLaunch, minSum, maxSum, raw_ptr, sizeOfShortlistedData, raw_ptr2, SliderValue);

	thrust::host_vector<_WindowCapturedElementsStruct> host_windowcapturedelements = device_windowcapturedelements;

	double TunedMass = 0;
	int oldElementCount = 0;

	for (int x = 0; x < NumOfThreadsToLaunch; x++)
	{
		if (oldElementCount < host_windowcapturedelements[x].elementCount)
		{
			oldElementCount = host_windowcapturedelements[x].elementCount;
			TunedMass = host_windowcapturedelements[x].TunedMass;
		}
		else if (oldElementCount == host_windowcapturedelements[x].elementCount)
		{
			if (abs(TunedMass - WholeProteinMass) >= abs(host_windowcapturedelements[x].TunedMass - WholeProteinMass))
			{
				TunedMass = host_windowcapturedelements[x].TunedMass;
			}
		}
	}
	cudaDeviceSynchronize();
	return TunedMass;
}
