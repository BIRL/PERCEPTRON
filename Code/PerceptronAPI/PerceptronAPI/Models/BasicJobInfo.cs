using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class BasicJobInfo
    {
        public string UserId;
        public string Title;


        public BasicJobInfo()
        {
            UserId = "";
            Title = "";
        }

        public BasicJobInfo(string cUserId, string cTitle)
        {
            UserId = cUserId;
            Title = cTitle;
        }

    }
}