using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class FastaProteinDataDto
    {
        public string ID;
        public string Sequence;
        public double MolecularWeight;
        public string InsilicoLeft;
        public string InsilicoRight;

        public FastaProteinDataDto(string cID, double cMolecularWeight, string cSequence, string cInsilicoLeft, string cInsilicoRight)
        {
            ID = cID;
            MolecularWeight = cMolecularWeight;
            Sequence = cSequence;
            InsilicoLeft = cInsilicoLeft;
            InsilicoRight = cInsilicoRight;
        }
    }
}