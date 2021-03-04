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

#define SIZE	1024 
using namespace std;

#define gpuErrchk(ans) { gpuAssert((ans), __FILE__, __LINE__); }
inline void gpuAssert(cudaError_t code, const char *file, int line, bool abort = true)
{
	if (code != cudaSuccess)
	{
		fprintf(stderr, "GPUassert: %s %s %d\n", cudaGetErrorString(code), file, line);
		if (abort) exit(code);
	}
}

//	--- Updated: 20210223 ---
__device__ int dev_data[50];
__device__ int dev_count = 0;
__device__ int dev_wind_count = 0;
__device__ int dev_pst_count = 0;
__device__ int multipleLengthPstCounter = 0;
__device__ int charArrayCounter = 0;

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
	int startIndex;
	int endIndex;
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

__global__ void vectorAdd(double *raw_ptr_masses, double *raw_ptr_intensities, double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, _DataForPsts *SingleLengthPSTs_ptr, double *dev_aminoAcidMassesList, char *dev_aminoAcidSymbolList, double MwTolerance, double NeutralLoss, double HopThreshold, int N) {
	int tid = blockIdx.x * blockDim.x + threadIdx.x;

	if (tid < N)
	{
		for (int i = tid + 1; i < N; i++)
		{
			double summationOfMasses = raw_ptr_masses[tid] + raw_ptr_masses[i] + NeutralLoss;
			double averageOfIntensities = (raw_ptr_intensities[tid] + raw_ptr_intensities[i]) / 2;
			my_push_back(dev_PeakListMassesSum, dev_PeakListIntensitiesAverage, summationOfMasses, averageOfIntensities);
			double differenceOfMasses = fabs(raw_ptr_masses[tid] - raw_ptr_masses[i]);
			for (int j = 0; j < 21; j++)
			{
				double TagError = fabs(dev_aminoAcidMassesList[j] - differenceOfMasses);
				if (TagError <= HopThreshold)
				{
					PST_push_back(SingleLengthPSTs_ptr, tid, i, raw_ptr_masses[tid], raw_ptr_masses[i], differenceOfMasses, dev_aminoAcidSymbolList[j], TagError, averageOfIntensities);
				}
			}
			
		}
	}
	else
		return;
}

__device__ char * my_strcpy(char *dest, const char *src) {
	int i = 0;
	do {
		dest[i] = src[i];
	} while (src[i++] != 0);
	return dest;
}

//__device__ void GeneratingMultipleLengthPstsInner(int HomePeak, char HomePeakAA, _PeptideSequenceTags *MultipleLengthPst_ptr, _DataForPsts *SingleLengthPSTs, char *tempPstArray, int IndexToInsertAA, int N)
//{
//	int i;
//	for (i=0; i<N; i++)
//	{
//		int HopPeak = SingleLengthPSTs[i].startIndex;
//		char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
//		if (HomePeak == HopPeak)
//		{
//			int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
//			//int ins_ptr = atomicAdd(&charArrayCounter, 1);
//			IndexToInsertAA = IndexToInsertAA + 1;
//			char *tempPstArrayCopy;
//			my_strcpy(tempPstArrayCopy, tempPstArray);
//			tempPstArrayCopy[IndexToInsertAA] = HopPeakAA;
//			//(MultipleLengthPst_ptr[insert_ptr].PstTag) = tempPstArray;
//			my_strcpy(MultipleLengthPst_ptr[insert_ptr].PstTag, tempPstArrayCopy);
//			//MultipleLengthPst_ptr[insert_ptr].PstTag[0] = HomePeakAA;
//			//MultipleLengthPst_ptr[insert_ptr].PstTag[1] = HopPeakAA;
//			//GeneratingMultipleLengthPstsInner(HopPeak, HopPeakAA, MultipleLengthPst_ptr, SingleLengthPSTs, tempPstArrayCopy, IndexToInsertAA, N);
//		}
//		else
//		{
//			int a = 1;
//		}
//	}
//	if (i == N)
//	{
//		return;
//	}
//	return;
//	//return;
//}

__device__ int fact(int f)
{
	if (f == 0)
		return 1;
	else
		return f * fact(f - 1);
}


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
			}
		}
		//char tempPstArray[8];
		//int IndexToInsertAA = 0;
		//tempPstArray[IndexToInsertAA] = HomePeakAA;
		//int a = fact(5);
		//GeneratingMultipleLengthPstsInner(HomePeak, HomePeakAA, MultipleLengthPst_ptr, SingleLengthPSTs, tempPstArray, IndexToInsertAA, N);
		/*GeneratingMultipleLengthPstsInner(HomePeak, HomePeakAA);*/
	}
	/*for (int i = 0; i < 2; i++)
	{
		for (int j=0; j<3; j++)
			push_back_multipleLengthPsts(MultipleLengthPst_ptr, 'L', 'G');
	}*/
}

