using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Acetylation_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();
        PtmScoreNormalizer _PtmScoreNormalizer = new PtmScoreNormalizer();

        // Function (Acetylation_A): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Acetylation_A(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    Updated 20200714

            var modName = "Acetylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'A';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Alanine
            var totalAla = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalAla (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'A') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length)) //Remove
                if (proteinSequence[i] == 'A')  // Updated 20200714
                {
                    totalAla = totalAla + 1;
                    // stores the amino acids found // Updated 20200714
                    var sub_sequence = new List<char>();

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]);     // Updated 20200714

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1) //Redundant // Updated 20200714
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);  // Updated 20200714
                        switch (proteinSequence[i + 1])
                        {
                            case 'R':
                            case 'H':
                            case 'M':
                            case 'Y':
                                score = 0.01;
                                break;
                            case 'N':
                            case 'C':
                            case 'K':
                            case 'F':
                                score = 0.02;
                                break;
                            case 'L':
                            case 'V':
                                score = 0.03;
                                break;
                            case 'Q':
                                score = 0.04;
                                break;
                            case 'G':
                                score = 0.06;
                                break;
                            case 'T':
                                score = 0.09;
                                break;
                            case 'A':
                                score = 0.24;
                                break;
                            case 'D':
                                score = 0.11;
                                break;
                            case 'E':
                            case 'S':
                                score = 0.14;
                                break;
                            case 'W':
                            case 'I':
                            case 'P':
                                score = 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2) // Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // Updated 20200714
                        switch (proteinSequence[i + 2])
                        {
                            case 'Y':
                            case 'M':
                            case 'H':
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'N':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'E':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'Q':
                            case 'L':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'A':
                                score = score * 0.17;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 3 position
                    if (proteinSequence.Length > i + 3) // Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // Updated 20200714
                        switch (proteinSequence[i + 3])
                        {
                            case 'C':
                            case 'W':
                            case 'M':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'R':
                            case 'F':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'N':
                            case 'Q':
                            case 'I':
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'E':
                            case 'V':
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'G':
                            case 'S':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.17;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 4 position
                    if (proteinSequence.Length > i + 4) // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // Updated 20200714
                        switch (proteinSequence[i + 4])
                        {
                            case 'H':
                            case 'M':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'P':
                            case 'S':
                            case 'V':
                                score = score * 0.08;
                                break;
                            case 'G':
                            case 'T':
                                score = score * 0.09;
                                break;
                            case 'E':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 5 position
                    if (proteinSequence.Length > i + 5) // Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // Updated 20200714
                        switch (proteinSequence[i + 5])
                        {
                            case 'C':
                            case 'W':
                            case 'H':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'F':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'K':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'Q':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'E':
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'A':
                                score = score * 0.15;
                                break;
                            case 'L':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 6 position
                    if (proteinSequence.Length > i + 6) // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  //Updated 20200714
                        switch (proteinSequence[i + 6])
                        {
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'C':
                            case 'F':
                            case 'Y':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'N':
                            case 'Q':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'T':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'G':
                                score = score * 0.07;
                                break;
                            case 'P':
                            case 'V':
                            case 'E':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.1;
                                break;
                            case 'A':
                                score = score * 0.12;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 1);

                    if (score >= Tolerance)
                    {
                        //l = proteinSequence[i + 1];   Updated 20200714
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   Updated 20200714
                        //aa.Add(l);
                        //aa.Add(k);
                        //aa.Add(j);
                        //aa.Add(m);
                        //aa.Add(h);
                        //aa.Add(g);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));


                        // score of Alanine at that position
                    }
                }

                // for the TotalAla if condition coming up ahead
                index = i;
            }

            // it displays total number of Alanine found in sequence
            //if (index == proteinSequence.Length) Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Alanine found: " + totalAla);
            //}
            //disp(['Total Alanine found: ', num2str(TotalAla)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_K): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Acetylation_K(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);  Updated 20200714

            var modName = "Acetylation";
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
                if (proteinSequence[i] == 'K')  // Updated 20200714
                {
                    totalLys = totalLys + 1;
                    // stores the amino acids found // Updated 20200714
                    var sub_sequence = new List<char>();

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // Updated 20200714 if (i - 6 > 0)
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // Updated 20200714
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'W':
                                score = 0.01; // Updated 20200714
                                break;
                            case 'M':
                            case 'H':
                                score = 0.02; // Updated 20200714
                                break;
                            case 'Y':
                                score = 0.03; // Updated 20200714
                                break;
                            case 'N':
                            case 'I':
                            case 'F':
                            case 'Q':
                                score = 0.04; // Updated 20200714
                                break;
                            case 'D':
                            case 'T':
                            case 'P':
                            case 'V':
                                score = 0.05; // Updated 20200714
                                break;
                            case 'G':
                                score = 0.06; // Updated 20200714
                                break;
                            case 'R':
                            case 'S':
                            case 'E':
                                score = 0.07; // Updated 20200714
                                break;
                            case 'A':
                            case 'L':
                                score = 0.09; // Updated 20200714
                                break;
                            case 'K':
                                score = 0.11; // Updated 20200714
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0) // Updated 20200714
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // Updated 20200714
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
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'D':
                            case 'Q':
                                score = score * 0.04;
                                break;
                            case 'I':
                            case 'T':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.06;
                                break;
                            case 'R':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.12;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0) // Updated 20200714
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // Updated 20200714
                        switch (proteinSequence[i - 4])
                        {
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'Y':
                            case 'H':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'R':
                            case 'Q':
                                score = score * 0.05;
                                break;
                            case 'P':
                            case 'T':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'A':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'E':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0) // Updated 20200714
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // Updated 20200714
                        switch (proteinSequence[i - 3])
                        {
                            case 'C':
                            case 'W':
                                score = score * 0.01;
                                break;
                            case 'M':
                                score = score * 0.02;
                                break;
                            case 'Y':
                            case 'H':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'Q':
                            case 'I':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'T':
                            case 'S':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'K':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'E':
                            case 'L':
                                score = score * 0.09;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0) // Updated 20200714
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // Updated 20200714
                        switch (proteinSequence[i - 2])
                        {
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                            case 'R':
                            case 'W':
                                score = score * 0.02;
                                break;
                            case 'Q':
                                score = score * 0.03;
                                break;
                            case 'P':
                            case 'T':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'Y':
                                score = score * 0.05;
                                break;
                            case 'E':
                            case 'I':
                            case 'K':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'G':
                            case 'F':
                                score = score * 0.08;
                                break;
                            case 'A':
                                score = score * 0.09;
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
                    if (i - 1 >= 0) // Updated 20200714
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // Updated 20200714
                        switch (proteinSequence[i - 1])
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
                            case 'P':
                            case 'R':
                                score = score * 0.03;
                                break;
                            case 'F':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'Q':
                            case 'D':
                            case 'T':
                            case 'K':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'S':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    sub_sequence.Add(proteinSequence[i]);  // Updated 20200714

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);  // Updated 20200714
                        switch (proteinSequence[i + 1])
                        {
                            case 'W':
                                score = score * 0.01;  // Updated 20200714
                                break;
                            case 'M':
                                score = score * 0.02;  // Updated 20200714
                                break;
                            case 'Q':
                                score = score * 0.03;  // Updated 20200714
                                break;
                            case 'N':
                            case 'H':
                            case 'I':
                            case 'T':
                                score = score * 0.04;  // Updated 20200714
                                break;
                            case 'V':
                            case 'S':
                            case 'F':
                            case 'R':
                                score = score * 0.05;  // Updated 20200714
                                break;
                            case 'G':
                            case 'D':
                            case 'P':
                                score = score * 0.06;  // Updated 20200714
                                break;
                            case 'K':
                                score = score * 0.07;  // Updated 20200714
                                break;
                            case 'A':
                            case 'E':
                            case 'Y':
                                score = score * 0.08;  // Updated 20200714
                                break;
                            case 'L':
                                score = score * 0.09;  // Updated 20200714
                                break;
                            case 'C':
                                score = 0;
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
                            case 'W':
                            case 'C':
                                score = score * 0.01;
                                break;
                            case 'M':
                            case 'H':
                                score = score * 0.02;
                                break;
                            case 'Q':
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'Y':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'P':
                            case 'F':
                            case 'R':
                                score = score * 0.05;
                                break;
                            case 'G':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'K':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'I':
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

                    //% It saves the score for i + 3 position
                    if (proteinSequence.Length > i + 3)
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // Updated 20200714
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
                            case 'Q':
                            case 'I':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'V':
                            case 'P':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'D':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'G':
                            case 'L':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'K':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 4 position
                    if (proteinSequence.Length > i + 4)
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // Updated 20200714
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
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'P':
                            case 'Q':
                            case 'F':
                                score = score * 0.04;
                                break;
                            case 'V':
                            case 'D':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'T':
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'A':
                            case 'G':
                            case 'R':
                                score = score * 0.07;
                                break;
                            case 'L':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'K':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 5 position
                    if (proteinSequence.Length > i + 5)
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // Updated 20200714
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
                            case 'P':
                            case 'Q':
                            case 'F':
                            case 'N':
                            case 'I':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'R':
                            case 'S':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'E':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.14;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 6 position
                    if (proteinSequence.Length > i + 6)
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // Updated 20200714
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
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'P':
                            case 'Q':
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                            case 'T':
                            case 'I':
                                score = score * 0.05;
                                break;
                            case 'S':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                            case 'E':
                            case 'R':
                                score = score * 0.07;
                                break;
                            case 'A':
                                score = score * 0.08;
                                break;
                            case 'L':
                                score = score * 0.09;
                                break;
                            case 'K':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 2);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= Tolerance)  // Updated 20200714
                    {
                        //l = proteinSequence[i + 1];
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
                        //aa.Add(a);
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
            //if (index == proteinSequence.Length)  // Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_M): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Acetylation_M(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // Updated 20200714

            var modName = "Acetylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'M';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Methionine
            var totalMet = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalMet (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'M') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'M')  // Updated 20200714
                {
                    totalMet = totalMet + 1;
                    // stores the amino acids found   
                    var sub_sequence = new List<char>();  //Updated 20200714

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]); // Updated 20200714

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // Updated 20200714
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'I':
                            case 'K':
                            case 'S':
                            case 'T':
                                score = 0.01;
                                break;
                            case 'R':
                            case 'C':
                            case 'G':
                            case 'H':
                            case 'P':
                            case 'W':
                            case 'Y':
                                score = 0;
                                break;
                            case 'N':
                                score = 0.09;
                                break;
                            case 'D':
                                score = 0.28;
                                break;
                            case 'E':
                                score = 0.39;
                                break;
                            case 'Q':
                            case 'V':
                                score = 0.03;
                                break;
                            case 'M':
                            case 'F':
                                score = 0.04;
                                break;
                            case 'L':
                                score = 0.12;
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
                            case 'A':
                                score = score * 0.1;
                                break;
                            case 'R':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'W':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                                score = score * 0.09;
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'Q':
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.14;
                                break;
                            case 'S':
                                score = score * 0.11;
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
                            case 'A':
                            case 'E':
                            case 'Q':
                                score = score * 0.09;
                                break;
                            case 'N':
                            case 'G':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'R':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'T':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'M':
                            case 'W':
                                score = score * 0;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.06;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 4 position
                    if (proteinSequence.Length > i + 4)  // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4]; // Updated 20200714
                        sub_sequence.Add(plus4);  // Updated 20200714
                        switch (proteinSequence[i + 4])
                        {
                            case 'A':
                            case 'E':
                            case 'Q':
                                score = score * 0.1;
                                break;
                            case 'R':
                            case 'H':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'I':
                                score = score * 0.06;
                                break;
                            case 'L':
                            case 'P':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'M':
                            case 'F':
                                score = score * 0.01;
                                break;
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'G':
                                score = score * 0.08;
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
                            case 'A':
                            case 'Q':
                                score = score * 0.07;
                                break;
                            case 'R':
                            case 'C':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'N':
                            case 'I':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'T':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'V':
                                score = score * 0.04;
                                break;
                            case 'G':
                            case 'P':
                                score = score * 0.08;
                                break;
                            case 'E':
                            case 'S':
                                score = score * 0.11;
                                break;
                            case 'H':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'L':
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

                    //% It saves the score for i + 6 position
                    if (proteinSequence.Length > i + 6)  // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // Updated 20200714
                        switch (proteinSequence[i + 6])
                        {
                            case 'A':
                                score = score * 0.09;
                                break;
                            case 'R':
                            case 'Q':
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'N':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'K':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'G':
                                score = score * 0.08;
                                break;
                            case 'I':
                            case 'H':
                            case 'F':
                            case 'Y':
                                score = score * 0.03;
                                break;
                            case 'L':
                            case 'S':
                                score = score * 0.1;
                                break;
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'P':
                            case 'E':
                                score = score * 0.07;
                                break;
                            default: //W=0
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 3);

                    if (score >= Tolerance)
                    {
                        //l = proteinSequence[i + 1];   // Updated 20200714
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   // Updated 20200714
                        //aa.Add(l);
                        //aa.Add(k);
                        //aa.Add(j);
                        //aa.Add(m);
                        //aa.Add(h);
                        //aa.Add(g);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));

                        objectArrayIndex++;

                        // score of Methionine at that position
                    }
                }

                // for the TotalMet if condition coming up ahead
                index = i;
            }

            // it displays total number of Methionine found in sequence
            //if (index == proteinSequence.Length)  // Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Methionine found: " + totalMet);
            //}
            //disp(['Total Methionine found: ', num2str(TotalMet)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_S): Returns an object array with all the required parameters stored
        public List<PostTranslationModificationsSiteDto> Acetylation_S(string proteinSequence, double Tolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // Updated 20200714

            var modName = "Acetylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'S';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Serine
            var totalSer = 0;

            //// stores the amino acids found   Updated 20200714
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalSer (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'S') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'S')  // Updated 20200714
                {
                    totalSer = totalSer + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // Updated 20200714

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]); // Updated 20200714

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // Updated 20200714
                    {
                        plus1 = proteinSequence[i + 1]; // Updated 20200714
                        sub_sequence.Add(plus1);  // Updated 20200714
                        switch (proteinSequence[i + 1])
                        {
                            case 'A':
                            case 'I':
                            case 'K':
                            case 'S':
                            case 'T':
                                score = 0.01;
                                break;
                            case 'R':
                            case 'C':
                            case 'G':
                            case 'H':
                            case 'P':
                            case 'W':
                            case 'Y':
                                score = 0;
                                break;
                            case 'N':
                                score = 0.09;
                                break;
                            case 'D':
                                score = 0.28;
                                break;
                            case 'E':
                                score = 0.39;
                                break;
                            case 'Q':
                            case 'V':
                                score = 0.03;
                                break;
                            case 'M':
                            case 'F':
                                score = 0.04;
                                break;
                            case 'L':
                                score = 0.12;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2)  // Updated 20200714
                    {
                        plus2 = proteinSequence[i + 2]; // Updated 20200714
                        sub_sequence.Add(plus2);  // Updated 20200714
                        switch (proteinSequence[i + 2])
                        {
                            case 'A':
                                score = score * 0.1;
                                break;
                            case 'R':
                            case 'I':
                                score = score * 0.02;
                                break;
                            case 'N':
                                score = score * 0.04;
                                break;
                            case 'D':
                                score = score * 0.06;
                                break;
                            case 'C':
                            case 'H':
                            case 'M':
                            case 'F':
                            case 'W':
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'G':
                                score = score * 0.09;
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.08;
                                break;
                            case 'Q':
                            case 'T':
                            case 'V':
                                score = score * 0.05;
                                break;
                            case 'K':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.14;
                                break;
                            case 'S':
                                score = score * 0.11;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 3 position
                    if (proteinSequence.Length > i + 3)  // Updated 20200714
                    {
                        plus3 = proteinSequence[i + 3]; // Updated 20200714
                        sub_sequence.Add(plus3);  // Updated 20200714
                        switch (proteinSequence[i + 3])
                        {
                            case 'A':
                            case 'E':
                            case 'Q':
                                score = score * 0.09;
                                break;
                            case 'N':
                            case 'G':
                            case 'S':
                                score = score * 0.08;
                                break;
                            case 'R':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'I':
                            case 'K':
                                score = score * 0.04;
                                break;
                            case 'L':
                            case 'T':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'M':
                            case 'W':
                                score = score * 0;
                                break;
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'P':
                                score = score * 0.06;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 4 position
                    if (proteinSequence.Length > i + 4)  // Updated 20200714
                    {
                        plus4 = proteinSequence[i + 4]; // Updated 20200714
                        sub_sequence.Add(plus4);  // Updated 20200714
                        switch (proteinSequence[i + 4])
                        {
                            case 'A':
                            case 'E':
                            case 'Q':
                                score = score * 0.1;
                                break;
                            case 'R':
                            case 'H':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'N':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'K':
                                score = score * 0.05;
                                break;
                            case 'C':
                            case 'I':
                                score = score * 0.06;
                                break;
                            case 'L':
                            case 'P':
                            case 'S':
                            case 'V':
                                score = score * 0.07;
                                break;
                            case 'M':
                            case 'F':
                                score = score * 0.01;
                                break;
                            case 'T':
                                score = score * 0.04;
                                break;
                            case 'W':
                                score = score * 0;
                                break;
                            case 'G':
                                score = score * 0.8;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 5 position
                    if (proteinSequence.Length > i + 5)  // Updated 20200714
                    {
                        plus5 = proteinSequence[i + 5]; // Updated 20200714
                        sub_sequence.Add(plus5);  // Updated 20200714
                        switch (proteinSequence[i + 5])
                        {
                            case 'C':
                            case 'W':
                            case 'M':
                                score = score * 0.01;
                                break;
                            case 'H':
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'R':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'N':
                            case 'I':
                            case 'K':
                            case 'P':
                                score = score * 0.04;
                                break;
                            case 'T':
                                score = score * 0.05;
                                break;
                            case 'S':
                                score = score * 0.06;
                                break;
                            case 'Q':
                                score = score * 0.07;
                                break;
                            case 'A':
                            case 'G':
                            case 'E':
                                score = score * 0.08;
                                break;
                            case 'D':
                            case 'L':
                            case 'V':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 6 position
                    if (proteinSequence.Length > i + 6)  // Updated 20200714
                    {
                        plus6 = proteinSequence[i + 6]; // Updated 20200714
                        sub_sequence.Add(plus6);  // Updated 20200714
                        switch (proteinSequence[i + 6])
                        {
                            case 'M':
                            case 'C':
                            case 'H':
                                score = score * 0.01;
                                break;
                            case 'Y':
                                score = score * 0.02;
                                break;
                            case 'N':
                            case 'Q':
                            case 'F':
                                score = score * 0.03;
                                break;
                            case 'D':
                            case 'R':
                                score = score * 0.04;
                                break;
                            case 'I':
                            case 'L':
                            case 'T':
                            case 'P':
                                score = score * 0.05;
                                break;
                            case 'E':
                            case 'V':
                                score = score * 0.06;
                                break;
                            case 'S':
                                score = score * 0.09;
                                break;
                            case 'G':
                                score = score * 0.12;
                                break;
                            case 'A':
                            case 'K':
                                score = score * 0.13;
                                break;
                            default: //% W = 0
                                score = 0;
                                break;
                        }
                    }

                    // score scaling according to higest score
                    score = _PtmScoreNormalizer.Normalize(score, 4);

                    if (score >= Tolerance)
                    {
                        //l = proteinSequence[i + 1];   // Updated 20200714
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   // Updated 20200714
                        //aa.Add(l);
                        //aa.Add(k);
                        //aa.Add(j);
                        //aa.Add(m);
                        //aa.Add(h);
                        //aa.Add(g);

                        // Storing the data in the list "array"
                        array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));


                        // score of Serine at that position
                    }
                }

                // for the TotalSer if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            //if (index == proteinSequence.Length)  // Updated 20200714 does not exist in spectrum
            //{
            //    Console.WriteLine("Total Serine found: " + totalSer);
            //}
            //disp(['Total Methionine found: ', num2str(TotalMet)])

            // returning the object array
            return array;
        }

    }
}
