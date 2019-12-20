using System;
using System.Collections.Generic;
using System.Linq;

namespace PerceptronLocalService.Utility
{
    public class AminoAcids
    {
        public const char Nomatchfound = '*';
        public const int MaximumAminoacidMass = 465;
        public const int AverageAminoacidMass = 110;

        public static readonly Dictionary<char, double> AminoAcidMasses = new Dictionary<char, double>()
        {
            {'M', 131.04049},
            {'Q', 128.05858},        
            {'A', 71.03711},
            {'R', 156.10111},
            {'N', 114.04293},
            {'D', 115.02694},
            {'C', 103.00919},
            {'E', 129.04259},
            {'G', 57.02146},
            {'H', 137.05891},
            {'I', 113.08406},
            {'L', 113.08406},
            {'K', 128.09496},
            {'F', 147.06841},
            {'P', 97.05276},
            {'S', 87.03203},
            {'T', 101.04768},
            {'W', 186.07931},
            {'Y', 163.06333},
            {'V', 99.06841},
            {'U', 168.964203},
            {'O', 255.158295}
        };




        public static char GetAminoAcidFromMw(double mw, double tol)
        {
            var aminoAcid = AminoAcidMasses.FirstOrDefault(x => x.Value < mw + tol && x.Value > mw - tol).Key; // 71'G' (71 is ASCII code for G)

            return aminoAcid == '\0' ? Nomatchfound : aminoAcid;
        }

        public static double GetAminoAcidMw(char aminoAcid)
        {
            double mw;
            return AminoAcidMasses.TryGetValue(aminoAcid, out mw) ? mw : AverageAminoacidMass;
        }

        public static List<string> GetModifiedAminoAcid(double mw, double tol)
        {
            var aa = new List<string>();

            if (Math.Abs(mw - (79.9663 + 163.06333)) < tol)
                aa.Add("Phosphorylation_Y");
            if (Math.Abs(mw - (79.9663 + 101.04768)) < tol)
                aa.Add("Phosphorylation_T");
            if (Math.Abs(mw - (79.9663 + 87.03203)) < tol)
                aa.Add("Phosphorylation_S");
            if (Math.Abs(mw - (79.9663 + 115.02694)) < tol)
                aa.Add("Phosphorylation_D");
            if (Math.Abs(mw - (79.9663 + 137.05891)) < tol)
                aa.Add("Phosphorylation_H");

            if (Math.Abs(mw - (14.0156 + 128.09496)) < tol)
                aa.Add("Methylation_K");
            if (Math.Abs(mw - (28.0313 + 128.09496)) < tol)
                aa.Add("DiMethylation_K");
            if (Math.Abs(mw - (128.05858 + 14.0156)) < tol)
                aa.Add("Methylation_G");
            if (Math.Abs(mw - (14.0156 + 156.10111)) < tol)
                aa.Add("Methylation_R");
            if (Math.Abs(mw - (28.0313 + 156.10111)) < tol)
                aa.Add("DiMethylation_R");
            if (Math.Abs(mw - (14.0156 + 103.00919)) < tol)
                aa.Add("Methylation_C");

            if (Math.Abs(mw - (42.0106 + 87.03203)) < tol)
                aa.Add("Acetylation_S");
            if (Math.Abs(mw - (42.0106 + 156.10111)) < tol)
                aa.Add("Acetylation_R");
            if (Math.Abs(mw - (42.0106 + 128.09496)) < tol)
                aa.Add("Acetylation_K");
            if (Math.Abs(mw - (42.0106 + 71.03711)) < tol)
                aa.Add("Acetylation_A");
            if (Math.Abs(mw - (42.0106 + 131.04049)) < tol)
                aa.Add("Acetylation_M");
            if (Math.Abs(mw - (42.0106 + 101.04768)) < tol)
                aa.Add("Acetylation_T");
            if (Math.Abs(mw - (42.0106 + 57.02146)) < tol)
                aa.Add("Acetylation_G");
            if (Math.Abs(mw - (42.0106 + 97.05276)) < tol)
                aa.Add("Acetylation_P");

            if (Math.Abs(mw - (15.9949 + 97.05276)) < tol)
                aa.Add("Hydroxylation_P");
            if (Math.Abs(mw - (31.9898 + 97.05276)) < tol)
                aa.Add("DiHydroxylation_P");
            if (Math.Abs(mw - (15.9949 + 128.09496)) < tol)
                aa.Add("Hydroxylation_K");
            if (Math.Abs(mw - (31.9898 + 128.09496)) < tol)
                aa.Add("DiHydroxylation_K");

            if (Math.Abs(mw - (203.0794 + 101.04768)) < tol)
                aa.Add("O-linked-Glycosylation_T");
            if (Math.Abs(mw - (203.0794 + 87.03203)) < tol)
                aa.Add("O-linked-Glycosylation_S");
            if (Math.Abs(mw - (317.122 + 114.04293)) < tol)
                aa.Add("N-linked-Glycosylation_N");

            if (Math.Abs(mw - (305.068 + 103.00919)) < tol)
                aa.Add("Glutathionylation_C");
            if (Math.Abs(mw - (28.9902 + 103.00919)) < tol)
                aa.Add("S-Nitrosylation_C");

            if (Math.Abs(mw - (128.05858 - 17.0265)) < tol)
                aa.Add("Pyrrolidone-Aarboxylic-Acid_Q");
            if (Math.Abs(mw - (129.04259 + 43.9898)) < tol)
                aa.Add("Gamma-Carboxyglutamic-Acid_E");
            if (Math.Abs(mw - (-17.0265 + 87.03203)) < tol)
                aa.Add("Pyruvate-S_S");

            if (Math.Abs(mw - (70.0055 + 103.00919)) < tol)
                aa.Add("Pyruvate-C_C");
            if (Math.Abs(mw - (44.9851 + 163.06333)) < tol)
                aa.Add("Nitration_Y");

            if (Math.Abs(mw - (31.9898 + 131.04049)) < tol)
                aa.Add("Sulfone_M");
            if (Math.Abs(mw - (15.9949 + 131.04049)) < tol)
                aa.Add("Sulfoxide_M");
            if (Math.Abs(mw - (27.9949 + 131.04049)) < tol)
                aa.Add("Formylation_M");
            if (Math.Abs(mw - (238.23 + 103.00919)) < tol)
                aa.Add("Palmitoylation_C");
            return aa;
        }

