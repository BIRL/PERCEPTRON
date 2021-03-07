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
#include "CudaHeaderFile.cuh"
#include "CudaHeaderForPst.cuh"

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
//__device__ int dev_data[50];
//__device__ int dev_count = 0;
//__device__ int dev_wind_count = 0;
//__device__ int dev_pst_count = 0;
//__device__ int multipleLengthPstCounter = 0;
//__device__ int charArrayCounter = 0;
//
//struct ParametersToCpp
//{
//	double MwTolerance;
//	double NeutralLoss;
//	double SliderValue;
//	double HopThreshhold;
//	int Autotune;
//	int DenovoAllow;
//	int MinimumPstLength;
//	int MaximumPstLength;
//};
//
//typedef struct _WindowCapturedElementsStruct
//{
//	double TunedMass;
//	int elementCount;
//} windowcapturedelementsstruct;
//
//typedef struct _ShortlistedMassSumsAndIntensities
//{
//	double massSum;
//	double AvgIntensity;
//	bool operator() (_ShortlistedMassSumsAndIntensities i, _ShortlistedMassSumsAndIntensities j) { return (i.massSum < j.massSum); }
//} ShortlistedMassSumsAndIntensities;
//
//typedef struct _DataForPsts
//{
//	int startIndex;
//	int endIndex;
//	double startIndexMass;
//	double endIndexMass;
//	double massDifferenceBetweenPeaks;
//	char AminoAcidSymbol;
//	double TagError;
//	double averageIntensity;
//} dataforpsts;
//
//typedef struct _PeptideSequenceTags
//{
//	char PstTag[8];
//	int PstTagLength;
//	double PstErrorScore;
//	double PstFrequency;
//	double IntensitySum;
//	int startIndex;
//	int endIndex;
//	double ErrorSum;
//	double RMSE;
//} peptidesequencetags;


//__device__ void my_push_back(double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, double summationOfMasses, double averageOfIntensities)
//{
//	int insert_pt = atomicAdd(&dev_count, 1);
//	dev_PeakListMassesSum[insert_pt] = summationOfMasses;
//	dev_PeakListIntensitiesAverage[insert_pt] = averageOfIntensities;
//	return;
//}

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

//__global__ void CalculatingTupleSumsAndSingleLengthPsts(double *raw_ptr_masses, double *raw_ptr_intensities, double *dev_PeakListMassesSum, double *dev_PeakListIntensitiesAverage, _DataForPsts *SingleLengthPSTs_ptr, double *dev_aminoAcidMassesList, char *dev_aminoAcidSymbolList, double MwTolerance, double NeutralLoss, double HopThreshold, int N, int AutoTune, int DenovoAllow) {
//	int tid = blockIdx.x * blockDim.x + threadIdx.x;
//
//	if (tid < N)
//	{
//		for (int i = tid + 1; i < N; i++)
//		{
//			double averageOfIntensities = (raw_ptr_intensities[tid] + raw_ptr_intensities[i]) / 2;
//			if (AutoTune == 1)
//			{
//				double summationOfMasses = raw_ptr_masses[tid] + raw_ptr_masses[i] + NeutralLoss;
//				my_push_back(dev_PeakListMassesSum, dev_PeakListIntensitiesAverage, summationOfMasses, averageOfIntensities);
//			}
//			if (DenovoAllow == 1)
//			{
//				double differenceOfMasses = fabs(raw_ptr_masses[tid] - raw_ptr_masses[i]);
//				for (int j = 0; j < 21; j++)
//				{
//					double TagError = pow(fabs(dev_aminoAcidMassesList[j] - differenceOfMasses), 2);
//					if (fabs(dev_aminoAcidMassesList[j] - differenceOfMasses) <= HopThreshold)
//					{
//						PST_push_back(SingleLengthPSTs_ptr, tid, i, raw_ptr_masses[tid], raw_ptr_masses[i], differenceOfMasses, dev_aminoAcidSymbolList[j], TagError, averageOfIntensities);
//					}
//				}
//			}		
//		}
//	}
//	else
//		return;
//}

//__device__ void window_push_back(_WindowCapturedElementsStruct *windowcapturedelements, double a, int b)
//{
//	int insert_ptr = atomicAdd(&dev_wind_count, 1);
//	windowcapturedelements[insert_ptr].TunedMass = a;
//	windowcapturedelements[insert_ptr].elementCount = b;
//	return;
//}

