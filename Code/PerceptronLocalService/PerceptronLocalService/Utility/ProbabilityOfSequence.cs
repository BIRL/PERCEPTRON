using System;
using System.Collections.Generic;
using System.Linq;

namespace PerceptronLocalService.Utility
{
    public class ProbabilityOfSequence
    {
        

        public double ProabailityAA(string Sequence)
        {
            List<Tuple<char, double>> letters = getSequenceProb(Sequence);

            double probAminoAcid = 1.0;

            for (int i = 0; i < Sequence.Length; i++)
            {
                int index = (Convert.ToInt32(Sequence[i]) - 64) - 1; // "-1" for Zero Indexing
                probAminoAcid = (probAminoAcid) * (letters[index].Item2);
            }

            probAminoAcid = probAminoAcid / (Sequence.Length);

            if (probAminoAcid == 0)
            {
                probAminoAcid = 4.94065645841247E-324; //not sure if this is the correct way to write this number
            }

            return probAminoAcid;
        }

        public List<Tuple<char, double>> getSequenceProb(string Sequence)
        {
            double total = 1.0 * Sequence.Length;
            List<Tuple<char, double>> letters = new List<Tuple<char, double>>();

            for (int i = 1; i <= 26; i++)
            {
                char letter = Convert.ToChar(i + 64);
                Tuple<char, double> to_add = Tuple.Create(letter, 0.0);
                letters.Add(to_add);

            }
            for (int i = 0; i < 26; i++)
            {
                double counter = 0.0;
                for (int j = 0; j < Sequence.Length; j++)
                {
                    if (letters[i].Item1 == Sequence[j])
                    {
                        counter++;
                    }
                }

                if (counter != 0)
                {
                    double probability = counter / total;
                    Tuple<char, double> to_add = Tuple.Create(letters[i].Item1, probability);
                    letters[i] = to_add;
                }
            }
            return letters;
        }
    }
}




//public static double ComputeProb(string sequence)
//        {
//            var probAa = GetSequenceProb(sequence);
//            var probAminoAcid = 1.0;

//            for (var indexAa = 1; indexAa < sequence.Length; indexAa++)
//                probAminoAcid = (probAminoAcid * probAa[(int)(sequence[indexAa]) - 65]);

//            probAminoAcid = probAminoAcid / sequence.Length;

//            if (Math.Abs(probAminoAcid) <= 0)
//                probAminoAcid = 4.94065645841247E-324; //Obtained from MSPathFinder - Handle UnderFlow
//            return probAminoAcid;
//        }

//        private static List<double> GetSequenceProb(string sequence)
//        {
//            var prob = new List<double>();
//            var A = sequence.Count(x => x == 'A');
//            var B = sequence.Count(x => x == 'B');
//            var C = sequence.Count(x => x == 'C');
//            var D = sequence.Count(x => x == 'D');
//            var E = sequence.Count(x => x == 'E');
//            var F = sequence.Count(x => x == 'F');
//            var G = sequence.Count(x => x == 'G');
//            var H = sequence.Count(x => x == 'H');
//            var I = sequence.Count(x => x == 'I');
//            var J = sequence.Count(x => x == 'J');
//            var K = sequence.Count(x => x == 'K');
//            var L = sequence.Count(x => x == 'L');
//            var M = sequence.Count(x => x == 'M');
//            var N = sequence.Count(x => x == 'N');
//            var O = sequence.Count(x => x == 'O');
//            var P = sequence.Count(x => x == 'P');
//            var Q = sequence.Count(x => x == 'Q');
//            var R = sequence.Count(x => x == 'R');
//            var S = sequence.Count(x => x == 'S');
//            var T = sequence.Count(x => x == 'T');
//            var U = sequence.Count(x => x == 'U');
//            var V = sequence.Count(x => x == 'V');
//            var W = sequence.Count(x => x == 'W');
//            var X = sequence.Count(x => x == 'X');
//            var Y = sequence.Count(x => x == 'Y');
//            var Z = sequence.Count(x => x == 'Z');

//            var total = Convert.ToDouble(sequence.Length);

//            prob.Add(A / total);
//            prob.Add(B / total);
//            prob.Add(C / total);
//            prob.Add(D / total);
//            prob.Add(E / total);
//            prob.Add(F / total);
//            prob.Add(G / total);
//            prob.Add(H / total);
//            prob.Add(I / total);
//            prob.Add(J / total);
//            prob.Add(K / total);
//            prob.Add(L / total);
//            prob.Add(M / total);
//            prob.Add(N / total);
//            prob.Add(O / total);
//            prob.Add(P / total);
//            prob.Add(Q / total);
//            prob.Add(R / total);
//            prob.Add(S / total);
//            prob.Add(T / total);
//            prob.Add(U / total);
//            prob.Add(V / total);
//            prob.Add(W / total);
//            prob.Add(X / total);
//            prob.Add(Y / total);
//            prob.Add(Z / total);

//            return prob;
//        }