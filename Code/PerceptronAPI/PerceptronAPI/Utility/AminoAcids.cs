using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Utility
{
    public class AminoAcids
    {
        public readonly Dictionary<char, double> AminoAcidMasses = new Dictionary<char, double>()
        {
        // GIVEN BELOW AMINO ACID LIST IS MODIFIED ACCORDING TO SPECTRUM>ENGINE>AA_MW_ARRAY.M 
        {'A', 71.03711},
        {'B', 114.5349}, //B = avg(D,N)
        {'C', 103.00919},
        {'D', 115.02694},
        {'E', 129.04259},
        {'F', 147.06841},
        {'G', 57.02146},
        {'H', 137.05891},
        {'I', 113.08406},
        {'J', 0},
        {'K', 128.09496},
        {'L', 113.08406},
        {'M', 131.04049},
        {'N', 114.04293},
        {'O', 255.158295},
        {'P', 97.05276},
        {'Q', 128.05858},
        {'R', 156.10111},
        {'S', 87.03203},
        {'T', 101.04768},
        {'U', 168.964203},
        {'V', 99.06841},
        {'W', 186.07931},
        {'X', 110},
        {'Y', 163.06333},
        {'Z', 128.5506}  //Z = avg(E,Q)
        };
    }
}