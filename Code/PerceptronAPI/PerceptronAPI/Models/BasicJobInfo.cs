using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class BasicJobInfo
    {
        public string UserId;
        public string EmailId;
        public string Title;
        public int GuestEnabled;

        public BasicJobInfo()
        {
            UserId = "";
            EmailId = "";
            Title = "";
            GuestEnabled = 0; //Initializing Not as Guest  
        }

        public BasicJobInfo(string cUserId,  string cEmailId, string cTitle, int cGuestEnabled)
        {
            UserId = cUserId;
            EmailId = cEmailId;
            Title = cTitle;
            GuestEnabled = cGuestEnabled;
        }
    }
}