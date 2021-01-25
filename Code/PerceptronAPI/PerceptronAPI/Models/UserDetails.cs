namespace PerceptronAPI.Models
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UniqueUserGuid { get; set; }
        public string VerfiedUser { get; set; }

        private static string Error;


        //public UserDetails(string cUserName, string cEmailAddress, string cPassword)
        //{
        //    UserName = cUserName;
        //    EmailAddress = cEmailAddress;
        //    Password = cPassword;
        //}


        public UserDetails()
        {
            UserName = "";
            EmailAddress = "";
            Password = "";
            UniqueUserGuid = "";
            VerfiedUser = "";                 // "True"       "False"     "Anonymous"
        }

        public UserDetails(string cUserName, string cEmailAddress, string cPassword, string cUniqueUserGuid, string cVerfiedUser)
        {
            UserName = cUserName;
            EmailAddress = cEmailAddress;
            Password = cPassword;
            UniqueUserGuid = cUniqueUserGuid;
            VerfiedUser = cVerfiedUser;                 // "True"       "False"     "Anonymous"
        }

        public static bool IsEqual(UserDetails DatabaseUser, UserDetails LoggedInUser)    // user1 -  Database save User          user2 - logged in user         // Authenticate User
        {

            if (DatabaseUser == null || LoggedInUser == null)
            {
                return false;
            }
            else
            {
                if (DatabaseUser.EmailAddress != LoggedInUser.EmailAddress)
                {
                    Error = "Username does not exist!";     // Error = "Username and/or password does not exist!";
                    return false;
                }
                //else if (DatabaseUser.Password != LoggedInUser.Password)
                //{
                //    Error = "Username and/or password does not exist!";
                //    return false;
                //}
                return true;
            }
        }
    }
}