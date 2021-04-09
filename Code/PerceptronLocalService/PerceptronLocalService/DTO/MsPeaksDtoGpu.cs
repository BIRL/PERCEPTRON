using System.Collections.Generic;
using System.Linq;

namespace PerceptronLocalService.DTO
{
    public class MsPeaksDtoGpu
    {
        public double[] PeakListMasses;
        public double[] PeakListIntensities;

        public MsPeaksDtoGpu()
        { }


        //public MsPeaksDtoGpu(List<double> intensity, List<double> massList)
        //{
        //    PeakListMasses = massList.ToArray();
        //    PeakListIntensities = intensity.ToArray();
        //}


        public MsPeaksDtoGpu(List<newMsPeaksDto> peakData2DList)
        {
            //List<newMsPeaksDto> Peak2DList = new List<newMsPeaksDto>();
            //Peak2DList.Add(new newMsPeaksDto(0, 0));  // It's a placeholder for putting the value of intact mass and its intensity

            //for (int i = 1; i < intensity.Count; i++)
            //{
            //    var tempnewMsPeakDto = new newMsPeaksDto(massList[i], intensity[i]);
            //    Peak2DList.Add(tempnewMsPeakDto);
            //}
            //var peakDatalistsort = Peak2DList.OrderBy(n => n.Mass).ToList();

            //peakDatalistsort[0].Mass = massList[0];
            //peakDatalistsort[0].Intensity = intensity[0];




            int SizeOfArray = peakData2DList.Count;
            double[] tempPeakListMasses = new double[SizeOfArray];
            double[] tempPeakListIntensities = new double[SizeOfArray];

            tempPeakListMasses[0] = peakData2DList[SizeOfArray - 1].Mass;
            tempPeakListIntensities[0] = peakData2DList[SizeOfArray - 1].Intensity;

            for (int index = 1; index < peakData2DList.Count; index++)
            {
                tempPeakListMasses[index] = peakData2DList[index].Mass;
                tempPeakListIntensities[index] = peakData2DList[index].Intensity;
            }

            PeakListMasses = tempPeakListMasses;
            PeakListIntensities = tempPeakListIntensities;
            //PeakListMasses = massList.ToArray();
            //PeakListIntensities = intensity.ToArray();
        }


    }
}
