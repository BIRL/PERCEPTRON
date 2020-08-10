using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class PostTranslationModificationsSiteDto
    {

        // Declaring Variables
        public List<int> ListSiteIndex = new List<int>();   // amino acid Indices
        public List<string> ListModName = new List<string>();
        public List<string> ListSite = new List<string>();

        public PostTranslationModificationsSiteDto(ResultPtmSite PtmSites)
        {
            ListSiteIndex = PtmSites.Index.Split(',').Select(int.Parse).ToList();//PtmSites.Index.Split(',').ToList<string>(); //List<int>();
            ListModName = PtmSites.ModName.Split(',').ToList();
            ListSite = PtmSites.Site.Split(',').ToList();
        }
    }
}