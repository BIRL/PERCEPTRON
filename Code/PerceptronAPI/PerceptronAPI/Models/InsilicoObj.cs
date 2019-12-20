using System.Collections.Generic;

namespace PerceptronAPI.Models
{
    public class InsilicoObj
    {
        public List<double> InsilicoMassLeft;
        public List<double> InsilicoMassRight;
        public List<double> PeaklistMassLeft;
        public List<double> PeaklistMassRight;

        public InsilicoObj()
        {
            InsilicoMassRight = new List<double>();
            InsilicoMassLeft = new List<double>();
            PeaklistMassLeft = new List<double>();
            PeaklistMassRight = new List<double>();
        }
    }
}