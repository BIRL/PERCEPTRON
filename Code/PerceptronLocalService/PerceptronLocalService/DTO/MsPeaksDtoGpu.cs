using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class MsPeaksDtoGpu
    {
        public double[] PeakListMasses;
        public double[] PeakListIntensities;

        public MsPeaksDtoGpu()
        { }


        public MsPeaksDtoGpu(List<double> intensity, List<double> massList)
        {
            PeakListMasses = massList.ToArray();
            PeakListIntensities = intensity.ToArray();
        }
    }
}
