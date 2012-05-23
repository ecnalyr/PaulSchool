using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{ 
    [Authorize (Roles= "Administrator, SuperAdministrator")]
    public class CourseTemplatesController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /CourseTemplates/

        public ViewResult Index()
        {
            return View(db.CourseTemplates.ToList());
        }

        //
        // GET: /CourseTemplates/Details/5

        public ViewResult Details(int id)
        {
            CourseTemplates coursetemplates = db.CourseTemplates.Find(id);
            return View(coursetemplates);
        }

        //
        // GET: /CourseTemplates/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CourseTemplates/Create

        [HttpPost]
        public ActionResult Create(CourseTemplates coursetemplates)
        {
            if (ModelState.IsValid)
            {
                db.CourseTemplates.Add(coursetemplates);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(coursetemplates);
        }
        
        //
        // GET: /CourseTemplates/Edit/5
 
        public ActionResult Edit(int id)
        {
            CourseTemplates coursetemplates = db.CourseTemplates.Find(id);
            return View(coursetemplates);
        }

        //
        // POST: /CourseTemplates/Edit/5

        [HttpPost]
        public ActionResult Edit(CourseTemplates coursetemplates)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coursetemplates).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coursetemplates);
        }

        //
        // GET: /CourseTemplates/Delete/5
 
        public ActionResult Delete(int id)
        {
            CourseTemplates coursetemplates = db.CourseTemplates.Find(id);
            return View(coursetemplates);
        }

        //
        // POST: /CourseTemplates/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            CourseTemplates coursetemplates = db.CourseTemplates.Find(id);
            db.CourseTemplates.Remove(coursetemplates);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}