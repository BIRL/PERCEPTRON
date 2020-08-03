using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Hydroxylation_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();
        PtmScoreNormalizer _PtmScoreNormalizer = new PtmScoreNormalizer();

        // Function (Hydroxylation_P): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Hydroxylation_P(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);

            var modName = "Hydroxylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'P';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Proline
            var totalPro = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalPro (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'P') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'P')
                {
                    totalPro = totalPro + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // Updated 20200714

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = proteinSequence[i + 1];
                        int a = i + 1;
                        sub_sequence.Add(plus1);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 2)  // Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 3)  // Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 4)  // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 5)  // Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 6)  // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // Updated 20200714
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

                    sub_sequence.Insert(0, proteinSequence[i]); // Updated 20200714

                    //% it will score amino acid at position i - 1
                    if (i - 1 >= 0)  // Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Insert(0, minus1);  // Updated 20200714
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
                    if (i - 2 >= 0)  // Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Insert(0, minus2);  // Updated 20200714
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
                    if (i - 3 >= 0)  // Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Insert(0, minus3);  // Updated 20200714
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
                    if (i - 4 >= 0)  // Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Insert(0, minus4);  // Updated 20200714
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
                    if (i - 5 >= 0)  // Updated 20200714
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Insert(0, minus5);  // Updated 20200714
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
                    if (i - 6 >= 0)
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Insert(0, minus6);  // Updated 20200714
                        switch (proteinSequence[i - 6])
                        {
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'M':
                            case 'Y':
                                score = score * 0.02;  // Updated 20200714
                                break;
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'V':
                                score = score * 0.03;  // Updated 20200714
                                break;
                            case 'T':
                                score = score * 0.04;  // Updated 20200714
                                break;
                            case 'S':
                                score = score * 0.05;  // Updated 20200714
                                break;
                            case 'Q':
                                score = score * 0.06;  // Updated 20200714
                                break;
                            case 'R':
                                score = score * 0.08;  // Updated 20200714
                                break;
                            case 'K':
                                score = score * 0.12;  // Updated 20200714
                                break;
                            case 'A':
                                score = score * 0.13;  // Updated 20200714
                                break;
                            case 'P':
                                score = score * 0.3;  // Updated 20200714
                                break;
                            default:
                                score = score * 0;
                                break;
                        }
                    }


                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 6);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= Tolerance)
                    {
                        //l = proteinSequence[i + 1];   Updated 20200714
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];
                        //f = proteinSequence[i - 1];
                        //e = proteinSequence[i - 2];
                        //d = proteinSequence[i - 3];
                        //c = proteinSequence[i - 4];
                        //b = proteinSequence[i - 5];
                        //a = proteinSequence[i - 6];

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

                        // score of Proline at that position
                    }
                }

                // for the TotalPro if condition coming up ahead
                index = i;
            }

            // it displays total number of Proline found in sequence
            //if (index == proteinSequence.Length)  Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Proline found: " + totalPro);
            //}
            //disp(['Total Proline found: ', num2str(TotalPro)])

            // returning the object array
            return array;
        }

    }
}
