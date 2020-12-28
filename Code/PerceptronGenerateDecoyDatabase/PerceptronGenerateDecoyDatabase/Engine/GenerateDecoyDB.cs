using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronGenerateDecoyDatabase.DTO;
using PerceptronGenerateDecoyDatabase.Utility;

namespace PerceptronGenerateDecoyDatabase.Engine
{
    public class GenerateDecoyDB
    {
        public List<FastaProteinInfo> GeneratingDecoyDB(List<FastaProteinInfo> ProteinList)
        {
            int a = 1;
            var ProteinDecoyList = new List<FastaProteinInfo>(3 * ProteinList.Count);
            string AminoAcidAlphbets = "ABCDEFGHIKLMNOPQRSTUVWYZ";
            int AminoAcidAlphbetsLength = AminoAcidAlphbets.Length;
            int SequenceLength = 0;
            
            Random RandomNoGenrate = new Random();
            for (int iter = 0; iter < ProteinList.Count; iter+=3)
            {
                SequenceLength = ProteinList[iter].Sequence.Length;

                ProteinList[iter].Header = ProteinList[iter].Header.Replace(">", ">XXX_");
                ProteinList[iter].Sequence = RandomSequenceGenerate(RandomNoGenrate, 0, SequenceLength - 1, AminoAcidAlphbets, AminoAcidAlphbetsLength);

                ProteinList[iter + 1].Header = ProteinList[iter + 1].Header.Replace(">", ">YYY_");
                ProteinList[iter + 1].Sequence = RandomSequenceGenerate(RandomNoGenrate, 0, SequenceLength - 1, AminoAcidAlphbets, AminoAcidAlphbetsLength);

                ProteinList[iter + 2].Header = ProteinList[iter + 2].Header.Replace(">", ">ZZZ_");
                ProteinList[iter + 2].Sequence = RandomSequenceGenerate(RandomNoGenrate, 0, SequenceLength - 1, AminoAcidAlphbets, AminoAcidAlphbetsLength);

                ProteinDecoyList.Add(ProteinList[iter]);
                ProteinDecoyList.Add(ProteinList[iter+1]);
                ProteinDecoyList.Add(ProteinList[iter+2]);

            }

            return ProteinDecoyList;

        }

        public string RandomSequenceGenerate(Random RandomNoGenrate, int min, int NoOfIterations, string AminoAcidAlphbets, int AminoAcidAlphbetsLength)
        {
            string DecoySequence = "";
            for (int i = 0; i <= NoOfIterations; i++)
            {
                double randNumber = NextDouble(RandomNoGenrate, 0.00, 1.00, 4);
                var NewDecoySeqAlphabet = (int)Math.Ceiling((randNumber) * AminoAcidAlphbetsLength);
                DecoySequence = String.Concat(DecoySequence, AminoAcidAlphbets[NewDecoySeqAlphabet- 1]);
            }
            return DecoySequence;
        }
        private double NextDouble(Random rand, double minValue, double maxValue, int decimalPlaces)
        {
            double randNumber = rand.NextDouble() * (maxValue - minValue) + minValue;
            return Convert.ToDouble(randNumber.ToString("f" + decimalPlaces));
        }
    }
}
