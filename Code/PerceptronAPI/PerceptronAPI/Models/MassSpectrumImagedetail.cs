using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class MassSpectrumImagedetail
    {
        public string QueryId;
        public string Path;

        public MassSpectrumImagedetail()
        {
            QueryId = "";
            Path = "";
        }
    }
}