//__global__ void GeneratingMultipleLengthPsts2(_PeptideSequenceTags *MultipleLengthPst_ptr, _PeptideSequenceTags *DupletPSTs, _DataForPsts *SingleLengthPSTs, int N)
//{
//	int tid = blockIdx.x * blockDim.x + threadIdx.x;
//	if (tid < N)
//	{
//		int HomePeak = DupletPSTs[tid].endIndex;
//		char HomePeakAA = DupletPSTs[tid].PstTag[0];
//		char HomePeakAA2 = DupletPSTs[tid].PstTag[1];
//		int size = DupletPSTs[tid].PstTag;
//		int StartIndex = DupletPSTs[tid].startIndex;
//		for (int i = 0; i < N; i++)
//		{
//			int HopPeak = SingleLengthPSTs[i].startIndex;
//			char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
//			int EndIndex = SingleLengthPSTs[tid].endIndex;
//			if (HomePeak == HopPeak)
//			{
//				int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
//				MultipleLengthPst_ptr[insert_ptr].PstTag[0] = HomePeakAA;
//				MultipleLengthPst_ptr[insert_ptr].PstTag[1] = HomePeakAA2;
//				MultipleLengthPst_ptr[insert_ptr].PstTag[2] = HopPeakAA;
//				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
//				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
//			}
//			else
//			{
//				int a = 0;
//			}
//		}
//	}
//}

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
				MultipleLengthPst_ptr[insert_ptr].PstTag[SizeOfPst-1] = HopPeakAA;
				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
			}
			else
			{
				int a = 0;
			}
		}
	}
}

string convertToString(char* a, int size)
{
	int i;
	string s = "";
	for (i = 0; i < size; i++) {
		s = s + a[i];
	}
	return s;
}

