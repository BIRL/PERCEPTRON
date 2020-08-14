using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class MsPeaksDto
    {
        public double Mass;
        public double Intensity;

        public MsPeaksDto()
        {
            Mass = 0.0;
            Intensity = 0.0;
        }

        public MsPeaksDto(double cMass, double cIntensity)
        {
            Mass = cMass;
            Intensity = cIntensity;
        }

        public List<MsPeaksDto> PeakListInfo(PeakListData PeakListData)
        {
            var PeakList = new List<MsPeaksDto>();

            List<double> PeakListMasses = PeakListData.PeakListMasses.Split(',').Select(double.Parse).ToList();
            List<double> PeakListIntensities = PeakListData.PeakListIntensities.Split(',').Select(double.Parse).ToList();

            for (int i = 0; i < PeakListMasses.Count; i++)
            {
                var temp = new MsPeaksDto(PeakListMasses[i], PeakListIntensities[i]);
                PeakList.Add(temp);
            }

            PeakList = PeakList.OrderBy(x => x.Mass).ToList();


            return PeakList;
        }
    }
}