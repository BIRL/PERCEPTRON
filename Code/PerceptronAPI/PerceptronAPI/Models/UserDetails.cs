namespace PerceptronAPI.Models
{
    public class UserDetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        private static string Error;

        public UserDetails(string cUserName, string cPassword)
        {
            UserName = cUserName;
            Password = cPassword;
        }

        public static bool IsEqual(UserDetails user1, UserDetails user2)    // user1 -  Database save User          user2 - logged in user         // Authenticate User
        {

            if (user1 == null || user2 == null)
            {
                return false;
            }
            else
            {
                if (user1.UserName != user2.UserName)
                {
                    Error = "Username does not exist!";     // Error = "Username and/or password does not exist!";
                    return false;
                }
                else if (user1.Password != user2.Password)
                {
                    Error = "Username and/or password does not exist!";
                    return false;
                }
                return true;
            }
        }
    }
}