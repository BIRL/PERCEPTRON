using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class PostTranslationModificationsSiteDto
    {

        // Declaring Variables
        public string stringSiteIndex;
        public string stringModName;
        public string stringSite;

        public int SiteIndex;
        public string ModName;
        public string Site;

        public List<int> ListSiteIndex = new List<int>();   // amino acid Indices
        public List<string> ListModName = new List<string>();
        public List<string> ListSite = new List<string>();

        //public PostTranslationModificationsSiteDto()
        //{
        //    // Initializing Variales
        //    stringSiteIndex = "";
        //    stringModName = "";
        //    stringSite = "";
        //}

        public PostTranslationModificationsSiteDto()
        {
            // Initializing Variales
            SiteIndex = 0;
            ModName = "";
            Site = "";
        }


        public PostTranslationModificationsSiteDto(int cSiteIndex, string cModName, string cSite)
        {
            // Initializing Variales
            SiteIndex = cSiteIndex;
            ModName = cModName;
            Site = cSite;
        }

        public PostTranslationModificationsSiteDto(ResultPtmSite PtmSites)
        {
            ListSiteIndex = PtmSites.Index.Split(',').Select(int.Parse).ToList();//PtmSites.Index.Split(',').ToList<string>(); //List<int>();
            ListModName = PtmSites.ModName.Split(',').ToList();
            ListSite = PtmSites.Site.Split(',').ToList();
        }


        public List<PostTranslationModificationsSiteDto> ProcessPtmSiteInfo(PostTranslationModificationsSiteDto cPtmSitesInfo)  // Just using to format the into required form
        {
            List<PostTranslationModificationsSiteDto> PtmSites = new List<PostTranslationModificationsSiteDto>();

            for (int i = 0; i < cPtmSitesInfo.ListSite.Count; i++)
            {
                PtmSites.Add(new PostTranslationModificationsSiteDto(cPtmSitesInfo.ListSiteIndex[i], cPtmSitesInfo.ListModName[i], cPtmSitesInfo.ListSite[i]));
            }
            return PtmSites;
        }
    }
}