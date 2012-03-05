using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();
        //
        // GET: /Course/

        public ActionResult Index()
        {
            var courses = db.Courses.ToList();
            return View(courses);
        }

        //
        // GET: /Course/Details/5

        public ViewResult Details(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

    }
}
