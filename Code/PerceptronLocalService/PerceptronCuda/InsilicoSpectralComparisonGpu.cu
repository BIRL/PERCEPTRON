#include <iostream>
#include <stdio.h>
#include <cublas.h>

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
#include <vector>
//#include "CudaHeaderFile.cuh"
using namespace std;

__device__ int index_count = 0;

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
	const char* peptideToleranceUnit;
	double peptideTolerance;
};

struct ProteinStructFromCS
{
	const char* Header;
	double* InsilicoMassLeft;
	double* InsilicoMassRight;
	double* InsilicoMassLeftAo;
	double* InsilicoMassLeftBo;
	double* InsilicoMassLeftAstar;
	double* InsilicoMassLeftBstar;
	double* InsilicoMassRightYo;
	double* InsilicoMassRightYstar;
	double* InsilicoMassRightZo;
	double* InsilicoMassRightZoo;
	int* SizeOfAllInsilicoArrays;
};
struct ProteinStructToReturn
{
	int Header;
	int MatchCounter;
	double InsilicoScore;
	int* LeftMatchedIndex;
	int* RightMatchedIndex;
	int* LeftPeakIndex;
	int* RightPeakIndex;
	int* LeftType;
	int* RightType;
	int LeftCount;
	int RightCount;
};

typedef struct ToDefineSizeStruct
{
	int sizeOfArray;
}Todefinesizestruct;

int PeptideTolUnitMapping(string pepUnit)
{
	int PepUnit;
	if (pepUnit == "Da")
		return PepUnit = 1;
	if (pepUnit == "mmu")
		return PepUnit = 2;
	if (pepUnit == "ppm")
		return PepUnit = 3;
}



__device__ void SpectralComparison(double difference, double dev_intensity, int indexPeakList, double peakDifferenceTolerance, int &Consecutive, int &Counter,
	int &OldConsec, int &OldConsec2, int &ConsecutiveRegion, double &Matches_Score, int &MatchCounter, int *Matched_IndexList, int *Peak_IndexList,
	int indexSide, int Type, int *TypeList, int &insert_ptr)
{
	double absdifference = fabs(difference);  //Taking Absoulte difference {Doesn't matter}
	int *acfg;
	if (absdifference <= peakDifferenceTolerance)
	{
		if (Consecutive == OldConsec + 1 && OldConsec == OldConsec2 + 1)
		{
			if (Counter == 0)
			{
				ConsecutiveRegion = ConsecutiveRegion + 1;
			}
			Counter = Counter + 1;
			Matches_Score = Matches_Score + 1.5;
			OldConsec2 = OldConsec;
			OldConsec = Consecutive;
		}
		else if (Consecutive == OldConsec && OldConsec == OldConsec2 + 1)
		{
			Counter = Counter + 1;
			Matches_Score = Matches_Score + 1.5;
		}
		else
		{
			Counter = 0;
			Matches_Score = Matches_Score + dev_intensity;
			OldConsec2 = OldConsec;
			OldConsec = Consecutive;
		}
		//int insert_ptr = atomicAdd(&index_count, 1);
		Matched_IndexList[insert_ptr] = indexSide;
		Peak_IndexList[insert_ptr] = indexPeakList;
		TypeList[insert_ptr] = Type;
		MatchCounter = MatchCounter + 1;
		insert_ptr = insert_ptr + 1;
	}
}



