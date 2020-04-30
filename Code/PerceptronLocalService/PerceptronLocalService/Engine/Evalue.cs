using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public class Evalue
    {
        public static void ComputeEvalue(List<ProteinDto> candidateProteins)
        {
            var probOfSequence = new List<double>();
            var spectralMatches = new List<int>();
            
            foreach (var protein in candidateProteins)
            {
                probOfSequence.Add(ProbabilityOfSequence.ComputeProb(protein.Sequence));
                spectralMatches.Add(protein.MatchCounter);
            }

            var uSpectralMatches = spectralMatches.Distinct().ToList();
            uSpectralMatches.Sort((a, b) => Math.Sign(b - a));
            
            for (var i = 0; i < uSpectralMatches.Count; i++)
            {
                var matches = uSpectralMatches[i];
                var ind = spectralMatches.Select(match => match == matches ? 1 : 0).ToList();
                var temp = Clone.CloneObject(ind);
                var setInd = Clone.Decrypt<List<int>>(temp);

                if (i > 0)
                {
                    for (var k = i - 1; k >= 1; k--)
                    {
                        var tempInd = spectralMatches.Select(match => match == uSpectralMatches[k] ? 1 : 0).ToList();
                        for (var index = 0; index < tempInd.Count; index++)
                        {
                            var element1 = tempInd[index];
                            var element2 = ind[index];
                            ind[index] = element1 + element2;
                        }
                    }
                }

                var eval = 0.0;
                var counter = 0;

                //  Computing E-Value
                for (var index = 0; index < spectralMatches.Count; index++)
                {
                    if (ind[index] != 1) continue;
                    eval = eval + probOfSequence[index];
                    counter = counter + 1;
                }
                eval = counter * eval;

                //  Saving E-Value for each Candidate Protein
                var proteinListIndex = 0;
                foreach (var protein in candidateProteins)
                {
                    if (setInd[proteinListIndex] != 1)
                    {
                        proteinListIndex = proteinListIndex + 1;
                        continue;
                    }
                    protein.Evalue = protein.Truncation == "" ? eval : eval*0.693;
                    proteinListIndex = proteinListIndex + 1;
                }
            }
        }
    }
}
