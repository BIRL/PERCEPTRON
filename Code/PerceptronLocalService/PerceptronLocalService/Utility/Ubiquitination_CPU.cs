using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Ubiquitination_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();
        PtmScoreNormalizer _PtmScoreNormalizer = new PtmScoreNormalizer();

        // Function (Ubiquitination_K): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Ubiquitination_K(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13);    Updated 20200714

            double modWeight = 8561;
            var modName = "protein_sequenceation";
            var site = 'K';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    Updated 20200714

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalLys (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'K') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'K')
                {
                    totalLys = totalLys + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  //Updated 20200714

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  //Updated 20200714
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);   // Updated 20200714
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'W':
                                score = 0.01;   // Updated 20200714
                                break;
                            case 'M':
                            case 'H':
                                score = 0.02;   // Updated 20200714
                                break;
                            case 'Y':
                            case 'F':
                                score = 0.03;   // Updated 20200714
                                break;
                            case 'N':
                            case 'P':
                                score = 0.04;   // Updated 20200714
                                break;
                            case 'Q':
                            case 'I':
                            case 'T':
                                score = 0.05;   // Updated 20200714
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                                score = 0.06;   // Updated 20200714
                                break;
                            case 'K':
                            case 'S':
                            case 'V':
                                score = 0.07;   // Updated 20200714
                                break;
                            case 'A':
                            case 'E':
                                score = 0.08;   // Updated 20200714
                                break;
                            case 'L':
                                score = 0.1;   // Updated 20200714
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0)  //Updated 20200714
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);   // Updated 20200714
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

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0)  //Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);   // Updated 20200714
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

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0)  //Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);   // Updated 20200714
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

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0)  //Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);   // Updated 20200714
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

                    //% it will score amino acid at position i - 1
                    if (i - 1 >= 0)  //Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);   // Updated 20200714
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
                    sub_sequence.Add(proteinSequence[i]);   // Updated 20200714

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  //Updated 20200714
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);   // Updated 20200714
                        switch (proteinSequence[i + 1])
                        {
                            case 'C':
                            case 'W':
                                score = score * 0.01;   // Updated 20200714
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;   // Updated 20200714
                                break;
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = score * 0.04;   // Updated 20200714
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                            case 'P':
                                score = score * 0.05;   // Updated 20200714
                                break;
                            case 'D':
                            case 'G':
                            case 'Q':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.06;   // Updated 20200714
                                break;
                            case 'A':
                                score = score * 0.08;   // Updated 20200714
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.1;   // Updated 20200714
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2)  //Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);   // Updated 20200714
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
                    if (proteinSequence.Length > i + 3)  //Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);   // Updated 20200714
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
                    if (proteinSequence.Length > i + 4)  //Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);   // Updated 20200714
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
                    if (proteinSequence.Length > i + 5)  //Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);   // Updated 20200714
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
                    if (proteinSequence.Length > i + 6)  //Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);   // Updated 20200714
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

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 15);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= Tolerance)  // Updated 20200714
                    {
                        //l = proteinSequence[i + 1];   Updated 20200714
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];
                        //a = proteinSequence[i - 6];
                        //b = proteinSequence[i - 5];
                        //c = proteinSequence[i - 4];
                        //d = proteinSequence[i - 3];
                        //e = proteinSequence[i - 2];
                        //f = proteinSequence[i - 1];

                        //% it stores the protein sub-sequence
                        //aa.Add(a);    Updated 20200714
                        //aa.Add(b);
                        //aa.Add(c);
                        //aa.Add(d);
                        //aa.Add(e);
                        //aa.Add(f);
                        //aa.Add(proteinSequence[i]);
                        //aa.Add(l);
                        //aa.Add(k);
                        //aa.Add(j);
                        //aa.Add(m);
                        //aa.Add(h);
                        //aa.Add(g);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));

                        objectArrayIndex++;

                        // score of Lysine at that position
                    }
                }

                // for the TotalLys if condition coming up ahead
                index = i;
            }

            // it displays total number of Lysine found in sequence
            //if (index == proteinSequence.Length)  Updated 20200714
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

    }
}
