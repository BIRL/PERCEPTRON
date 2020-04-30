using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class DetailedResults
    {
        public ExecutionTime ExecutionTime;
        public SearchParametersDto Paramters;
        public ResultsDto Results;

        public DetailedResults()
        {
            ExecutionTime=new ExecutionTime();
            Paramters=new SearchParametersDto();
            Results=new ResultsDto();
        }
        

    }
}