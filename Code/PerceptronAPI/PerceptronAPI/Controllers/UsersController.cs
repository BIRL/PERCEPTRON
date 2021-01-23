using System;
using PerceptronAPI.Models;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;


namespace PerceptronAPI.Controllers
{
    public class UsersController
    {
        public void RegisterUser()
        {
            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = "Ei3lIKbJlKC5hztLMFl14zMIQe8SadxmfQsYoEOj",
                BasePath = "https://perceptron-51a86.firebaseio.com/"
            };

            string ErrorMessage = "";
            try
            {
                IFirebaseClient client = new FireSharp.FirebaseClient(IFC);


                UserDetails user = new UserDetails("DummyUser", "123456");

                if((user.UserName == "" || user.Password  == "") && (user.UserName == null || user.Password == null))
                {
                    try { }
                    catch
                    {
                        ErrorMessage = "All fields are required.";
                        throw;
                    }
                    
                }

                SetResponse set = client.Set(@"CallingPerceptronApiUsers/" + user.UserName, user);

                



                //FirebaseResponse res = client.Get(@"perceptron/" + "Farhan");
                //var result = res.ResultAs<User>();

            }
            catch (Exception e)
            {

                ErrorMessage = "Unable to connect with Firebase please try later.";
            }
        }

        public void LoginUser()
        {
            IFirebaseConfig IFC = new FirebaseConfig()
            {
                AuthSecret = "Ei3lIKbJlKC5hztLMFl14zMIQe8SadxmfQsYoEOj",
                BasePath = "https://perceptron-51a86.firebaseio.com/"
            };

            //UserDetails user = new UserDetails("DummyUser", "123456");

            UserDetails CurrentUser = new UserDetails("DummyUser", "123456");
            IFirebaseClient client = new FireSharp.FirebaseClient(IFC);
            string ErrorMessage = "";
            if ((CurrentUser.UserName == "" || CurrentUser.Password == "") && (CurrentUser.UserName == null || CurrentUser.Password == null))
            {
                ErrorMessage = "All fields are required.";
            }
            else
            {
                FirebaseResponse res = client.Get(@"CallingPerceptronApiUsers/" + CurrentUser.UserName);
                UserDetails ResUser = res.ResultAs<UserDetails>();

                if(UserDetails.IsEqual(ResUser, CurrentUser))
                {
                    int a = 1;
                }

            }

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