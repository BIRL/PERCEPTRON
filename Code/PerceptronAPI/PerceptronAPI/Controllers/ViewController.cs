using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PerceptronAPI.Models;
using PerceptronAPI.Repository;

namespace PerceptronAPI.Controllers
{
    public class ViewController : Controller
    {
        //IDataAccessLayer _dataLayer;
        // GET: View/Details/5
        public ActionResult Details(string id = "2")   //CHANGE HERE TTO QID    //CHANGE HERE TTO QID
        {
            var sql = new SqlDatabase();
            
            var abc = sql.GetImagePathMassSpectrum(id);
            //var db = new PerceptronDatabaseEntities();
            //var employee = db.Tests.Select(x => x.QueryId == id);
            return View(abc);
        }


    }
}




//// GET: View/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: View/Create
//        [HttpPost]
//        public ActionResult Create(FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add insert logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: View/Edit/5
//        public ActionResult Edit(int id)
//        {
//            return View();
//        }

//        // POST: View/Edit/5
//        [HttpPost]
//        public ActionResult Edit(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add update logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        // GET: View/Delete/5
//        public ActionResult Delete(int id)
//        {
//            return View();
//        }

//        // POST: View/Delete/5
//        [HttpPost]
//        public ActionResult Delete(int id, FormCollection collection)
//        {
//            try
//            {
//                // TODO: Add delete logic here

//                return RedirectToAction("Index");
//            }
//            catch
//            {
//                return View();
//            }
//        }