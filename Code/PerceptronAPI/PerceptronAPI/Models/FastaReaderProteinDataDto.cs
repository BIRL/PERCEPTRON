using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class FastaReaderProteinDataDto
    {
        public string ID;
        public string ProteinDescription;
        public string Sequence;
        public double MolecularWeight;
        public string InsilicoLeft;
        public string InsilicoRight;

        public FastaReaderProteinDataDto(string cID, string cProteinDescription, double cMolecularWeight, string cSequence, string cInsilicoLeft, string cInsilicoRight)
        {
            ID = cID;
            ProteinDescription = cProteinDescription;
            MolecularWeight = cMolecularWeight;
            Sequence = cSequence;
            InsilicoLeft = cInsilicoLeft;
            InsilicoRight = cInsilicoRight;
        }
    }
}