//__global__ void WindowLaunchKernel(int NumOfThreadsToLaunch, double minSum, double maxSum, _ShortlistedMassSumsAndIntensities *shortListedData, int sizeOfShortlistedData, _WindowCapturedElementsStruct *windowcapturedelements, double SliderValue)
//{
//	int tid = blockIdx.x * blockDim.x + threadIdx.x;
//	if (tid < NumOfThreadsToLaunch)
//	{
//		double WindowStart = minSum + (tid * SliderValue);
//		double WindowEnd = WindowStart + 1.00727647;
//		double sumoftunedmassesandintensities = 0;
//		double sumoftunedintensities = 0;
//		int Count = 0;
//		for (int i = 0; i < sizeOfShortlistedData; i++)
//		{
//			if (WindowStart <= shortListedData[i].massSum && shortListedData[i].massSum < WindowEnd)//#DISCUSSION
//			{
//				double data = shortListedData[i].massSum * shortListedData[i].AvgIntensity;
//				sumoftunedmassesandintensities = sumoftunedmassesandintensities + data;
//				sumoftunedintensities = sumoftunedintensities + shortListedData[i].AvgIntensity;
//				Count = Count + 1;
//			}
//			else if (shortListedData[i].massSum >= WindowEnd)
//			{
//				break;
//			}
//		}
//		double TunedMass = sumoftunedmassesandintensities / sumoftunedintensities;
//		int elementCount = Count;
//		window_push_back(windowcapturedelements, TunedMass, elementCount);
//	}
//}

//__device__ char * my_strcpy(char *dest, const char *src) {
//	int i = 0;
//	do {
//		dest[i] = src[i];
//	} while (src[i++] != 0);
//	return dest;
//}

//__global__ void GeneratingMultipleLengthPsts(_PeptideSequenceTags *MultipleLengthPst_ptr, _DataForPsts *SingleLengthPSTs, int N)
//{
//	int tid = blockIdx.x * blockDim.x + threadIdx.x;
//	if (tid < N)
//	{
//		int HomePeak = SingleLengthPSTs[tid].endIndex;
//		char HomePeakAA = SingleLengthPSTs[tid].AminoAcidSymbol;
//		int StartIndex = SingleLengthPSTs[tid].startIndex;
//		for (int i = 0; i < N; i++)
//		{
//			int HopPeak = SingleLengthPSTs[i].startIndex;
//			char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
//			int EndIndex = SingleLengthPSTs[i].endIndex;
//			if (HomePeak == HopPeak)
//			{
//				int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
//				MultipleLengthPst_ptr[insert_ptr].PstTag[0] = HomePeakAA;
//				MultipleLengthPst_ptr[insert_ptr].PstTag[1] = HopPeakAA;
//				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
//				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
//				MultipleLengthPst_ptr[insert_ptr].PstTagLength = 2;
//				MultipleLengthPst_ptr[insert_ptr].IntensitySum = SingleLengthPSTs[tid].averageIntensity + SingleLengthPSTs[i].averageIntensity;
//				MultipleLengthPst_ptr[insert_ptr].PstFrequency = ((SingleLengthPSTs[tid].averageIntensity + SingleLengthPSTs[i].averageIntensity) / 2)*(2*2);
//				MultipleLengthPst_ptr[insert_ptr].ErrorSum = SingleLengthPSTs[tid].TagError + SingleLengthPSTs[i].TagError;
//				MultipleLengthPst_ptr[insert_ptr].RMSE = MultipleLengthPst_ptr[insert_ptr].ErrorSum / 2;
//				double RMSE = (sqrt(MultipleLengthPst_ptr[insert_ptr].ErrorSum) / 2)*10;
//				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = exp(-RMSE * 2);
//				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = (MultipleLengthPst_ptr[insert_ptr].PstTagLength * MultipleLengthPst_ptr[insert_ptr].PstFrequency) / RMSE;
//			}
//		}
//	}
//}

