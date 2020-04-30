using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Hydroxylation
    {

        public static List<PostTranslationModificationsSiteDto> Hydroxylation_P(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 15.9949;
            var modName = "Hydroxylation";
            var site = 'P';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Proline
            var totalPro = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalPro (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'P') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalPro = totalPro + 1;

                    //variables to store sub - sequence
                    char a, b, c, d, e, f, g, h, m, j, k, l;

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length >= i + 1)
                    {
                        l = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'R':
                            case 'C':
                            case 'K':
                            case 'Y':
                            case 'V':
                            case 'I':
                                score = 0.01;
                                break;
                            case 'A':
                                score = 0.03;
                                break;
                            case 'G':
                                score = 0.61;
                                break;
                            case 'P':
                                score = 0.13;
                                break;
                            case 'T':
                                score = 0.08;
                                break;
                            case 'S':
                                score = 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length >= i + 2)
                    {
                        k = proteinSequence[i + 2];
                        switch (proteinSequence[i + 2])
                        {
                            case 'N':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                            case 'H':
                            case 'I':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'K':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'A':
                                score = score * 0.09;
                                break;
                            case 'Y':
                                score = score * 0.12;
                                break;
                            case 'P':
                                score = score * 0.25;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 3 position
                    if (proteinSequence.Length >= i + 3)
                    {
                        j = proteinSequence[i + 3];
                        switch (proteinSequence[i + 3])
                        {
                            case 'E':
                            case 'I':
                            case 'L':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'N':
                                score = score * 0.02;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'D':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'Y':
                                score = score * 0.07;
                                break;
                            case 'K':
                                score = score * 0.13;
                                break;
                            case 'A':
                                score = score * 0.11;
                                break;
                            case 'P':
                                score = score * 0.29;
                                break;
                            case 'H':
                            case 'C':
                            case 'F':
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 4 position
                    if (proteinSequence.Length >= i + 4)
                    {
                        m = proteinSequence[i + 4];
                        switch (proteinSequence[i + 4])
                        {
                            case 'R':
                            case 'D':
                            case 'C':
                            case 'I':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'T':
                                score = score * 0.03;
                                break;
                            case 'S':
                                score = score * 0.04;
                                break;
                            case 'A':
                                score = score * 0.06;
                                break;
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'P':
                                score = score * 0.13;
                                break;
                            case 'G':
                                score = score * 0.58;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 5 position
                    if (proteinSequence.Length >= i + 5)
                    {
                        h = proteinSequence[i + 5];
                        switch (proteinSequence[i + 5])
                        {
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'R':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'E':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'A':
                                score = score * 0.15;
                                break;
                            case 'P':
                                score = score * 0.21;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 6 position
                    if (proteinSequence.Length >= i + 6)
                    {
                        g = proteinSequence[i + 6];
                        switch (proteinSequence[i + 6])
                        {
                            case 'H':
                            case 'L':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'E':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'D':
                                score = score * 0.04;
                                break;
                            case 'T':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'S':
                            case 'Y':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'R':
                                score = score * 0.08;
                                break;
                            case 'K':
                                score = score * 0.12;
                                break;
                            case 'P':
                                score = score * 0.29;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 1
                    if (i - 1 > 0)
                    {
                        f = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'N':
                            case 'D':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'Q':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'I':
                            case 'T':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.05;
                                break;
                            case 'L':
                                score = score * 0.06;
                                break;
                            case 'Y':
                            case 'S':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.11;
                                break;
                            case 'P':
                                score = score * 0.28;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 2
                    if (i - 2 > 0)
                    {
                        e = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'R':
                            case 'E':
                            case 'Q':
                            case 'I':
                            case 'K':
                                score = score * 0.01;
                                break;
                            case 'L':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'T':
                                score = score * 0.03;
                                break;
                            case 'Y':
                                score = score * 0.06;
                                break;
                            case 'A':
                                score = score * 0.07;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'G':
                                score = score * 0.6;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 3
                    if (i - 3 > 0)
                    {
                        d = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'L':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'G':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'K':
                                score = score * 0.09;
                                break;
                            case 'R':
                            case 'S':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.13;
                                break;
                            case 'P':
                                score = score * 0.28;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 4
                    if (i - 4 > 0)
                    {
                        c = proteinSequence[i - 4];
                        switch (proteinSequence[i - 4])
                        {
                            case 'C':
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                            case 'I':
                            case 'T':
                                score = score * 0.03;
                                break;
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'S':
                                score = score * 0.05;
                                break;
                            case 'Y':
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'K':
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'P':
                                score = score * 0.25;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        b = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'R':
                            case 'N':
                            case 'D':
                            case 'Q':
                            case 'Y':
                            case 'V':
                                score = score * 0.01;
                                break;
                            case 'L':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.03;
                                break;
                            case 'K':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'A':
                                score = score * 0.09;
                                break;
                            case 'G':
                                score = score * 0.58;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        a = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'M':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'S':
                                score = score * 0.05;
                                break;
                            case 'Q':
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.08;
                                break;
                            case 'K':
                                score = score * 0.12;
                                break;
                            case 'A':
                                score = score * 0.13;
                                break;
                            case 'P':
                                score = score * 0.3;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 6);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
                        l = proteinSequence[i + 1];
                        k = proteinSequence[i + 2];
                        j = proteinSequence[i + 3];
                        m = proteinSequence[i + 4];
                        h = proteinSequence[i + 5];
                        g = proteinSequence[i + 6];
                        f = proteinSequence[i - 1];
                        e = proteinSequence[i - 2];
                        d = proteinSequence[i - 3];
                        c = proteinSequence[i - 4];
                        b = proteinSequence[i - 5];
                        a = proteinSequence[i - 6];

                        //% it stores the protein sub-sequence
                        aa.Add(a);
                        aa.Add(b);
                        aa.Add(c);
                        aa.Add(d);
                        aa.Add(e);
                        aa.Add(f);
                        aa.Add(proteinSequence[i]);
                        aa.Add(l);
                        aa.Add(k);
                        aa.Add(j);
                        aa.Add(m);
                        aa.Add(h);
                        aa.Add(g);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Proline at that position
                    }
                }

                // for the TotalPro if condition coming up ahead
                index = i;
            }

            // it displays total number of Proline found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Proline found: " + totalPro);
            }
            //disp(['Total Proline found: ', num2str(TotalPro)])

            // returning the object array
            return array;
        }

    }
}
