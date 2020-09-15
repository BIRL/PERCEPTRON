
// PerceptronLocalService.Engine.WholeProteinMassTunerGpu
extern "C" __global__  void DeviceGenerateFragmentPairMatrix( double* peaks, int peaksLen0,  int* len, int lenLen0,  double* output, int outputLen0, int outputLen1);

// PerceptronLocalService.Engine.WholeProteinMassTunerGpu
extern "C" __global__  void DeviceGenerateFragmentPairMatrix( double* peaks, int peaksLen0,  int* len, int lenLen0,  double* output, int outputLen0, int outputLen1)
{
	int num = threadIdx.x + blockIdx.x * blockDim.x;
	int num2 = num % len[(0)];
	int num3 = num2 + num / len[(0)];
	bool flag = num3 >= len[(0)];
	if (!flag)
	{
		double num4 = peaks[(num3)] + peaks[(num2)];
		output[(num2) * outputLen1 + ( num3)] = num4;
	}
}
