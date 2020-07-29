using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Methylation_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();
        PtmScoreNormalizer _PtmScoreNormalizer = new PtmScoreNormalizer();

        // Function (Methylation_K): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Methylation_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13); Updated 20200714
            var modName = "Methylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'K';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

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
                    var sub_sequence = new List<char>();  // Updated 20200714

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // Updated 20200714
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // Updated 20200714
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
                    if (i - 5 >= 0)  // Updated 20200714
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // Updated 20200714
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
                    if (i - 4 >= 0)  // Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // Updated 20200714
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
                    if (i - 3 >= 0)  // Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // Updated 20200714
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
                    if (i - 2 >= 0)  // Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // Updated 20200714
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
                    if (i - 1 >= 0)  // Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // Updated 20200714
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

                    sub_sequence.Add(proteinSequence[i]);   //Updated 20200714

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 2)  // Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 3)  // Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 4)  // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 5)  // Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 6)  // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // Updated 20200714
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
                    score = _PtmScoreNormalizer.Normalize(score, 7);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // Updated 20200714
                    {
                        //b = proteinSequence[i - 6];  // Updated 20200714
                        //c = proteinSequence[i - 5];
                        //d = proteinSequence[i - 4];
                        //e = proteinSequence[i - 3];
                        //f = proteinSequence[i - 2];
                        //g = proteinSequence[i - 1];
                        //h = proteinSequence[i + 1];
                        //j = proteinSequence[i + 2];
                        //k = proteinSequence[i + 3];
                        //l = proteinSequence[i + 4];
                        //m = proteinSequence[i + 5];
                        //n = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(b);  // Updated 20200714
                        //aa.Add(c);
                        //aa.Add(d);
                        //aa.Add(e);
                        //aa.Add(f);
                        //aa.Add(g);
                        //aa.Add(proteinSequence[i]);
                        //aa.Add(h);
                        //aa.Add(j);
                        //aa.Add(k);
                        //aa.Add(l);
                        //aa.Add(m);
                        //aa.Add(n);

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
            //if (index == proteinSequence.Length)  Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        // Function (Methylation_R): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Methylation_R(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(14);   Updated 20200714

            var modName = "Methylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'R';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of argenine
            var totalArg = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalArg (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'R') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'R')
                {
                    totalArg = totalArg + 1;
                    //stores the amino acids found   
                    var sub_sequence = new List<char>();  // Updated 20200714

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // Updated 20200714
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // Updated 20200714
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
                    if (i - 5 >= 0)  // Updated 20200714
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // Updated 20200714
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
                    if (i - 4 >= 0)  // Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // Updated 20200714
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
                    if (i - 3 >= 0)  // Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // Updated 20200714
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
                    if (i - 2 >= 0)  // Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // Updated 20200714
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
                    if (i - 1 >= 0)  // Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // Updated 20200714
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
                    sub_sequence.Add(proteinSequence[i]);  // Updated 20200714

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 2)  // Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 3)  // Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 4)  // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 5)  // Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // Updated 20200714
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
                    if (proteinSequence.Length > i + 6)  // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // Updated 20200714
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
                    score = _PtmScoreNormalizer.Normalize(score, 8);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //b = proteinSequence[i - 6];   Updated 20200714
                        //c = proteinSequence[i - 5];
                        //d = proteinSequence[i - 4];
                        //e = proteinSequence[i - 3];
                        //f = proteinSequence[i - 2];
                        //g = proteinSequence[i - 1];
                        //h = proteinSequence[i + 1];
                        //j = proteinSequence[i + 2];
                        //k = proteinSequence[i + 3];
                        //l = proteinSequence[i + 4];
                        //m = proteinSequence[i + 5];
                        //n = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(b);    Updated 20200714
                        //aa.Add(c);
                        //aa.Add(d);
                        //aa.Add(e);
                        //aa.Add(f);
                        //aa.Add(g);
                        //aa.Add(proteinSequence[i]);
                        //aa.Add(proteinSequence[i + 1]);
                        //aa.Add(h);
                        //aa.Add(j);
                        //aa.Add(k);
                        //aa.Add(l);
                        //aa.Add(m);
                        //aa.Add(n);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));

                        objectArrayIndex++;

                        // score of Argenine at that position
                    }
                }

                // for the TotalArg if condition coming up ahead
                index = i;
            }

            // it displays total number of Argenine found in sequence
            //if (index == proteinSequence.Length)  // Updated 20200714 does not exist inspectrum
            //{
            //    Console.WriteLine("Total Argenine found: " + totalArg);
            //}
            //disp(['Total Argenine found: ', num2str(TotalArg)])

            // returning the object array
            return array;
        }

    }
}