//__global__ void GeneratingMultipleLengthPsts2(_PeptideSequenceTags *MultipleLengthPst_ptr, _PeptideSequenceTags *DupletPSTs, _DataForPsts *SingleLengthPSTs, int N, int num, int SizeOfPst)
//{
//	int tid = blockIdx.x * blockDim.x + threadIdx.x;
//	if (tid < N)
//	{
//		int HomePeak = DupletPSTs[tid].endIndex;
//		char *HomePeakAA = DupletPSTs[tid].PstTag;
//		int StartIndex = DupletPSTs[tid].startIndex;
//		for (int i = 0; i < num; i++)
//		{
//			int HopPeak = SingleLengthPSTs[i].startIndex;
//			char HopPeakAA = SingleLengthPSTs[i].AminoAcidSymbol;
//			int EndIndex = SingleLengthPSTs[i].endIndex;
//			if (HomePeak == HopPeak)
//			{
//				int insert_ptr = atomicAdd(&multipleLengthPstCounter, 1);
//				my_strcpy(MultipleLengthPst_ptr[insert_ptr].PstTag, HomePeakAA);
//				MultipleLengthPst_ptr[insert_ptr].PstTag[SizeOfPst-1] = HopPeakAA;
//				MultipleLengthPst_ptr[insert_ptr].startIndex = StartIndex;
//				MultipleLengthPst_ptr[insert_ptr].endIndex = EndIndex;
//				MultipleLengthPst_ptr[insert_ptr].PstTagLength = SizeOfPst;
//				MultipleLengthPst_ptr[insert_ptr].IntensitySum = DupletPSTs[tid].IntensitySum + SingleLengthPSTs[i].averageIntensity;
//				MultipleLengthPst_ptr[insert_ptr].PstFrequency = ((DupletPSTs[tid].IntensitySum + SingleLengthPSTs[i].averageIntensity) / SizeOfPst)*(SizeOfPst*SizeOfPst);
//				MultipleLengthPst_ptr[insert_ptr].ErrorSum = DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError;
//				MultipleLengthPst_ptr[insert_ptr].RMSE = (DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError) / SizeOfPst;
//				double RMSE = (sqrt(DupletPSTs[tid].ErrorSum + SingleLengthPSTs[i].TagError) / SizeOfPst) * 10;
//				MultipleLengthPst_ptr[insert_ptr].PstErrorScore = exp(-RMSE * 2);
//				//MultipleLengthPst_ptr[insert_ptr].PstErrorScore = (MultipleLengthPst_ptr[insert_ptr].PstTagLength * MultipleLengthPst_ptr[insert_ptr].PstFrequency) / RMSE;
//			}
//		}
//	}
//}
void AccomodateIsoforms(vector< _PeptideSequenceTags> &Final_MultipleLengthPsts, ParametersToCpp Parameters)
{
	char ResidueForReplacement[] = { 'L', 'D', 'N', 'E', 'Q' };
	char newResidue;
	int CountOfFinalPsts = Final_MultipleLengthPsts.size();
	int g = strlen(ResidueForReplacement);
	for (int i = 0; i < Final_MultipleLengthPsts.size(); i++)
	{
		for (int j = 0; j < 5; j++)
		{
			char OldResidue = ResidueForReplacement[j];
			if (strchr(Final_MultipleLengthPsts[i].PstTag, OldResidue))
			{
				if (OldResidue == 'L')//Here I think Switch Case will be more better....!!!!
					newResidue = 'I';
				else if (OldResidue == 'D')
					newResidue = 'B';
				else if (OldResidue == 'N')
					newResidue = 'B';
				else if (OldResidue == 'E')
					newResidue = 'Z';
				else if (OldResidue == 'Q' && Parameters.HopThreshhold <= 1.5)
					newResidue = 'Z';
				else if (OldResidue == 'Q' && Parameters.HopThreshhold > 1.5)
					newResidue = 'K';

				string BeforeAccomodatePst = Final_MultipleLengthPsts[i].PstTag;
				_PeptideSequenceTags AccomodatedPstTag = Final_MultipleLengthPsts[i];
				for (int iter = 0; iter < Final_MultipleLengthPsts[i].PstTagLength; iter++)
				{
					if (BeforeAccomodatePst[iter] == OldResidue)
					{
						BeforeAccomodatePst[iter] = newResidue;
						strcpy(AccomodatedPstTag.PstTag, BeforeAccomodatePst.c_str());
						Final_MultipleLengthPsts.push_back(AccomodatedPstTag);
					}
				}
			}
		}
	}
	for (int i = 0; i < Final_MultipleLengthPsts.size() - 1; i++)
	{
		if (strcmp(Final_MultipleLengthPsts[i].PstTag, Final_MultipleLengthPsts[i + 1].PstTag) == 0)
		{
			Final_MultipleLengthPsts.erase(Final_MultipleLengthPsts.begin() + i + 1);
			i--;
		}
	}
}