        public static double ModificationTable(string mod)
        {
            double mass = 0;
            if (string.Compare(mod, "Pyruvate-S", StringComparison.Ordinal) == 0)
                mass = -17.0265;

            else if (string.Compare(mod, "Pyruvate-C", StringComparison.Ordinal) == 0)
                mass = 70.0055;

            else if (string.Compare(mod, "Amidation", StringComparison.Ordinal) == 0)
                mass = -0.984016;

            else if (string.Compare(mod, "Citrullination", StringComparison.Ordinal) == 0)
                mass = 0.984016;

            else if (string.Compare(mod, "Methylation", StringComparison.Ordinal) == 0)
                mass = 14.0156;

            else if (string.Compare(mod, "Hydroxylation", StringComparison.Ordinal) == 0)
                mass = 15.9949;

            else if (string.Compare(mod, "Sulfoxide", StringComparison.Ordinal) == 0)
                mass = 15.9949;

            else if (string.Compare(mod, "Formylation", StringComparison.Ordinal) == 0)
                mass = 27.9949;

            else if (string.Compare(mod, "DiMethylation", StringComparison.Ordinal) == 0)
                mass = 28.0313;

            else if (string.Compare(mod, "S-Nitrosylation", StringComparison.Ordinal) == 0)
                mass = 28.9902;

            else if (string.Compare(mod, "Sulfone", StringComparison.Ordinal) == 0)
                mass = 31.9898;

            else if (string.Compare(mod, "DiHydroxylation", StringComparison.Ordinal) == 0)
                mass = 31.9898;

            else if (string.Compare(mod, "TriMethylation", StringComparison.Ordinal) == 0)
                mass = 42.047;

            else if (string.Compare(mod, "Acetylation", StringComparison.Ordinal) == 0)
                mass = 42.0106;

            else if (string.Compare(mod, "Gamma-Carboxyglutamic-Acid", StringComparison.Ordinal) == 0)
                mass = 43.9898;

            else if (string.Compare(mod, "Nitration", StringComparison.Ordinal) == 0)
                mass = 44.9851;

            else if (string.Compare(mod, "Phosphorylation", StringComparison.Ordinal) == 0)
                mass = 79.9663;

            else if (string.Compare(mod, "Pyrrolidone-Aarboxylic-Acid", StringComparison.Ordinal) == 0)
                mass = -17.0265;

            else if (string.Compare(mod, "O-linked-Glycosylation", StringComparison.Ordinal) == 0)
                mass = 203.0794;

            else if (string.Compare(mod, "Palmitoylation", StringComparison.Ordinal) == 0)
                mass = 238.23;

            else if (string.Compare(mod, "Glutathionylation", StringComparison.Ordinal) == 0)
                mass = 305.068;

            else if (string.Compare(mod, "N-linked-Glycosylation", StringComparison.Ordinal) == 0)
                mass = 317.122;

            return mass;
        }
    }
}