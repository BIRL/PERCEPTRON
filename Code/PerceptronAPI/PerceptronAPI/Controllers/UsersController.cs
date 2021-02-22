using System;
using System.IO;
using PerceptronAPI.Models;
using PerceptronAPI.Utility;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace PerceptronAPI.Controllers
{
    public class UsersController : ApiController
    {
        StreamReader AuthSecretFile = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\AuthSecret.txt");
        StreamReader BasePathFile = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\BasePath.txt");
        string RootDirectoryForFTP = @"E:\10_PERCEPTRON_Live\PlaceHolderFolder\";
        string Message = "";


        //StreamReader ReadPerceptronEmailAddress = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailAddress.txt");
        //StreamReader ReadPerceptronEmailPassword = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailPassword.txt");

        //SendingEmail.SendingEmailMethod(ReadPerceptronEmailAddress.ReadLine(), ReadPerceptronEmailPassword.ReadLine(), parametersDto.SearchParameters.EmailId, creationTime);

        string UserName = "DummyTest1";
        string EmailAddress = "DummyUser1@dummy.com";
        string Password = "123456";

        

        public UsersController()
        {
            int a = 1;
        }



        [HttpPost]
        [Route("api/user/CallingPerceptronApi_RegisterUser")]
        public async Task<string> CallingPerceptronApi_RegisterUser(HttpRequestMessage request)
        //public string RegisterUser()
        {

            var RequestInput = request.Content.ReadAsStringAsync();  //.Content.ToString();  //.
            string input = RequestInput.Result.ToString();

            string[] UserInfoArray = input.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            UserName = UserInfoArray[0];
            EmailAddress = UserInfoArray[1];
            Password = UserInfoArray[2];

            int IsEmailAddressValid = EmailAddress.Length - EmailAddress.Replace("@", "").Length;
            if (IsEmailAddressValid != 1)
                return Message = "EmailAddress is not valid";

            //if (!UserName.Any(char.IsDigit))
            //    return ErrorMessage = "Please enter a unique username. Entered username is already registered.";


            DateTime time = DateTime.Now;             // Fetching Current Time
            string format = "yyyy/MM/dd HH:mm:ss";
            var CreationTime = time.ToString(format); // Formating creationTime and assigning  // CHECK IF ITS NEEDED...

            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = AuthSecretFile.ReadLine(),
                BasePath = BasePathFile.ReadLine()
            };

            try
            {
                IFirebaseClient client = new FireSharp.FirebaseClient(IFC);

                var UniqueUserGuid = Guid.NewGuid().ToString();
                UserDetails NewUser = new UserDetails(UserName, EmailAddress, Password, UniqueUserGuid, "False");

                if (NewUser.UserName.Length > 25)
                    return Message = "Your username is too lengthy. please proceed with relatively shorter username. Moreover, username should contains less than 50 alphabets.";

                if (NewUser.UserName == "" || NewUser.EmailAddress == "" || NewUser.Password == "" || NewUser.UserName == null || NewUser.EmailAddress == null || NewUser.Password == null)
                {
                    return Message = "All fields are required.";
                }
                else
                {
                    try   //If User Already Exists then, give a Message
                    {
                        FirebaseResponse FirebaseUserData = client.Get(@"CallingPerceptronApiUsers/" + NewUser.UserName);   // Check if email id exists...???
                        UserDetails FirebaseFetchedUser = FirebaseUserData.ResultAs<UserDetails>();
                        if (FirebaseFetchedUser.UserName == NewUser.UserName || FirebaseFetchedUser.EmailAddress == NewUser.EmailAddress)
                            return Message = "User is already registered with the given Username/Email Address. So, please register with other Username/Email Address and if you forgot the password then, you can change it.";
                    }
                    catch (Exception e) //If User is New alongwith its Username and Email address then, make a new object  // If Email id is not exists then,...
                    {
                        SetResponse set = client.Set(@"CallingPerceptronApiUsers/" + NewUser.UserName, NewUser);


                        StreamReader ReadPerceptronEmailAddress = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailAddress.txt");
                        StreamReader ReadPerceptronEmailPassword = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailPassword.txt");

                        SendingEmail.SendingEmailMethod(ReadPerceptronEmailAddress.ReadLine(), ReadPerceptronEmailPassword.ReadLine(), NewUser.EmailAddress, UniqueUserGuid, CreationTime, "VerifyEmail");
                        return Message = "Dear User, please verfify your email address."; 
                    }
                }
            }
            catch (Exception e)
            {

                Message = "Unable to process your registration please try again later.";
            }
            return Message;
        }


        [HttpPost]
        [Route("api/user/CallingPerceptronApi_VerfiyingEmailAddress")]
        public async Task<string> VerfiyingEmailAddress(HttpRequestMessage request)
        {

            var RequestInput = request.Content.ReadAsStringAsync();  //.Content.ToString();  //.
            string input = RequestInput.Result.ToString();


            string[] UserInfoArray = input.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            UserDetails UserVerfiyingEmailAddress = new UserDetails(UserInfoArray[0], UserInfoArray[1], UserInfoArray[2], UserInfoArray[3], "False");

            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = AuthSecretFile.ReadLine(),
                BasePath = BasePathFile.ReadLine()
            };

            IFirebaseClient client = new FireSharp.FirebaseClient(IFC);
            FirebaseResponse FirebaseUserData = client.Get(@"CallingPerceptronApiUsers/" + UserVerfiyingEmailAddress.UserName);   // Check if email id exists...???
            UserDetails FirebaseFetchedUser = FirebaseUserData.ResultAs<UserDetails>();

            if ((FirebaseFetchedUser.EmailAddress == UserVerfiyingEmailAddress.EmailAddress) && (FirebaseFetchedUser.Password == UserVerfiyingEmailAddress.Password))
            {
                if (FirebaseFetchedUser.UniqueUserGuid == UserVerfiyingEmailAddress.UniqueUserGuid)
                {
                    FirebaseFetchedUser.VerfiedUser = "True";
                    client.Update(@"CallingPerceptronApiUsers/" + UserVerfiyingEmailAddress.UserName, FirebaseFetchedUser);

                    if (!Directory.Exists(RootDirectoryForFTP + FirebaseFetchedUser.UserName))
                    {
                        Directory.CreateDirectory(RootDirectoryForFTP + FirebaseFetchedUser.UserName);
                    }

                    return Message = "Dear User, Your email address has been successfully verified.";
                }
            }
                

            else // If there is not User exists with the given email address.
                return Message = "There is no Username exists with this Username so, please first signup then, proceed.";


            return Message;
        }


        // 
        [HttpPost]
        [Route("api/user/CallingPerceptronApi_LoginUserWithSearchQuery")]
        public async Task<string> LoginUserWithSearchQuery(HttpRequestMessage request)
        {

            var RequestInput = request.Content.ReadAsStringAsync();
            string input = RequestInput.Result.ToString();

            string[] UserInfoArray = input.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = AuthSecretFile.ReadLine(),
                BasePath = BasePathFile.ReadLine()
            };

            //UserDetails user = new UserDetails("DummyUser", "123456");

            UserDetails LoggedInUser = new UserDetails(UserInfoArray[0], UserInfoArray[1], UserInfoArray[2], "", "");
            IFirebaseClient client = new FireSharp.FirebaseClient(IFC);


            FirebaseResponse FirebaseUserData = client.Get(@"CallingPerceptronApiUsers/" + LoggedInUser.UserName);
            UserDetails FirebaseFetchedUser = FirebaseUserData.ResultAs<UserDetails>();

            if (LoggedInUser.UserName == FirebaseFetchedUser.UserName && LoggedInUser.EmailAddress == FirebaseFetchedUser.EmailAddress && LoggedInUser.Password == FirebaseFetchedUser.Password)
            {

                if (FirebaseFetchedUser.VerfiedUser == "True")
                {
                    SearchController _SearchController = new SearchController();
                    _SearchController.SearchQuery(UserInfoArray);
                    //////if (UserDetails.IsEqual(ResUser, LoggedInUser))           // ITS HEALTHY...
                    //////{
                    //////    int a = 1;
                    //////    //Send Email for verification of email address...!!!
                    //////}

                }
                else  // Use Calling Perceptron Api as a Guest User
                {
                    //return Message = "Please verify your email address first, then proceed.";
                }
            }
            else
            {
                return Message = "Credential information is incorrect.";
            }
            return Message;
        }

        public void AuthenticateUser()   //By One Time Enter Key
        {

        }
    }

}
//    public class UsersController : ApiController
//    {
//        public Users UsersElemenet = new Users();