void FindingUniquePSTs(vector< _PeptideSequenceTags> &Final_MultipleLengthPsts)
{
	for (int i = 0; i < Final_MultipleLengthPsts.size() - 1; i++)	// extracting the unique PSTs from the total PSTs obtained. If 2 PST tags are same, their error sums and intensities are compared
	{
		if (strcmp(Final_MultipleLengthPsts[i].PstTag, Final_MultipleLengthPsts[i + 1].PstTag) == 0)
		{
			if (Final_MultipleLengthPsts[i].ErrorSum < Final_MultipleLengthPsts[i + 1].ErrorSum)
			{
				Final_MultipleLengthPsts.erase(Final_MultipleLengthPsts.begin() + i + 1);
				i--;
			}
			else if (Final_MultipleLengthPsts[i].ErrorSum > Final_MultipleLengthPsts[i + 1].ErrorSum)
			{
				Final_MultipleLengthPsts.erase(Final_MultipleLengthPsts.begin() + i);
				i--;
			}
			else
			{
				if (Final_MultipleLengthPsts[i].PstFrequency <= Final_MultipleLengthPsts[i + 1].PstFrequency)
				{
					Final_MultipleLengthPsts.erase(Final_MultipleLengthPsts.begin() + i);
					i--;
				}
				else if (Final_MultipleLengthPsts[i].PstFrequency > Final_MultipleLengthPsts[i + 1].PstFrequency)
				{
					Final_MultipleLengthPsts.erase(Final_MultipleLengthPsts.begin() + i + 1);
					i--;
				}
			}
		}
	}
}

