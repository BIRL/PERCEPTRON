using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Methylation
    {
        // Function (Methylation_K): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Methylation_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 14.0156;
            var modName = "Methylation";
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
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                                score = 0.23;
                                break;
                            case 'D':
                            case 'C':
                            case 'Q':
                            case 'F':
                                score = 0;
                                break;
                            case 'G':
                            case 'P':
                                score = 0.11;
                                break;
                            case 'E':
                            case 'K':
                            case 'S':
                                score = 0.05;
                                break;
                            case 'L':
                            case 'M':
                            case 'Y':
                                score = 0.04;
                                break;
                            case 'T':
                                score = 0.14;
                                break;
                            case 'V':
                                score = 0.07;
                                break;
                            case 'R':
                            case 'N':
                            case 'H':
                            case 'I':
                            case 'W':
                                score = 0.02;
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
                            case 'S':
                            case 'T':
                                score = score * 0.12;
                                break;
                            case 'R':
                            case 'N':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'C':
                            case 'Q':
                            case 'I':
                            case 'L':
                            case 'M':
                            case 'F':
                            case 'W':
                            case 'Y':
                            case 'V':
                                score = score * 0;
                                break;
                            case 'E':
                                score = score * 0.11;
                                break;
                            case 'K':
                                score = score * 0.19;
                                break;
                            case 'P':
                                score = score * 0.07;
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
                                score = score * 0.09;
                                break;
                            case 'R':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'F':
                            case 'S':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'E':
                            case 'H':
                            case 'P':
                            case 'T':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'Q':
                                score = score * 0.14;
                                break;
                            case 'K':
                                score = score * 0.19;
                                break;
                            case 'I':
                            case 'M':
                            case 'W':
                                score = score * 0;
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
                                score = score * 0.21;
                                break;
                            case 'R':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'C':
                            case 'E':
                            case 'Q':
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'D':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'G':
                                score = score * 0.11;
                                break;
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.16;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.19;
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
                                score = score * 0.33;
                                break;
                            case 'R':
                            case 'H':
                            case 'P':
                            case 'S':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'K':
                                score = score * 0.09;
                                break;
                            case 'D':
                            case 'E':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'C':
                            case 'Q':
                            case 'L':
                            case 'M':
                            case 'F':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'G':
                                score = score * 0.19;
                                break;
                            case 'T':
                            case 'V':
                                score = score * 0.05;
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
                            case 'C':
                            case 'S':
                                score = score * 0.04;
                                break;
                            case 'R':
                                score = score * 0.4;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                            case 'M':
                            case 'F':
                            case 'P':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'D':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.07;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.12;
                                break;
                            case 'T':
                            case 'V':
                                score = score * 0.05;
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
                                score = score * 0.14;
                                break;
                            case 'R':
                            case 'D':
                            case 'I':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'N':
                            case 'G':
                            case 'E':
                            case 'L':
                            case 'M':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'H':
                            case 'P':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'Q':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'K':
                                score = score * 0.14;
                                break;
                            case 'S':
                                score = score * 0.21;
                                break;
                            case 'V':
                                score = score * 0.11;
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
                                score = score * 0.21;
                                break;
                            case 'R':
                            case 'N':
                            case 'D':
                            case 'Q':
                            case 'L':
                            case 'F':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'C':
                            case 'H':
                            case 'I':
                            case 'M':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'G':
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'E':
                                score = score * 0.11;
                                break;
                            case 'K':
                            case 'S':
                                score = score * 0.05;
                                break;
                            case 'T':
                                score = score * 0.18;
                                break;
                            case 'W':
                                score = score * 0.02;
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
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                            case 'L':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'C':
                            case 'E':
                            case 'M':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            case 'G':
                                score = score * 0.19;
                                break;
                            case 'H':
                                score = score * 0.04;
                                break;
                            case 'I':
                                score = score * 0.07;
                                break;
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'K':
                                score = score * 0.12;
                                break;
                            case 'F':
                            case 'T':
                                score = score * 0.02;
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
                                score = score * 0.18;
                                break;
                            case 'R':
                            case 'D':
                            case 'E':
                            case 'K':
                            case 'S':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'H':
                            case 'L':
                            case 'M':
                            case 'T':
                                score = score * 0.02;
                                break;
                            case 'C':
                            case 'Q':
                            case 'W':
                                score = score * 0;
                                break;
                            case 'G':
                                score = score * 0.16;
                                break;
                            case 'I':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'F':
                                score = score * 0.04;
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
                                score = score * 0.11;
                                break;
                            case 'R':
                            case 'Q':
                            case 'S':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'I':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'G':
                            case 'H':
                            case 'L':
                            case 'W':
                                score = score * 0;
                                break;
                            case 'E':
                            case 'F':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.18;
                                break;
                            case 'T':
                                score = score * 0.14;
                                break;
                            case 'P':
                                score = score * 0.07;
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
                            case 'K':
                                score = score * 0.19;
                                break;
                            case 'R':
                            case 'L':
                            case 'F':
                            case 'P':
                            case 'S':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'C':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.11;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.12;
                                break;
                            case 'Q':
                            case 'E':
                            case 'H':
                            case 'M':
                            case 'W':
                            case 'Y':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 7);

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
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        // Function (Methylation_R): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> Methylation_R(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 14.0156;
            var modName = "Methylation";
            var site = 'R';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of argenine
            var totalArg = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalArg (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'R') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalArg = totalArg + 1;

                    //variables to store sub - sequence
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                            case 'I':
                            case 'M':
                            case 'Y':
                            case 'V':
                                score = 0.03;
                                break;
                            case 'R':
                                score = 0.15;
                                break;
                            case 'N':
                            case 'F':
                                score = 0.02;
                                break;
                            case 'D':
                            case 'Q':
                            case 'L':
                                score = 0.05;
                                break;
                            case 'G':
                                score = 0.25;
                                break;
                            case 'E':
                            case 'K':
                            case 'T':
                                score = 0.04;
                                break;
                            case 'H':
                                score = 0.01;
                                break;
                            case 'P':
                                score = 0.08;
                                break;
                            case 'S':
                                score = 0.06;
                                break;
                            case 'C':
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
                                score = score * 0.09;
                                break;
                            case 'S':
                            case 'R':
                                score = score * 0.1;
                                break;
                            case 'D':
                            case 'N':
                            case 'L':
                            case 'M':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'G':
                                score = score * 0.26;
                                break;
                            case 'E':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'K':
                            case 'T':
                                score = score * 0.02;
                                break;
                            case 'Q':
                            case 'F':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0;
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
                            case 'L':
                                score = score * 0.05;
                                break;
                            case 'R':
                                score = score * 0.19;
                                break;
                            case 'N':
                            case 'D':
                            case 'Q':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'G':
                                score = score * 0.18;
                                break;
                            case 'E':
                            case 'K':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'M':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'P':
                                score = score * 0.1;
                                break;
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'Y':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0;
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
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'N':
                            case 'L':
                            case 'M':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'E':
                            case 'T':
                                score = score * 0.02;
                                break;
                            case 'C':
                            case 'I':
                                score = score * 0.01;
                                break;
                            case 'G':
                                score = score * 0.29;
                                break;
                            case 'Q':
                            case 'K':
                            case 'F':
                                score = score * 0.05;
                                break;
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'H':
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
                                score = score * 0.1;
                                break;
                            case 'R':
                                score = score * 0.18;
                                break;
                            case 'N':
                            case 'C':
                            case 'H':
                            case 'F':
                            case 'T':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'L':
                            case 'K':
                            case 'M':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'G':
                                score = score * 0.23;
                                break;
                            case 'E':
                                score = score * 0.05;
                                break;
                            case 'Q':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'W':
                                score = score * 0;
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
                            case 'L':
                                score = score * 0.05;
                                break;
                            case 'R':
                                score = score * 0.06;
                                break;
                            case 'N':
                            case 'I':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'D':
                                score = score * 0.07;
                                break;
                            case 'G':
                                score = score * 0.32;
                                break;
                            case 'E':
                            case 'K':
                            case 'T':
                            case 'V':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.1;
                                break;
                            case 'W':
                            case 'C':
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
                                score = score * 0.07;
                                break;
                            case 'R':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'H':
                            case 'I':
                            case 'T':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'E':
                            case 'Q':
                            case 'M':
                            case 'F':
                            case 'S':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'G':
                                score = score * 0.56;
                                break;
                            case 'L':
                                score = score * 0.05;
                                break;
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'W':
                            case 'C':
                                score = score * 0;
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
                                score = score * 0.07;
                                break;
                            case 'R':
                                score = score * 0.22;
                                break;
                            case 'N':
                            case 'C':
                            case 'M':
                            case 'W':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'E':
                            case 'H':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'G':
                                score = score * 0.31;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'L':
                                score = score * 0.06;
                                break;
                            case 'K':
                            case 'F':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.05;
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
                            case 'P':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'R':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'N':
                            case 'D':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'G':
                                score = score * 0.23;
                                break;
                            case 'E':
                            case 'M':
                                score = score * 0.04;
                                break;
                            case 'Q':
                            case 'H':
                            case 'K':
                            case 'T':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.01;
                                break;
                            case 'F':
                                score = score * 0.09;
                                break;
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0;
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
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.18;
                                break;
                            case 'N':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'E':
                            case 'Q':
                            case 'L':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.22;
                                break;
                            case 'H':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'F':
                            case 'T':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'C':
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
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.09;
                                break;
                            case 'N':
                            case 'E':
                            case 'H':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'L':
                            case 'K':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.29;
                                break;
                            case 'Q':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'I':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'C':
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
                            case 'D':
                            case 'Q':
                            case 'L':
                                score = score * 0.05;
                                break;
                            case 'R':
                                score = score * 0.14;
                                break;
                            case 'N':
                            case 'E':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.21;
                                break;
                            case 'H':
                            case 'I':
                                score = score * 0.01;
                                break;
                            case 'K':
                            case 'F':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'M':
                                score = score * 0.03;
                                break;
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'T':
                            case 'W':
                            case 'Y':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'C':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 8);

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
                        aa.Add(proteinSequence[i + 1]);
                        aa.Add(h);
                        aa.Add(j);
                        aa.Add(k);
                        aa.Add(l);
                        aa.Add(m);
                        aa.Add(n);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Argenine at that position
                    }
                }

                // for the TotalArg if condition coming up ahead
                index = i;
            }

            // it displays total number of Argenine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Argenine found: " + totalArg);
            }
            //disp(['Total Argenine found: ', num2str(TotalArg)])

            // returning the object array
            return array;
        }
    
       
    }
}
