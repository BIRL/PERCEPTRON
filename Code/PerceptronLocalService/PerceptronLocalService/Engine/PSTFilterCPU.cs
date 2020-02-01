using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    public class PstFilterCpu : IPeptideSequenceTagScoring
    {
        public void ScoreProteinsByPst(List<PstTagList> pstList, List<ProteinDto> mwProt)  //mwProt ShortListed Candidate Proteins According to Mass Tolerance
        {
            //int COUNTME = 0;
            for (int iterationOnProteinList = 0; iterationOnProteinList <= mwProt.Count - 1; iterationOnProteinList++)
            {

                //if (mwProt[iterationOnProteinList].Header == "Q9BTM9")
                //{



                    double score = 0;
                    for (int iteration = 0; iteration <= pstList.Count - 1; iteration++)
                    {
                        int TagOccurrences = Regex.Matches(mwProt[iterationOnProteinList].Sequence, pstList[iteration].PstTags).Count;
                        score += (pstList[iteration].PstErrorScore + pstList[iteration].PstFrequency) * TagOccurrences;
                        //if (TagOccurrences != 0 && pstList[iteration].PstTags == "RHR")
                        //    COUNTME += 1;

                    }
                    score = score / mwProt[iterationOnProteinList].Sequence.Length;
                    if (score > 1)
                        score = 1;
                    mwProt[iterationOnProteinList].PstScore = score;

                //}
            }
        }
    }
}
