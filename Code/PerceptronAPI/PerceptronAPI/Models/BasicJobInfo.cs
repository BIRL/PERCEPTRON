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
        public int GuestEnabled;

        public BasicJobInfo()
        {
            UserId = "";
            Title = "";
            GuestEnabled = 0; //Initializing as Not as Guest
        }

        public BasicJobInfo(string cUserId, string cTitle, int cGuestEnabled)
        {
            UserId = cUserId;
            Title = cTitle;
            GuestEnabled = cGuestEnabled;
        }
    }
}