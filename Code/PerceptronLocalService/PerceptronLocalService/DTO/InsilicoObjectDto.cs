using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class InsilicoObjectDto
    {
        public List<double> PeaklistMassLeft; //Remove
        public List<double> PeaklistMassRight;
        public List<double> InsilicoMassLeft;
        public List<double> InsilicoMassRight;
        public List<double> InsilicoMassLeftAo;
        public List<double> InsilicoMassLeftBo;
        public List<double> InsilicoMassLeftAstar;
        public List<double> InsilicoMassLeftBstar;
        public List<double> InsilicoMassRightYo;
        public List<double> InsilicoMassRightYstar;
        public List<double> InsilicoMassRightZo;
        public List<double> InsilicoMassRightZoo;

        public InsilicoObjectDto()
        {
            InsilicoMassRight = new List<double>();
            InsilicoMassLeft = new List<double>();
            PeaklistMassLeft = new List<double>();
            PeaklistMassRight = new List<double>();
            InsilicoMassLeftAo = new List<double>();
            InsilicoMassLeftBo = new List<double>();
            InsilicoMassLeftAstar = new List<double>();
            InsilicoMassLeftBstar = new List<double>();
            InsilicoMassRightYo = new List<double>();
            InsilicoMassRightYstar = new List<double>();
            InsilicoMassRightZo = new List<double>();
            InsilicoMassRightZoo = new List<double>();
        }
    }
}
