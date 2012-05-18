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

        public ActionResult AttendanceView(int id)
        {
            //
            // Generates list of Attendances specifically for current Course
            var attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            // End of generating list of Attendances


            var student = attendanceItemsList.Select(a => a.Student).Distinct().OrderBy(s => s.LastName);
            List<Student> StudentList = student.ToList();
            

            //
            // Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            List<int> attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            };
            // End of generating list of AttendingDays

            AttendanceReportViewModel model = new AttendanceReportViewModel
            {
                AttendanceDays = attDayList,
                Students = StudentList,
                Attendances = attendanceItemsList,
            };
            return View(model);
        }

        public ActionResult StudentDetails(int id)
        {
            //
            // Generates list of Attendances specifically for current Course
            var attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            // End of generating list of Attendances

            var student = db.Students.Where(a => a.UserName == User.Identity.Name);  // This works for adding one student, not all of them.
            List<Student> StudentList = student.ToList();

            //
            // Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            List<int> attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            };
            // End of generating list of AttendingDays

            AttendanceReportViewModel model = new AttendanceReportViewModel
            {
                AttendanceDays = attDayList,
                Students = StudentList, // should be only one student for this ActionResult
                Attendances = attendanceItemsList,
            };
            return View(model);
        }
    }
}