__global__ void ComputeInsilicoScore(ProteinStructFromCS *h_a, ProteinStructToReturn *DeviceCandidateProteinReturnPtr,
	int candidateProteinsCount, ParametersToCpp Parameters, double *dev_masses, double *dev_intensities, int PeakListCount, int intPepUnit)
{
	int tid = blockIdx.x * blockDim.x + threadIdx.x;

	if (tid < candidateProteinsCount)
	{
		ProteinStructFromCS temp = h_a[tid];

		int Type;
		int InsilicoMassLeftCount = temp.SizeOfAllInsilicoArrays[0], InsilicoMassRightCount = temp.SizeOfAllInsilicoArrays[1],
			InsilicoMassLeftAoCount = temp.SizeOfAllInsilicoArrays[2], InsilicoMassLeftBoCount = temp.SizeOfAllInsilicoArrays[3],
			InsilicoMassLeftAstarCount = temp.SizeOfAllInsilicoArrays[4], InsilicoMassLeftBstarCount = temp.SizeOfAllInsilicoArrays[5],
			InsilicoMassRightYoCount = temp.SizeOfAllInsilicoArrays[6], InsilicoMassRightYstarCount = temp.SizeOfAllInsilicoArrays[7],
			InsilicoMassRightZoCount = temp.SizeOfAllInsilicoArrays[8], InsilicoMassRightZooCount = temp.SizeOfAllInsilicoArrays[9];


		int SpecialLeftFragments = temp.SizeOfAllInsilicoArrays[0] + temp.SizeOfAllInsilicoArrays[2] + temp.SizeOfAllInsilicoArrays[3] +
			temp.SizeOfAllInsilicoArrays[4] + temp.SizeOfAllInsilicoArrays[5];
		int SpecialRightFragments = temp.SizeOfAllInsilicoArrays[1] + temp.SizeOfAllInsilicoArrays[6] + temp.SizeOfAllInsilicoArrays[7] +
			temp.SizeOfAllInsilicoArrays[8] + temp.SizeOfAllInsilicoArrays[9];

		double Matches_Score = 0;
		int MatchCounter = 0;
		int Counter = 0;
		int OldConsec = -1;
		int OldConsec2 = -1;
		int ConsecutiveRegion = 0;
		int IdxL = 0;
		int IdxR = 0;
		int LeftMatched_Index[5000];
		int LeftPeak_Index[5000];
		int LeftType[5000];
		int RightMatched_Index[5000];
		int RightPeak_Index[5000];
		int RightType[5000];
		int insert_ptrLeft = 0;
		int insert_ptrRight = 0;
		double a = 0.0;
		double b = 0.0;

		for (int indexPeakList = 1; indexPeakList < PeakListCount; indexPeakList++)//indexPeakList = 1; indexPeakList < PeakListCount; indexPeakList++)
		{
			double peakDifferenceTolerance;
			if (intPepUnit == 1 || intPepUnit == 2)
				peakDifferenceTolerance = Parameters.peptideTolerance;
			else if (intPepUnit == 3)
				peakDifferenceTolerance = (Parameters.peptideTolerance * dev_masses[indexPeakList]) / 1000000;
			int Consecutive = indexPeakList;
			for (int indexLeftSide = IdxL; indexLeftSide < InsilicoMassLeftCount; indexLeftSide++) //indexLeftSide = IdxL; indexLeftSide < InsilicoMassLeftCount; indexLeftSide++)	//indexLeftSide = 27; indexLeftSide < 28; indexLeftSide++) 
			{
				Type = 1;
				a = dev_masses[indexPeakList];
				b = temp.InsilicoMassLeft[indexLeftSide];
				double difference = dev_masses[indexPeakList] - temp.InsilicoMassLeft[indexLeftSide];
				SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType, insert_ptrLeft);

				if (SpecialLeftFragments > 0)
				{
					if (InsilicoMassLeftAoCount > 0)
					{
						Type = 2;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassLeftAo[indexLeftSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType, insert_ptrLeft);
					}
					if (InsilicoMassLeftBoCount > 0)
					{
						Type = 3;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassLeftBo[indexLeftSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType, insert_ptrLeft);
					}
					if (InsilicoMassLeftAstarCount > 0)
					{
						Type = 4;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassLeftAstar[indexLeftSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType, insert_ptrLeft);
					}
					if (InsilicoMassLeftBstarCount > 0)
					{
						Type = 5;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassLeftBstar[indexLeftSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, LeftMatched_Index, LeftPeak_Index, indexLeftSide, Type, LeftType, insert_ptrLeft);
						//insert_ptr++;
					}
				}
				if (difference < -peakDifferenceTolerance && indexLeftSide > 0)  // Updated 20200917   -- Changed from 1 to 0
				{
					IdxL = indexLeftSide - 1;
					break;
				}
			}


			for (int indexRightSide = IdxR; indexRightSide < InsilicoMassLeftCount; indexRightSide++)
			{
				Type = 6;
				double difference = dev_masses[indexPeakList] - temp.InsilicoMassRight[indexRightSide];
				SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType, insert_ptrRight);
				if (SpecialLeftFragments > 0)
				{
					if (InsilicoMassRightYoCount > 0)
					{
						Type = 7;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassRightYo[indexRightSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType, insert_ptrRight);
					}
					if (InsilicoMassRightYstarCount > 0)
					{
						Type = 8;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassRightYstar[indexRightSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType, insert_ptrRight);
					}
					if (InsilicoMassRightZoCount > 0)
					{
						Type = 9;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassRightZo[indexRightSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType, insert_ptrRight);
					}
					if (InsilicoMassRightZooCount > 0)
					{
						Type = 10;
						difference = dev_masses[indexPeakList] - temp.InsilicoMassRightZoo[indexRightSide];
						SpectralComparison(difference, dev_intensities[indexPeakList], indexPeakList, peakDifferenceTolerance, Consecutive, Counter, OldConsec, OldConsec2, ConsecutiveRegion, Matches_Score, MatchCounter, RightMatched_Index, RightPeak_Index, indexRightSide, Type, RightType, insert_ptrRight);
					}
				}
				if (difference < -peakDifferenceTolerance && indexRightSide > 0)
				{
					IdxR = indexRightSide - 1;
					break;
				}
			}
		}

		DeviceCandidateProteinReturnPtr[tid].Header = tid;
		DeviceCandidateProteinReturnPtr[tid].InsilicoScore = Matches_Score / PeakListCount;
		DeviceCandidateProteinReturnPtr[tid].MatchCounter = MatchCounter;

		for (int i = 0; i < insert_ptrLeft; i++)
		{
			DeviceCandidateProteinReturnPtr[tid].LeftMatchedIndex[i] = LeftMatched_Index[i];
			DeviceCandidateProteinReturnPtr[tid].LeftPeakIndex[i] = LeftPeak_Index[i];
			DeviceCandidateProteinReturnPtr[tid].LeftType[i] = LeftType[i];
		}

		for (int i = 0; i < insert_ptrRight; i++)
		{
			DeviceCandidateProteinReturnPtr[tid].RightMatchedIndex[i] = RightMatched_Index[i];
			DeviceCandidateProteinReturnPtr[tid].RightPeakIndex[i] = RightPeak_Index[i];
			DeviceCandidateProteinReturnPtr[tid].RightType[i] = RightType[i];
		}

		DeviceCandidateProteinReturnPtr[tid].LeftCount = insert_ptrLeft;
		DeviceCandidateProteinReturnPtr[tid].RightCount = insert_ptrRight;
	}

}

