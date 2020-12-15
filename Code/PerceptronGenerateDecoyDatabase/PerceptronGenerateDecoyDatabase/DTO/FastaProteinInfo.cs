using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronGenerateDecoyDatabase.Utility;

namespace PerceptronGenerateDecoyDatabase.DTO
{
    public class FastaProteinInfo
    {
        public string Header;
        public string Sequence;

        public FastaProteinInfo()
        {
            Header = "";
            Sequence = "";
        }


        public FastaProteinInfo(string cHeader, string cSequence)
        {
            Header = cHeader;
            Sequence = cSequence;
        }

        public FastaProteinInfo(FastaProteinInfo temp)
        {
            var ProteinClone = Clone.DeepClone<FastaProteinInfo>(temp);
            Header = ProteinClone.Header;
            Sequence = ProteinClone.Sequence;
        }

    }
}
