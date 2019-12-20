using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Phosphorylation
    {
        // Function (Phosphorylation_S): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Phosphorylation_S(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'S';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Serine
            var totalSer = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalSer (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'S') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalSer = totalSer + 1;

                    //variables to store sub - sequence
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                            case 'G':
                            case 'L':
                            case 'K':
                                score = 0.07;
                                break;
                            case 'R':
                            case 'E':
                            case 'P':
                                score = 0.08;
                                break;
                            case 'N':
                            case 'I':
                                score = 0.03;
                                break;
                            case 'D':
                            case 'V':
                                score = 0.05;
                                break;
                            case 'C':
                                score = 0.01;
                                break;
                            case 'Q':
                                score = 0.04;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = 0.02;
                                break;
                            case 'S':
                                score = 0.12;
                                break;
                            case 'T':
                                score = 0.06;
                                break;
                            case 'W':
                                score = 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        c = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'A':
                            case 'G':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'E':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 4
                    if (i - 4 > 0)
                    {
                        d = proteinSequence[i - 4];
                        switch (proteinSequence[i - 4])
                        {
                            case 'A':
                            case 'G':
                            case 'L':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'E':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'Q':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'H':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'S':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 3
                    if (i - 3 > 0)
                    {
                        e = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'A':
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.15;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'E':
                            case 'L':
                            case 'K':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'Q':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.13;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 2
                    if (i - 2 > 0)
                    {
                        f = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'A':
                            case 'E':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Q':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'C':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'D':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'H':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.16;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 1
                    if (i - 1 > 0)
                    {
                        g = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'R':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Q':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'C':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'E':
                                score = score * 0.06;
                                break;
                            case 'H':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'K':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length >= i + 1)
                    {
                        h = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'R':
                            case 'T':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'N':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                            case 'W':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'E':
                                score = score * 0.07;
                                break;
                            case 'I':
                            case 'F':
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.27;
                                break;
                            case 'S':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 2
                    if (proteinSequence.Length >= i + 2)
                    {
                        j = proteinSequence[i + 2];
                        switch (proteinSequence[i + 2])
                        {
                            case 'A':
                            case 'L':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'K':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'C':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.11;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.15;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 3
                    if (proteinSequence.Length >= i + 3)
                    {
                        k = proteinSequence[i + 3];
                        switch (proteinSequence[i + 3])
                        {
                            case 'A':
                            case 'R':
                            case 'L':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'C':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.13;
                                break;
                            case 'Q':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'I':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'S':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 4
                    if (proteinSequence.Length >= i + 4)
                    {
                        l = proteinSequence[i + 4];
                        switch (proteinSequence[i + 4])
                        {
                            case 'A':
                            case 'R':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'C':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.1;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'V':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 5
                    if (proteinSequence.Length >= i + 5)
                    {
                        m = proteinSequence[i + 5];
                        switch (proteinSequence[i + 5])
                        {
                            case 'A':
                            case 'R':
                            case 'D':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'M':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'E':
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'Y':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 6
                    if (proteinSequence.Length >= i + 6)
                    {
                        n = proteinSequence[i + 6];
                        switch (proteinSequence[i + 6])
                        {
                            case 'A':
                            case 'R':
                            case 'G':
                            case 'L':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'P':
                                score = score * 0.08;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 12);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
                        b = proteinSequence[i - 6];
                        c = proteinSequence[i - 5];
                        d = proteinSequence[i - 4];
                        e = proteinSequence[i - 3];
                        f = proteinSequence[i - 2];
                        g = proteinSequence[i - 1];
                        h = proteinSequence[i + 1];
                        j = proteinSequence[i + 2];
                        k = proteinSequence[i + 3];
                        l = proteinSequence[i + 4];
                        m = proteinSequence[i + 5];
                        n = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        aa.Add(b);
                        aa.Add(c);
                        aa.Add(d);
                        aa.Add(e);
                        aa.Add(f);
                        aa.Add(g);
                        aa.Add(proteinSequence[i]);
                        aa.Add(h);
                        aa.Add(j);
                        aa.Add(k);
                        aa.Add(l);
                        aa.Add(m);
                        aa.Add(n);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Serine at that position
                    }
                }

                // for the TotalSer if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Serine found: " + totalSer);
            }
            //disp(['Total Serine found: ', num2str(TotalSer)])

            // returning the object array
            return array;
        }

        // Function (Phosphorylation_T): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Phosphorylation_T(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'T';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Thrmine
            var totalThr = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalThr (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'T') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalThr = totalThr + 1;

                    //variables to store sub - sequence
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                            case 'R':
                            case 'E':
                            case 'L':
                            case 'K':
                                score = 0.07;
                                break;
                            case 'D':
                            case 'V':
                                score = 0.05;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                                score = 0.04;
                                break;
                            case 'C':
                            case 'W':
                                score = 0.01;
                                break;
                            case 'G':
                            case 'T':
                                score = 0.06;
                                break;
                            case 'S':
                                score = 0.12;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = 0.02;
                                break;
                            case 'P':
                                score = 0.09;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        c = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'A':
                            case 'R':
                            case 'E':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 4
                    if (i - 4 > 0)
                    {
                        d = proteinSequence[i - 4];
                        switch (proteinSequence[i - 4])
                        {
                            case 'A':
                            case 'R':
                            case 'G':
                            case 'E':
                            case 'L':
                            case 'K':
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'H':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.13;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 3
                    if (i - 3 > 0)
                    {
                        e = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'A':
                            case 'G':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'R':
                                score = score * 0.1;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'E':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'D':
                            case 'V':
                                score = score * 0.05;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 2
                    if (i - 2 > 0)
                    {
                        f = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'A':
                            case 'E':
                            case 'L':
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'G':
                                score = score * 0.06;
                                break;
                            case 'D':
                            case 'K':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'Q':
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'H':
                            case 'F':
                            case 'M':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'S':
                                score = score * 0.15;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 1
                    if (i - 1 > 0)
                    {
                        g = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'R':
                            case 'E':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'P':
                                score = score * 0.1;
                                break;
                            case 'V':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length >= i + 1)
                    {
                        h = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'D':
                            case 'G':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'V':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'I':
                            case 'K':
                            case 'R':
                            case 'T':
                                score = score * 0.03;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.32;
                                break;
                            case 'S':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 2
                    if (proteinSequence.Length >= i + 2)
                    {
                        j = proteinSequence[i + 2];
                        switch (proteinSequence[i + 2])
                        {
                            case 'A':
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'K':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'C':
                            case 'M':
                            case 'Y':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.13;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 3
                    if (proteinSequence.Length >= i + 3)
                    {
                        k = proteinSequence[i + 3];
                        switch (proteinSequence[i + 3])
                        {
                            case 'A':
                            case 'G':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'R':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.08;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'Y':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 4
                    if (proteinSequence.Length >= i + 4)
                    {
                        l = proteinSequence[i + 4];
                        switch (proteinSequence[i + 4])
                        {
                            case 'A':
                            case 'R':
                            case 'D':
                            case 'G':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'E':
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'L':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'S':
                                score = score * 0.13;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 5
                    if (proteinSequence.Length >= i + 5)
                    {
                        m = proteinSequence[i + 5];
                        switch (proteinSequence[i + 5])
                        {
                            case 'A':
                            case 'R':
                            case 'K':
                            case 'L':
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'P':
                                score = score * 0.1;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'Y':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 6
                    if (proteinSequence.Length >= i + 6)
                    {
                        n = proteinSequence[i + 6];
                        switch (proteinSequence[i + 6])
                        {
                            case 'A':
                            case 'R':
                            case 'L':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'G':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'Q':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 13);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
                        b = proteinSequence[i - 6];
                        c = proteinSequence[i - 5];
                        d = proteinSequence[i - 4];
                        e = proteinSequence[i - 3];
                        f = proteinSequence[i - 2];
                        g = proteinSequence[i - 1];
                        h = proteinSequence[i + 1];
                        j = proteinSequence[i + 2];
                        k = proteinSequence[i + 3];
                        l = proteinSequence[i + 4];
                        m = proteinSequence[i + 5];
                        n = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        aa.Add(b);
                        aa.Add(c);
                        aa.Add(d);
                        aa.Add(e);
                        aa.Add(f);
                        aa.Add(g);
                        aa.Add(proteinSequence[i]);
                        aa.Add(h);
                        aa.Add(j);
                        aa.Add(k);
                        aa.Add(l);
                        aa.Add(m);
                        aa.Add(n);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Thrmine at that position
                    }
                }

                // for the TotalThr if condition coming up ahead
                index = i;
            }

            // it displays total number of Thrmine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Thrmine found: " + totalThr);
            }
            //disp(['Total Thrmine found: ', num2str(TotalThr)])

            // returning the object array
            return array;
        }

        // Function (Phosphorylation_Y): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Phosphorylation_Y(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'Y';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Tyrosine
            var totalTyr = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalTyr (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'Y') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalTyr = totalTyr + 1;

                    //variables to store sub - sequence
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                            case 'P':
                                score = 0.06;
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                            case 'K':
                                score = 0.07;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                                score = 0.04;
                                break;
                            case 'C':
                            case 'W':
                                score = 0.01;
                                break;
                            case 'E':
                            case 'L':
                                score = 0.08;
                                break;
                            case 'S':
                                score = 0.09;
                                break;
                            case 'H':
                            case 'M':
                                score = 0.02;
                                break;
                            case 'V':
                            case 'T':
                                score = 0.05;
                                break;
                            case 'F':
                            case 'Y':
                                score = 0.03;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        c = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'A':
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'K':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'V':
                            case 'T':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'F':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'R':
                                score = score * 0.06;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 4
                    if (i - 4 > 0)
                    {
                        d = proteinSequence[i - 4];
                        switch (proteinSequence[i - 4])
                        {
                            case 'A':
                            case 'R':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'Q':
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'F':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'L':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'S':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 3
                    if (i - 3 > 0)
                    {
                        e = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'A':
                            case 'R':
                            case 'L':
                            case 'K':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'Q':
                            case 'V':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'E':
                                score = score * 0.11;
                                break;
                            case 'H':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 2
                    if (i - 2 > 0)
                    {
                        f = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'A':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'N':
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'G':
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'E':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'H':
                            case 'F':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'K':
                            case 'L':
                                score = score * 0.06;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'Y':
                            case 'I':
                                score = score * 0.03;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 1
                    if (i - 1 > 0)
                    {
                        g = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'P':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'R':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'C':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'I':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'H':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length >= i + 1)
                    {
                        h = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'L':
                            case 'G':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'F':
                            case 'P':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.1;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.08;
                                break;
                            case 'I':
                            case 'K':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'T':
                                score = score * 0.06;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 2
                    if (proteinSequence.Length >= i + 2)
                    {
                        j = proteinSequence[i + 2];
                        switch (proteinSequence[i + 2])
                        {
                            case 'A':
                            case 'D':
                            case 'G':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'N':
                                score = score * 0.05;
                                break;
                            case 'V':
                            case 'T':
                            case 'P':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'C':
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'Q':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'F':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'W':
                                score = score * 0.01;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 3
                    if (proteinSequence.Length >= i + 3)
                    {
                        k = proteinSequence[i + 3];
                        switch (proteinSequence[i + 3])
                        {
                            case 'A':
                            case 'I':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                            case 'E':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'Q':
                            case 'M':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'P':
                                score = score * 0.1;
                                break;
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'V':
                                score = score * 0.08;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 4
                    if (proteinSequence.Length >= i + 4)
                    {
                        l = proteinSequence[i + 4];
                        switch (proteinSequence[i + 4])
                        {
                            case 'A':
                            case 'K':
                            case 'D':
                            case 'T':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'G':
                            case 'L':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 5
                    if (proteinSequence.Length >= i + 5)
                    {
                        m = proteinSequence[i + 5];
                        switch (proteinSequence[i + 5])
                        {
                            case 'A':
                            case 'R':
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'I':
                            case 'Y':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'Q':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'K':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i + 6
                    if (proteinSequence.Length >= i + 6)
                    {
                        n = proteinSequence[i + 6];
                        switch (proteinSequence[i + 6])
                        {
                            case 'A':
                            case 'R':
                            case 'G':
                            case 'E':
                            case 'K':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 14);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
                        b = proteinSequence[i - 6];
                        c = proteinSequence[i - 5];
                        d = proteinSequence[i - 4];
                        e = proteinSequence[i - 3];
                        f = proteinSequence[i - 2];
                        g = proteinSequence[i - 1];
                        h = proteinSequence[i + 1];
                        j = proteinSequence[i + 2];
                        k = proteinSequence[i + 3];
                        l = proteinSequence[i + 4];
                        m = proteinSequence[i + 5];
                        n = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        aa.Add(b);
                        aa.Add(c);
                        aa.Add(d);
                        aa.Add(e);
                        aa.Add(f);
                        aa.Add(g);
                        aa.Add(proteinSequence[i]);
                        aa.Add(h);
                        aa.Add(j);
                        aa.Add(k);
                        aa.Add(l);
                        aa.Add(m);
                        aa.Add(n);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Tyrosine at that position
                    }
                }

                // for the TotalTyr if condition coming up ahead
                index = i;
            }

            // it displays total number of Tyrosine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Tyrosine found: " + totalTyr);
            }
            //disp(['Total Tyrosine found: ', num2str(TotalTyr)])

            // returning the object array
            return array;
        }

    }
}