extern "C" __declspec(dllexport) int __cdecl
insilicospectralcomparisongpu(ParametersToCpp Parameters, ProteinStructFromCS **candidateProteins, double *PeakListMasses,
	double *PeakListIntensities, int PeakListCount, int candidateProteinsCount, ProteinStructToReturn **DataToReturn)
{
	ProteinStructFromCS *CandidateProteinsToCuda = new ProteinStructFromCS[candidateProteinsCount];
	ProteinStructFromCS *dev_CandidateProteinsToCuda;

	vector<double> ghk;
	for (int i = 0; i < PeakListCount; i++)
	{
		ghk.push_back(PeakListMasses[i]);
	}


	thrust::host_vector<ProteinStructFromCS> host_CandidateProteinsToCuda;
	ProteinStructFromCS *h_a = new ProteinStructFromCS[candidateProteinsCount];
	ProteinStructFromCS *d_a;
	int *SizeOfAllInsilicoArrays = new int[10];

	//
	int SizeOfLeftIonsTotal = 0;
	int *SizeOfIndividualLeftIons = new int[candidateProteinsCount];
	//
	int SizeOfRightIonsTotal = 0;
	int *SizeOfIndividualRightIons = new int[candidateProteinsCount];

	int SizeOfLeftAoIonsTotal = 0;
	int *SizeOfIndividualLeftAoIons = new int[candidateProteinsCount];

	int SizeOfLeftBoIonsTotal = 0;
	int *SizeOfIndividualLeftBoIons = new int[candidateProteinsCount];

	int SizeOfLeftAstarIonsTotal = 0;
	int *SizeOfIndividualLeftAstarIons = new int[candidateProteinsCount];


	int SizeOfLeftBstarIonsTotal = 0;
	int *SizeOfIndividualLeftBstarIons = new int[candidateProteinsCount];


	int SizeOfRightYoIonsTotal = 0;
	int *SizeOfIndividualRightYoIons = new int[candidateProteinsCount];

	int SizeOfRightYstarIonsTotal = 0;
	int *SizeOfIndividualRightYstarIons = new int[candidateProteinsCount];

	int SizeOfRightZoIonsTotal = 0;
	int *SizeOfIndividualRightZoIons = new int[candidateProteinsCount];

	int SizeOfRightZooIonsTotal = 0;
	int *SizeOfIndividualRightZooIons = new int[candidateProteinsCount];


	for (int i = 0; i < candidateProteinsCount; i++)
	{
		SizeOfAllInsilicoArrays = (*candidateProteins)->SizeOfAllInsilicoArrays;

		CandidateProteinsToCuda[i] = (**candidateProteins);
		SizeOfIndividualLeftIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[0];
		SizeOfLeftIonsTotal = SizeOfLeftIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[0];

		SizeOfIndividualRightIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[1];
		SizeOfRightIonsTotal = SizeOfRightIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[1];

		SizeOfIndividualLeftAoIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[2];
		SizeOfLeftAoIonsTotal = SizeOfLeftAoIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[2];

		SizeOfIndividualLeftBoIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[3];
		SizeOfLeftBoIonsTotal = SizeOfLeftBoIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[3];

		SizeOfIndividualLeftAstarIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[4];
		SizeOfLeftAstarIonsTotal = SizeOfLeftAstarIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[4];




		SizeOfIndividualLeftBstarIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[5];
		SizeOfLeftBstarIonsTotal = SizeOfLeftBstarIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[5];

		SizeOfIndividualRightYoIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[6];
		SizeOfRightYoIonsTotal = SizeOfRightYoIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[6];

		SizeOfIndividualRightYstarIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[7];
		SizeOfRightYstarIonsTotal = SizeOfRightYstarIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[7];

		SizeOfIndividualRightZoIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[8];
		SizeOfRightZoIonsTotal = SizeOfRightZoIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[8];


		SizeOfIndividualRightZooIons[i] = (*candidateProteins)->SizeOfAllInsilicoArrays[9];
		SizeOfRightZooIonsTotal = SizeOfRightZooIonsTotal + (*candidateProteins)->SizeOfAllInsilicoArrays[9];

		//////////////////////////////////////////////////////////////////////
		//SizeOfIndividualLeftMatchedIndex[i] = 5000;
		//SizeOfIndividualRightMatchedIndex[i] = 5000;
		//SizeOfIndividualLeftPeakIndex[i] = 5000;
		//SizeOfIndividualRightPeakIndex[i] = 5000;
		//SizeOfIndividualLeftType[i] = 5000;
		//SizeOfIndividualRightType[i] = 5000;
		//////////////////////////////////////////////////////////////////


		host_CandidateProteinsToCuda.push_back(**candidateProteins);

		candidateProteins++;
	}


	double* h_arr = new double[SizeOfLeftIonsTotal];

	double* h_arr_Right = new double[SizeOfRightIonsTotal];
	double* h_arr_LeftAo = new double[SizeOfLeftAoIonsTotal];
	double* h_arr_LeftBo = new double[SizeOfLeftBoIonsTotal];
	double* h_arr_LeftAstar = new double[SizeOfLeftAstarIonsTotal];
	double* h_arr_LeftBstar = new double[SizeOfLeftBstarIonsTotal];
	double* h_arr_RightYo = new double[SizeOfRightYoIonsTotal];
	double* h_arr_RightYstar = new double[SizeOfRightYstarIonsTotal];
	double* h_arr_RightZo = new double[SizeOfRightZoIonsTotal];
	double* h_arr_RightZoo = new double[SizeOfRightZooIonsTotal];

	int sizeOfSizesArrayTotal = 10 * candidateProteinsCount;
	int *h_arrSizes = new int[sizeOfSizesArrayTotal];

	int k = 0, kRight = 0, kLeftAo = 0, kLeftBo = 0, kLeftAstar = 0, kLeftBstar = 0,
		kRightYo = 0, kRightYstar = 0, kRightZo = 0, kRightZoo = 0;

	int kSizes = 0;
	for (int i = 0; i < candidateProteinsCount; i++)
	{
		for (int j = 0; j < 10; j++)
		{
			h_arrSizes[kSizes] = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[j];
			kSizes++;
		}

		///Left
		int InnerCount = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[0];
		for (int j = 0; j < InnerCount; j++)
		{
			h_arr[k] = host_CandidateProteinsToCuda[i].InsilicoMassLeft[j];
			k++;
		}
		////

		int InnerCountRight = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[1];
		for (int j = 0; j < InnerCountRight; j++)
		{
			h_arr_Right[kRight] = host_CandidateProteinsToCuda[i].InsilicoMassRight[j];
			kRight++;
		}


		int InnerCountLeftAo = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[2];
		for (int j = 0; j < InnerCountLeftAo; j++)
		{
			h_arr_LeftAo[kLeftAo] = host_CandidateProteinsToCuda[i].InsilicoMassLeftAo[j];
			kLeftAo++;
		}


		int InnerCountLeftBo = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[3];
		for (int j = 0; j < InnerCountLeftBo; j++)
		{
			h_arr_LeftBo[kLeftBo] = host_CandidateProteinsToCuda[i].InsilicoMassLeftBo[j];
			kLeftBo++;
		}


		int InnerCountLeftAstar = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[4];
		for (int j = 0; j < InnerCountLeftAstar; j++)
		{
			h_arr_LeftAstar[kLeftAstar] = host_CandidateProteinsToCuda[i].InsilicoMassLeftAstar[j];
			kLeftAstar++;
		}


		int InnerCountLeftBstar = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[5];
		for (int j = 0; j < InnerCountLeftBstar; j++)
		{
			h_arr_LeftBstar[kLeftBstar] = host_CandidateProteinsToCuda[i].InsilicoMassLeftBstar[j];
			kLeftBstar++;
		}

		int InnerCountRightYo = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[6];
		for (int j = 0; j < InnerCountRightYo; j++)
		{
			h_arr_RightYo[kRightYo] = host_CandidateProteinsToCuda[i].InsilicoMassRightYo[j];
			kRightYo++;
		}


		int InnerCountRightYstar = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[7];
		for (int j = 0; j < InnerCountRightYstar; j++)
		{
			h_arr_RightYstar[kRightYstar] = host_CandidateProteinsToCuda[i].InsilicoMassRightYstar[j];
			kRightYstar++;
		}


		int InnerCountRightZo = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[8];
		for (int j = 0; j < InnerCountRightZo; j++)
		{
			h_arr_RightZo[kRightZo] = host_CandidateProteinsToCuda[i].InsilicoMassRightZo[j];
			kRightZo++;
		}


		int InnerCountRightZoo = CandidateProteinsToCuda[i].SizeOfAllInsilicoArrays[9];
		for (int j = 0; j < InnerCountRightZoo; j++)
		{
			h_arr_RightZoo[kRightZoo] = host_CandidateProteinsToCuda[i].InsilicoMassRightZoo[j];
			kRightZoo++;
		}
	}

	double *d_arr, *d_arrRight, *d_arrLeftAo, *d_arrLeftBo, *d_arrLeftAstar, *d_arrLeftBstar,
		*d_arrRightYo, *d_arrRightYstar, *d_arrRightZo, *d_arrRightZoo;
	int *d_arrSizes;

	cudaMalloc((void**) &(d_arrSizes), sizeof(int)*sizeOfSizesArrayTotal);
	cudaMemcpy(d_arrSizes, h_arrSizes, sizeof(int)*sizeOfSizesArrayTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arr), sizeof(double)*SizeOfLeftIonsTotal);
	cudaMemcpy(d_arr, h_arr, sizeof(double)*SizeOfLeftIonsTotal, cudaMemcpyHostToDevice);
	//


	cudaMalloc((void**) &(d_arrRight), sizeof(double)*SizeOfRightIonsTotal);
	cudaMemcpy(d_arrRight, h_arr_Right, sizeof(double)*SizeOfRightIonsTotal, cudaMemcpyHostToDevice);


	cudaMalloc((void**) &(d_arrLeftAo), sizeof(double)*SizeOfLeftAoIonsTotal);
	cudaMemcpy(d_arrLeftAo, h_arr_LeftAo, sizeof(double)*SizeOfLeftAoIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrLeftBo), sizeof(double)*SizeOfLeftBoIonsTotal);
	cudaMemcpy(d_arrLeftBo, h_arr_LeftBo, sizeof(double)*SizeOfLeftBoIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrLeftAstar), sizeof(double)*SizeOfLeftAstarIonsTotal);
	cudaMemcpy(d_arrLeftAstar, h_arr_LeftAstar, sizeof(double)*SizeOfLeftAstarIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrLeftBstar), sizeof(double)*SizeOfLeftBstarIonsTotal);
	cudaMemcpy(d_arrLeftBstar, h_arr_LeftBstar, sizeof(double)*SizeOfLeftBstarIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrRightYo), sizeof(double)*SizeOfRightYoIonsTotal);
	cudaMemcpy(d_arrRightYo, h_arr_RightYo, sizeof(double)*SizeOfRightYoIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrRightYstar), sizeof(double)*SizeOfRightYstarIonsTotal);
	cudaMemcpy(d_arrRightYstar, h_arr_RightYstar, sizeof(double)*SizeOfRightYstarIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrRightZo), sizeof(double)*SizeOfRightZoIonsTotal);
	cudaMemcpy(d_arrRightZo, h_arr_RightZo, sizeof(double)*SizeOfRightZoIonsTotal, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_arrRightZoo), sizeof(double)*SizeOfRightZooIonsTotal);
	cudaMemcpy(d_arrRightZoo, h_arr_RightZoo, sizeof(double)*SizeOfRightZooIonsTotal, cudaMemcpyHostToDevice);

	int megaSize = candidateProteinsCount * 5000;

	int *h_LeftMatchedIndex = new int[megaSize], *h_RightMatchedIndex = new int[megaSize], *h_LeftPeakIndex = new int[megaSize], *h_RightPeakIndex = new int[megaSize], *h_LeftType = new int[megaSize], *h_RightType = new int[megaSize];
	for (int i = 0; i < megaSize; i++)
	{
		h_LeftMatchedIndex[i] = 0;
		h_RightMatchedIndex[i] = 0;
		h_LeftPeakIndex[i] = 0;
		h_RightPeakIndex[i] = 0;
		h_LeftType[i] = 0;
		h_RightType[i] = 0;
	}


	int *d_LeftMatchedIndex, *d_RightMatchedIndex, *d_LeftPeakIndex, *d_RightPeakIndex, *d_LeftType, *d_RightType;
	cudaMalloc((void**) &(d_LeftMatchedIndex), sizeof(int)*megaSize);
	cudaMemcpy(d_LeftMatchedIndex, h_LeftMatchedIndex, sizeof(int)*megaSize, cudaMemcpyHostToDevice);
	cudaMalloc((void**) &(d_RightMatchedIndex), sizeof(int)*megaSize);
	cudaMemcpy(d_RightMatchedIndex, h_RightMatchedIndex, sizeof(int)*megaSize, cudaMemcpyHostToDevice);
	cudaMalloc((void**) &(d_LeftPeakIndex), sizeof(int)*megaSize);
	cudaMemcpy(d_LeftPeakIndex, h_LeftPeakIndex, sizeof(int)*megaSize, cudaMemcpyHostToDevice);
	cudaMalloc((void**) &(d_RightPeakIndex), sizeof(int)*megaSize);
	cudaMemcpy(d_RightPeakIndex, h_RightPeakIndex, sizeof(int)*megaSize, cudaMemcpyHostToDevice);
	cudaMalloc((void**) &(d_LeftType), sizeof(int)*megaSize);
	cudaMemcpy(d_LeftType, h_LeftType, sizeof(int)*megaSize, cudaMemcpyHostToDevice);
	cudaMalloc((void**) &(d_RightType), sizeof(int)*megaSize);
	cudaMemcpy(d_RightType, h_RightType, sizeof(int)*megaSize, cudaMemcpyHostToDevice);

	ProteinStructToReturn *h_return = new ProteinStructToReturn[candidateProteinsCount];
	ProteinStructToReturn *d_return;
	///////////////////////////////////////////////////////////////

	int PrevLeftIons = 0, PrevRightIons = 0, PrevLeftAoIons = 0, PrevLeftBoIons = 0,
		PrevLeftAstarIons = 0, PrevLeftBstarIons = 0,
		PrevRightYoIons = 0, PrevRightYstarIons = 0, PrevRightZoIons = 0, PrevRightZooIons = 0;

	for (int i = 0; i < candidateProteinsCount; i++)
	{
		//////////////////////////////////////////////////////////////
		h_return[i].LeftMatchedIndex = d_LeftMatchedIndex + (i * 5000);
		h_return[i].LeftPeakIndex = d_LeftPeakIndex + (i * 5000);
		h_return[i].LeftType = d_LeftType + (i * 5000);
		h_return[i].RightMatchedIndex = d_RightMatchedIndex + (i * 5000);
		h_return[i].RightPeakIndex = d_RightPeakIndex + (i * 5000);
		h_return[i].RightType = d_RightType + (i * 5000);
		////////////////////////////////////////////////////////////

		h_a[i].SizeOfAllInsilicoArrays = d_arrSizes + (i * 10);

		h_a[i].InsilicoMassLeft = d_arr + (PrevLeftIons);
		PrevLeftIons = PrevLeftIons + SizeOfIndividualLeftIons[i];
		///

		h_a[i].InsilicoMassRight = d_arrRight + (PrevRightIons);
		PrevRightIons = PrevRightIons + SizeOfIndividualRightIons[i];

		h_a[i].InsilicoMassLeftAo = d_arrLeftAo + (PrevLeftAoIons);
		PrevLeftAoIons = PrevLeftAoIons + SizeOfIndividualLeftAoIons[i];

		h_a[i].InsilicoMassLeftBo = d_arrLeftBo + (PrevLeftBoIons);
		PrevLeftBoIons = PrevLeftBoIons + SizeOfIndividualLeftBoIons[i];

		h_a[i].InsilicoMassLeftAstar = d_arrLeftAstar + (PrevLeftAstarIons);
		PrevLeftAstarIons = PrevLeftAstarIons + SizeOfIndividualLeftAstarIons[i];

		h_a[i].InsilicoMassLeftBstar = d_arrLeftBstar + (PrevLeftBstarIons);
		PrevLeftBstarIons = PrevLeftBstarIons + SizeOfIndividualLeftBstarIons[i];

		h_a[i].InsilicoMassRightYo = d_arrRightYo + (PrevRightYoIons);
		PrevRightYoIons = PrevRightYoIons + SizeOfIndividualRightYoIons[i];

		h_a[i].InsilicoMassRightYstar = d_arrRightYstar + (PrevRightYstarIons);
		PrevRightYstarIons = PrevRightYstarIons + SizeOfIndividualRightYstarIons[i];

		h_a[i].InsilicoMassRightZo = d_arrRightZo + (PrevRightZoIons);
		PrevRightZoIons = PrevRightZoIons + SizeOfIndividualRightZoIons[i];

		h_a[i].InsilicoMassRightZoo = d_arrRightZoo + (PrevRightZooIons);
		PrevRightZooIons = PrevRightZooIons + SizeOfIndividualRightZooIons[i];

	}

	cudaMalloc((void**) &(d_a), sizeof(ProteinStructFromCS)*candidateProteinsCount);
	cudaMemcpy(d_a, h_a, sizeof(ProteinStructFromCS)*candidateProteinsCount, cudaMemcpyHostToDevice);

	cudaMalloc((void**) &(d_return), sizeof(ProteinStructToReturn)*candidateProteinsCount);
	cudaMemcpy(d_return, h_return, sizeof(ProteinStructToReturn)*candidateProteinsCount, cudaMemcpyHostToDevice);

	int NumOfThreadsToLaunch = 256;
	int NoOfBlocks = candidateProteinsCount / NumOfThreadsToLaunch;	///HERE	

	if (NoOfBlocks == 0)
		NoOfBlocks = 1;

	std::string stringPepUnit = Parameters.peptideToleranceUnit;
	int intPepUnit = PeptideTolUnitMapping(stringPepUnit);

	double *dev_masses, *dev_intensities;
	cudaMalloc((void**)&dev_masses, sizeof(double) * PeakListCount);
	cudaMalloc((void**)&dev_intensities, sizeof(double) * PeakListCount);
	cudaMemcpy(dev_masses, PeakListMasses, PeakListCount * sizeof(double), cudaMemcpyHostToDevice);
	cudaMemcpy(dev_intensities, PeakListIntensities, PeakListCount * sizeof(double), cudaMemcpyHostToDevice);

	ComputeInsilicoScore << <NoOfBlocks, NumOfThreadsToLaunch >> > (d_a, d_return, candidateProteinsCount, Parameters, dev_masses, dev_intensities, PeakListCount, intPepUnit);

	cudaMemcpy(h_a, d_a, sizeof(ProteinStructFromCS)*candidateProteinsCount, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_arr, d_arr, sizeof(double) * SizeOfLeftIonsTotal, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_return, d_return, sizeof(ProteinStructToReturn)*candidateProteinsCount, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_LeftMatchedIndex, d_LeftMatchedIndex, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_LeftPeakIndex, d_LeftPeakIndex, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_LeftType, d_LeftType, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_RightMatchedIndex, d_RightMatchedIndex, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_RightPeakIndex, d_RightPeakIndex, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);
	cudaMemcpy(h_RightType, d_RightType, sizeof(int) * megaSize, cudaMemcpyDeviceToHost);

	double x = h_return[0].InsilicoScore;
	h_LeftMatchedIndex[0] = h_LeftMatchedIndex[0];
	/////////////////////////////////////////////////////////////////

	int SizeOfDataToReturn = 0;
	for (int i = 0; i < candidateProteinsCount; i++)	///CHANGED HERE!!!
	{
		int LeftCount = h_return[i].LeftCount;
		int RightCount = h_return[i].RightCount;
		if (LeftCount > 0 || RightCount > 0)
		{
			(*DataToReturn)->Header = h_return[i].Header;
			(*DataToReturn)->InsilicoScore = h_return[i].InsilicoScore;
			(*DataToReturn)->MatchCounter = h_return[i].MatchCounter;
			(*DataToReturn)->LeftCount = LeftCount;
			(*DataToReturn)->RightCount = RightCount;
			(*DataToReturn)->LeftMatchedIndex = new int[LeftCount];
			(*DataToReturn)->LeftPeakIndex = new int[LeftCount];
			(*DataToReturn)->LeftType = new int[LeftCount];

			int index = i * 5000;
			int ind = 0;
			for (int j = index; j < index + LeftCount; j++) {
				(*DataToReturn)->LeftMatchedIndex[ind] = h_LeftMatchedIndex[j];
				(*DataToReturn)->LeftPeakIndex[ind] = h_LeftPeakIndex[j];
				(*DataToReturn)->LeftType[ind] = h_LeftType[j];
				ind++;
			}
			ind = 0;
			(*DataToReturn)->RightMatchedIndex = new int[RightCount];
			(*DataToReturn)->RightPeakIndex = new int[RightCount];
			(*DataToReturn)->RightType = new int[RightCount];
			for (int j = index; j < index + RightCount; j++) {
				(*DataToReturn)->RightMatchedIndex[ind] = h_RightMatchedIndex[j];
				(*DataToReturn)->RightPeakIndex[ind] = h_RightPeakIndex[j];
				(*DataToReturn)->RightType[ind] = h_RightType[j];
				ind++;
			}
			SizeOfDataToReturn++;
			DataToReturn++;
		}
	}

	return SizeOfDataToReturn;
}
