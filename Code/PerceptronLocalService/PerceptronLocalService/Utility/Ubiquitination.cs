using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Ubiquitination
    {
        // Function (Ubiquitination_K): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Ubiquitination_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            double modWeight = 8561;
            var modName = "protein_sequenceation";
            var site = 'K';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalLys (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'K') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalLys = totalLys + 1;

                    //variables to store sub - sequence
                    char a, b, c, d, e, f, g, h, m, j, k, l;

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length >= i + 1)
                    {
                        l = (proteinSequence[i + 1]);

                        switch (proteinSequence[i + 1])
                        {
                            case 'C':
                            case 'W':
                                score = 0.01;
                                break;
                            case 'H':
                            case 'M':
                                score = 0.02;
                                break;
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = 0.04;
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                            case 'P':
                                score = 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'Q':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = 0.06;
                                break;
                            case 'A':
                                score = 0.08;
                                break;
                            case 'E':
                            case 'L':
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
                            case 'W':
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                            case 'K':
                            case 'P':
                            case 'F':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'I':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'G':
                            case 'E':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'L':
                                score = score * 0.14;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'I':
                            case 'K':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'Q':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'L':
                                score = score * 0.1;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'Q':
                            case 'I':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'K':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'L':
                                score = score * 0.1;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'Q':
                            case 'I':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'K':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.09;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'P':
                            case 'T':
                            case 'Q':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'K':
                            case 'S':
                            case 'V':
                            case 'A':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.09;
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
                            case 'C':
                            case 'H':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'Y':
                            case 'P':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'N':
                            case 'K':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'Q':
                            case 'I':
                            case 'T':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'L':
                                score = score * 0.12;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'R':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'Q':
                            case 'K':
                            case 'F':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'I':
                                score = score * 0.06;
                                break;
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.11;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'K':
                            case 'Q':
                            case 'I':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'S':
                            case 'A':
                                score = score * 0.07;
                                break;
                            case 'L':
                                score = score * 0.11;
                                break;
                            case 'E':
                                score = score * 0.1;
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
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'Q':
                            case 'I':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'G':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'V':
                            case 'A':
                            case 'D':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'L':
                                score = score * 0.1;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'Q':
                            case 'I':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'R':
                            case 'G':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'K':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.1;
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
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Y':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'P':
                                score = score * 0.04;
                                break;
                            case 'Q':
                            case 'I':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                                score = score * 0.06;
                                break;
                            case 'K':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 15);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
                        l = proteinSequence[i + 1];
                        k = proteinSequence[i + 2];
                        j = proteinSequence[i + 3];
                        m = proteinSequence[i + 4];
                        h = proteinSequence[i + 5];
                        g = proteinSequence[i + 6];
                        a = proteinSequence[i - 6];
                        b = proteinSequence[i - 5];
                        c = proteinSequence[i - 4];
                        d = proteinSequence[i - 3];
                        e = proteinSequence[i - 2];
                        f = proteinSequence[i - 1];

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

                        // score of Lysine at that position
                    }
                }

                // for the TotalLys if condition coming up ahead
                index = i;
            }

            // it displays total number of Lysine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Lysine found: " + totalLys);
            }
            return array;
        }

    }
}
