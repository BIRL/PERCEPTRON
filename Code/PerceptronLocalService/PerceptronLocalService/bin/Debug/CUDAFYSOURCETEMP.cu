
// PerceptronLocalService.Engine.PstGeneratorGpu
extern "C" __global__  void PstGenerationKernel( double* peaks, int peaksLen0,  int* peakListCountArray, int peakListCountArrayLen0,  double* aminoAcidMasses, int aminoAcidMassesLen0,  double* hopTolerance, int hopToleranceLen0,  unsigned short* pstMatchMatrix, int pstMatchMatrixLen0, int pstMatchMatrixLen1,  double* errors, int errorsLen0, int errorsLen1,  double* AveragePstIntensity, int AveragePstIntensityLen0, int AveragePstIntensityLen1);
// PerceptronLocalService.Engine.PstGeneratorGpu
extern "C" __global__  void GpuGeneratedAverageIntensities( double* IntensityPeakDataDevice, int IntensityPeakDataDeviceLen0,  int* intensitypeakListLengthDevice, int intensitypeakListLengthDeviceLen0,  double* GpuIntensityPeakMatrix, int GpuIntensityPeakMatrixLen0, int GpuIntensityPeakMatrixLen1);

// PerceptronLocalService.Engine.PstGeneratorGpu
extern "C" __global__  void PstGenerationKernel( double* peaks, int peaksLen0,  int* peakListCountArray, int peakListCountArrayLen0,  double* aminoAcidMasses, int aminoAcidMassesLen0,  double* hopTolerance, int hopToleranceLen0,  unsigned short* pstMatchMatrix, int pstMatchMatrixLen0, int pstMatchMatrixLen1,  double* errors, int errorsLen0, int errorsLen1,  double* AveragePstIntensity, int AveragePstIntensityLen0, int AveragePstIntensityLen1)
{
	int num = peakListCountArray[(0)];
	int num2 = threadIdx.x + blockIdx.x * blockDim.x;
	int num3 = num2 % num;
	int num4 = num2 / (num * 26);
	int num5 = num2 / num % 26;
	if (num4 < num && num4 > num3)
	{
		double num6 = peaks[(num4)] - peaks[(num3)];
		if (num6 < 0.0)
		{
			num6 *= -1.0;
		}
		double num7 = num6 - aminoAcidMasses[(num5)];
		if (num7 < 0.0)
		{
			num7 *= -1.0;
		}
		if (num7 < hopTolerance[(0)])
		{
			pstMatchMatrix[(num3) * pstMatchMatrixLen1 + ( num4)] = (unsigned short)num5;
			errors[(num3) * errorsLen1 + ( num4)] = num7;
		}
	}
}
// PerceptronLocalService.Engine.PstGeneratorGpu
extern "C" __global__  void GpuGeneratedAverageIntensities( double* IntensityPeakDataDevice, int IntensityPeakDataDeviceLen0,  int* intensitypeakListLengthDevice, int intensitypeakListLengthDeviceLen0,  double* GpuIntensityPeakMatrix, int GpuIntensityPeakMatrixLen0, int GpuIntensityPeakMatrixLen1)
{
	int num = threadIdx.x + blockIdx.x * blockDim.x;
	int num2 = num % intensitypeakListLengthDevice[(0)];
	int num3 = num2 + num / intensitypeakListLengthDevice[(0)];
	if (num3 < intensitypeakListLengthDevice[(0)] && num3 > num2)
	{
		double num4 = (IntensityPeakDataDevice[(num2)] + IntensityPeakDataDevice[(num3)]) / 2.0;
		GpuIntensityPeakMatrix[(num2) * GpuIntensityPeakMatrixLen1 + ( num3)] = num4;
	}
}
