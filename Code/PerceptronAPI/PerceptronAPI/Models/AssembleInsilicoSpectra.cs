using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class AssembleInsilicoSpectra
    {
        public List<int> ListIndices;
        public List<string> ListFragIon;
        public List<double> ListExperimental_mz;
        public List<double> ListTheoretical_mz;
        public List<double> ListAbsError;

        public AssembleInsilicoSpectra(List<int> cListIndices, List<string> cListFragIon, List<double> cListExperimental_mz, List<double> cListTheoretical_mz, List<double> cListAbsError)
        {
            ListIndices = cListIndices;
            ListFragIon = cListFragIon;
            ListExperimental_mz = cListExperimental_mz;
            ListTheoretical_mz = cListTheoretical_mz;
            ListAbsError = cListAbsError;
        }
    }
}





//public int MatchedIndex;
//        public int PeakIndex;
//        public string IonType;
//        public double Theoretical_mz;
//        public double Experimental_mz;

//        public AssembleInsilicoSpectra(int cMatchedIndex, int cPeakIndex, string cIonType, double cTheoretical_mz, double cExperimental_mz)
//        {
//            MatchedIndex = cMatchedIndex;
//            PeakIndex = cPeakIndex;
//            IonType = cIonType;
//            Theoretical_mz = cTheoretical_mz;
//            Experimental_mz = cExperimental_mz;
//        }