using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PaulSchool.Models;
using PaulSchool.Resources;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

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
            IQueryable<Attendance> attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            // End of generating list of Attendances

            //
            // Generates list of Students in alphabetical order sorted by LastName
            // This was the original - it works var student = attendanceItemsList.Select(a => a.Student).Distinct().OrderBy(s => s.LastName);
            // var student = attendanceItemsList.Select(a => a.Student).Distinct(); // This line does not have the attendance editor bug, but does not order alphabetically

            IOrderedQueryable<Student> student =
                db.Enrollments.Where(e => e.CourseID == id).Select(e => e.Student).OrderBy(s => s.LastName);
            List<Student> StudentList = student.ToList();
            // End of generating list of Students

            //
            // Generates a list of all Enrollments for course - to be used to generate grades
            // Should to be refactored with above code-block
            IQueryable<Enrollment> enrollment = db.Enrollments.Where(i => i.CourseID == id);
            //List<Enrollment> EnrollmentList = enrollment.ToList();
            IEnumerable<Enrollment> enrollmentList = enrollment;

            //
            // Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            var attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            }
            // End of generating list of AttendingDays

            var model = new AttendanceReportViewModel
                            {
                                AttendanceDays = attDayList,
                                Students = StudentList,
                                Enrollments = enrollmentList,
                                Attendances = attendanceItemsList,
                                courseId = id
                            };
            return View(model);
        }

        public ActionResult EditIndividualAttendance(int id, int studentId)
        {
            //
            // Generates list of Attendances specifically for current Course
            IQueryable<Attendance> attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            // End of generating list of Attendances

            //
            // Generates list of Students in alphabetical order sorted by LastName
            // This was the original - it works var student = attendanceItemsList.Select(a => a.Student).Distinct().OrderBy(s => s.LastName);
            //var student = attendanceItemsList.Select(a => a.Student).Distinct(); // This line does not have the attendance editor bug, but does not order alphabetically

            IOrderedQueryable<Student> student =
                db.Enrollments.Where(e => e.CourseID == id && e.StudentID == studentId).Select(e => e.Student).OrderBy(
                    s => s.LastName);
            List<Student> studentList = student.ToList();
            // End of generating list of Students

            //
            // Generates a list of all Enrollments for course - to be used to generate grades
            // Should to be refactored with above code-block
            IQueryable<Enrollment> enrollment = db.Enrollments.Where(i => i.CourseID == id && i.StudentID == studentId);
            //List<Enrollment> EnrollmentList = enrollment.ToList();
            IEnumerable<Enrollment> enrollmentList = enrollment;

            //
            // Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            var attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            }
            // End of generating list of AttendingDays

            var model = new AttendanceReportViewModel
                            {
                                AttendanceDays = attDayList,
                                Students = studentList,
                                Enrollments = enrollmentList,
                                Attendances = attendanceItemsList,
                                courseId = id
                            };
            return View(model);
        }

        public ActionResult StudentDetails(int id)
        {
            //// Generates list of Attendances specifically for current Course
            IQueryable<Attendance> attendanceItems = db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            //// End of generating list of Attendances

            //// Generates list of Students (should be one only one student)
            IQueryable<Student> student = db.Students.Where(a => a.UserName == User.Identity.Name);
            //// This works for adding one student, not all of them.
            List<Student> studentList = student.ToList();
            //// End of generating list of Students

            //// Generates list of AttendingDays specifically for current Course
            Course course = db.Courses.FirstOrDefault(p => p.CourseID == id);
            var attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            }
            //// End of generating list of AttendingDays

            Enrollment enrollment =
                db.Enrollments.FirstOrDefault(q => q.Student.UserName == User.Identity.Name && q.CourseID == id);

            var model = new AttendanceReportViewModel
                            {
                                AttendanceDays = attDayList,
                                Students = studentList,
                                ////should be only one student for this ActionResult
                                Attendances = attendanceItemsList,
                                Comments = enrollment.Comments
                            };

            return View(model);
        }

        //
        // GET: /Attendance/EditComment/5 and another int

        public ActionResult EditComment(int studentId, int courseId)
        {
            Enrollment enrollment =
                db.Enrollments.FirstOrDefault(s => s.StudentID == studentId && s.CourseID == courseId);
            var model = new EditCommentGradeViewModel
                            {
                                EnrollmentID = enrollment.EnrollmentID,
                                Student = enrollment.Student,
                                Grades = new List<SelectListItem>
                                             {
                                                 new SelectListItem {Text = PaulSchoolResource.AttendanceController_EditComment_Incomplete, Value = PaulSchoolResource.AttendanceController_EditComment_Incomplete},
                                                 new SelectListItem {Text = PaulSchoolResource.AttendanceController_EditComment_Pass, Value = PaulSchoolResource.AttendanceController_EditComment_Pass},
                                                 new SelectListItem {Text = PaulSchoolResource.AttendanceController_EditComment_Fail, Value = PaulSchoolResource.AttendanceController_EditComment_Fail},
                                             },
                                Grade = enrollment.Grade,
                                Comments = enrollment.Comments
                            };
            return View(model);
        }

        //
        // POST: /Attendance/EditComment/etc. . .

        [HttpPost]
        public ActionResult EditComment(Enrollment enrollmentComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollmentComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AttendanceView", new {id = enrollmentComment.CourseID});
            }
            return View(enrollmentComment);
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
            Attendance attendance = db.Attendance.FirstOrDefault(s => s.StudentID == userId
                                                                      && s.CourseID == courseId
                                                                      && s.AttendanceDay == attendanceDay);
            if (attendance != null)
            {
                attendance.Present = Convert.ToBoolean(present);
                db.Entry(attendance).State = EntityState.Modified;
            }
            db.SaveChanges();
            return true;
        }
    }
}