using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class AssembleInsilicoSpectra
    {
        public int MatchedIndex;
        public int PeakIndex;
        public string IonType;
        public double Theoretical_mz;
        public double Experimental_mz;

        public AssembleInsilicoSpectra(int cMatchedIndex, int cPeakIndex, string cIonType, double cTheoretical_mz, double cExperimental_mz)
        {
            MatchedIndex = cMatchedIndex;
            PeakIndex = cPeakIndex;
            IonType = cIonType;
            Theoretical_mz = cTheoretical_mz;
            Experimental_mz = cExperimental_mz;
        }



    }
}


//public int MatchedIndex;
// public int PeakIndex;
// public List<string> IonType;
// public List<double> Theoretical_mz;
// public List<double> Experimental_mz;

// public AssembleInsilicoSpectra(List<int> cMatchedIndex, List<int> cPeakIndex, List<string> cIonType, List<double> cTheoretical_mz, List<double> cExperimental_mz)
// {
//     MatchedIndex = cMatchedIndex;
//     PeakIndex = cPeakIndex;
//     IonType = cIonType;
//     Theoretical_mz = cTheoretical_mz;
//     Experimental_mz = cExperimental_mz;
// }