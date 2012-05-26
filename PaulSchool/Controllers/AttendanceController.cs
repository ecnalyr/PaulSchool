using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using PaulSchool.ViewModels;
using System.Data;

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

            //
            // Generates list of Students in alphabetical order sorted by LastName
            var student = attendanceItemsList.Select(a => a.Student).Distinct().OrderBy(s => s.LastName);
            List<Student> StudentList = student.ToList();
            // End of generating list of Students
            

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
                courseId = id
            };
            return View(model);
        }

        public ActionResult StudentDetails(int id)
        {
            //// Generates list of Attendances specifically for current Course
            var attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            //// End of generating list of Attendances

            //// Generates list of Students (should be one only one student)
            var student = db.Students.Where(a => a.UserName == User.Identity.Name);  //// This works for adding one student, not all of them.
            List<Student> StudentList = student.ToList();
            //// End of generating list of Students

            //// Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            List<int> attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            };
            //// End of generating list of AttendingDays

            AttendanceReportViewModel model = new AttendanceReportViewModel
            {
                AttendanceDays = attDayList,
                Students = StudentList, ////should be only one student for this ActionResult
                Attendances = attendanceItemsList,
            };
            return View(model);
        }

        /// <summary>
        /// Method to Update the Attendance of the student
        /// </summary>
        /// <param name="userId">Student Id to be updated</param>
        /// <param name="attendanceDay">Day for which the Attendance must be updated</param>
        /// <param name="courseId">Course Identifier</param>
        /// <param name="present">Is the student Present or not flag</param>
        /// <returns>returns true / false</returns>
        [HttpPost]
        public bool UpdateAttendance(int userId, int attendanceDay, int courseId, int present)
        {
            //// Taking the attendence of corresponding userId
            Attendance attendance = db.Attendance.Where(s => s.StudentID == userId 
                                                        && s.CourseID == courseId 
                                                        && s.AttendanceDay == attendanceDay).FirstOrDefault();
            attendance.Present = Convert.ToBoolean(present);
            db.Entry(attendance).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
    }
}
