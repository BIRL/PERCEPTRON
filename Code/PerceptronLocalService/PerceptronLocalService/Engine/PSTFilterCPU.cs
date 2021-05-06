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
        public void ScoreProteinsByPst(SearchParametersDto parameters, List<PstTagList> pstList, List<ProteinDto> mwProt)  //mwProt ShortListed Candidate Proteins According to Mass Tolerance
        {
            //int COUNTME = 0;
            for (int iterationOnProteinList = 0; iterationOnProteinList <= mwProt.Count - 1; iterationOnProteinList++)
            {

                var PstTagsExists = new List<string>();

                double score = 0;
                for (int iteration = 0; iteration <= pstList.Count - 1; iteration++)
                {

                    //int TagOccurrences = Regex.Matches(mwProt[iterationOnProteinList].Sequence, pstList[iteration].PstTags).Count;       //Commented 20210505
                    int TagOccurrences = CountStringOccurrences(mwProt[iterationOnProteinList].Sequence, pstList[iteration].PstTags);      //Updated 20210505

                    score += (pstList[iteration].PstErrorScore + pstList[iteration].PstFrequency) * TagOccurrences;
                    //score += (pstList[iteration].PstErrorScore + pstList[iteration].PstTagLength) * TagOccurrences;

                    if (TagOccurrences != 0)
                    {
                        PstTagsExists.Add(pstList[iteration].PstTags);

                        //Added BELOW 20210505
                        if (parameters.PeakListFileName.Length > 1 && parameters.PstSweight == 0)
                        {
                            break;
                        }
                        //Added ABOVE 20210505


                        //int ab = mwProt[iterationOnProteinList].Sequence.IndexOf("GGA");
                    }
                }

                mwProt[iterationOnProteinList].PstTagsWithComma = string.Join(",", PstTagsExists); //Joining Pst Tags with comma in a One string


                score = score / mwProt[iterationOnProteinList].Sequence.Length;
                if (score > 1)
                    score = 1;
                mwProt[iterationOnProteinList].PstScore = score;

                //}
            }
        }


        public static int CountStringOccurrences(string text, string pattern)   //Updated 20210505
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = text.IndexOf(pattern, i)) != -1)
            {
                i++;
                count++;
            }
            return count;
        }
    }
}
