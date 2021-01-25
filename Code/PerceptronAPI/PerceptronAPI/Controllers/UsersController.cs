using System;
using System.IO;
using PerceptronAPI.Models;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace PerceptronAPI.Controllers
{
    public class UsersController
    {
        StreamReader AuthSecretFile = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\AuthSecret.txt");
        StreamReader BasePathFile = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\BasePath.txt");

        //StreamReader ReadPerceptronEmailAddress = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailAddress.txt");
        //StreamReader ReadPerceptronEmailPassword = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailPassword.txt");

        //SendingEmail.SendingEmailMethod(ReadPerceptronEmailAddress.ReadLine(), ReadPerceptronEmailPassword.ReadLine(), parametersDto.SearchParameters.EmailId, creationTime);

        string UserName = "DummyTest1";
        string EmailAddress = "DummyUser1@dummy.com";
        string DummyPassword = "123456";


        public string RegisterUser()
        {

            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = AuthSecretFile.ReadLine(),
                BasePath = BasePathFile.ReadLine()
            };

            string ErrorMessage = "";
            try
            {
                IFirebaseClient client = new FireSharp.FirebaseClient(IFC);

                var UniqueUserGuid = Guid.NewGuid().ToString();
                UserDetails NewUser = new UserDetails(UserName, EmailAddress, DummyPassword, UniqueUserGuid, "False");

                if (NewUser.UserName == "" || NewUser.EmailAddress == "" || NewUser.Password == "" || NewUser.UserName == null || NewUser.EmailAddress == null || NewUser.Password == null)
                {
                    return ErrorMessage = "All fields are required.";
                }
                else
                {
                    try   //If User Already Exists then, give a Message
                    {
                        FirebaseResponse FirebaseUserData = client.Get(@"CallingPerceptronApiUsers/" + NewUser.UserName);
                        UserDetails FirebaseFetchedUser = FirebaseUserData.ResultAs<UserDetails>();
                        if (FirebaseFetchedUser.UserName == NewUser.UserName || FirebaseFetchedUser.EmailAddress == NewUser.EmailAddress)
                            return ErrorMessage = "User is already registered with the given Username/Email Address. So, please register with other Username/Email Address and if you forgot the password then, you can change it.";
                    }
                    catch (Exception e) //If User is New alongwith its Username and Email address then, make a new object 
                    {
                        SetResponse set = client.Set(@"CallingPerceptronApiUsers/" + NewUser.UserName, NewUser);
                        return ErrorMessage = "Dear User, please verfify your email address."; 
                    }
                }

                
                //FirebaseResponse res = client.Get(@"perceptron/" + "Farhan");
                //var result = res.ResultAs<User>();

            }
            catch (Exception e)
            {

                ErrorMessage = "Unable to connect with Firebase please try later.";
            }
            return ErrorMessage;
        }

        public void CheckEmailIdExist()
        {

        }



        //public void LoginUser()
        //{
        //    IFirebaseConfig IFC = new FirebaseConfig()
        //    {
        //        AuthSecret = AuthSecretFile.ReadLine(),
        //        BasePath = BasePathFile.ReadLine()
        //    };

        //    //UserDetails user = new UserDetails("DummyUser", "123456");

        //    UserDetails CurrentUser = new UserDetails("DummyUser", "123456");
        //    IFirebaseClient client = new FireSharp.FirebaseClient(IFC);
        //    string ErrorMessage = "";
        //    if ((CurrentUser.UserName == "" || CurrentUser.Password == "") && (CurrentUser.UserName == null || CurrentUser.Password == null))
        //    {
        //        ErrorMessage = "All fields are required.";
        //    }
        //    else
        //    {
        //        FirebaseResponse res = client.Get(@"CallingPerceptronApiUsers/" + CurrentUser.UserName);
        //        UserDetails ResUser = res.ResultAs<UserDetails>();

        //        if (UserDetails.IsEqual(ResUser, CurrentUser))
        //        {
        //            int a = 1;
        //            //Send Email for verification of email address...!!!
        //        }

        //    }
        //}

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