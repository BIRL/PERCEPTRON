using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class PostTranslationModificationsSiteDto
    {
        // Declaring Variables
        public int Index; // amino acid Index
        public double Score; // protein Score
        public double ModWeight;
        public string ModName;
        public char Site;
        public List<char> AminoAcid = new List<char>(); // amino acid sequence

        // Constructor
        public PostTranslationModificationsSiteDto()
        {
            // Initializing Variales
            Index = 0;
            Score = 0;
            ModWeight = 0;
            ModName = null;
            Site = '\0';
        }

        //Another Constructor
        public PostTranslationModificationsSiteDto(int index, double score, double modWeight, string modName, char site, List<char> aminoAcid)
        {
            // TODO: Complete member initialization
            Index = index;
            Score = score;
            ModWeight = modWeight;
            ModName = modName;
            Site = site;
            AminoAcid = aminoAcid;
        }
    }
}
