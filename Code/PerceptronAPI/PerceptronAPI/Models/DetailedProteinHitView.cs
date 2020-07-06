using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class DetailedProteinHitView
    {
        public ExecutionTime ExecutionTime;
        public ResultsDto Results;
        public SearchParameter searchParameters;
        public PeakListData PeakListData;  // Just Used in Results Writing in TXT file
        public SearchFile FileInfo;  // Just Used in Results Writing in TXT file

        public DetailedProteinHitView()
        {
            ExecutionTime = new ExecutionTime(); 
            Results = new ResultsDto();
            searchParameters = new SearchParameter();
            PeakListData = new PeakListData();
            FileInfo = new SearchFile();
        }
    }
}