//        //public string Get_registeration(string uName, string name, string email, string rPass)
//        public string Get_registeration(string id, string pass, string g, string k)
//        {
//            //return UsersElemenet.NewUser(uName, name, email, rPass);
//            return UsersElemenet.NewUser(id, pass, g, k);

//        }

//        public string Get_Session(string id, string g)
//        {
//            return UsersElemenet.SessionCheck(id, g) ? "T" : "F";
//        }


//        public string Get_login(string id, string pass, string g)
//        {
//            return UsersElemenet.Login(id, pass, g);
//        }


//        public bool Get_logout(string id, string g)
//        {
//            return UsersElemenet.Logout(id, g);
//        }

//        public bool Get_UpdateInfo(string id, string pass, string g)
//        {
//            return UsersElemenet.UpdateInfo(id, g, pass);
//        }

//        public bool Get_UpdatePassword(string id, string g)
//        {
//            return UsersElemenet.UpdatePassword(id, g);
//        }

//        public bool Get_activate(string em)
//        {
//            return UsersElemenet.Activate(em);
//        }

//        //public Class2 Get_Nothing()
//        //{
//        //    Class2 xyz = new Class2();
//        //    xyz.mass = 5;
//        //    return xyz;
//        //}

//        public UserDetails Get_User_information(string em)
//        {
//            var x = UsersElemenet.retieve_user(em);
//            x.RPass = "****";
//            return x;
//        }


//        public List<Searchlist> Get_my_searches(string em)
//        {
//            var answer = UsersElemenet.retrieve_searches(em);
//            return answer;
//        }

//        public Searchview Get_viewsearch(string em)
//        {
//            var answer = UsersElemenet.retrieve_searchview(em);
//            return answer;
//        }
//    }
//}