vector<_PeptideSequenceTags> CalculatingPeptideSequenceTags(thrust::host_vector<_DataForPsts> Host_SingleLengthPSTs, ParametersToCpp Parameters, int zN)
{
	thrust::device_vector<_DataForPsts> Final_SingleLengthPSTs;

	for (int i = 0; i < zN; i++)	// Populating the single length PSTs calculated while running mass tuner function
	{
		if (Host_SingleLengthPSTs[i].startIndexMass == 0)
			break;
		Final_SingleLengthPSTs.push_back(Host_SingleLengthPSTs[i]);
	}
	_DataForPsts *Final_SingleLengthPSTs_ptr = thrust::raw_pointer_cast(Final_SingleLengthPSTs.data());	// Pointer pointing to single length PST vector that is to be sent to gpu

	thrust::device_vector<_PeptideSequenceTags> dev_MultipleLengthPst(1000);
	_PeptideSequenceTags *MultipleLengthPst_ptr = thrust::raw_pointer_cast(dev_MultipleLengthPst.data());	// multiple length PSTs pointer

	int numOfThr = Final_SingleLengthPSTs.size();	// each single length PST will be compared with the others in each thread
	int THREADSforPst = 256;	// optimal num of threads per block
	int BLOCKSforPst = numOfThr / THREADSforPst + 5;	// total threads we want to launch and the optimal threads will give us the num of blocks to be launched. +5 gist foe safety
	GeneratingMultipleLengthPsts << <BLOCKSforPst, THREADSforPst >> > (MultipleLengthPst_ptr, Final_SingleLengthPSTs_ptr, numOfThr);	// Here duplets of PSTs will be created in GPU
	thrust::host_vector<_PeptideSequenceTags> host_MultipleLengthPst = dev_MultipleLengthPst;	// Copying data back to CPU from GPU
	thrust::device_vector<_PeptideSequenceTags> dev_MultipleLengthPst2;

	for (int i = 0; i < host_MultipleLengthPst.size(); i++)	// Extracting the duplets and chopping the excess data so these duplets can be passed to gpu again to compute triplets
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
			break;
		dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
	}
	_PeptideSequenceTags *MultipleLengthPst_ptr2 = thrust::raw_pointer_cast(dev_MultipleLengthPst2.data());

	int size = 0;
	thrust::host_vector<_PeptideSequenceTags> host_MultipleLengthPst2;

	for (int num = 3; num <= Parameters.MaximumPstLength; num++)	// loop runs to calculate triplets, tetraplets, pentaplets and so on according to user defined parameters
	{
		numOfThr = dev_MultipleLengthPst2.size();
		int PstSize = num;
		GeneratingMultipleLengthPsts2 << <BLOCKSforPst, THREADSforPst >> > (MultipleLengthPst_ptr, MultipleLengthPst_ptr2, Final_SingleLengthPSTs_ptr, numOfThr, Final_SingleLengthPSTs.size(), PstSize);	// gpu kernel to calculate multiple length psts
		host_MultipleLengthPst = dev_MultipleLengthPst;

		size = size + dev_MultipleLengthPst2.size();
		dev_MultipleLengthPst2.clear();
		for (int i = size; i < host_MultipleLengthPst.size(); i++)	// Extracting the PSTs and chopping the excess data so these PSTs can be passed to gpu again to compute further longer length PSTs
		{
			if (host_MultipleLengthPst[i].endIndex == 0)
				break;
			dev_MultipleLengthPst2.push_back(host_MultipleLengthPst[i]);
		}
	}

	vector<_PeptideSequenceTags> Final_MultipleLengthPsts;
	for (int i = 0; i < host_MultipleLengthPst.size(); i++)	// Final multiple length PSTs being stored into vector according to user defined min and max PST length
	{
		if (host_MultipleLengthPst[i].endIndex == 0)
			break;
		if (host_MultipleLengthPst[i].PstTagLength >= Parameters.MinimumPstLength && host_MultipleLengthPst[i].PstTagLength <= Parameters.MaximumPstLength)
		{
			Final_MultipleLengthPsts.push_back(host_MultipleLengthPst[i]);
		}
	}

	std::sort(Final_MultipleLengthPsts.begin(), Final_MultipleLengthPsts.end(),	// sorting the data according to the PST tags
		[](const _PeptideSequenceTags &pst1, const _PeptideSequenceTags &pst2)
	{ return strcmp(pst1.PstTag, pst2.PstTag) < 0; });

	Final_MultipleLengthPsts[0] = Final_MultipleLengthPsts[0];	// DELME Just for checking values
	
	FindingUniquePSTs(Final_MultipleLengthPsts);	// Function call to extract unique PSTs
	
	Final_MultipleLengthPsts[0] = Final_MultipleLengthPsts[0];	// DELME Just for checking values

	AccomodateIsoforms(Final_MultipleLengthPsts, Parameters);	// Isoforms are being accomodated here and then their unique is taken

	Final_MultipleLengthPsts[0] = Final_MultipleLengthPsts[0];	// DELME Just for checking values
	return Final_MultipleLengthPsts;
}

