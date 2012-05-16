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
            //IEnumerable<Attendance> model = db.Attendance.Where(s => s.CourseID == 4);
            //return View(model);
            // All of the below code is a work in progress - above code yields a functional (although incorrect) table when used
            // with the List view.
            var z = 4;
            var model = from s in db.Attendance
                                 where s.CourseID == z
                                 group s.AttendanceDay by s.StudentID into t
                                 select new
                                 {
                                     StudentID = t.Key,
                                     Days = t.OrderBy(x => x)
                                 };
            
            //model.Pivot(o => o.AttendanceDay, 
            return View(model);

        }

        public ActionResult AttendnaceLinq()
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

            var student = attendanceItemsList.Select(a => a.Student).Distinct()/*.OrderBy(a => a)*/;  // This works for adding one student, not all of them.
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
    }
}