extern "C" __declspec(dllexport) double __cdecl
wholeproteinmasstuner(double PeakListMasses[], double PeakListIntensities[], int PeakListLength, double MwTolerance, double NeutralLoss, double Slider_Value, double HopThreshold)
{
	double WholeProteinMass = PeakListMasses[0];

	const int N = PeakListLength;
	const int zN = (floor(PeakListLength*PeakListLength) / 2) - (floor(PeakListLength / 2));
	double *dev_masses, *dev_intensities;

	double aminoAcidMassesList[21] = { 57.0214600000000, 71.0371100000000, 87.0320300000000, 97.0527600000000, 99.0684100000000, 101.047680000000, 103.009190000000, 113.084060000000, 114.042930000000, 115.026940000000, 128.058580000000, 128.094960000000, 129.042590000000, 131.040490000000, 137.058910000000, 147.068410000000, 156.101110000000, 163.063330000000, 168.964203000000, 186.079310000000, 255.158295000000 };
	char aminoAcidSymbolList[21] = { 'G', 'A', 'S', 'P', 'V', 'T', 'C', 'L', 'N', 'D', 'Q', 'K', 'E', 'M', 'H', 'F', 'R', 'Y', 'U', 'W', 'O' };
	
	double *dev_aminoAcidMassesList;
	char *dev_aminoAcidSymbolList;

	cudaMalloc((void**)&dev_masses, sizeof(double) * N);
	cudaMalloc((void**)&dev_intensities, sizeof(double) * N);
	cudaMalloc((void**)&dev_aminoAcidMassesList, sizeof(double)*21);
	cudaMalloc((void**)&dev_aminoAcidSymbolList, sizeof(char)*21);

	thrust::device_vector<double> DevicePeakListMassesSum(zN);
	double *DevicePeakListMassesSum_ptr = thrust::raw_pointer_cast(DevicePeakListMassesSum.data());

	thrust::device_vector<double> DevicePeakListAvgIntensities(zN);
	double *DevicePeakListAvgIntensities_ptr = thrust::raw_pointer_cast(DevicePeakListAvgIntensities.data());

	thrust::device_vector<_DataForPsts> SingleLengthPSTs(zN);
	_DataForPsts *SingleLengthPSTs_ptr = thrust::raw_pointer_cast(SingleLengthPSTs.data());

	cudaMemcpy(dev_masses, PeakListMasses, N * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_intensities, PeakListIntensities, N * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_aminoAcidMassesList, aminoAcidMassesList, 21 * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_aminoAcidSymbolList, aminoAcidSymbolList, 21 * sizeof(char), cudaMemcpyHostToDevice);

	//int Count = 0;
	//int *devCount;
	//cudaMalloc((void**)&devCount, sizeof(int));
	//cudaMemcpy(devCount, &Count, sizeof(int), cudaMemcpyHostToDevice);

	int THREADS = 256;
	int BLOCKS = (N + THREADS - 1);
	vectorAdd << < BLOCKS, THREADS >> > (dev_masses, dev_intensities, DevicePeakListMassesSum_ptr, DevicePeakListAvgIntensities_ptr, SingleLengthPSTs_ptr, dev_aminoAcidMassesList, dev_aminoAcidSymbolList, MwTolerance, NeutralLoss, HopThreshold, N);
	//cudaMemcpy(&Count, devCount, sizeof(int), cudaMemcpyHostToDevice);

	thrust::host_vector<_DataForPsts> Host_SingleLengthPSTs = SingleLengthPSTs;
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
	// from here
	thrust::device_vector<_DataForPsts> Final_SingleLengthPSTs;
	int startIndex[100];
	int endIndex[100];
	double startIndexMass[100];
	double endIndexMass[100];
	double massDifferenceBetweenPeaks[100];
	char AminoAcidSymbol[100];
	double TagError[100];
	double averageIntensity[100];
	int k = 0;
	for (int i = 0; i < zN; i++)
	{
		if (Host_SingleLengthPSTs[i].startIndexMass == 0)
		{
			k = i;
			break;
		}
		else
		{
			startIndex[i] = Host_SingleLengthPSTs[i].startIndex;
			endIndex[i] = Host_SingleLengthPSTs[i].endIndex;
			startIndexMass[i] = Host_SingleLengthPSTs[i].startIndexMass;
			endIndexMass[i] = Host_SingleLengthPSTs[i].endIndexMass;
			massDifferenceBetweenPeaks[i] = Host_SingleLengthPSTs[i].massDifferenceBetweenPeaks;
			AminoAcidSymbol[i] = Host_SingleLengthPSTs[i].AminoAcidSymbol;
			TagError[i] = Host_SingleLengthPSTs[i].TagError;
			averageIntensity[i] = Host_SingleLengthPSTs[i].averageIntensity;
			Final_SingleLengthPSTs.push_back(Host_SingleLengthPSTs[i]);
		}
	}
	k = k;
	startIndex[0] = startIndex[0];
	endIndex[0] = endIndex[0];
	startIndexMass[0] = startIndexMass[0];
	endIndexMass[0] = endIndexMass[0];
	massDifferenceBetweenPeaks[0] = massDifferenceBetweenPeaks[0];
	AminoAcidSymbol[0] = AminoAcidSymbol[0];
	TagError[0] = TagError[0];
	averageIntensity[0] = averageIntensity[0];

	Final_SingleLengthPSTs[1] = Final_SingleLengthPSTs[1];
	_DataForPsts *Final_SingleLengthPSTs_ptr = thrust::raw_pointer_cast(Final_SingleLengthPSTs.data());

	/*thrust::device_vector<_PeptideSequenceTags> dev_MultipleLengthPst(Final_SingleLengthPSTs.size());*/
	thrust::device_vector<_PeptideSequenceTags> dev_MultipleLengthPst(1000);
	_PeptideSequenceTags *MultipleLengthPst_ptr = thrust::raw_pointer_cast(dev_MultipleLengthPst.data());

	int THREADS3 = 256;
	int BLOCKS3 = N + THREADS3 - 1;;
	int numOfThr = Final_SingleLengthPSTs.size();
	GeneratingMultipleLengthPsts << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, Final_SingleLengthPSTs_ptr, numOfThr);
	gpuErrchk(cudaPeekAtLastError());
	gpuErrchk(cudaDeviceSynchronize());
	thrust::host_vector<_PeptideSequenceTags> host_MultipleLengthPst = dev_MultipleLengthPst;
	thrust::device_vector<_PeptideSequenceTags> dev_MultipleLengthPst2;
	
	int b = 0;
	for (int i = 0; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			b = i;
			break;
		}	
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
		char *array = host_MultipleLengthPst[i].PstTag;
		int startInd = host_MultipleLengthPst[i].startIndex;
		int endInd = host_MultipleLengthPst[i].endIndex;
	}
	b = b;
	_PeptideSequenceTags *MultipleLengthPst_ptr2 = thrust::raw_pointer_cast(dev_MultipleLengthPst2.data());
	numOfThr = dev_MultipleLengthPst2.size();
	int PstSize = 3;
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	host_MultipleLengthPst = dev_MultipleLengthPst;
	thrust::host_vector<_PeptideSequenceTags> host_MultipleLengthPst2 = dev_MultipleLengthPst2;
	int size = host_MultipleLengthPst2.size();
	dev_MultipleLengthPst2.clear();
	int h = 0;
	vector<char*> array;
	int StartIndArray[116];
	int EndIndArray[116];

	for (int i = size; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			h = i;
			break;
		}
		array.push_back(host_MultipleLengthPst[i].PstTag);
		int gfsd = host_MultipleLengthPst[i].startIndex;
		int hshfla = host_MultipleLengthPst[i].endIndex;
		StartIndArray[i] = host_MultipleLengthPst[i].startIndex;
		EndIndArray[i] = host_MultipleLengthPst[i].endIndex;
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	StartIndArray[0] = StartIndArray[0];
	EndIndArray[0] = EndIndArray[0];
	//array[0] = array[0];
	h = h;
	PstSize = 4;
	numOfThr = dev_MultipleLengthPst2.size();
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	host_MultipleLengthPst = dev_MultipleLengthPst;
	host_MultipleLengthPst2 = dev_MultipleLengthPst2;
	size = size + host_MultipleLengthPst2.size();
	dev_MultipleLengthPst2.clear();
	array.clear();
	for (int i = size; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			h = i;
			break;
		}
		array.push_back(host_MultipleLengthPst[i].PstTag);
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	//array[0] = array[0];
	h = h;
	PstSize = 5;
	numOfThr = dev_MultipleLengthPst2.size();
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	host_MultipleLengthPst = dev_MultipleLengthPst;
	host_MultipleLengthPst2 = dev_MultipleLengthPst2;
	size = size + host_MultipleLengthPst2.size();
	dev_MultipleLengthPst2.clear();
	array.clear();
	string arr[1000];
	for (int i = size; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			h = i;
			break;
		}
		arr[i- size] = convertToString(host_MultipleLengthPst[i].PstTag, 8);
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	arr[0] = arr[0];
	//array[0] = array[0];
	h = h;
	PstSize = 6;
	numOfThr = dev_MultipleLengthPst2.size();
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	host_MultipleLengthPst = dev_MultipleLengthPst;
	host_MultipleLengthPst2 = dev_MultipleLengthPst2;
	size = size + host_MultipleLengthPst2.size();
	dev_MultipleLengthPst2.clear();
	string arr2[1000];
	for (int i = size; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			h = i;
			break;
		}
		arr2[i- size] = convertToString(host_MultipleLengthPst[i].PstTag, 8);
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	arr2[0] = arr2[0];
	//array[0] = array[0];
	h = h;
	PstSize = 7;
	numOfThr = dev_MultipleLengthPst2.size();
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	host_MultipleLengthPst = dev_MultipleLengthPst;
	host_MultipleLengthPst2 = dev_MultipleLengthPst2;
	size = size + host_MultipleLengthPst2.size();
	dev_MultipleLengthPst2.clear();

	for (int i = size; i < host_MultipleLengthPst.size(); i++)
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
		{
			h = i;
			break;
		}
		arr[i- size] = convertToString(host_MultipleLengthPst[i].PstTag, 8);
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	arr[0] = arr[0];
	StartIndArray[0] = StartIndArray[0];
	EndIndArray[0] = EndIndArray[0];
	//array[0] = array[0];
	h = h;
	PstSize = 8;
	numOfThr = dev_MultipleLengthPst2.size();
	GeneratingMultipleLengthPsts2 << <BLOCKS3, THREADS3 >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);
	// till here
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

	//WindowLaunchKernel << <BLOCKS2, THREADS2 >> > (NumOfThreadsToLaunch, minSum, maxSum, raw_ptr, sizeOfShortlistedData, raw_ptr2, SliderValue);

	//thrust::host_vector<_WindowCapturedElementsStruct> host_windowcapturedelements = device_windowcapturedelements;

	//double TunedMass = 0;
	//int oldElementCount = 0;

	//for (int x = 0; x < NumOfThreadsToLaunch; x++)
	//{
	//	if (oldElementCount < host_windowcapturedelements[x].elementCount)
	//	{
	//		oldElementCount = host_windowcapturedelements[x].elementCount;
	//		TunedMass = host_windowcapturedelements[x].TunedMass;
	//	}
	//	else if (oldElementCount == host_windowcapturedelements[x].elementCount)
	//	{
	//		if (abs(TunedMass - WholeProteinMass) >= abs(host_windowcapturedelements[x].TunedMass - WholeProteinMass))
	//		{
	//			TunedMass = host_windowcapturedelements[x].TunedMass;
	//		}
	//	}
	//}

	
	cudaDeviceSynchronize();
	return 1.1;
}
