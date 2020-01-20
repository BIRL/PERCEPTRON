using System;
using System.Collections.Generic;
using System.Linq;

namespace PerceptronLocalService.Utility
{
    public static class ProbabilityOfSequence
    {
        public static double ComputeProb(string sequence)
        {
            var probAa = GetSequenceProb(sequence);
            var probAminoAcid = 1.0;

            for (var indexAa = 1; indexAa < sequence.Length; indexAa++)
                probAminoAcid = (probAminoAcid * probAa[(int)(sequence[indexAa]) - 65]);

            probAminoAcid = probAminoAcid / sequence.Length;

            if (Math.Abs(probAminoAcid) <= 0)
                probAminoAcid = 4.94065645841247E-324; //Obtained from MSPathFinder - Handle UnderFlow
            return probAminoAcid;
        }

        private static List<double> GetSequenceProb(string sequence)
        {
            var prob = new List<double>();
            var A = sequence.Count(x => x == 'A');
            var B = sequence.Count(x => x == 'B');
            var C = sequence.Count(x => x == 'C');
            var D = sequence.Count(x => x == 'D');
            var E = sequence.Count(x => x == 'E');
            var F = sequence.Count(x => x == 'F');
            var G = sequence.Count(x => x == 'G');
            var H = sequence.Count(x => x == 'H');
            var I = sequence.Count(x => x == 'I');
            var J = sequence.Count(x => x == 'J');
            var K = sequence.Count(x => x == 'K');
            var L = sequence.Count(x => x == 'L');
            var M = sequence.Count(x => x == 'M');
            var N = sequence.Count(x => x == 'N');
            var O = sequence.Count(x => x == 'O');
            var P = sequence.Count(x => x == 'P');
            var Q = sequence.Count(x => x == 'Q');
            var R = sequence.Count(x => x == 'R');
            var S = sequence.Count(x => x == 'S');
            var T = sequence.Count(x => x == 'T');
            var U = sequence.Count(x => x == 'U');
            var V = sequence.Count(x => x == 'V');
            var W = sequence.Count(x => x == 'W');
            var X = sequence.Count(x => x == 'X');
            var Y = sequence.Count(x => x == 'Y');
            var Z = sequence.Count(x => x == 'Z');

            var total = Convert.ToDouble(sequence.Length);

            prob.Add(A / total);
            prob.Add(B / total);
            prob.Add(C / total);
            prob.Add(D / total);
            prob.Add(E / total);
            prob.Add(F / total);
            prob.Add(G / total);
            prob.Add(H / total);
            prob.Add(I / total);
            prob.Add(J / total);
            prob.Add(K / total);
            prob.Add(L / total);
            prob.Add(M / total);
            prob.Add(N / total);
            prob.Add(O / total);
            prob.Add(P / total);
            prob.Add(Q / total);
            prob.Add(R / total);
            prob.Add(S / total);
            prob.Add(T / total);
            prob.Add(U / total);
            prob.Add(V / total);
            prob.Add(W / total);
            prob.Add(X / total);
            prob.Add(Y / total);
            prob.Add(Z / total);

            return prob;
        }
    }
}
