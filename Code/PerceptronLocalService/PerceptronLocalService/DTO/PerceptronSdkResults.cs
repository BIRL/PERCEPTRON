using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class PerceptronSdkResults
    {
        public string QueryId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string ResultsAvailable { get; set; }

        //public PerceptronSdkResults(DateTime cJobSubmission, string cQueryId, string cTitle, string cUserName, string cResultsAvailable)
        //{
        //    JobSubmission = cJobSubmission;
        //    QueryId = cQueryId;
        //    Title = cTitle;
        //    UserName = cUserName;
        //    ResultsAvailable = cResultsAvailable;
        //}

    }
}
