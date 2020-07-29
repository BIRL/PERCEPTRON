using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Amidation_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();
        PtmScoreNormalizer _PtmScoreNormalizer = new PtmScoreNormalizer();

        // Function (Amidation_F): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Amidation_F(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // Updated 20200714

            var modName = "Amidation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'F';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Phenylalanine
            var totalPhe = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalSer (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'F') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'F')  // Updated 20200714
                {
                    totalPhe = totalPhe + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // Updated 20200714

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // Updated 20200714
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // Updated 20200714
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'L':
                            case 'T':
                            case 'W':
                                score = 0.01;  // Updated 20200714
                                break;
                            case 'I':
                            case 'M':
                            case 'V':
                                score = 0.02;  // Updated 20200714
                                break;
                            case 'H':
                                score = 0.03;  // Updated 20200714
                                break;
                            case 'A':
                            case 'N':
                            case 'E':
                            case 'P':
                                score = 0.06;  // Updated 20200714
                                break;
                            case 'Q':
                            case 'S':
                                score = 0.07;  // Updated 20200714
                                break;
                            case 'K':
                                score = 0.08;  // Updated 20200714
                                break;
                            case 'G':
                                score = 0.09;  // Updated 20200714
                                break;
                            case 'Y':
                                score = 0.1;  // Updated 20200714
                                break;
                            case 'R':
                            case 'D':
                                score = 0.11;  // Updated 20200714
                                break;
                            case 'F':
                                score = 0;  // Updated 20200714
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
                        sub_sequence.Add(minus5);  // Updated 20200714
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

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0)  // Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // Updated 20200714
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

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0)  // Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // Updated 20200714
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

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0)  // Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // Updated 20200714
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

                    //% it will score amino acid at position i - 1
                    if (i - 1 >= 0)  // Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // Updated 20200714
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'I':
                            case 'L':
                            case 'K':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.01;  // Updated 20200714
                                break;
                            case 'G':
                                score = score * 0.02;  // Updated 20200714
                                break;
                            case 'H':
                                score = score * 0.03;  // Updated 20200714
                                break;
                            case 'D':
                                score = score * 0.15;  // Updated 20200714
                                break;
                            case 'R':
                                score = score * 0.72;  // Updated 20200714
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
                                score = score * 0;  // Updated 20200714
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    sub_sequence.Add(proteinSequence[i]);

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 5);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //f = proteinSequence[i - 1];   Updated 20200714
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

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));

                        objectArrayIndex++;

                        // score of Phenylalanine at that position
                    }
                }

                // for the TotalPhe if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            //if (index == proteinSequence.Length)  Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Phenylalanine found: " + totalPhe);
            //}
            //disp(['Total Phenylalanine found: ', num2str(TotalPhe)])

            // returning the object array
            return array;
        }

    }
}
