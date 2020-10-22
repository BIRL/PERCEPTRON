using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ScanInputDataDto
    {
        public List<string> FileUniqueIdsList = new List<string>();
        public List<string> FileNamesList = new List<string>();
        public List<string> UniqueFileNameList = new List<string>();
        public SearchParameter searchParameters;

        public ScanInputDataDto(List<string> cFileUniqueIdsList, List<string> cFileNamesList, List<string> cUniqueFileNameList, SearchParameter csearchParameters)
        {
            FileUniqueIdsList = cFileUniqueIdsList;
            FileNamesList = cFileNamesList;
            UniqueFileNameList = cUniqueFileNameList;
            searchParameters = csearchParameters;
        }
    }
}