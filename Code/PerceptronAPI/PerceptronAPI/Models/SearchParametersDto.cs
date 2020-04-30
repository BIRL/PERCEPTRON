using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class SearchParametersDto
    {
        public SearchQuery SearchQuerry;
        public SearchParameter SearchParameters;
        public List<SearchFile> SearchFiles;
        public List<PtmFixedModification> FixedMods;
        public List<PtmVariableModification> VarMods;

        public SearchParametersDto()
        {
            SearchQuerry=new SearchQuery();
            SearchParameters = new SearchParameter();
            SearchFiles = new List<SearchFile>();
            FixedMods= new List<PtmFixedModification>();
            VarMods=new List<PtmVariableModification>();

        }

    }
}