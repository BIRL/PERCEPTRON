using System; //Updated 20201113
using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    [Serializable] //Updated 20201113
    public class PostTranslationModificationsSiteDto
    {
        // Declaring Variables
        public int Index; // amino acid Index
        public double Score; // protein Score
        public double ModWeight;
        public string ModName;
        public char Site;
        public List<char> AminoAcid;
        public int ModStartSite;
        public int ModEndSite;
        //public char AminoAcid;


        // Constructor
        public PostTranslationModificationsSiteDto()
        {
            // Initializing Variales
            Index = -1;
            Score = 0.0;
            ModWeight = 0.0;
            ModName = "";
            Site = '\0';
            AminoAcid = new List<char>(); // amino acid sequence
            ModStartSite = -1;
            ModEndSite = -1;
        }

        public PostTranslationModificationsSiteDto(int cIndex, string cModName, char cSite)
        {
            Index = cIndex;
            ModName = cModName;
            Site = cSite;
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

        public PostTranslationModificationsSiteDto(int index, double score, double modWeight, string modName, char site, List<char> aminoAcid, int cModStartSite, int cModEndSite)
        {
            // TODO: Complete member initialization
            Index = index;
            Score = score;
            ModWeight = modWeight;
            ModName = modName;
            Site = site;
            AminoAcid = aminoAcid;
            ModStartSite = cModStartSite;
            ModEndSite = cModEndSite;
        }

        public PostTranslationModificationsSiteDto(PostTranslationModificationsSiteDto PtmData)
        {

            //var  = new PostTranslationModificationsSiteDto();

            //PtmDataClone = PtmData;

            // Initializing Variales
            Index = PtmData.Index;
            Score = PtmData.Score;
            ModWeight = PtmData.ModWeight;
            ModName = PtmData.ModName;
            Site = PtmData.Site;
            AminoAcid = PtmData.AminoAcid; // amino acid sequence
            ModStartSite = PtmData.ModStartSite;
            ModEndSite = PtmData.ModEndSite;
        }


    }
}
