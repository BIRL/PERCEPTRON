#include <cuda.h>
#include <cuda_runtime.h>
#include <cuda_runtime_api.h>
#include <device_launch_parameters.h>

using namespace std;

extern "C" __declspec(dllexport) void __cdecl MainInitializer()
{
	int *IamInitializing;
	cudaMalloc((void**)&IamInitializing, sizeof(int) * 10);
}

