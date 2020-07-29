using System;

namespace PerceptronLocalService.Utility
{
    public class PtmScoreNormalizer
    {
        // Normalization
        public double Normalize(double value, int select)
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
                // N_Linked_Glycosylation_N
                case 9:
                    max = 0.09 * 0.08 * 0.09 * 0.08 * 0.1 * 0.1 * 0.1 * 0.63 * 0.11 * 0.09 * 0.1 * 0.09;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // O_Linked_Glycosylation_T
                case 10:
                    max = 0.44 * 0.36 * 0.45 * 0.26 * 0.44 * 0.32 * 0.31 * 0.43 * 0.36 * 0.44 * 0.33 * 0.48;

                    normFactor = Math.Log10(max);
                    if (value == 0)
                        normalizedScore = 0;
                    else
                        normalizedScore = normFactor / Math.Log10(value);
                    break;
                // O_Linked_Glycosylation_S
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

    }
}