using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class InsilicoMassIons
    {
        public List<double> InsilicoMassLeftIons;
        public List<double> InsilicoMassRightIons;
        public List<double> InsilicoMassLeftAo;
        public List<double> InsilicoMassLeftBo;
        public List<double> InsilicoMassLeftAstar;
        public List<double> InsilicoMassLeftBstar;
        public List<double> InsilicoMassRightYo;
        public List<double> InsilicoMassRightYstar;
        public List<double> InsilicoMassRightZo;
        public List<double> InsilicoMassRightZoo;

        public InsilicoMassIons()
        {
            InsilicoMassLeftIons = new List<double>();
            InsilicoMassRightIons = new List <double>();
            InsilicoMassLeftAo = new List<double>();
            InsilicoMassLeftBo = new List<double>();
            InsilicoMassLeftAstar = new List<double>();
            InsilicoMassLeftBstar = new List<double>();
            InsilicoMassRightYo = new List<double>();
            InsilicoMassRightYstar = new List<double>();
            InsilicoMassRightZo = new List<double>();
            InsilicoMassRightZoo = new List<double>();
        }

        public InsilicoMassIons(SearchResult Results)
        {
            
            InsilicoMassLeftIons = Results.InsilicoMassLeft.Split(',').Select(double.Parse).ToList();
            InsilicoMassRightIons = Results.InsilicoMassRight.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftAo != "")
                InsilicoMassLeftAo = Results.InsilicoMassLeftAo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftBo != "")
                InsilicoMassLeftBo = Results.InsilicoMassLeftBo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftAstar != "")
                InsilicoMassLeftAstar = Results.InsilicoMassLeftAstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftBstar != "")
                InsilicoMassLeftBstar = Results.InsilicoMassLeftBstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightYo != "")
                InsilicoMassRightYo = Results.InsilicoMassRightYo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightYstar != "")
                InsilicoMassRightYstar = Results.InsilicoMassRightYstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightZo != "")
                InsilicoMassRightZo = Results.InsilicoMassRightZo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightZoo != "")
                InsilicoMassRightZoo = Results.InsilicoMassRightZoo.Split(',').Select(double.Parse).ToList();

        }

        //public InsilicoMassIons(InsilicoMassIons insilico)
        //{
        //    InsilicoMassLeftIons = insilico.InsilicoMassLeftIons;
        //    InsilicoMassRightIons = insilico.InsilicoMassRightIons;
        //    InsilicoMassLeftAo = insilico.InsilicoMassLeftAo;
        //    InsilicoMassLeftBo = insilico.InsilicoMassLeftBo;
        //    InsilicoMassLeftAstar = insilico.InsilicoMassLeftAstar;
        //    InsilicoMassLeftBstar = insilico.InsilicoMassLeftBstar;
        //    InsilicoMassRightYo = insilico.InsilicoMassRightYo;
        //    InsilicoMassRightYstar = insilico.InsilicoMassRightYstar;
        //    InsilicoMassRightZo = insilico.InsilicoMassRightZo;
        //    InsilicoMassRightZoo = insilico.InsilicoMassRightZoo;
        //}
    }
}