extern "C" __declspec(dllexport) double __cdecl
wholeproteinmasstunerandpst(double PeakListMasses[], double PeakListIntensities[], int PeakListLength, ParametersToCpp Parameters)
{
	double WholeProteinMass = PeakListMasses[0];
	vector<double> masses;  vector<double> intensities;
	for (int i = 0; i < PeakListLength; i++)
	{
		masses.push_back(PeakListMasses[i]);
		intensities.push_back(PeakListIntensities[i]);
	}

	const int N = PeakListLength;
	const int zN = (floor(PeakListLength*PeakListLength) / 2) - (floor(PeakListLength / 2));
	double *dev_masses, *dev_intensities;

	double aminoAcidMassesList[21] = { 57.0214600000000, 71.0371100000000, 87.0320300000000, 97.0527600000000, 99.0684100000000, 101.047680000000, 103.009190000000, 113.084060000000, 114.042930000000, 115.026940000000, 128.058580000000, 128.094960000000, 129.042590000000, 131.040490000000, 137.058910000000, 147.068410000000, 156.101110000000, 163.063330000000, 168.964203000000, 186.079310000000, 255.158295000000 };
	char aminoAcidSymbolList[21] = { 'G', 'A', 'S', 'P', 'V', 'T', 'C', 'L', 'N', 'D', 'Q', 'K', 'E', 'M', 'H', 'F', 'R', 'Y', 'U', 'W', 'O' };
	
	double *dev_aminoAcidMassesList; char *dev_aminoAcidSymbolList;

	cudaMalloc((void**)&dev_masses, sizeof(double) * N);
	cudaMalloc((void**)&dev_intensities, sizeof(double) * N);
	cudaMalloc((void**)&dev_aminoAcidMassesList, sizeof(double)*21);
	cudaMalloc((void**)&dev_aminoAcidSymbolList, sizeof(char)*21);

	thrust::device_vector<double> DevicePeakListMassesSum(zN);
	double *DevicePeakListMassesSum_ptr = thrust::raw_pointer_cast(DevicePeakListMassesSum.data());
	thrust::device_vector<double> DevicePeakListAvgIntensities(zN);
	double *DevicePeakListAvgIntensities_ptr = thrust::raw_pointer_cast(DevicePeakListAvgIntensities.data());
	thrust::device_vector<_DataForPsts> DeviceSingleLengthPSTs(zN);
	_DataForPsts *SingleLengthPSTs_ptr = thrust::raw_pointer_cast(DeviceSingleLengthPSTs.data());

	cudaMemcpy(dev_masses, PeakListMasses, N * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_intensities, PeakListIntensities, N * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_aminoAcidMassesList, aminoAcidMassesList, 21 * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_aminoAcidSymbolList, aminoAcidSymbolList, 21 * sizeof(char), cudaMemcpyHostToDevice);

	int THREADS = 256;
	int BLOCKS = (N/THREADS + 5);
	CalculatingTupleSumsAndSingleLengthPsts << < BLOCKS, THREADS >> > (dev_masses, dev_intensities, DevicePeakListMassesSum_ptr, DevicePeakListAvgIntensities_ptr, SingleLengthPSTs_ptr, dev_aminoAcidMassesList, dev_aminoAcidSymbolList, Parameters.MwTolerance, Parameters.NeutralLoss, Parameters.HopThreshhold, N, Parameters.Autotune, Parameters.DenovoAllow);

	thrust::host_vector<_DataForPsts> Host_SingleLengthPSTs = DeviceSingleLengthPSTs;
	thrust::host_vector<double> PeakListMassesSum = DevicePeakListMassesSum;
	thrust::host_vector<double> PeakListIntensitiesAverage = DevicePeakListAvgIntensities;
	thrust::host_vector<_ShortlistedMassSumsAndIntensities> shortlistedMassSumAndIntensities;

	for (int i = 0; i < zN; i++)
	{
		if (PeakListMasses[0] - Parameters.MwTolerance <= PeakListMassesSum[i] && PeakListMassesSum[i] <= PeakListMasses[0] + Parameters.MwTolerance)
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

	double minSum = shortlistedMassSumAndIntensities[0].massSum;
	double maxSum = shortlistedMassSumAndIntensities[shortlistedMassSumAndIntensities.size() - 1].massSum;

	int sizeOfShortlistedData = shortlistedMassSumAndIntensities.size();
	double SliderValue = (WholeProteinMass * Parameters.SliderValue) / (pow(10.0, 6.0));
	int NumOfThreadsToLaunch = floor((maxSum - minSum) * (1 / SliderValue));

	thrust::device_vector<_ShortlistedMassSumsAndIntensities> device_shortlistedMassSumAndIntensities = shortlistedMassSumAndIntensities;
	_ShortlistedMassSumsAndIntensities *raw_ptr = thrust::raw_pointer_cast(device_shortlistedMassSumAndIntensities.data());

	thrust::device_vector<_WindowCapturedElementsStruct> device_windowcapturedelements(NumOfThreadsToLaunch);
	_WindowCapturedElementsStruct *raw_ptr2 = thrust::raw_pointer_cast(device_windowcapturedelements.data());

	int THREADS2 = 256;
	int BLOCKS2 = (NumOfThreadsToLaunch / THREADS + 5);

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

	// --------- PST STARTS HERE ---------

	vector<_PeptideSequenceTags> PeptideSequenceTags = CalculatingPeptideSequenceTags(Host_SingleLengthPSTs, Parameters, zN);

	// --------- PST ENDS HERE ---------

	cudaDeviceSynchronize();
	return TunedMass;
}
