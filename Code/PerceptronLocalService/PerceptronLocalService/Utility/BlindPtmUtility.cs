namespace PerceptronLocalService.Utility
{
    public class BlindPtmUtility
    {
        public static double[] ReturnModificationMass()
        {
            var difference = new double[37]; // List of Modification weights
            difference[0] = 79.9663 + 163.06333;
            difference[1] = 79.9663 + 101.04768;
            difference[2] = 79.9663 + 87.03203;
            difference[3] = 79.9663 + 115.02694;
            difference[4] = 79.9663 + 137.05891;
            difference[5] = 14.0156 + 128.09496;
            difference[6] = 28.0313 + 128.09496;
            difference[7] = 128.05858 + 14.0156;
            difference[8] = 14.0156 + 156.10111;
            difference[9] = 28.0313 + 156.10111;
            difference[10] = 14.0156 + 103.00919;
            difference[11] = 42.0106 + 87.03203;
            difference[12] = 42.0106 + 156.10111;
            difference[13] = 42.0106 + 128.09496;
            difference[14] = 42.0106 + 71.03711;
            difference[15] = 42.0106 + 131.04049;
            difference[16] = 42.0106 + 101.04768;
            difference[17] = 42.0106 + 57.02146;
            difference[18] = 42.0106 + 97.05276;
            difference[19] = 15.9949 + 97.05276;
            difference[20] = 31.9898 + 97.05276;
            difference[21] = 15.9949 + 128.09496;
            difference[22] = 31.9898 + 128.09496;
            difference[23] = 203.0794 + 101.04768;
            difference[24] = 203.0794 + 87.03203;
            difference[25] = 317.122 + 114.04293;
            difference[26] = 305.068 + 103.00919;
            difference[27] = 28.9902 + 103.00919;
            difference[28] = 128.05858 - 17.0265;
            difference[29] = 129.04259 + 43.9898;
            difference[30] = -17.0265 + 87.03203;
            difference[31] = 70.0055 + 103.00919;
            difference[32] = 44.9851 + 163.06333;
            difference[33] = 31.9898 + 131.04049;
            difference[34] = 15.9949 + 131.04049;
            difference[35] = 27.9949 + 131.04049;
            difference[36] = 238.23 + 103.00919;
            return difference;
        }

        public static string [] ReturnModificationNames()
        {
            var modificationName = new string[37];
            // Symbol for each Modification
            modificationName[0] = "Phosphorylation_Y";
            modificationName[1] = "Phosphorylation_T";
            modificationName[2] = "Phosphorylation_S";
            modificationName[3] = "Phosphorylation_D";
            modificationName[4] = "Phosphorylation_H";
            modificationName[5] = "Methylation_K";
            modificationName[6] = "DiMethylation_K";
            modificationName[7] = "Methylation_G";
            modificationName[8] = "Methylation_R";
            modificationName[9] = "DiMethylation_R";
            modificationName[10] = "Methylation_C";
            modificationName[11] = "Acetylation_S";
            modificationName[12] = "Acetylation_R";
            modificationName[13] = "Acetylation_K";
            modificationName[14] = "Acetylation_A";
            modificationName[15] = "Acetylation_M";
            modificationName[16] = "Acetylation_T";
            modificationName[17] = "Acetylation_G";
            modificationName[18] = "Acetylation_P";
            modificationName[19] = "Hydroxylation_P";
            modificationName[20] = "DiHydroxylation_P";
            modificationName[21] = "Hydroxylation_K";
            modificationName[22] = "DiHydroxylation_K";
            modificationName[23] = "O-linked-Glycosylation_T";
            modificationName[24] = "O-linked-Glycosylation_S";
            modificationName[25] = "N-linked-Glycosylation_N";
            modificationName[26] = "Glutathionylation_C";
            modificationName[27] = "S-Nitrosylation_C";
            modificationName[28] = "Pyrrolidone-Aarboxylic-Acid_Q";
            modificationName[29] = "Gamma-Carboxyglutamic-Acid_E";
            modificationName[30] = "Pyruvate-S_S";
            modificationName[31] = "Pyruvate-C_C";
            modificationName[32] = "Nitration_Y";
            modificationName[33] = "Sulfone_M";
            modificationName[34] = "Sulfoxide_M";
            modificationName[35] = "Formylation_M";
            modificationName[36] = "Palmitoylation_C";
            return modificationName;
        }

        public static char[] ReturnModificationTags()
        {
            var aminoAcids = new char[37];
            // Symbol for each Modification
            aminoAcids[0] = 'Y';
            aminoAcids[1] = 'T';
            aminoAcids[2] = 'S';
            aminoAcids[3] = 'D';
            aminoAcids[4] = 'H';
            aminoAcids[5] = 'K';
            aminoAcids[6] = 'K';
            aminoAcids[7] = 'G';
            aminoAcids[8] = 'R';
            aminoAcids[9] = 'R';
            aminoAcids[10] = 'C';
            aminoAcids[11] = 'S';
            aminoAcids[12] = 'R';
            aminoAcids[13] = 'K';
            aminoAcids[14] = 'A';
            aminoAcids[15] = 'M';
            aminoAcids[16] = 'T';
            aminoAcids[17] = 'G';
            aminoAcids[18] = 'P';
            aminoAcids[19] = 'P';
            aminoAcids[20] = 'P';
            aminoAcids[21] = 'K';
            aminoAcids[22] = 'K';
            aminoAcids[23] = 'T';
            aminoAcids[24] = 'S';
            aminoAcids[25] = 'N';
            aminoAcids[26] = 'C';
            aminoAcids[27] = 'C';
            aminoAcids[28] = 'Q';
            aminoAcids[29] = 'E';
            aminoAcids[30] = 'S';
            aminoAcids[31] = 'C';
            aminoAcids[32] = 'Y';
            aminoAcids[33] = 'M';
            aminoAcids[34] = 'M';
            aminoAcids[35] = 'M';
            aminoAcids[36] = 'C';
            return aminoAcids;
        }
    }
}
