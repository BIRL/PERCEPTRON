using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class DetailedProteinHitView
    {
        public ResultsDto Results;

        public DetailedProteinHitView()
        {
            Results = new ResultsDto();
        }
    }
}