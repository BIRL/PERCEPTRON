using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class newMsPeaksDto
    {
        public double Mass;
        public double Intensity;

        //public List<double> MassList;
        //public List<double> IntensityList;

        public newMsPeaksDto(double cMass, double cIntensity)
        {
            Mass = cMass;
            Intensity = cIntensity;
        }
        //public newMsPeaksDto(newMsPeaksDto peakDataList)
        //{
        //    Mass = peakDataList.Select(x => x.Mass).ToList();
        //}

    } 
}
