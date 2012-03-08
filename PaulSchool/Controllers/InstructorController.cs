using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using PagedList;

namespace PaulSchool.Controllers
{ 
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Instructor/

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName desc" : "FName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email desc" : "Email";
            ViewBag.UserNameSortParm = sortOrder == "UserName" ? "UserName desc" : "UserName";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var instructors = from s in db.Instructors
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                instructors = instructors.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name desc":
                    instructors = instructors.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    instructors = instructors.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    instructors = instructors.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "FName":
                    instructors = instructors.OrderBy(s => s.FirstMidName);
                    break;
                case "FName desc":
                    instructors = instructors.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Email":
                    instructors = instructors.OrderBy(s => s.Email);
                    break;
                case "Email desc":
                    instructors = instructors.OrderByDescending(s => s.Email);
                    break;
                case "UserName":
                    instructors = instructors.OrderBy(s => s.UserName);
                    break;
                case "UserName desc":
                    instructors = instructors.OrderByDescending(s => s.UserName);
                    break;
                default:
                    instructors = instructors.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(instructors.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Instructor/Details/5

        public ViewResult Details(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // GET: /Instructor/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Instructor/Create

        [HttpPost]
        public ActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(instructor);
        }
        
        //
        // GET: /Instructor/Edit/5
 
        public ActionResult Edit(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // POST: /Instructor/Edit/5

        [HttpPost]
        public ActionResult Edit(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        //
        // GET: /Instructor/Delete/5
 
        public ActionResult Delete(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // POST: /Instructor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
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