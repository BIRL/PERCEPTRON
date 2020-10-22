using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ScanResultsDownloadDataDto
    {

        //public List<string> FileUniqueIdsList = new List<string>();
        //public List<string> FileNamesList = new List<string>();
        //public List<string> UniqueFileNameList = new List<string>();
        //public SearchParameter searchParameters;

        public List<SearchResult> ListOfSearchResults = new List<SearchResult>();
        public List<ResultPtmSite> PtmSites = new List<ResultPtmSite>();

        //public ScanResultsDownloadDataDto()
        //{

        //}
        //List<string> cFileUniqueIdsList, List<string> cFileNamesList, List<string> cUniqueFileNameList, 
        public ScanResultsDownloadDataDto(List<SearchResult> cListOfSearchResults, List<ResultPtmSite> cPtmSites)
        {

            //FileUniqueIdsList = cFileUniqueIdsList;
            //FileNamesList = cFileNamesList;
            //UniqueFileNameList = cUniqueFileNameList;
            //searchParameters = csearchParameters;

            ListOfSearchResults = cListOfSearchResults;
            PtmSites = cPtmSites;
        }
    }
}