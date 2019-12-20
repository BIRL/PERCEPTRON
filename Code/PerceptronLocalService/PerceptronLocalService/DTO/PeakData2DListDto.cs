using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class PeakData2DListDto
    {
        //(double Mass, double Intenstity){
        public double Mass { get; set; }
        public double Intensity { get; set; }

        public PeakData2DListDto(double i, double j)  /*!< Parameterized constructor */
        {
            Mass = i;
            Intensity = j;
        }
        //*** WHAT IS THE PURPOSE OF DEFAULT CONSTRUCTOR?***//
        public PeakData2DListDto()
        {
            Mass = 0;
            Intensity = 0;
        }
    }
}
