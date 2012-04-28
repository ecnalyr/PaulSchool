using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    public class AttendanceController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Attendance/

        public ActionResult Index()
        {
            return View();
        }
        
        //
        // GET: /Attendance/Attendance/1
        public ActionResult Attendance(int id)
        {
            var viewModel = new AttendanceViewModel
            {
                AttendanceItems = db.Attendance.Where(s => s.CourseID == id)
            };
            return View(viewModel);
        }

        //
        // GET: /Attendance/List  - Should list all attendance values from ONLY courseID = 4
        public ActionResult List()
        {
            IEnumerable<Attendance> model = db.Attendance.Where(s => s.CourseID == 4);
            return View(model);
        }

    }
}
