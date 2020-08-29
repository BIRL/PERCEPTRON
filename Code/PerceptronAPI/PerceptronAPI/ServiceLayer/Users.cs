//using System;
//using System.Collections.Generic;
//using PerceptronAPI.Models;
//using PerceptronAPI.Repository;

//namespace PerceptronAPI.ServiceLayer
//{
//    public class Users
//    {
//        public string NewUser(string uName, string name, string email, string rPass)
//        {
//            var db = new MySqlDatabase();
//            var g = Guid.NewGuid();
//            return db.Registeration(uName, rPass, name, email, g.ToString());
//        }

//        public string Login(string id, string pass, string g)
//        {
//            var db = new MySqlDatabase();
//            return db.Login(id, pass, g);
//        }

//        public bool Activate(string email)
//        {
//            var db = new MySqlDatabase();
//            return db.Activate(email);
//        }

//        public bool SessionCheck(string id, string g)
//        {
//            var db = new MySqlDatabase();
//            return db.Session_validator(id, g);
//        }

//        public bool Logout(string id, string g)
//        {
//            var db = new MySqlDatabase();
//            return db.Logout(id, g);
//        }

//        public bool UpdateInfo(string id, string newName, string newEmail)
//        {
//            var db = new MySqlDatabase();
//            return db.UpdateInfo(newEmail, id, newName);
//        }

//        public bool UpdatePassword(string id, string pwd)
//        {
//            var db = new MySqlDatabase();
//            return db.UpdatePassword(pwd, id);
//        }

//        public UserDetails retieve_user(string userid)
//        {
//            var db = new MySqlDatabase();
//            return db.Get_user(userid);
//        }

//        public List<Searchlist> retrieve_searches(string userid)
//        {
//            var db = new MySqlDatabase();
//            return db.retrieve_searches_db(userid);
//        }

//        public Searchview retrieve_searchview(string userid)
//        {
//            var db = new MySqlDatabase();
//            return db.retrieve_searchview_db(userid);
//        }
//    }
//}