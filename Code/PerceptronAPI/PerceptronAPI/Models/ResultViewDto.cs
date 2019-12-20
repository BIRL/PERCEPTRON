using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultViewDto
    {
        public ExecutionTime ExecutionTime;
        public SearchParametersDto Paramters;
        public List<ResultsDto> Results;

    }
}