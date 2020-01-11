using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class stat // This table is added into UserHistory
    {
        public string user { get; set; }  // It should be Email irrespective to user
        public string search { get; set; }
    }
}