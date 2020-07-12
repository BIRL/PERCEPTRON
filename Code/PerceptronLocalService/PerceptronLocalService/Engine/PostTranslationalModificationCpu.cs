/*
 * 
 *      #CFTTB
 *
 */


using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{


    public class PostTranslationalModificationModuleCpu //: IPostTranslationalModificationModule
    {
        private readonly IInsilicoFilter _insilicoFilter;

        public PostTranslationalModificationModuleCpu()
        {
            _insilicoFilter = new InsilicoFilterCpu();
        }
        /// No need for this!!!
        //private int _aaSize;

        //private void SetAaSize(int size)
        //{
        //    _aaSize = size;
        //}
        private char _siteDetect = '\0';

        private void SetsiteDetect(char letter)
        {
            _siteDetect = letter;
        }

        private IEnumerable<int[]> Combinations(int m, int n) // nCr = nCm
        {
            var result = new int[m];
            var stack = new Stack<int>();
            stack.Push(0);

            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();

                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return result;
                    break;
                }
            }
        }

        // Function: returns the total number of sites found in the given protein sequence
        private int GetSiteNumber(string proteinSequence)
        {
            var arraySize = 0;
            int i;
            for (i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == _siteDetect)
                    arraySize++;
            }
            return arraySize;
        }

        // Normalization
        private double Normalize(double value, int select)
        {
            double normalizedScore = 0;
            double max;
            double normFactor;

            switch (select)
            {
                //Acetylation_A
                case 1:
                    max = 0.24 * 0.17 * 0.17 * 0.14 * 0.15 * 0.12;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Acetylation_K
                case 2:
                    max = 0.11 * 0.12 * 0.11 * 0.09 * 0.11 * 0.11 * 0.09 * 0.14 * 0.11 * 0.14 * 0.12 * 0.11;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Acetylation_M
                case 3:
                    max = 0.39 * 0.14 * 0.09 * 0.10 * 0.12 * 0.10;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Acetylation_S
                case 4:
                    max = 0.14 * 0.13 * 0.15 * 0.17 * 0.10 * 0.13;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Amidation_F
                case 5:
                    max = 0.11 * 0.16 * 0.19 * 0.32 * 0.29 * 0.72;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Hydroxylation_P
                case 6:
                    max = 0.30 * 0.59 * 0.28 * 0.21 * 0.61 * 0.32 * 0.62 * 0.26 * 0.31 * 0.59 * 0.22 * 0.32;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Methylation_K
                case 7:
                    max = 0.14 * 0.15 * 0.18 * 0.14 * 0.22 * 0.24 * 0.19 * 0.16 * 0.15 * 0.12 * 0.15 * 0.17;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Methylation_R
                case 8:
                    max = 0.25 * 0.26 * 0.19 * 0.29 * 0.23 * 0.32 * 0.56 * 0.31 * 0.23 * 0.22 * 0.29 * 0.21;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // N_linked_glycosylation_N
                case 9:
                    max = 0.09 * 0.08 * 0.09 * 0.08 * 0.1 * 0.1 * 0.1 * 0.63 * 0.11 * 0.09 * 0.1 * 0.09;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // O_linked_glycosylation_T
                case 10:
                    max = 0.44 * 0.36 * 0.45 * 0.26 * 0.44 * 0.32 * 0.31 * 0.43 * 0.36 * 0.44 * 0.33 * 0.48;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // O_linked_glycosylation_S
                case 11:
                    max = 0.21 * 0.17 * 0.16 * 0.16 * 0.21 * 0.31 * 0.20 * 0.30 * 0.26 * 0.31 * 0.14 * 0.29;


                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Phosphorylation_S
                case 12:
                    max = 0.12 * 0.12 * 0.14 * 0.15 * 0.16 * 0.12 * 0.27 * 0.15 * 0.14 * 0.14 * 0.12 * 0.12;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Phosphorylation_T
                case 13:
                    max = 0.12 * 0.11 * 0.13 * 0.11 * 0.15 * 0.11 * 0.32 * 0.13 * 0.12 * 0.13 * 0.11 * 0.11;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Phosphorylation_Y
                case 14:
                    max = 0.09 * 0.09 * 0.10 * 0.11 * 0.09 * 0.09 * 0.11 * 0.09 * 0.10 * 0.09 * 0.09 * 0.08;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // Ubiquitination
                /*case 15:
                    min = 0;
                    max = 0.09 * 0.09 * 0.10 * 0.11 * 0.09 * 0.09 * 0.11 * 0.09 * 0.10 * 0.09 * 0.09 * 0.08;
                    norm_factor = 1 - Math.Exp(max);
                    normalized_score = (1 - Math.Exp(value)) / norm_factor;
                    break;*/
            }

            return normalizedScore;
        }

        // Function (Acetylation_A): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Acetylation_A(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    DoubA

            var modWeight = 42.0106;
            var modName = "Acetylation";
            var site = 'A';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Alanine
            var totalAla = 0;

            //// stores the amino acids found   DoubA
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
                if (proteinSequence[i] == 'A')  // DoubA
                {
                    totalAla = totalAla + 1;
                    // stores the amino acids found // DoubA
                    var sub_sequence = new List<char>();

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]);     // DoubA

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1) //Redundant // DoubA
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2) // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3) // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4) // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5) // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6) // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  //DoubA
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
                    score = Normalize(score, 1);

                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   DoubA
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   DoubA
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
            //if (index == proteinSequence.Length) DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Alanine found: " + totalAla);
            //}
            //disp(['Total Alanine found: ', num2str(TotalAla)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_K): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Acetylation_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);  DoubA

            var modWeight = 42.0106;
            var modName = "Acetylation";
            var site = 'K';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            //// stores the amino acids found   DoubA
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
                if (proteinSequence[i] == 'K')  // DoubA
                {
                    totalLys = totalLys + 1;
                    // stores the amino acids found // DoubA
                    var sub_sequence = new List<char>();

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // DoubA if (i - 6 > 0)
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'W':
                                score = 0.01; // DoubA
                                break;
                            case 'M':
                            case 'H':
                                score = 0.02; // DoubA
                                break;
                            case 'Y':
                                score = 0.03; // DoubA
                                break;
                            case 'N':
                            case 'I':
                            case 'F':
                            case 'Q':
                                score = 0.04; // DoubA
                                break;
                            case 'D':
                            case 'T':
                            case 'P':
                            case 'V':
                                score = 0.05; // DoubA
                                break;
                            case 'G':
                                score = 0.06; // DoubA
                                break;
                            case 'R':
                            case 'S':
                            case 'E':
                                score = 0.07; // DoubA
                                break;
                            case 'A':
                            case 'L':
                                score = 0.09; // DoubA
                                break;
                            case 'K':
                                score = 0.11; // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0) // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0) // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0) // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0) // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0) // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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

                    sub_sequence.Add(proteinSequence[i]);  // DoubA

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);  // DoubA
                        switch (proteinSequence[i + 1])
                        {
                            case 'W':
                                score = score * 0.01;  // DoubA
                                break;
                            case 'M':
                                score = score * 0.02;  // DoubA
                                break;
                            case 'Q':
                                score = score * 0.03;  // DoubA
                                break;
                            case 'N':
                            case 'H':
                            case 'I':
                            case 'T':
                                score = score * 0.04;  // DoubA
                                break;
                            case 'V':
                            case 'S':
                            case 'F':
                            case 'R':
                                score = score * 0.05;  // DoubA
                                break;
                            case 'G':
                            case 'D':
                            case 'P':
                                score = score * 0.06;  // DoubA
                                break;
                            case 'K':
                                score = score * 0.07;  // DoubA
                                break;
                            case 'A':
                            case 'E':
                            case 'Y':
                                score = score * 0.08;  // DoubA
                                break;
                            case 'L':
                                score = score * 0.09;  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                        sub_sequence.Add(plus3);  // DoubA
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
                        sub_sequence.Add(plus4);  // DoubA
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
                        sub_sequence.Add(plus5);  // DoubA
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
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 2);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // DoubA
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
            //if (index == proteinSequence.Length)  // DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_M): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Acetylation_M(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // DoubA

            var modWeight = 42.0106;
            var modName = "Acetylation";
            var site = 'M';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Methionine
            var totalMet = 0;

            //// stores the amino acids found   DoubA
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
                if (proteinSequence[i] == 'M')  // DoubA
                {
                    totalMet = totalMet + 1;
                    // stores the amino acids found   
                    var sub_sequence = new List<char>();  //DoubA

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]); // DoubA

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4]; // DoubA
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 3);

                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   // DoubA
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   // DoubA
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
            //if (index == proteinSequence.Length)  // DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Methionine found: " + totalMet);
            //}
            //disp(['Total Methionine found: ', num2str(TotalMet)])

            // returning the object array
            return array;
        }

        // Function (Acetylation_S): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Acetylation_S(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // DoubA

            var modWeight = 42.0106;
            var modName = "Acetylation";
            var site = 'S';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Serine
            var totalSer = 0;

            //// stores the amino acids found   DoubA
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
                if (proteinSequence[i] == 'S')  // DoubA
                {
                    totalSer = totalSer + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char plus1, plus2, plus3, plus4, plus5, plus6;
                    sub_sequence.Add(proteinSequence[i]); // DoubA

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1]; // DoubA
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2]; // DoubA
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3]; // DoubA
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4]; // DoubA
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5]; // DoubA
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6]; // DoubA
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 4);

                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   // DoubA
                        //k = proteinSequence[i + 2];
                        //j = proteinSequence[i + 3];
                        //m = proteinSequence[i + 4];
                        //h = proteinSequence[i + 5];
                        //g = proteinSequence[i + 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(proteinSequence[i]);   // DoubA
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
            //if (index == proteinSequence.Length)  // DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Serine found: " + totalSer);
            //}
            //disp(['Total Methionine found: ', num2str(TotalMet)])

            // returning the object array
            return array;
        }

        // Function (Amidation_F): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Amidation_F(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(7);    // DoubA

            var modWeight = -0.984016;
            var modName = "Amidation";
            var site = 'F';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Phenylalanine
            var totalPhe = 0;

            //// stores the amino acids found   DoubA
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
                if (proteinSequence[i] == 'F')  // DoubA
                {
                    totalPhe = totalPhe + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'L':
                            case 'T':
                            case 'W':
                                score = 0.01;  // DoubA
                                break;
                            case 'I':
                            case 'M':
                            case 'V':
                                score = 0.02;  // DoubA
                                break;
                            case 'H':
                                score = 0.03;  // DoubA
                                break;
                            case 'A':
                            case 'N':
                            case 'E':
                            case 'P':
                                score = 0.06;  // DoubA
                                break;
                            case 'Q':
                            case 'S':
                                score = 0.07;  // DoubA
                                break;
                            case 'K':
                                score = 0.08;  // DoubA
                                break;
                            case 'G':
                                score = 0.09;  // DoubA
                                break;
                            case 'Y':
                                score = 0.1;  // DoubA
                                break;
                            case 'R':
                            case 'D':
                                score = 0.11;  // DoubA
                                break;
                            case 'F':
                                score = 0;  // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
                        switch (proteinSequence[i - 1])
                        {
                            case 'A':
                            case 'I':
                            case 'L':
                            case 'K':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.01;  // DoubA
                                break;
                            case 'G':
                                score = score * 0.02;  // DoubA
                                break;
                            case 'H':
                                score = score * 0.03;  // DoubA
                                break;
                            case 'D':
                                score = score * 0.15;  // DoubA
                                break;
                            case 'R':
                                score = score * 0.72;  // DoubA
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
                                score = score * 0;  // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    sub_sequence.Add(proteinSequence[i]);

                    // score scaling according to higest score
                    score = Normalize(score, 5);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //f = proteinSequence[i - 1];   DoubA
                        //e = proteinSequence[i - 2];
                        //d = proteinSequence[i - 3];
                        //c = proteinSequence[i - 4];
                        //b = proteinSequence[i - 5];
                        //a = proteinSequence[i - 6];

                        //% it stores the protein sub-sequence
                        //aa.Add(a);    DoubA
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
            //if (index == proteinSequence.Length)  DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Phenylalanine found: " + totalPhe);
            //}
            //disp(['Total Phenylalanine found: ', num2str(TotalPhe)])

            // returning the object array
            return array;
        }

        // Function (Hydroxylation_P): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Hydroxylation_P(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);

            var modWeight = 15.9949;
            var modName = "Hydroxylation";
            var site = 'P';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Proline
            var totalPro = 0;

            //// stores the amino acids found   DoubA
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
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'N':
                            case 'E':
                            case 'I':
                            case 'M':
                            case 'Y':
                                score = 0.02;  // DoubA
                                break;
                            case 'D':
                            case 'G':
                            case 'L':
                            case 'V':
                                score = 0.03;  // DoubA
                                break;
                            case 'T':
                                score = 0.04;  // DoubA
                                break;
                            case 'S':
                                score = 0.05;  // DoubA
                                break;
                            case 'Q':
                                score = 0.06;  // DoubA
                                break;
                            case 'R':
                                score = 0.08;  // DoubA
                                break;
                            case 'K':
                                score = 0.12;  // DoubA
                                break;
                            case 'A':
                                score = 0.13;  // DoubA
                                break;
                            case 'P':
                                score = 0.3;  // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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

                    //% it will score amino acid at position i - 1
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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

                    sub_sequence.Add(proteinSequence[i]); // DoubA

                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
                        switch (proteinSequence[i + 1])
                        {
                            case 'R':
                            case 'C':
                            case 'K':
                            case 'Y':
                            case 'V':
                            case 'I':
                                score = score * 0.01;
                                break;
                            case 'A':
                                score = score * 0.03;
                                break;
                            case 'G':
                                score = score * 0.61;
                                break;
                            case 'P':
                                score = score * 0.13;
                                break;
                            case 'T':
                                score = score * 0.08;
                                break;
                            case 'S':
                                score = score * 0.1;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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

                    // score scaling according to higest score
                    score = Normalize(score, 6);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   DoubA
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
                        //aa.Add(a);    DoubA
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
            //if (index == proteinSequence.Length)  DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Proline found: " + totalPro);
            //}
            //disp(['Total Proline found: ', num2str(TotalPro)])

            // returning the object array
            return array;
        }

        // Function (Methylation_K): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Methylation_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13); DoubA

            var modWeight = 14.0156;
            var modName = "Methylation";
            var site = 'K';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            //// stores the amino acids found   DoubA
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
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
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
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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

                    sub_sequence.Add(proteinSequence[i]);   //DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 7);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // DoubA
                    {
                        //b = proteinSequence[i - 6];  // DoubA
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
                        //aa.Add(b);  // DoubA
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
            //if (index == proteinSequence.Length)  DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        // Function (Methylation_R): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Methylation_R(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(14);   DoubA

            var modWeight = 14.0156;
            var modName = "Methylation";
            var site = 'R';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of argenine
            var totalArg = 0;

            //// stores the amino acids found   DoubA
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
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
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
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);  // DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 8);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //b = proteinSequence[i - 6];   DoubA
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
                        //aa.Add(b);    DoubA
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
            //if (index == proteinSequence.Length)  // DoubA does not exist inspectrum
            //{
            //    Console.WriteLine("Total Argenine found: " + totalArg);
            //}
            //disp(['Total Argenine found: ', num2str(TotalArg)])

            // returning the object array
            return array;
        }

        // Function (N_linked_glycosylation_N): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> N_linked_glycosylation_N(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(14);

            var modWeight = 317.122;
            var modName = "N-linked Glycosylation";
            var site = 'N';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Asparagine
            var totalAsn = 0;

            //// stores the amino acids found   DoubA
            //var aa = new List<char>();

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalAsn (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'N') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'N')
                {
                    totalAsn = totalAsn + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
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
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);  // DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 9);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  //DoubA
                    {
                        //b = proteinSequence[i - 6];   DoubA
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
                        //aa.Add(b);    DoubA
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

                        // score of Asparagine at that position
                    }
                }

                // for the TotalAsn if condition coming up ahead
                index = i;
            }

            // it displays total number of Asparagine found in sequence
            //if (index == proteinSequence.Length)  does not exist in spectrum
            //{
            //    Console.WriteLine("Total Asparagine found: " + totalAsn);
            //}
            //disp(['Total Asparagine found: ', num2str(TotalAsn)])

            // returning the object array
            return array;
        }

        // Function (O_linked_glycosylation_T): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> O_linked_glycosylation_T(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);   DoubA

            var modWeight = 203.079;
            var modName = "O-Linked Glycosylation";
            var site = 'T';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Threonine
            var totalThr = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalThr (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'T') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'T')
                {
                    totalThr = totalThr + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6); // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'N':
                                score = 0.01;
                                break;
                            case 'D':
                            case 'Q':
                            case 'H':
                            case 'M':
                            case 'F':
                                score = 0.03;
                                break;
                            case 'R':
                            case 'I':
                            case 'L':
                            case 'V':
                                score = 0.04;
                                break;
                            case 'E':
                                score = 0.05;
                                break;
                            case 'G':
                                score = 0.07;
                                break;
                            case 'K':
                                score = 0.08;
                                break;
                            case 'A':
                                score = 0.09;
                                break;
                            case 'P':
                                score = 0.11;
                                break;
                            case 'S':
                                score = 0.16;
                                break;
                            case 'T':
                                score = 0.1;
                                break;
                            case 'W':
                            case 'Y':
                            case 'C':
                                score = 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0) // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5); // DoubA
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

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0) // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4); // DoubA
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

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0) // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3); // DoubA
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

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0) // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2); // DoubA
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

                    //% it will score amino acid at position i - 1
                    if (i - 1 >= 0) // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1); // DoubA
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
                    sub_sequence.Add(proteinSequence[i]); // DoubA
                    
                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1) // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1); // DoubA
                        switch (proteinSequence[i + 1])
                        {
                            case 'Y':
                                score = score * 0.01;
                                break;
                            case 'N':
                            case 'I':
                            case 'M':
                            case 'F':
                                score = score * 0.02;
                                break;
                            case 'D':
                                score = score * 0.03;
                                break;
                            case 'R':
                            case 'G':
                            case 'E':
                            case 'L':
                                score = score * 0.04;
                                break;
                            case 'K':
                                score = score * 0.06;
                                break;
                            case 'P':
                                score = score * 0.07;
                                break;
                            case 'Q':
                            case 'V':
                                score = 0.08;
                                break;
                            case 'A':
                                score = score * 0.1;
                                break;
                            case 'T':
                                score = score * 0.14;
                                break;
                            case 'S':
                                score = score * 0.17;
                                break;
                            case 'C':
                            case 'H':
                            case 'W':
                                score = score * 0;
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2) // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2); // DoubA
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
                    if (proteinSequence.Length > i + 3) // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3); // DoubA
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
                    if (proteinSequence.Length > i + 4) // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4); // DoubA
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
                    if (proteinSequence.Length > i + 5) // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5); // DoubA
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
                    if (proteinSequence.Length > i + 6) // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6); // DoubA
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

                    // score scaling according to higest score
                    score = Normalize(score, 10);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   // DoubA
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

                        ////% it stores the protein sub-sequence
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

                        // score of Threonine at that position
                    }
                }

                // for the TotalThr if condition coming up ahead
                index = i;
            }

            // it displays total number of Threonine found in sequence
            //if (index == proteinSequence.Length)  // DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Threonine found: " + totalThr);
            //}
            //disp(['Total Threonine found: ', num2str(TotalThr)])

            // returning the object array
            return array;
        }

        // Function (O_linked_glycosylation_S): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> O_linked_glycosylation_S(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            // SetAaSize(13);

            var modWeight = 203.079;
            var modName = "O-Linked Glycosylation";
            var site = 'S';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Serine
            var totalSer = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalSer (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'S') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'S')
                {
                    totalSer = totalSer + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);    // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'M':
                            case 'Y':
                                score = 0.01; // DoubA
                                break;
                            case 'D':
                            case 'C':
                            case 'Q':
                            case 'H':
                            case 'K':
                            case 'F':
                            case 'W':
                                score = 0.02; // DoubA
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                                score = 0.03; // DoubA
                                break;
                            case 'E':
                            case 'V':
                                score = 0.04; // DoubA
                                break;
                            case 'G':
                            case 'L':
                                score = 0.05; // DoubA
                                break;
                            case 'A':
                                score = 0.07; // DoubA
                                break;
                            case 'P':
                                score = 0.18; // DoubA
                                break;
                            case 'S':
                                score = 0.21; // DoubA
                                break;
                            case 'T':
                                score = 0.14; // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0) // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);    // DoubA
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

                    //% it will score amino acid at position i - 4
                    if (i - 4 >= 0) // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);   // DoubA
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

                    //% it will score amino acid at position i - 3
                    if (i - 3 >= 0) // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);    // DoubA
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

                    //% it will score amino acid at position i - 2
                    if (i - 2 >= 0) // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);    // DoubA
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

                    //% it will score amino acid at position i - 1
                    if (i - 1 > -0) // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);    // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);    // DoubA
                    
                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1) // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);    // DoubA
                        switch (proteinSequence[i + 1])
                        {
                            case 'C':
                            case 'H':
                            case 'Y':
                                score = score * 0.01; // DoubA
                                break;
                            case 'D':
                            case 'I':
                            case 'K':
                                score = score * 0.02; // DoubA
                                break;
                            case 'Q':
                            case 'V':
                                score = score * 0.03; // DoubA
                                break;
                            case 'R':
                                score = score * 0.04; // DoubA
                                break;
                            case 'G':
                                score = score * 0.06; // DoubA
                                break;
                            case 'L':
                                score = score * 0.07; // DoubA
                                break;
                            case 'S':
                                score = score * 0.09; // DoubA
                                break;
                            case 'P':
                                score = score * 0.1; // DoubA
                                break;
                            case 'E':
                                score = score * 0.16; // DoubA
                                break;
                            case 'A':
                                score = score * 0.14; // DoubA
                                break;
                            case 'T':
                                score = score * 0.2; // DoubA
                                break;
                            case 'N':
                            case 'M':
                            case 'F':
                            case 'W':
                                score = score * 0; // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2) // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);    // DoubA
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
                    if (proteinSequence.Length > i + 3) // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);    // DoubA
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
                    if (proteinSequence.Length > i + 4) // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);    // DoubA
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
                    if (proteinSequence.Length > i + 5) // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);    // DoubA
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
                    if (proteinSequence.Length > i + 6) // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);    // DoubA
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

                    // score scaling according to higest score
                    score = Normalize(score, 11);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0)) DoubA
                    if (score >= ptmTolerance)
                    {
                        //l = proteinSequence[i + 1];   DoubA
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
                        //aa.Add(a);    DoubA
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

                        // score of Serine at that position
                    }
                }

                // for the TotalSer if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            //if (index == proteinSequence.Length)  DoubA does not exist in spectrum
            //{
            //    Console.WriteLine("Total Serine found: " + totalSer);
            //}
            //disp(['Total Serine found: ', num2str(TotalSer)])

            // returning the object array
            return array;
        }

        // Function (Phosphorylation_S): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Phosphorylation_S(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13);    DoubA

            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'S';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Serine
            var totalSer = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalSer (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'S') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'S')
                {
                    totalSer = totalSer + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);  // DoubA
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
                    if (i - 5 >= 0)  // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);  // DoubA
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
                    if (i - 4 >= 0)  // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);  // DoubA
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
                    if (i - 3 >= 0)  // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);  // DoubA
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
                    if (i - 2 >= 0)  // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);  // DoubA
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
                    if (i - 1 >= 0)  // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);  // DoubA
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
                    sub_sequence.Add(proteinSequence[i]); // DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1)  // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);  // DoubA
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
                    if (proteinSequence.Length > i + 2)  // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);  // DoubA
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
                    if (proteinSequence.Length > i + 3)  // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);  // DoubA
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
                    if (proteinSequence.Length > i + 4)  // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);  // DoubA
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
                    if (proteinSequence.Length > i + 5)  // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);  // DoubA
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
                    if (proteinSequence.Length > i + 6)  // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);  // DoubA
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
                    score = Normalize(score, 12);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // DoubA
                    {
                        //b = proteinSequence[i - 6];   DoubA
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
                        //aa.Add(b);    DoubA
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

                        // score of Serine at that position
                    }
                }

                // for the TotalSer if condition coming up ahead
                index = i;
            }

            // it displays total number of Serine found in sequence
            //if (index == proteinSequence.Length)  DoubA
            //{
            //    Console.WriteLine("Total Serine found: " + totalSer);
            //}
            //disp(['Total Serine found: ', num2str(TotalSer)])

            // returning the object array
            return array;
        }

        // Function (Phosphorylation_T): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Phosphorylation_T(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13);    DoubA

            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'T';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Thrmine
            var totalThr = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalThr (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'T') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'T')
                {
                    totalThr = totalThr + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);   // DoubA
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
                    if (i - 5 >= 0) // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);   // DoubA
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
                    if (i - 4 >= 0) // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);   // DoubA
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
                    if (i - 3 >= 0) // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);   // DoubA
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
                    if (i - 2 >= 0) // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);   // DoubA
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
                    if (i - 1 >= 0) // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);   // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);   // DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1) // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);   // DoubA
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
                    if (proteinSequence.Length > i + 2) // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);   // DoubA
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
                    if (proteinSequence.Length > i + 3) // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);   // DoubA
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
                    if (proteinSequence.Length > i + 4) // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);   // DoubA
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
                    if (proteinSequence.Length > i + 5) // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);   // DoubA
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
                    if (proteinSequence.Length > i + 6) // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);   // DoubA
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
                    score = Normalize(score, 13);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // DoubA
                    {
                        //b = proteinSequence[i - 6];   DoubA
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
                        //aa.Add(b);    DoubA
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

                        // score of Thrmine at that position
                    }
                }

                // for the TotalThr if condition coming up ahead
                index = i;
            }

            // it displays total number of Thrmine found in sequence
            //if (index == proteinSequence.Length)  DoubA
            //{
            //    Console.WriteLine("Total Thrmine found: " + totalThr);
            //}
            //disp(['Total Thrmine found: ', num2str(TotalThr)])

            // returning the object array
            return array;
        }

        // Function (Phosphorylation_Y): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Phosphorylation_Y(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13);    DoubA

            var modWeight = 79.9663;
            var modName = "Phosphorylation";
            var site = 'Y';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Tyrosine
            var totalTyr = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // a higher scope variable declared to show the TotalTyr (if statement at the end)
            var index = 0;

            // an indexing variable to iterate the dynamic array "array"
            var objectArrayIndex = 0;

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                //if ((proteinSequence[i] == 'Y') && (i < proteinSequence.Length) && (i + 1 < proteinSequence.Length) &&
                //    (i + 2 < proteinSequence.Length) && (i + 3 < proteinSequence.Length) &&
                //    (i + 4 < proteinSequence.Length) && (i + 5 < proteinSequence.Length) &&
                //    (i + 6 < proteinSequence.Length))
                if (proteinSequence[i] == 'Y')  //DoubA
                {
                    totalTyr = totalTyr + 1;
                    // stores the amino acids found
                    var sub_sequence = new List<char>();  // DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0) // DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);   // DoubA
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
                    if (i - 5 >= 0) // DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);   // DoubA
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
                    if (i - 4 >= 0) // DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);   // DoubA
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
                    if (i - 3 >= 0) // DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);   // DoubA
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
                    if (i - 2 >= 0) // DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);   // DoubA
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
                    if (i - 1 >= 0) // DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);   // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);   // DoubA

                    //% it will score amino acid at position i + 1
                    if (proteinSequence.Length > i + 1) // DoubA
                    {
                        plus1 = proteinSequence[i + 1];
                        sub_sequence.Add(plus1);   // DoubA
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
                    if (proteinSequence.Length > i + 2) // DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);   // DoubA
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
                    if (proteinSequence.Length > i + 3) // DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);   // DoubA
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
                    if (proteinSequence.Length > i + 4) // DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);   // DoubA
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
                    if (proteinSequence.Length > i + 5) // DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);   // DoubA
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
                    if (proteinSequence.Length > i + 6) // DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);   // DoubA
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
                    score = Normalize(score, 14);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)
                    {
                        //b = proteinSequence[i - 6];   DoubA
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
                        //aa.Add(b);    DoubA
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

                        // score of Tyrosine at that position
                    }
                }

                // for the TotalTyr if condition coming up ahead
                index = i;
            }

            // it displays total number of Tyrosine found in sequence
            //if (index == proteinSequence.Length)  DoubA
            //{
            //    Console.WriteLine("Total Tyrosine found: " + totalTyr);
            //}
            //disp(['Total Tyrosine found: ', num2str(TotalTyr)])

            // returning the object array
            return array;
        }

        // Function (Ubiquitination_K): Returns an object array with all the required parameters stored
        private List<PostTranslationModificationsSiteDto> Ubiquitination_K(string proteinSequence, double ptmTolerance)
        {
            //Sequence of protein
            //proteinSequence = "MAPNASCLCVHVRSEEWDLMTFDANPYDSVKKIKEHVRSKTKVPVQDQVLLLGSKILKPRRSLSSYGIDKEKTIHLTLKVVKPSDEELPLFLVESGDEAKRHLLQVRRSSSVAQVKAMIETKTGIIPETQIVTCNGKRLEDGKMMADYGIRKGNLLFLACYCIGG";

            //SetAaSize(13);    DoubA

            double modWeight = 8561;
            var modName = "protein_sequenceation";
            var site = 'K';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalLys = 0;

            //// stores the amino acids found
            //var aa = new List<char>();    DoubA

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
                    var sub_sequence = new List<char>();  //DoubA

                    //variables to store sub - sequence
                    char minus6, minus5, minus4, minus3, minus2, minus1, plus1, plus2, plus3, plus4, plus5, plus6;

                    //% it will score amino acid at position i - 6
                    if (i - 6 >= 0)  //DoubA
                    {
                        minus6 = proteinSequence[i - 6];
                        sub_sequence.Add(minus6);   // DoubA
                        switch (proteinSequence[i - 6])
                        {
                            case 'C':
                            case 'W':
                                score = 0.01;   // DoubA
                                break;
                            case 'M':
                            case 'H':
                                score = 0.02;   // DoubA
                                break;
                            case 'Y':
                            case 'F':
                                score = 0.03;   // DoubA
                                break;
                            case 'N':
                            case 'P':
                                score = 0.04;   // DoubA
                                break;
                            case 'Q':
                            case 'I':
                            case 'T':
                                score = 0.05;   // DoubA
                                break;
                            case 'R':
                            case 'D':
                            case 'G':
                                score = 0.06;   // DoubA
                                break;
                            case 'K':
                            case 'S':
                            case 'V':
                                score = 0.07;   // DoubA
                                break;
                            case 'A':
                            case 'E':
                                score = 0.08;   // DoubA
                                break;
                            case 'L':
                                score = 0.1;   // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% it will score amino acid at position i - 5
                    if (i - 5 >= 0)  //DoubA
                    {
                        minus5 = proteinSequence[i - 5];
                        sub_sequence.Add(minus5);   // DoubA
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
                    if (i - 4 >= 0)  //DoubA
                    {
                        minus4 = proteinSequence[i - 4];
                        sub_sequence.Add(minus4);   // DoubA
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
                    if (i - 3 >= 0)  //DoubA
                    {
                        minus3 = proteinSequence[i - 3];
                        sub_sequence.Add(minus3);   // DoubA
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
                    if (i - 2 >= 0)  //DoubA
                    {
                        minus2 = proteinSequence[i - 2];
                        sub_sequence.Add(minus2);   // DoubA
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
                    if (i - 1 >= 0)  //DoubA
                    {
                        minus1 = proteinSequence[i - 1];
                        sub_sequence.Add(minus1);   // DoubA
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
                    sub_sequence.Add(proteinSequence[i]);   // DoubA
                    
                    //It saves the score for i + 1 position
                    if (proteinSequence.Length > i + 1)  //DoubA
                    {
                        plus1 = (proteinSequence[i + 1]);
                        sub_sequence.Add(plus1);   // DoubA
                        switch (proteinSequence[i + 1])
                        {
                            case 'C':
                            case 'W':
                                score = score * 0.01;   // DoubA
                                break;
                            case 'H':
                            case 'M':
                                score = score * 0.02;   // DoubA
                                break;
                            case 'K':
                            case 'F':
                            case 'Y':
                                score = score * 0.04;   // DoubA
                                break;
                            case 'R':
                            case 'N':
                            case 'I':
                            case 'P':
                                score = score * 0.05;   // DoubA
                                break;
                            case 'D':
                            case 'G':
                            case 'Q':
                            case 'S':
                            case 'T':
                            case 'V':
                                score = score * 0.06;   // DoubA
                                break;
                            case 'A':
                                score = score * 0.08;   // DoubA
                                break;
                            case 'E':
                            case 'L':
                                score = score * 0.1;   // DoubA
                                break;
                            default:
                                score = 0;
                                break;
                        }
                    }

                    //% It saves the score for i + 2 position
                    if (proteinSequence.Length > i + 2)  //DoubA
                    {
                        plus2 = proteinSequence[i + 2];
                        sub_sequence.Add(plus2);   // DoubA
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
                    if (proteinSequence.Length > i + 3)  //DoubA
                    {
                        plus3 = proteinSequence[i + 3];
                        sub_sequence.Add(plus3);   // DoubA
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
                    if (proteinSequence.Length > i + 4)  //DoubA
                    {
                        plus4 = proteinSequence[i + 4];
                        sub_sequence.Add(plus4);   // DoubA
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
                    if (proteinSequence.Length > i + 5)  //DoubA
                    {
                        plus5 = proteinSequence[i + 5];
                        sub_sequence.Add(plus5);   // DoubA
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
                    if (proteinSequence.Length > i + 6)  //DoubA
                    {
                        plus6 = proteinSequence[i + 6];
                        sub_sequence.Add(plus6);   // DoubA
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
                    score = Normalize(score, 15);

                    //if ((score >= ptmTolerance) && (i - 6 >= 0) && (i - 5 >= 0) && (i - 4 >= 0) && (i - 3 >= 0) &&
                    //    (i - 2 >= 0) && (i - 1 >= 0))
                    if (score >= ptmTolerance)  // DoubA
                    {
                        //l = proteinSequence[i + 1];   DoubA
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
                        //aa.Add(a);    DoubA
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
            //if (index == proteinSequence.Length)  DoubA
            //{
            //    Console.WriteLine("Total Lysine found: " + totalLys);
            //}
            //disp(['Total Lysine found: ', num2str(TotalLys)])

            // returning the object array
            return array;
        }

        private List<PostTranslationModificationsSiteDto> Cys_CAM(string proteinSequence, double ptmTolerance)
        {
            double modWeight = 160.03065;
            var modName = "Carboxyamidomethylation";
            var site = 'C';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalCys = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'C')
                {
                    totalCys = totalCys + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        private List<PostTranslationModificationsSiteDto> Cys_CM(string proteinSequence, double ptmTolerance)
        {
            double modWeight = 161.01466;
            var modName = "Carboxymethylation";
            var site = 'C';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalCys = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'C')
                {
                    totalCys = totalCys + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        private List<PostTranslationModificationsSiteDto> Cys_PAM(string proteinSequence, double ptmTolerance)
        {
            string modName = "Propionamidation";

            var ModificationTableClass = new ModificationMWShift();
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            
            var site = 'C';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalCys = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'C')
                {
                    totalCys = totalCys + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        private List<PostTranslationModificationsSiteDto> Cys_PE(string proteinSequence, double ptmTolerance)
        {
            double modWeight = 208.067039;
            var modName = "Pyridylethylation";
            var site = 'C';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalCys = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'C')
                {
                    totalCys = totalCys + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        private List<PostTranslationModificationsSiteDto> MSO(string proteinSequence, double ptmTolerance)
        {
            string modName = "Sulfoxide";

            var ModificationTableClass = new ModificationMWShift();
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);

            var site = 'M';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalMeth = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'M')
                {
                    totalMeth = totalMeth + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        private List<PostTranslationModificationsSiteDto> MSONE(string proteinSequence, double ptmTolerance)
        {
            double modWeight = 32.00;
            var modName = "Sulfone";
            var site = 'M';

            SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalMeth = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'M')
                {
                    totalMeth = totalMeth + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }

        //*******************************


        // calls functions of PTMs and calculates scores
        private void RunAlgosv(string protSequence, double tol, List<PostTranslationModificationsSiteDto> filtered, List<int> ptmCode)
        {
            // Runs only the PTMS that the user selected

            //List<Sites> filtered_sites = new List<Sites>();
            var dummy = new List<PostTranslationModificationsSiteDto>();

            foreach (var a in ptmCode)
            {
                switch (a)
                {
                    case 1:
                        dummy = Acetylation_A(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 2:
                        dummy = Acetylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 3:
                        dummy = Acetylation_M(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 4:
                        dummy = Acetylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 5:
                        dummy = Amidation_F(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 6:
                        dummy = Hydroxylation_P(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 7:
                        dummy = Methylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 8:
                        dummy = Methylation_R(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 9:
                        dummy = N_linked_glycosylation_N(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 10:
                        dummy = O_linked_glycosylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 11:
                        dummy = O_linked_glycosylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 12:
                        dummy = Phosphorylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 13:
                        dummy = Phosphorylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 14:
                        dummy = Phosphorylation_Y(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 15:
                        dummy = Ubiquitination_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    default:
                        // idle
                        break;
                }
            }
        }

        private void RunAlgosf(string protSequence, double tol, List<PostTranslationModificationsSiteDto> filtered, List<int> ptmCode)
        {
            //List<Sites> filtered_sites = new List<Sites>();
            var dummy = new List<PostTranslationModificationsSiteDto>();

            foreach (var a in ptmCode)
            {
                switch (a)
                {
                    case 1:
                        dummy = Acetylation_A(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 2:
                        dummy = Acetylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 3:
                        dummy = Acetylation_M(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 4:
                        dummy = Acetylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 5:
                        dummy = Amidation_F(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 6:
                        dummy = Hydroxylation_P(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 7:
                        dummy = Methylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 8:
                        dummy = Methylation_R(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 9:
                        dummy = N_linked_glycosylation_N(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 10:
                        dummy = O_linked_glycosylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 11:
                        dummy = O_linked_glycosylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 12:
                        dummy = Phosphorylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 13:
                        dummy = Phosphorylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 14:
                        dummy = Phosphorylation_Y(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 15:
                        dummy = Ubiquitination_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    default:
                        // idle
                        break;
                }
            }
        }

        // FUNCTION FOR MAKING MODIFIED PROTEINS
        private void make_mod_proteins_2(List<PostTranslationModificationsSiteDto> filteredSites, List<ProteinDto> modProteins, List<int> combos,
            ProteinDto parent, List<ProteinDto> shortProt, string clevageType, string ions, List<double> peakList,
            double tol, double mwWeight, double pstWeight, double insilicoWeight)
        {
            var dummyMw = parent.Mw; // MW of the original sequence
            double dummyPtmScore = 0; // ptm score of the unmodified sequence
            //var aStringBuilder = new StringBuilder(sequence);

            /*foreach(int a in combos)
            {
                Console.WriteLine(a);
            }*/

            var sitesInfo = new List<PostTranslationModificationsSiteDto>();

            foreach (var i in combos)
            {
                if (i != 777)
                {
                    dummyMw += filteredSites.ElementAt(i).ModWeight;
                    // accumulates the mod weight of all the sites in the current combination
                    dummyPtmScore += filteredSites.ElementAt(i).Score; //accumlates the ptm score
                    sitesInfo.Add(filteredSites.ElementAt(i));

                    //aStringBuilder.Remove(filtered_sites.ElementAt(i).i, 1);
                    //aStringBuilder.Insert(filtered_sites.ElementAt(i).i, (filtered_sites.ElementAt(i).site + "mod"));
                    //obj.sequence = aStringBuilder.ToString();
                }
                else if (i == 777)
                {
                    var temp = new ProteinDto();

                    //mod_proteins.Add(temp);     // adding the protein

                    var min = 0;
                    double val = 0;
                    ;

                    if (shortProt.Count == 10000)
                    {
                        min = 0;
                        val = shortProt.ElementAt(0).Score;
                        for (var kkk = 1; kkk < shortProt.Count; kkk++)
                        {
                            if (val > shortProt.ElementAt(kkk).Score)
                            {
                                min = kkk;
                                val = shortProt.ElementAt(kkk).Score;
                            }
                        }

                        shortProt.RemoveAt(min);
                    }


                    shortProt.Add(temp);// Remove (populate temp first)

                    // using count - 1 because the list counts a single object as 1 and not as zeroth element
                    // in case of fixed modifications, the first element is 777. so the unmodified protein is added with the default weight and score
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).MW = dummyMW;
                    shortProt.ElementAt(shortProt.Count - 1).Mw = dummyMw;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).ptm_score = dummy_ptm_score;
                    shortProt.ElementAt(shortProt.Count - 1).PtmScore = dummyPtmScore;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).sequence = parent.sequence;
                    shortProt.ElementAt(shortProt.Count - 1).Sequence = parent.Sequence;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).header = parent.header;
                    shortProt.ElementAt(shortProt.Count - 1).Header = parent.Header;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).est_score = parent.est_score;
                    shortProt.ElementAt(shortProt.Count - 1).PstScore = parent.PstScore;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).insilico_score = parent.insilico_score;
                    shortProt.ElementAt(shortProt.Count - 1).InsilicoScore = parent.InsilicoScore;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).MW_score = parent.MW_score;
                    shortProt.ElementAt(shortProt.Count - 1).MwScore = parent.MwScore;
                    ////mod_proteins.ElementAt(mod_proteins.Count - 1).ptm_particulars = parent.ptm_particulars;
                    ////mod_proteins.ElementAt(mod_proteins.Count - 1).ptm_particulars.Add(filtered_sites.ElementAt(i));
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).score = parent.score;
                    shortProt.ElementAt(shortProt.Count - 1).Score = parent.Score;
                    //mod_proteins.ElementAt(mod_proteins.Count - 1).insilico_details = parent.insilico_details;
                    shortProt.ElementAt(shortProt.Count - 1).InsilicoDetails = parent.InsilicoDetails;

                    foreach (var a in sitesInfo) //Remove (assign list by using =)
                    {
                        //mod_proteins.ElementAt(mod_proteins.Count - 1).ptm_particulars.Add(a);
                        shortProt.ElementAt(shortProt.Count - 1).PtmParticulars.Add(a);
                    }

                    InsilicoFragmentationPtmCpu.insilico_fragmentation(shortProt.ElementAt(shortProt.Count - 1), clevageType,
                        ions);
                    var temp1 = new List<ProteinDto>();
                    temp1.Add(shortProt.ElementAt(shortProt.Count - 1));
                    string pepUnit = "Its Just Commented";
                    //_insilicoFilter.ComputeInsilicoScore(temp1, peakList, tol, pepUnit);
                    //_insilicoFilter.ComputeInsilicoScore(temp1, peakList, tol); // Commented
                    shortProt.ElementAt(shortProt.Count - 1).set_score(mwWeight, pstWeight, insilicoWeight);

                    dummyMw = parent.Mw;
                    dummyPtmScore = 0;
                    sitesInfo = new List<PostTranslationModificationsSiteDto>();
                    //aStringBuilder = new StringBuilder(sequence);
                }
            }
        }

        private double variable_mods_2(List<double> curve, string proteinSequence, List<PostTranslationModificationsSiteDto> filteredSitesA,
            double tol, List<int> indices, List<ProteinDto> modProt, List<int> ptmCodeV, ProteinDto parentPro,
            List<ProteinDto> shortlisted, string clevageType, string ions, List<double> peakList, double insilicoTol,
            double mwWeight, double pstWeight, double insilicoWeight)
        {
            //Console.WriteLine("working");
            RunAlgosv(proteinSequence, tol, filteredSitesA, ptmCodeV); // filtered sites is currently empty

            var sortedList = filteredSitesA.OrderBy(o => o.Score).ToList();
            //sorting in ascending order of scores of sites

            int j;
            var combos = new List<string>();
            //List<int> indices = new List<int>();

            var limitCombs = 0;

            //for (j = 1; j <= SortedList.Count; j++)
            for (j = 1; j <= 5; j++) //Remove (Hard coded value) (Does not return any combination greater than 5)
            {
                foreach (var c in Combinations(j, sortedList.Count))
                {
                    var dummy = "";
                    //Console.WriteLine(int.MaxValue);
                    int i;
                    for (i = 0; i < c.Length; i++)
                    {
                        dummy += c[i].ToString();
                        indices.Add(c[i]); // separates indices
                    }
                    combos.Add(dummy);
                    indices.Add(777);
                    //Console.WriteLine();
                }
                //Console.WriteLine("CHECK");
            }
            //Console.WriteLine("CHECK");

            shortlisted.Add(parentPro);

            InsilicoFragmentationPtmCpu.insilico_fragmentation(shortlisted.ElementAt(shortlisted.Count - 1), clevageType, ions);
            var temp = new List<ProteinDto>();
            temp.Add(shortlisted.ElementAt(shortlisted.Count - 1));
            string pepUnit = "Its Just Commented";//Commented
            //_insilicoFilter.ComputeInsilicoScore(temp, peakList, insilicoTol, pepUnit);
            //_insilicoFilter.ComputeInsilicoScore(temp, peakList, insilicoTol); //Commented
            shortlisted.ElementAt(shortlisted.Count - 1).set_score(mwWeight, pstWeight, insilicoWeight);

            if (sortedList.Count > 0)
                make_mod_proteins_2(filteredSitesA, modProt, indices, parentPro, shortlisted, clevageType, ions,
                    peakList, insilicoTol, mwWeight, pstWeight, insilicoWeight);

            //mod_prot.ElementAt(0).ptm_particulars = new List<Sites>();
            var counter = 1;
            var con = false;
            var dummyList = new List<PostTranslationModificationsSiteDto>();

            ////////////////////////////////////////////////////////////////////////////////////////////

            //for (int x = 0; x < indices.Count; x++)
            //{
            //    if (indices.ElementAt(x) != 777)
            //    {
            //        con = false;
            //        dummyList.Add(filtered_sitesA.ElementAt(indices.ElementAt(x)));
            //    }
            //    else if (indices.ElementAt(x) == 777)
            //    {
            //        if (con == false)
            //        {
            //            mod_prot.ElementAt(counter).ptm_particulars = new List<Sites>();
            //            con = true;
            //        }
            //        mod_prot.ElementAt(counter).ptm_particulars.AddRange(dummyList);
            //        counter++;
            //        dummyList.Clear();
            //    }
            //    //mod_prot.ElementAt(x+1).ptm_particulars
            //}

            ////////////////////////////////////////////////////////////////////////////////////////////

            double totalScore = 0;

            var cou = 0;
            double finalScore = 0;

            foreach (var a in modProt)
            {
                if (a.PtmScore != 0)
                {
                    a.PtmScore = 1 - Math.Exp(-a.PtmScore);
                }
            }

            return finalScore;
        }

        private double fixed_mods_2(List<double> curve, string proteinSequence, List<PostTranslationModificationsSiteDto> filteredSitesB,
            double tol, List<int> indices, List<ProteinDto> modProt, List<int> ptmCodeF, ProteinDto parentPro,
            List<ProteinDto> shortProt, string clevageType, string ions, List<double> peakList, double insilicoTol,
            double mwWeight, double pstWeight, double insilicoWeight)
        {
            //Console.WriteLine("working");
            //int j;

            RunAlgosf(proteinSequence, 0, filteredSitesB, ptmCodeF);
            // runs the modifications selected by the user and stores the sites in filtered sites

            var sortedList = filteredSitesB.OrderBy(o => o.Score).ToList();
            // sorts the filtered sites in ascending order of their ptm scores

            //indices.Add(777);
            for (var i = 0; i < sortedList.Count; i++)
            {
                indices.Add(i);
            }
            indices.Add(777);

            if (sortedList.Count > 0)
                make_mod_proteins_2(filteredSitesB, modProt, indices, parentPro, shortProt, clevageType, ions,
                    peakList, insilicoTol, mwWeight, pstWeight, insilicoWeight);

            //short_prot.ElementAt(short_prot.Count - 1).ptm_particulars = new List<Sites>();
            //foreach (Sites a in filtered_sitesB)
            //{
            //    mod_prot.ElementAt(mod_prot.Count - 1).ptm_particulars.Add(a);
            //}


            double totalScore = 0;

            var cou = 1;
            foreach (var a in modProt)
            {
                if ((a.PtmScore != 0) && (cou == modProt.Count))
                {
                    a.PtmScore = 1 - Math.Exp(-a.PtmScore);
                }
                cou++;
            }

            return totalScore;
        }


        private void FinalFilter(List<ProteinDto> filterProts)
        {
            //filterProts.Sort((x, y) => y.score.CompareTo(x.score));
            filterProts.AsParallel().OrderBy(x => x.Score);
            filterProts.RemoveRange(10000, filterProts.Count - 10000);
        }

        public void ExecutePtmModule(List<ProteinDto> input, List<ProteinDto> modifiedProteins, List<ProteinDto> shortlistedProt, MsPeaksDto peakData, SearchParametersDto parameters)
        {
            var tolerance = parameters.PtmTolerance;
            var ptmCodeVar = parameters.PtmCodeVar;
            var ptmCodeFix = parameters.PtmCodeFix;
            var clevageType = parameters.InsilicoFragType;
            var ions = parameters.HandleIons;
            var peakList = peakData.Mass;
            var insilicoTol = parameters.HopThreshhold;
            var mwWeight = parameters.MwSweight;
            var pstWeight = parameters.PstSweight;
            var insilicoWeight = parameters.InsilicoSweight;

            var filteredSitesA = new List<PostTranslationModificationsSiteDto>(); // for the variable modifications
            var filteredSitesB = new List<PostTranslationModificationsSiteDto>(); // for the fixed modifications
            var filteredSitesC = new List<PostTranslationModificationsSiteDto>(); // for both kind of modifications
            var curveV = new List<double>(); // for the variable modifications
            var curveF = new List<double>(); // for the fixed modifications
            var curveVarFix = new List<double>(); // for both kind of modifications
            var indicesV = new List<int>(); // and so on
            var indicesF = new List<int>();
            var indicesVarFix = new List<int>();
            double totalScoreVar = 0;
            double totalScoreFix = 0;
            double totalScoreVarFix = 0;

            //string typeCode = "";

            if (ptmCodeVar.Count != 0)
            {
                foreach (var p in input)
                {
                    filteredSitesA.Clear(); // resetting all variables before calling another protein
                    filteredSitesB.Clear();
                    filteredSitesC.Clear();
                    curveV.Clear();
                    curveF.Clear();
                    curveVarFix.Clear();
                    indicesV.Clear();
                    indicesF.Clear();
                    indicesVarFix.Clear();
                    totalScoreVar = 0;
                    totalScoreFix = 0;
                    totalScoreVarFix = 0;
                    totalScoreVar = variable_mods_2(curveV, p.Sequence, filteredSitesA, tolerance, indicesV,
                        modifiedProteins, ptmCodeVar, p, shortlistedProt, clevageType, ions, peakList, insilicoTol,
                        mwWeight, pstWeight, insilicoWeight);
                    p.PtmScore = totalScoreVar;

                    if (shortlistedProt.Count >= 50000)
                    {
                        FinalFilter(shortlistedProt);
                    }
                }
            }

            if (ptmCodeFix.Count != 0)
            {
                foreach (var p in input)
                {
                    //filtered_sitesA.Clear();        // resetting all variables before calling another protein
                    filteredSitesB.Clear();
                    filteredSitesC.Clear();
                    curveV.Clear();
                    curveF.Clear();

                    //indicesV.Clear();
                    indicesF.Clear();

                    totalScoreVar = 0;
                    totalScoreFix = 0;

                    totalScoreFix = fixed_mods_2(curveF, p.Sequence, filteredSitesB, tolerance, indicesF,
                        modifiedProteins, ptmCodeFix, p, shortlistedProt, clevageType, ions, peakList, insilicoTol,
                        mwWeight, pstWeight, insilicoWeight);
                    p.PtmScore
                        = totalScoreFix;

                    var counter = shortlistedProt.Count;
                    var fixProtIndex = shortlistedProt.Count - 1;
                    make_mod_proteins_2(filteredSitesA, modifiedProteins, indicesV,
                        shortlistedProt.ElementAt(shortlistedProt.Count - 1), shortlistedProt, clevageType, ions,
                        peakList, insilicoTol, mwWeight, pstWeight, insilicoWeight);

                    //modified_proteins.ElementAt(0).ptm_particulars = new List<Sites>();
                    //int counter = modified_proteins.Count - 1;
                    /*bool con = false;
                    List<Sites> dummyList = new List<Sites>();
                    foreach (Sites abc in modified_proteins.ElementAt(fix_prot_index).ptm_par
                        {
                            con = false;

                            dummyList.Add(filtered_sitesA.ElementAt(indicesV.ElementAt(x)));
                        }
                        else if (indicesV.ElementAt(x) == 777)
                        {
                            if (con == false)
                            {
                                modified_proteins.ElementAt(counter).ptm_particulars = new List<Sites>();
                                con = true;
                            }

                            foreach (Sites site in dummyList)
                            {
                                modified_proteins.ElementAt(counter).ptm_particulars.Add(site);
                            }

                            //modified_proteins.ElementAt(counter).ptm_particulars.AddRange(dummyList);
                            counter++;
                            dummyList.Clear();
                            foreach (Sites abc in modified_proteins.ElementAt(fix_prot_index).ptm_particulars)
                                dummyList.Add(abc);
                        }
                    }*/
                }
            }
        }

        ///////////////////////////////////////// 
                     //DoubA//
        public void BlindPTMExtraction(MsPeaksDto peakData, SearchParametersDto parameters)
        {
            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            var peakDatalist = new List<peakData2Dlist>();
            for (int row = 0; row <= peakData.Mass.Count - 1; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);
            }
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakDatalist.OrderBy(n => n.Mass).ToList();
            var MolW = peakData.Mass[peakData.Mass.Count-1];    // Molar weight that is the last row of the peak list
            var UserHopThreshold = 1;

            // peaks has first index as 0, followed by all the peaklist except MolW, followed by MolW - all the peaks from peaklist and then at the last index, there is MolW
            var peaks = new List<double>();
            peaks.Add(0);
            for (int row = 0; row <= peakData.Mass.Count - 2; row++)
            {
                peaks.Add(peakData.Mass[row]);
            }
            for (int row = 0; row <= peakData.Mass.Count - 2; row++)
            {
                peaks.Add(MolW - peakData.Mass[row]);
            }
            peaks.Add(MolW);
            peaks = peaks.OrderBy(n => n).ToList(); // Sorting of peaks

            // InfoTable data
            double[] InfoModMass = { 70.0055, 99.0321, 111.0321, 113.0477, 113.0477, 117.0248, 129.0426, 129.0426, 131.9994, 139.0634, 142.0742, 142.1106, 143.0583, 144.0899, 147.0354, 156.1263, 159.0354, 160.0848, 163.0303, 166.9983, 170.1056, 170.1167, 173.0147, 173.0324, 173.0511, 181.0140, 184.1324, 194.9932, 198.1117, 208.0484, 217.0252, 243.0296, 290.1114, 304.1271, 341.2392, 408.0772, 431.1649 };
            char[] InfoModAminoAcids = { 'S','G','Q','A','P','C','S','P','C','P','G','K','T','K','M','K','M','K','M','S','K','R','C','E','M','T','R','D','R','Y','H','Y','S','T','C','C','N' };
            string[] InfoModName = { "Pyruvate-S", "Acetylation", "Pyrrolidone-Aarboxylic-Acid", "Acetylation", "Hydroxylation", "Methylation", "Acetylation", "DiHydroxylation", "S-Nitrosylation", "Acetylation", "Methylation", "Methylation", "Acetylation", "Hydroxylation", "Sulfoxide", "DiMethylation", "Formylation", "DiHydroxylation", "Sulfone", "Phosphorylation", "Acetylation", "Methylation", "Pyruvate-C", "Gamma-Carboxyglutamic-Acid", "Acetylation", "Phosphorylation", "DiMethylation", "Phosphorylation", "Acetylation", "Nitration", "Phosphorylation", "Phosphorylation", "O-linked-Glycosylation", "O-linked-Glycosylation", "Palmitoylation", "Glutathionylation", "N-linked-Glycosylation" };

            // Initialization
            var HopInfoName = new List<string>();
            var HopInfoAA = new List<char>();
            var HopInfoEnd = new List<double>();
            var HopInfoStart = new List<double>();
            var LadderIndex = 0;

            // Extraction of Modified and Unmodified tags
            for (int ExpI = 1; ExpI <= peaks.Count - 2; ExpI++)
            {
                for (int ExpJ = ExpI+1; ExpJ <= peaks.Count - 1; ExpJ++)
                {
                    double PeakDiff = peaks[ExpI] - peaks[ExpJ];
                    for (int AAIndex = 0; AAIndex <= InfoModMass.Length - 1; AAIndex++)
                    {
                        double Error = PeakDiff - InfoModMass[AAIndex];
                        double Abs_Error = Math.Abs(Error);
                        if (Abs_Error <= UserHopThreshold)
                        {
                            LadderIndex = LadderIndex + 1;
                            HopInfoName.Add(InfoModName[AAIndex]);
                            HopInfoAA.Add(InfoModAminoAcids[AAIndex]);
                            HopInfoStart.Add(peaks[ExpI]); 
                            HopInfoEnd.Add(peaks[ExpJ]);
                        }
                        else if (Error < -UserHopThreshold)
                        {
                            break;
                        }
                    }
                }
            }
            var sizeHopInfo = HopInfoAA.Count;
            // Function for BlindPTM should be called here
        }

        public void BlindPTMGeneral(List<ProteinDto> CandidateProtList, MsPeaksDto peakData, double UserHopThreshold, double sizeHopInfo, List<string> HopInfoName, List<char> HopInfoAA, List<double> HopInfoEnd, List<double> HopInfoStart, SearchParametersDto parameters, string TypeOfFunction)
        //A general function for BlindPTM, BlindPTM_Truncation_Left and BlindPTM_Truncation_Right
        {
            //Variable initialization
            var PeptideTolerance = parameters.PeptideTolerance;
            var PeptideToleranceUnit = parameters.PeptideToleranceUnit;
            List<ProteinDto> CandidateProtListModified = new List<ProteinDto>();
            var proteinIndex = 0;

            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            var peakDatalist = new List<peakData2Dlist>();
            for (int row = 0; row < peakData.Mass.Count; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);
            }
            //Sorting the peakDatalist with respect to the Mass in ascending order
            var ExperimentalSpectrum = peakDatalist.OrderBy(n => n.Mass).ToList();
            var MolW = peakData.Mass[peakData.Mass.Count - 1];    // Molar weight that is the last row of the peak list
            double tolConv = 0;

            // if size of peakData is 1, then tolConv is equal to that one mass value, else it is the second-last mass value from the sorted peakData list
            if (peakData.Mass.Count == 1)
            {
                tolConv = peakData.Mass[peakData.Mass.Count - 1]; 
            }
            else
            {
                tolConv = peakData.Mass[peakData.Mass.Count - 2];
            }
            List<PTMDataDto> ShortlistedHops = new List<PTMDataDto>();   
            if (sizeHopInfo > 0)
            {
                for (int row = 0; row < CandidateProtList.Count; row++)
                {
                    var protein = CandidateProtList.ElementAt(row); //Protein at index row is being processed
                    var sequence = protein.Sequence;
                    //If the function is BlindPTM_Truncation_Left, the protein sequence is flipped, otherwise the original sequence is processed
                    if (TypeOfFunction == "BlindPTM_Truncation_Left")
                    {
                        char[] array = sequence.ToCharArray();
                        Array.Reverse(array);
                        sequence = new String(array);
                    }
                    //Variable initialization
                    var HopI = 0;
                    var ThrI = 0;
                    var loop = 1;
                    var Ladder_Index = -1;
                    
                    while(loop == 1)
                    {
                        var Start = HopInfoStart[HopI];
                        if (Ladder_Index != -1 && ShortlistedHops.ElementAt(Ladder_Index).End > Start) //Ladder_Index should be accessible i.e. it should be from 0  or greater and to check if the size of ShortlistedHop is non-zero, we check that end should be greater than start
                        {
                            HopI = HopI + 1;
                            if (HopI >= sizeHopInfo - 1) //if Hop index exceeds the size of Hop, break the loop
                                break;
                            else
                                continue;
                        }
                        var diff = 0.0;
                        if (TypeOfFunction == "BlindPTM_Truncation_Left")
                        {
                            diff = Start - protein.InsilicoDetails.InsilicoMassRight[ThrI];
                        }                    
                        else
                        {
                            diff = Start - protein.InsilicoDetails.InsilicoMassLeft[ThrI];
                        }                            
                        if (diff <= UserHopThreshold && diff >= -UserHopThreshold)
                        {
                            string AA = HopInfoAA[HopI].ToString();
                            if (AA[0] == sequence[ThrI+1])
                            {
                                var Mod = HopInfoName[HopI];
                                var ModWeight = ModTable(Mod); //Function ModTable is defined below BlindPTMGeneral
                                var End = HopInfoEnd[HopI];
                                if (TypeOfFunction == "BlindPTM_Truncation_Left")
                                    diff = Math.Abs(End - (protein.InsilicoDetails.InsilicoMassRight[ThrI + 1] + ModWeight));
                                else
                                    diff = Math.Abs(End - (protein.InsilicoDetails.InsilicoMassLeft[ThrI + 1] + ModWeight));
                                if (PeptideToleranceUnit == "ppm")
                                    diff = (diff/tolConv)*1000000;
                                else if (PeptideToleranceUnit == "%")
                                    diff = (diff/tolConv)*100;
                                if (diff < PeptideTolerance)
                                {
                                    //InsilicoMassLeft and InsilicoMassRight are being updated by the addition of the mass of modification
                                    if (TypeOfFunction == "BlindPTM_Truncation_Left")
                                    {
                                        for (int i = ThrI + 1; i < protein.InsilicoDetails.InsilicoMassRight.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassRight[i] = protein.InsilicoDetails.InsilicoMassRight[i] + ModWeight;

                                        for (int i = protein.Sequence.Length - ThrI - 2; i < protein.InsilicoDetails.InsilicoMassLeft.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] = protein.InsilicoDetails.InsilicoMassLeft[i] + ModWeight;
                                    }
                                    else
                                    {
                                        for (int i = ThrI + 1; i < protein.InsilicoDetails.InsilicoMassLeft.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassLeft[i] = protein.InsilicoDetails.InsilicoMassLeft[i] + ModWeight;

                                        for (int i = protein.Sequence.Length - ThrI - 2; i < protein.InsilicoDetails.InsilicoMassRight.Count; i++)
                                            protein.InsilicoDetails.InsilicoMassRight[i] = protein.InsilicoDetails.InsilicoMassRight[i] + ModWeight;
                                    }

                                    protein.Mw = protein.Mw + ModWeight;
                                    Ladder_Index = Ladder_Index + 1;
                                    
                                    //Protein components are being updated 
                                    ShortlistedHops.ElementAt(Ladder_Index).ModName = Mod;
                                    ShortlistedHops.ElementAt(Ladder_Index).AminoAcidName = (AA);
                                    ShortlistedHops.ElementAt(Ladder_Index).Start = Start;
                                    ShortlistedHops.ElementAt(Ladder_Index).End = End;
                                    ShortlistedHops.ElementAt(Ladder_Index).ThrI = ThrI;
                                }                         
                            }
                        }
                        else if (diff > UserHopThreshold)
                        {
                            ThrI = ThrI + 1;
                            if (TypeOfFunction == "BlindPTM_Truncation_Left")
                            {
                                if (ThrI >= protein.InsilicoDetails.InsilicoMassRight.Count - 1) //If index exceeds the length of insilico right list, break the loop
                                    break;
                                else
                                    continue;
                            }
                            else
                            {
                                if (ThrI >= protein.InsilicoDetails.InsilicoMassLeft.Count - 1)
                                    break;
                                else
                                    continue;
                            }   
                        }
                        else if (diff < -UserHopThreshold)
                        {
                            HopI = HopI + 1;
                            if (HopI >= sizeHopInfo - 1) //if Hop index exceeds the size of Hop, break the loop
                                break;
                            else
                                continue;
                        }
                        HopI = HopI + 1;
                        if (HopI >= sizeHopInfo - 1)
                            break;
                    }

                    for (int HopIndex = 0; HopIndex < ShortlistedHops.Count;  HopIndex++)
                    {
                        // Updating the protein
                        protein.PtmParticulars[HopIndex].ModName = ShortlistedHops.ElementAt(HopIndex).ModName;
                        protein.PtmParticulars[HopIndex].Site = Convert.ToChar(ShortlistedHops.ElementAt(HopIndex).AminoAcidName);
                        if (TypeOfFunction == "BlindPTM_Truncation_Left")
                            protein.PtmParticulars[HopIndex].Index = sequence.Length - ShortlistedHops.ElementAt(HopIndex).ThrI;
                        else
                            protein.PtmParticulars[HopIndex].Index = ShortlistedHops.ElementAt(HopIndex).ThrI + 1;
                    }
                    // Protein molar weight scoring
                    if (TypeOfFunction == "BlindPTM")
                    {
                        var error = Math.Abs(MolW - protein.Mw);
                        if (error == 0)
                            protein.MwScore = 1;
                        else
                            protein.MwScore = Math.Pow((1 / 2), error);
                    }
                    // All the updated proteins are being stored in a new list CandidateProtListModified
                    if (ShortlistedHops.Count > 0)
                    {
                        proteinIndex = proteinIndex + 1;
                        CandidateProtListModified[proteinIndex] = protein;
                    }
                }
            }
        }
        public double ModTable(string Mod) //Gives the weight of different types of modifications
        {

            //SHIFTed this with name (ModificationMWShift) TO THE UTILITY FOR COMBINE USE
            // DELETE FROM HERE AFTER STABLILITY.
            // NO MORE EDITTING HERE

            //#Enhancement Switch Cases will be more better here
            double ModWeight = 0;
            if(Mod == "Propionamidation")
                ModWeight = 174.04631;
            else if (Mod == "Pyruvate-S")
                ModWeight = -17.0265;
            else if (Mod == "Pyruvate-C")
                ModWeight = 70.0055;
            else if (Mod == "Amidation")
                ModWeight = -0.984016;
            else if (Mod == "Citrullination")
                ModWeight =  0.984016;
            else if (Mod == "Methylation")
                ModWeight = 14.0156;
            else if (Mod == "Hydroxylation")
                ModWeight = 15.9949;
            else if (Mod == "Sulfoxide")
                ModWeight = 15.9949;
            else if (Mod == "Formylation")
                ModWeight = 27.9949;
            else if (Mod == "DiMethylation")
                ModWeight = 28.0313;
            else if (Mod == "S-Nitrosylation")
                ModWeight = 28.9902;
            else if (Mod == "Sulfone")
                ModWeight = 31.9898;
            else if (Mod == "DiHydroxylation")
                ModWeight = 31.9898;
            else if (Mod == "TriMethylation")
                ModWeight = 42.047;
            else if (Mod == "Acetylation")
                ModWeight = 42.0106;
            else if (Mod == "Gamma-Carboxyglutamic-Acid")
                ModWeight = 43.9898;
            else if (Mod == "Nitration")
                ModWeight = 44.9851;
            else if (Mod == "Phosphorylation")
                ModWeight = 79.9663;
            else if (Mod == "Pyrrolidone-Aarboxylic-Acid")
                ModWeight = -17.0265;
            else if (Mod == "O-linked-Glycosylation")
                ModWeight = 203.0794;
            else if (Mod == "Palmitoylation")
                ModWeight = 238.23;
            else if (Mod == "Glutathionylation")
                ModWeight = 305.068;
            else if (Mod == "N-linked-Glycosylation")
                ModWeight = 317.122;
            return ModWeight;
        }

        public void BlindPTMLocalization(List<ProteinDto> Matches, MsPeaksDto peakData, SearchParametersDto parameters, double MolW)
        {
            for (int index=0; index<=Matches.Count; index++)
            {
                // Ptm parameters initialization
                var protein = Matches.ElementAt(index);
                protein.PtmParticulars[index].ModStartSite = -1;
                protein.PtmParticulars[index].ModEndSite = -1;
                protein.PtmParticulars[index].ModWeight = -1;
                var MassDiff = MolW - protein.Mw;

                if (parameters.PtmAllow == 1)
                {
                    if (MassDiff > 13 && MassDiff < 951.3660) //Arbitrary number less then weight of methyl group && 3*N-linked-Glycosylation
                    {
                        var left = 0;
                        var right = 0;
                        if (protein.LeftMatchedIndex.Count > 0)
                        {
                            left = protein.LeftMatchedIndex[protein.LeftMatchedIndex.Count - 1] + 1; //The last LeftMatchedIndex + 1 gives the start site of modification
                        }
                        else
                        {
                            left = 1;
                        }
                        if (protein.RightMatchedIndex.Count > 0)
                        {
                            right = protein.InsilicoDetails.InsilicoMassLeft.Count - protein.RightMatchedIndex[protein.RightMatchedIndex.Count-1] + 1; //Num of LeftIons - last LeftMatchedIndex + 1 gives the end site of modification
                        }
                        else
                        {
                            right = protein.InsilicoDetails.InsilicoMassLeft.Count; //Num of LeftIons (i.e. the index right next to where LeftIons end) gives the end site of modification
                        }
                        if (left < right && left > 1 && right < protein.LeftMatchedIndex.Count)
                        {
                            // Ptm parameters are beig updated
                            protein.PtmParticulars[index].ModStartSite = left;
                            protein.PtmParticulars[index].ModEndSite = right;
                            protein.PtmParticulars[index].ModWeight = MassDiff;
                            protein.Mw = protein.Mw + MassDiff;
                        }
                    }
                }
                // Scoring
                var error = Math.Abs(MassDiff);
                if (error == 0)
                {
                    protein.MwScore = 1;
                }
                else
                {
                    protein.MwScore = Math.Pow((1 / 2), error);
                }
            }
        }

    }


}
