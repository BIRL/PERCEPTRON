using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Glycosylation
    {
        // Function (N_linked_glycosylation_N): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> N_linked_glycosylation_N(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 317.122;
            var modName = "N-linked Glycosylation";
            var site = 'N';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Asparagine
            var totalAsn = 0;

            // stores the amino acids found
            var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalAsn (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if ((proteinSequence[i] == 'N') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalAsn = totalAsn + 1;

                    //variables to store sub - sequence
                    char b, c, d, e, f, g, h, j, k, l, m, n;

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        b = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'A':
                            case 'D':
                            case 'E':
                            case 'F':
                                score = 0.05;
                                break;
                            case 'R':
                            case 'Q':
                            case 'K':
                            case 'Y':
                                score = 0.04;
                                break;
                            case 'N':
                            case 'T':
                            case 'V':
                                score = 0.07;
                                break;
                            case 'C':
                            case 'W':
                            case 'H':
                            case 'M':
                                score = 0.02;
                                break;
                            case 'G':
                            case 'I':
                            case 'P':
                            case 'S':
                                score = 0.06;
                                break;
                            case 'L':
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
                            case 'I':
                                score = score * 0.07;
                                break;
                            case 'Q':
                            case 'R':
                            case 'F':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'G':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'D':
                            case 'E':
                            case 'K':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'C':
                                score = score * 0.03;
                                break;
                            case 'H':
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'S':
                            case 'T':
                                score = score * 0.08;
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
                            case 'E':
                            case 'K':
                            case 'F':
                            case 'P':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'C':
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'G':
                            case 'S':
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'I':
                            case 'N':
                                score = score * 0.06;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'V':
                                score = score * 0.08;
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
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'P':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'Q':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'C':
                                score = score * 0.03;
                                break;
                            case 'G':
                            case 'K':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'H':
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'V':
                                score = score * 0.08;
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
                            case 'G':
                            case 'I':
                            case 'F':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'D':
                            case 'Q':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'E':
                            case 'P':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'L':
                                score = score * 0.1;
                                break;
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.07;
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
                            case 'G':
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                            case 'F':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'E':
                            case 'K':
                            case 'P':
                                score = score * 0.04;
                                break;
                            case 'C':
                            case 'Q':
                                score = score * 0.03;
                                break;
                            case 'H':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.1;
                                break;
                            case 'M':
                                score = score * 0.01;
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

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length >= i + 1)
                    {
                        h = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'R':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'E':
                            case 'Q':
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'S':
                                score = score * 0.9;
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'P':
                                score = score * 0;
                                break;
                            case 'V':
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
                            case 'S':
                                score = score * 0.36;
                                break;
                            case 'T':
                                score = score * 0.63;
                                break;
                            default:
                                score = score * 0;
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
                            case 'E':
                            case 'I':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'Q':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'D':
                            case 'K':
                            case 'F':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'W':
                                score = score * 0.03;
                                break;
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.11;
                                break;
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'T':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'P':
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
                            case 'N':
                            case 'E':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'Q':
                            case 'K':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'G':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'L':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'S':
                                score = score * 0.08;
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
                            case 'N':
                            case 'D':
                            case 'G':
                            case 'E':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'Q':
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'C':
                                score = score * 0.03;
                                break;
                            case 'H':
                            case 'M':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'L':
                                score = score * 0.1;
                                break;
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'V':
                                score = score * 0.08;
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
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'G':
                            case 'T':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'Q':
                            case 'K':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'H':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'L':
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'M':
                                score = score * 0.01;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 9);

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

                        // score of Asparagine at that position
                    }
                }

                // for the TotalAsn if condition coming up ahead
                index = i;
            }

            // it displays total number of Asparagine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Asparagine found: " + totalAsn);
            }
            //disp(['Total Asparagine found: ', num2str(TotalAsn)])

            // returning the object array
            return array;
        }

        // Function (O_linked_glycosylation_T): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> O_linked_glycosylation_T(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 203.079;
            var modName = "O-Linked Glycosylation";
            var site = 'T';



            // variable score is initialized
            double score = 0;

            // Variable to store total number of Threonine
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
                    char a, b, c, d, e, f, g, h, m, j, k, l;

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length >= i + 1)
                    {
                        l = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'Y':
                                score = 0.01;
                                break;
                            case 'N':
                            case 'I':
                            case 'M':
                            case 'F':
                                score = 0.02;
                                break;
                            case 'D':
                                score = 0.03;
                                break;
                            case 'R':
                            case 'G':
                            case 'E':
                            case 'L':
                                score = 0.04;
                                break;
                            case 'K':
                                score = 0.06;
                                break;
                            case 'P':
                                score = 0.07;
                                break;
                            case 'Q':
                            case 'V':
                                score = 0.08;
                                break;
                            case 'A':
                                score = 0.1;
                                break;
                            case 'T':
                                score = 0.14;
                                break;
                            case 'S':
                                score = 0.17;
                                break;
                            case 'C':
                            case 'H':
                            case 'W':
                                score = 0;
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
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'E':
                            case 'Q':
                            case 'I':
                            case 'K':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'H':
                                score = score * 0.04;
                                break;
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'P':
                                score = score * 0.13;
                                break;
                            case 'S':
                            case 'T':
                                score = score * 0.15;
                                break;
                            case 'A':
                                score = score * 0.17;
                                break;
                            case 'C':
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

                    //% It saves the score for i + 3 position
                    if (proteinSequence.Length >= i + 3)
                    {
                        j = proteinSequence[i + 3];
                        switch (proteinSequence[i + 3])
                        {
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'I':
                            case 'K':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'E':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.05;
                                break;
                            case 'Q':
                                score = score * 0.06;
                                break;
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.09;
                                break;
                            case 'V':
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'S':
                                score = score * 0.14;
                                break;
                            case 'H':
                            case 'W':
                                score = score * 0;
                                break;
                            case 'A':
                                score = score * 0.13;
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
                            case 'Q':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'H':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'E':
                            case 'P':
                            case 'Y':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.11;
                                break;
                            case 'S':
                                score = score * 0.14;
                                break;
                            case 'T':
                                score = score * 0.18;
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

                    //% It saves the score for i + 5 position
                    if (proteinSequence.Length >= i + 5)
                    {
                        h = proteinSequence[i + 5];
                        switch (proteinSequence[i + 5])
                        {
                            case 'C':
                            case 'H':
                            case 'F':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'D':
                            case 'E':
                            case 'Q':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'G':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'L':
                            case 'K':
                                score = score * 0.08;
                                break;
                            case 'I':
                            case 'V':
                                score = score * 0.09;
                                break;
                            case 'A':
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'S':
                            case 'T':
                                score = score * 0.1;
                                break;
                            case 'W':
                            case 'M':
                                score = score * 0;
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
                            case 'N':
                            case 'D':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'C':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.03;
                                break;
                            case 'E':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'Q':
                            case 'H':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'P':
                                score = score * 0.09;
                                break;
                            case 'T':
                                score = score * 0.13;
                                break;
                            case 'S':
                                score = score * 0.14;
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
                        f = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'D':
                            case 'H':
                            case 'F':
                                score = score * 0.01;
                                break;
                            case 'C':
                            case 'E':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'R':
                                score = score * 0.03;
                                break;
                            case 'I':
                            case 'L':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'G':
                                score = score * 0.06;
                                break;
                            case 'A':
                                score = score * 0.1;
                                break;
                            case 'P':
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'T':
                                score = score * 0.15;
                                break;
                            case 'V':
                                score = score * 0.16;
                                break;
                            case 'W':
                            case 'Y':
                            case 'N':
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
                        e = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'D':
                            case 'C':
                            case 'Y':
                            case 'E':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'I':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'K':
                            case 'V':
                                score = score * 0.09;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'A':
                                score = score * 0.12;
                                break;
                            case 'P':
                                score = score * 0.16;
                                break;
                            case 'W':
                            case 'M':
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
                        d = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'E':
                            case 'H':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'D':
                                score = score * 0.02;
                                break;
                            case 'G':
                                score = score * 0.03;
                                break;
                            case 'Q':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'A':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'Y':
                                score = score * 0.06;
                                break;
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.09;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'V':
                                score = score * 0.15;
                                break;
                            case 'P':
                                score = score * 0.17;
                                break;
                            case 'C':
                            case 'M':
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
                        c = proteinSequence[i - 4];
                        switch (proteinSequence[i - 4])
                        {
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'I':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'V':
                            case 'R':
                                score = score * 0.05;
                                break;
                            case 'E':
                            case 'Q':
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.1;
                                break;
                            case 'T':
                                score = score * 0.13;
                                break;
                            case 'S':
                                score = score * 0.15;
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

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        b = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'C':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'I':
                            case 'G':
                            case 'E':
                                score = score * 0.04;
                                break;
                            case 'A':
                            case 'D':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'L':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'S':
                            case 'R':
                                score = score * 0.08;
                                break;
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'T':
                                score = score * 0.13;
                                break;
                            case 'W':
                                score = score * 0;
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
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'Q':
                            case 'H':
                            case 'M':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'I':
                            case 'L':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'E':
                                score = score * 0.05;
                                break;
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'K':
                                score = score * 0.08;
                                break;
                            case 'A':
                                score = score * 0.09;
                                break;
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'S':
                                score = score * 0.16;
                                break;
                            case 'T':
                                score = score * 0.1;
                                break;
                            case 'W':
                            case 'Y':
                            case 'C':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 10);

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

                        // score of Threonine at that position
                    }
                }

                // for the TotalThr if condition coming up ahead
                index = i;
            }

            // it displays total number of Threonine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Threonine found: " + totalThr);
            }
            //disp(['Total Threonine found: ', num2str(TotalThr)])

            // returning the object array
            return array;
        }

        // Function (O_linked_glycosylation_S): Returns an object array with all the required parameters stored
        public static List<PostTranslationModificationsSiteDto> O_linked_glycosylation_S(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";



            var modWeight = 203.079;
            var modName = "O-Linked Glycosylation";
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
                    char a, b, c, d, e, f, g, h, m, j, k, l;

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length >= i + 1)
                    {
                        l = proteinSequence[i + 1];
                        switch (proteinSequence[i + 1])
                        {
                            case 'C':
                            case 'H':
                            case 'Y':
                                score = 0.01;
                                break;
                            case 'D':
                            case 'I':
                            case 'K':
                                score = 0.02;
                                break;
                            case 'Q':
                            case 'V':
                                score = 0.03;
                                break;
                            case 'R':
                                score = 0.04;
                                break;
                            case 'G':
                                score = 0.06;
                                break;
                            case 'L':
                                score = 0.07;
                                break;
                            case 'S':
                                score = 0.09;
                                break;
                            case 'P':
                                score = 0.1;
                                break;
                            case 'E':
                                score = 0.16;
                                break;
                            case 'A':
                                score = 0.14;
                                break;
                            case 'T':
                                score = 0.2;
                                break;
                            case 'N':
                            case 'M':
                            case 'F':
                            case 'W':
                                score = 0;
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
                            case 'D':
                            case 'K':
                            case 'M':
                            case 'F':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'Q':
                            case 'I':
                            case 'L':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'E':
                                score = score * 0.03;
                                break;
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'G':
                                score = score * 0.11;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'P':
                                score = score * 0.17;
                                break;
                            case 'A':
                                score = score * 0.3;
                                break;
                            case 'N':
                            case 'C':
                            case 'H':
                            case 'Y':
                                score = score * 0;
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
                            case 'R':
                            case 'N':
                            case 'C':
                            case 'M':
                            case 'F':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'E':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Q':
                            case 'L':
                                score = score * 0.03;
                                break;
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'D':
                                score = score * 0.09;
                                break;
                            case 'T':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.17;
                                break;
                            case 'W':
                            case 'K':
                                score = score * 0;
                                break;
                            case 'P':
                                score = score * 0.26;
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
                            case 'E':
                            case 'M':
                            case 'W':
                            case 'Y':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'D':
                            case 'Q':
                            case 'K':
                                score = score * 0.02;
                                break;
                            case 'I':
                            case 'L':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.05;
                                break;
                            case 'A':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'T':
                                score = score * 0.31;
                                break;
                            case 'P':
                                score = score * 0.2;
                                break;
                            case 'C':
                            case 'F':
                                score = score * 0;
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
                            case 'M':
                            case 'F':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'D':
                            case 'E':
                            case 'I':
                            case 'K':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'Q':
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'S':
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'R':
                                score = score * 0.1;
                                break;
                            case 'G':
                                score = score * 0.16;
                                break;
                            case 'P':
                                score = score * 0.11;
                                break;
                            case 'A':
                                score = score * 0.14;
                                break;
                            case 'W':
                                score = score * 0;
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
                            case 'D':
                            case 'C':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'E':
                            case 'I':
                            case 'K':
                                score = score * 0.02;
                                break;
                            case 'Q':
                                score = score * 0.03;
                                break;
                            case 'R':
                                score = score * 0.04;
                                break;
                            case 'A':
                            case 'G':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'T':
                                score = score * 0.09;
                                break;
                            case 'H':
                                score = score * 0.1;
                                break;
                            case 'S':
                                score = score * 0.12;
                                break;
                            case 'P':
                                score = score * 0.29;
                                break;
                            case 'W':
                            case 'F':
                            case 'N':
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
                        f = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'R':
                            case 'N':
                            case 'E':
                            case 'H':
                            case 'K':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'Q':
                                score = score * 0.02;
                                break;
                            case 'L':
                                score = score * 0.03;
                                break;
                            case 'I':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'A':
                                score = score * 0.06;
                                break;
                            case 'V':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'G':
                                score = score * 0.13;
                                break;
                            case 'P':
                                score = score * 0.14;
                                break;
                            case 'T':
                                score = score * 0.31;
                                break;
                            case 'W':
                            case 'C':
                            case 'D':
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
                        e = proteinSequence[i - 2];
                        switch (proteinSequence[i - 2])
                        {
                            case 'C':
                            case 'D':
                            case 'H':
                            case 'I':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'R':
                            case 'Q':
                            case 'L':
                            case 'K':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'E':
                                score = score * 0.04;
                                break;
                            case 'N':
                            case 'G':
                            case 'T':
                                score = score * 0.06;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'V':
                                score = score * 0.13;
                                break;
                            case 'P':
                                score = score * 0.21;
                                break;
                            case 'A':
                                score = score * 0.18;
                                break;
                            case 'W':
                            case 'F':
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
                        d = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'N':
                            case 'C':
                            case 'H':
                            case 'K':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'R':
                            case 'Q':
                            case 'I':
                            case 'L':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'W':
                                score = score * 0.03;
                                break;
                            case 'E':
                                score = score * 0.04;
                                break;
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'G':
                                score = score * 0.12;
                                break;
                            case 'A':
                                score = score * 0.14;
                                break;
                            case 'D':
                                score = score * 0.15;
                                break;
                            case 'P':
                                score = score * 0.16;
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
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'R':
                            case 'Q':
                            case 'I':
                            case 'M':
                            case 'F':
                            case 'V':
                            case 'N':
                                score = score * 0.02;
                                break;
                            case 'G':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'E':
                                score = score * 0.05;
                                break;
                            case 'T':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'H':
                                score = score * 0.09;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'P':
                                score = score * 0.16;
                                break;
                            case 'D':
                                score = score * 0.14;
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
                            case 'N':
                            case 'H':
                            case 'F':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'D':
                            case 'Q':
                                score = score * 0.03;
                                break;
                            case 'V':
                            case 'L':
                                score = score * 0.04;
                                break;
                            case 'E':
                                score = score * 0.05;
                                break;
                            case 'K':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'T':
                                score = score * 0.09;
                                break;
                            case 'R':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.14;
                                break;
                            case 'G':
                                score = score * 0.17;
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

                    //% it will score amino acid at position i - 6
                    if (i - 6 > 0)
                    {
                        a = proteinSequence[i - 6];
                        switch (proteinSequence[i - 6])
                        {
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'C':
                            case 'Q':
                            case 'H':
                            case 'K':
                            case 'F':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'E':
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'L':
                                score = score * 0.05;
                                break;
                            case 'A':
                                score = score * 0.07;
                                break;
                            case 'P':
                                score = score * 0.18;
                                break;
                            case 'S':
                                score = score * 0.21;
                                break;
                            case 'T':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 11);

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

    }
}
