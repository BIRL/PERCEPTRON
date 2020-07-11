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
        public PtmFixedModification FixedMods;
        public PtmVariableModification VarMods;

        public SearchParametersDto()
        {
            SearchQuerry=new SearchQuery();
            SearchParameters = new SearchParameter();
            SearchFiles = new List<SearchFile>();
            FixedMods= new PtmFixedModification();
            VarMods=new PtmVariableModification();

        }

    }
}