using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ScanResultsDownloadDataDto
    {

        public List<string> FileUniqueIdsList = new List<string>();
        public List<string> FileNamesList = new List<string>();
        public List<SearchResult> ListOfSearchResults = new List<SearchResult>();
        public SearchParameter searchParameters;
        public List<ResultPtmSite> PtmSites = new List<ResultPtmSite>();


        public ScanResultsDownloadDataDto()
        {

        }

        public ScanResultsDownloadDataDto(List<string> cFileUniqueIdsList, List<string> cFileNamesList, List<SearchResult> cListOfSearchResults,
            SearchParameter csearchParameters, List<ResultPtmSite> cPtmSites)
        {

            FileUniqueIdsList = cFileUniqueIdsList;
            FileNamesList = cFileNamesList;
            ListOfSearchResults = cListOfSearchResults;
            searchParameters = csearchParameters;
            PtmSites = cPtmSites;
        }
    }
}