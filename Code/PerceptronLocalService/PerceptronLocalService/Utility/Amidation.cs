using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class Amidation
    {
        // Function (Amidation_F): Returns an object array with all the required parameters stored
        public  static List<PostTranslationModificationsSiteDto> Amidation_F(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

          

            var modWeight = -0.984016;
            var modName = "Amidation";
            var site = 'F';


            // variable score is initialized
            double score = 0;

            // Variable to store total number of Phenylalanine
            var totalPhe = 0;

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
                if ((proteinSequence[i] == 'F') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                    (i + 6 < proteinSequence.Length))
                {
                    totalPhe = totalPhe + 1;

                    //variables to store sub - sequence
                    char a, b, c, d, e, f;

                    //% it will score amino acid at position i - 1
                    if (i - 1 > 0)
                    {
                        f = proteinSequence[i - 1];
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'I':
                            case 'L':
                            case 'K':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = 0.01;
                                break;
                            case 'G':
                                score = 0.02;
                                break;
                            case 'H':
                                score = 0.03;
                                break;
                            case 'D':
                                score = 0.15;
                                break;
                            case 'R':
                                score = 0.72;
                                break;
                            case 'N':
                            case 'C':
                            case 'E':
                            case 'Q':
                            case 'M':
                            case 'F':
                            case 'P':
                            case 'W':
                            case 'Y':
                                score = 0;
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
                            case 'A':
                            case 'R':
                            case 'C':
                            case 'T':
                            case 'Y':
                            case 'V':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'Q':
                            case 'H':
                            case 'K':
                            case 'P':
                            case 'S':
                                score = score * 0.02;
                                break;
                            case 'E':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'G':
                                score = score * 0.21;
                                break;
                            case 'L':
                                score = score * 0.22;
                                break;
                            case 'M':
                                score = score * 0.29;
                                break;
                            case 'N':
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
                        d = proteinSequence[i - 3];
                        switch (proteinSequence[i - 3])
                        {
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'D':
                            case 'G':
                            case 'H':
                            case 'L':
                            case 'P':
                                score = score * 0.02;
                                break;
                            case 'A':
                                score = score * 0.03;
                                break;
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'R':
                            case 'K':
                            case 'S':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'Q':
                                score = score * 0.16;
                                break;
                            case 'F':
                                score = score * 0.32;
                                break;
                            case 'W':
                                score = score * 0.14;
                                break;
                            case 'N':
                            case 'C':
                            case 'E':
                            case 'T':
                            case 'I':
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
                            case 'Q':
                            case 'K':
                            case 'M':
                            case 'T':
                            case 'V':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'S':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'E':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'A':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'N':
                                score = score * 0.08;
                                break;
                            case 'P':
                            case 'L':
                                score = score * 0.11;
                                break;
                            case 'D':
                                score = score * 0.19;
                                break;
                            case 'G':
                                score = score * 0.2;
                                break;
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

                    //% it will score amino acid at position i - 5
                    if (i - 5 > 0)
                    {
                        b = proteinSequence[i - 5];
                        switch (proteinSequence[i - 5])
                        {
                            case 'I':
                                score = score * 0.01;
                                break;
                            case 'L':
                                score = score * 0.02;
                                break;
                            case 'N':
                            case 'H':
                            case 'F':
                            case 'W':
                            case 'V':
                                score = score * 0.03;
                                break;
                            case 'M':
                                score = score * 0.04;
                                break;
                            case 'A':
                            case 'G':
                            case 'Q':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'T':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'K':
                                score = score * 0.09;
                                break;
                            case 'D':
                                score = score * 0.1;
                                break;
                            case 'C':
                                score = score * 0;
                                break;
                            case 'E':
                                score = score * 0.16;
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
                            case 'L':
                            case 'T':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'M':
                            case 'V':
                                score = score * 0.02;
                                break;
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'A':
                            case 'N':
                            case 'E':
                            case 'P':
                                score = score * 0.06;
                                break;
                            case 'Q':
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'K':
                                score = score * 0.08;
                                break;
                            case 'G':
                                score = score * 0.09;
                                break;
                            case 'Y':
                                score = score * 0.1;
                                break;
                            case 'R':
                            case 'D':
                                score = score * 0.11;
                                break;
                            case 'F':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = PtmScoreNormalizer.Normalize(score, 5);

                    if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                        (i - 2 >= 0) && (i - 1 >= 0))
                    {
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

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, aa));

                        objectArrayIndex++;

                        // score of Phenylalanine at that position
                    }
                }

                // for the TotalPhe if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            if (index == proteinSequence.Length)
            {
                Console.WriteLine("Total Phenylalanine found: " + totalPhe);
            }
            //disp(['Total Phenylalanine found: ', num2str(TotalPhe)])

            // returning the object array
            return array;
        }

    }
}
