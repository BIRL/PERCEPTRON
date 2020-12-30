namespace PerceptronLocalService.Utility
{
    public class BlindPtmUtility
    {
        public static double[] ReturnModificationMass()
        {
            var difference = new double[37]; // List of Modification weights
            difference[0] = 70.0055;
            difference[1] = 99.0321;
            difference[2] = 111.0321;
            difference[3] = 113.0477;
            difference[4] = 113.0477;
            difference[5] = 117.0248;
            difference[6] = 129.0426;
            difference[7] = 129.0426;
            difference[8] = 131.9994;
            difference[9] = 139.0634;
            difference[10] = 142.0742;
            difference[11] = 142.1106;
            difference[12] = 143.0583;
            difference[13] = 144.0899;
            difference[14] = 147.0354;
            difference[15] = 156.1263;
            difference[16] = 159.0354;
            difference[17] = 160.0848;
            difference[18] = 163.0303;
            difference[19] = 166.9983;
            difference[20] = 170.1056;
            difference[21] = 170.1167;
            difference[22] = 173.0147;
            difference[23] = 173.0324;
            difference[24] = 173.0511;
            difference[25] = 181.0140;
            difference[26] = 184.1324;
            difference[27] = 194.9932;
            difference[28] = 198.1117;
            difference[29] = 208.0484;
            difference[30] = 217.0252;
            difference[31] = 243.0296;
            difference[32] = 290.1114;
            difference[33] = 304.1271;
            difference[34] = 341.2392;
            difference[35] = 408.0772;
            difference[36] = 431.16493;
            return difference;
        }

        public static string[] ReturnModificationNames()
        {
            var modificationName = new string[37];
            // Symbol for each Modification
            modificationName[0] = "Pyruvate-S";
            modificationName[1] = "Acetylation";
            modificationName[2] = "Pyrrolidone-Aarboxylic-Acid";
            modificationName[3] = "Acetylation";
            modificationName[4] = "Hydroxylation";
            modificationName[5] = "Methylation";
            modificationName[6] = "Acetylation";
            modificationName[7] = "DiHydroxylation";
            modificationName[8] = "S-Nitrosylation";
            modificationName[9] = "Acetylation";
            modificationName[10] = "Methylation";
            modificationName[11] = "Methylation";
            modificationName[12] = "Acetylation";
            modificationName[13] = "Hydroxylation";
            modificationName[14] = "Sulfoxide";
            modificationName[15] = "DiMethylation";
            modificationName[16] = "Formylation";
            modificationName[17] = "DiHydroxylation";
            modificationName[18] = "Sulfone";
            modificationName[19] = "Phosphorylation";
            modificationName[20] = "Acetylation";
            modificationName[21] = "Methylation";
            modificationName[22] = "Pyruvate-C";
            modificationName[23] = "Gamma-Carboxyglutamic-Acid";
            modificationName[24] = "Acetylation";
            modificationName[25] = "Phosphorylation";
            modificationName[26] = "DiMethylation";
            modificationName[27] = "Phosphorylation";
            modificationName[28] = "Acetylation";
            modificationName[29] = "Nitration";
            modificationName[30] = "Phosphorylation";
            modificationName[31] = "Phosphorylation";
            modificationName[32] = "O-linked-Glycosylation";
            modificationName[33] = "O-linked-Glycosylation";
            modificationName[34] = "Palmitoylation";
            modificationName[35] = "Glutathionylation";
            modificationName[36] = "N-linked-Glycosylation";
            return modificationName;
        }

        public static char[] ReturnModificationTags()
        {
            var aminoAcids = new char[37];
            // Symbol for each Modification
            aminoAcids[0] = 'S';
            aminoAcids[1] = 'G';
            aminoAcids[2] = 'Q';
            aminoAcids[3] = 'A';
            aminoAcids[4] = 'P';    // Updated 20201230
            aminoAcids[5] = 'C';
            aminoAcids[6] = 'S';
            aminoAcids[7] = 'P';
            aminoAcids[8] = 'C';
            aminoAcids[9] = 'P';
            aminoAcids[10] = 'G';
            aminoAcids[11] = 'K';
            aminoAcids[12] = 'T';
            aminoAcids[13] = 'K';
            aminoAcids[14] = 'M';
            aminoAcids[15] = 'K';
            aminoAcids[16] = 'M';
            aminoAcids[17] = 'K';
            aminoAcids[18] = 'M';
            aminoAcids[19] = 'S';
            aminoAcids[20] = 'K';
            aminoAcids[21] = 'R';
            aminoAcids[22] = 'C';
            aminoAcids[23] = 'E';
            aminoAcids[24] = 'M';
            aminoAcids[25] = 'T';
            aminoAcids[26] = 'R';
            aminoAcids[27] = 'D';
            aminoAcids[28] = 'R';
            aminoAcids[29] = 'Y';
            aminoAcids[30] = 'H';
            aminoAcids[31] = 'Y';
            aminoAcids[32] = 'S';
            aminoAcids[33] = 'T';
            aminoAcids[34] = 'C';
            aminoAcids[35] = 'C';
            aminoAcids[36] = 'N';
            return aminoAcids;
        }
    }
}




