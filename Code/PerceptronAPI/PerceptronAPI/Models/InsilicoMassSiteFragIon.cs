using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class InsilicoMassSiteFragIon
    {
        public List<double> InsilicoMassLeftAo;
        public List<double> InsilicoMassLeftBo;
        public List<double> InsilicoMassLeftAstar;
        public List<double> InsilicoMassLeftBstar;
        public List<double> InsilicoMassRightYo;
        public List<double> InsilicoMassRightYstar;
        public List<double> InsilicoMassRightZo;
        public List<double> InsilicoMassRightZoo;

        public InsilicoMassSiteFragIon()
        {
            InsilicoMassLeftAo = new List<double>();
            InsilicoMassLeftBo = new List<double>();
            InsilicoMassLeftAstar = new List<double>();
            InsilicoMassLeftBstar = new List<double>();
            InsilicoMassRightYo = new List<double>();
            InsilicoMassRightYstar = new List<double>();
            InsilicoMassRightZo = new List<double>();
            InsilicoMassRightZoo = new List<double>();
        }

        public InsilicoMassSiteFragIon(InsilicoMassSiteFragIon insilico)
        {
            InsilicoMassLeftAo = insilico.InsilicoMassLeftAo;
            InsilicoMassLeftBo = insilico.InsilicoMassLeftBo;
            InsilicoMassLeftAstar = insilico.InsilicoMassLeftAstar;
            InsilicoMassLeftBstar = insilico.InsilicoMassLeftBstar;
            InsilicoMassRightYo = insilico.InsilicoMassRightYo;
            InsilicoMassRightYstar = insilico.InsilicoMassRightYstar;
            InsilicoMassRightZo = insilico.InsilicoMassRightZo;
            InsilicoMassRightZoo = insilico.InsilicoMassRightZoo;
        }
    }
}