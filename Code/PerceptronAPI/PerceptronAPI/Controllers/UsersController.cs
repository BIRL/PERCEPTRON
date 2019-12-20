using System.Collections.Generic;
using System.Web.Http;
using PerceptronAPI.Models;
using PerceptronAPI.ServiceLayer;

namespace PerceptronAPI.Controllers
{
    public class UsersController : ApiController
    {
        public Users UsersElemenet = new Users();

        //public string Get_registeration(string uName, string name, string email, string rPass)
        public string Get_registeration(string id, string pass, string g, string k)
        {
            //return UsersElemenet.NewUser(uName, name, email, rPass);
            return UsersElemenet.NewUser(id, pass, g, k);

        }

        public string Get_Session(string id, string g)
        {
            return UsersElemenet.SessionCheck(id, g) ? "T" : "F";
        }


        public string Get_login(string id, string pass, string g)
        {
            return UsersElemenet.Login(id, pass, g);
        }


        public bool Get_logout(string id, string g)
        {
            return UsersElemenet.Logout(id, g);
        }

        public bool Get_UpdateInfo(string id, string pass, string g)
        {
            return UsersElemenet.UpdateInfo(id, g, pass);
        }

        public bool Get_UpdatePassword(string id, string g)
        {
            return UsersElemenet.UpdatePassword(id, g);
        }

        public bool Get_activate(string em)
        {
            return UsersElemenet.Activate(em);
        }

        //public Class2 Get_Nothing()
        //{
        //    Class2 xyz = new Class2();
        //    xyz.mass = 5;
        //    return xyz;
        //}

        public UserDetails Get_User_information(string em)
        {
            var x = UsersElemenet.retieve_user(em);
            x.RPass = "****";
            return x;
        }


        public List<Searchlist> Get_my_searches(string em)
        {
            var answer = UsersElemenet.retrieve_searches(em);
            return answer;
        }

        public Searchview Get_viewsearch(string em)
        {
            var answer = UsersElemenet.retrieve_searchview(em);
            return answer;
        }
    }
}