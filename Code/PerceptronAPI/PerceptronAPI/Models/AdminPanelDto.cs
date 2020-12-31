using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class AdminPanelDto
    {
        public string UpdateDatabase { get; set; }
        public string DownloadDatabase { get; set; }
        public string FileName { get; set; }
    }
}