//namespace PerceptronLocalService.Utility
//{
//    public class BlindPtmUtility
//    {
//        public static double[] ReturnModificationMass()
//        {
//            var difference = new double[37]; // List of Modification weights
//            difference[0] = 79.9663 + 163.06333;
//            difference[1] = 79.9663 + 101.04768;
//            difference[2] = 79.9663 + 87.03203;
//            difference[3] = 79.9663 + 115.02694;
//            difference[4] = 79.9663 + 137.05891;
//            difference[5] = 14.0156 + 128.09496;
//            difference[6] = 28.0313 + 128.09496;
//            difference[7] = 128.05858 + 14.0156;
//            difference[8] = 14.0156 + 156.10111;
//            difference[9] = 28.0313 + 156.10111;
//            difference[10] = 14.0156 + 103.00919;
//            difference[11] = 42.0106 + 87.03203;
//            difference[12] = 42.0106 + 156.10111;
//            difference[13] = 42.0106 + 128.09496;
//            difference[14] = 42.0106 + 71.03711;
//            difference[15] = 42.0106 + 131.04049;
//            difference[16] = 42.0106 + 101.04768;
//            difference[17] = 42.0106 + 57.02146;
//            difference[18] = 42.0106 + 97.05276;
//            difference[19] = 15.9949 + 97.05276;
//            difference[20] = 31.9898 + 97.05276;
//            difference[21] = 15.9949 + 128.09496;
//            difference[22] = 31.9898 + 128.09496;
//            difference[23] = 203.0794 + 101.04768;
//            difference[24] = 203.0794 + 87.03203;
//            difference[25] = 317.122 + 114.04293;
//            difference[26] = 305.068 + 103.00919;
//            difference[27] = 28.9902 + 103.00919;
//            difference[28] = 128.05858 - 17.0265;
//            difference[29] = 129.04259 + 43.9898;
//            difference[30] = -17.0265 + 87.03203;
//            difference[31] = 70.0055 + 103.00919;
//            difference[32] = 44.9851 + 163.06333;
//            difference[33] = 31.9898 + 131.04049;
//            difference[34] = 15.9949 + 131.04049;
//            difference[35] = 27.9949 + 131.04049;
//            difference[36] = 238.23 + 103.00919;
//            return difference;
//        }

//        public static string [] ReturnModificationNames()
//        {
//            var modificationName = new string[37];
//            // Symbol for each Modification
//            modificationName[0] = "Phosphorylation_Y";
//            modificationName[1] = "Phosphorylation_T";
//            modificationName[2] = "Phosphorylation_S";
//            modificationName[3] = "Phosphorylation_D";
//            modificationName[4] = "Phosphorylation_H";
//            modificationName[5] = "Methylation_K";
//            modificationName[6] = "DiMethylation_K";
//            modificationName[7] = "Methylation_G";
//            modificationName[8] = "Methylation_R";
//            modificationName[9] = "DiMethylation_R";
//            modificationName[10] = "Methylation_C";
//            modificationName[11] = "Acetylation_S";
//            modificationName[12] = "Acetylation_R";
//            modificationName[13] = "Acetylation_K";
//            modificationName[14] = "Acetylation_A";
//            modificationName[15] = "Acetylation_M";
//            modificationName[16] = "Acetylation_T";
//            modificationName[17] = "Acetylation_G";
//            modificationName[18] = "Acetylation_P";
//            modificationName[19] = "Hydroxylation_P";
//            modificationName[20] = "DiHydroxylation_P";
//            modificationName[21] = "Hydroxylation_K";
//            modificationName[22] = "DiHydroxylation_K";
//            modificationName[23] = "O-linked-Glycosylation_T";
//            modificationName[24] = "O-linked-Glycosylation_S";
//            modificationName[25] = "N-linked-Glycosylation_N";
//            modificationName[26] = "Glutathionylation_C";
//            modificationName[27] = "S-Nitrosylation_C";
//            modificationName[28] = "Pyrrolidone-Aarboxylic-Acid_Q";
//            modificationName[29] = "Gamma-Carboxyglutamic-Acid_E";
//            modificationName[30] = "Pyruvate-S_S";
//            modificationName[31] = "Pyruvate-C_C";
//            modificationName[32] = "Nitration_Y";
//            modificationName[33] = "Sulfone_M";
//            modificationName[34] = "Sulfoxide_M";
//            modificationName[35] = "Formylation_M";
//            modificationName[36] = "Palmitoylation_C";
//            return modificationName;
//        }

//        public static char[] ReturnModificationTags()
//        {
//            var aminoAcids = new char[37];
//            // Symbol for each Modification
//            aminoAcids[0] = 'Y';
//            aminoAcids[1] = 'T';
//            aminoAcids[2] = 'S';
//            aminoAcids[3] = 'D';
//            aminoAcids[4] = 'H';
//            aminoAcids[5] = 'K';
//            aminoAcids[6] = 'K';
//            aminoAcids[7] = 'G';
//            aminoAcids[8] = 'R';
//            aminoAcids[9] = 'R';
//            aminoAcids[10] = 'C';
//            aminoAcids[11] = 'S';
//            aminoAcids[12] = 'R';
//            aminoAcids[13] = 'K';
//            aminoAcids[14] = 'A';
//            aminoAcids[15] = 'M';
//            aminoAcids[16] = 'T';
//            aminoAcids[17] = 'G';
//            aminoAcids[18] = 'P';
//            aminoAcids[19] = 'P';
//            aminoAcids[20] = 'P';
//            aminoAcids[21] = 'K';
//            aminoAcids[22] = 'K';
//            aminoAcids[23] = 'T';
//            aminoAcids[24] = 'S';
//            aminoAcids[25] = 'N';
//            aminoAcids[26] = 'C';
//            aminoAcids[27] = 'C';
//            aminoAcids[28] = 'Q';
//            aminoAcids[29] = 'E';
//            aminoAcids[30] = 'S';
//            aminoAcids[31] = 'C';
//            aminoAcids[32] = 'Y';
//            aminoAcids[33] = 'M';
//            aminoAcids[34] = 'M';
//            aminoAcids[35] = 'M';
//            aminoAcids[36] = 'C';
//            return aminoAcids;
//        }
//    }
//}
