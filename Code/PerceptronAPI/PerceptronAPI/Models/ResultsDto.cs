using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultsDto
    {
        
        public SearchResult Results;
        public List<ResultInsilicoMatchLeft> InsilicioLeft;
        public List<ResultInsilicoMatchRight> InsilicoRight;
        public List<ResultPtmSite> PtmSites;

        public ResultsDto()
        {
            Results=new SearchResult();
            InsilicioLeft=new List<ResultInsilicoMatchLeft>();
            InsilicoRight=new List<ResultInsilicoMatchRight>();
            PtmSites=new List<ResultPtmSite>();
        }

    }
}