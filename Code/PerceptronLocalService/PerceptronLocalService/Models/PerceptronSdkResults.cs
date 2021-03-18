using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Models
{
    public class PerceptronSdkResults
    {
        public System.DateTime JobSubmission { get; set; }
        public string QueryId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string ResultsAvailable { get; set; }
    }
}
