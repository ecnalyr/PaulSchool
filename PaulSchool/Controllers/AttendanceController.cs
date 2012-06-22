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
            IOrderedQueryable<Student> student =
                db.Enrollments.Where(e => e.CourseID == id).Select(e => e.Student).OrderBy(s => s.LastName);
            List<Student> StudentList = student.ToList();
            // End of generating list of Students

            //
            // Generates a list of all Enrollments for course - to be used to generate grades
            // Should to be refactored with above code-block
            IQueryable<Enrollment> enrollment = db.Enrollments.Where(i => i.CourseID == id);
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
            ViewBag.CourseName = course.Title;
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
            IOrderedQueryable<Student> student =
                db.Enrollments.Where(e => e.CourseID == id && e.StudentID == studentId).Select(e => e.Student).OrderBy(
                    s => s.LastName);
            List<Student> studentList = student.ToList();
            // End of generating list of Students

            //
            // Generates a list of all Enrollments for course - to be used to generate grades
            IQueryable<Enrollment> enrollment = db.Enrollments.Where(i => i.CourseID == id && i.StudentID == studentId);
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
                                                 new SelectListItem
                                                     {
                                                         Text =
                                                             PaulSchoolResource.
                                                             AttendanceController_EditComment_Incomplete,
                                                         Value =
                                                             PaulSchoolResource.
                                                             AttendanceController_EditComment_Incomplete
                                                     },
                                                 new SelectListItem
                                                     {
                                                         Text = PaulSchoolResource.AttendanceController_EditComment_Pass,
                                                         Value = PaulSchoolResource.AttendanceController_EditComment_Pass
                                                     },
                                                 new SelectListItem
                                                     {
                                                         Text = PaulSchoolResource.AttendanceController_EditComment_Fail,
                                                         Value = PaulSchoolResource.AttendanceController_EditComment_Fail
                                                     },
                                             },
                                Grade = enrollment.Grade,
                                Comments = enrollment.Comments
                            };
            return View(model);
        }

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

        public ActionResult CompleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            course.Completed = true;

            BuildNotificationForAdmin(course);
            BuildNotificationForInstructor(course);
            BuildNotificationsForStudents(course);
            db.SaveChanges();

            return View("CompletedCourse");
        }

        private void BuildNotificationsForStudents(Course course)
        {
            foreach (Enrollment item in course.Enrollments)
            {
                string studentUserName = item.Student.UserName;
                BuildNotificationForStudent(item, course);
            }
        }

        private void BuildNotificationForStudent(Enrollment item, Course course)
        {
            var newNotificationForStudent = new Notification
                                                {
                                                    Time = DateTime.Now,
                                                    Details =
                                                        "The Course titled: " + course.Title + " and Instructed by " +
                                                        course.Instructor.LastName +
                                                        ", in which you were enrolled has been marked Complete.  Your grade and attendance values are final.",
                                                    Link = Url.Action("StudentDetails", "Attendance", new {id = course.CourseID}),
                                                    ViewableBy = item.Student.UserName,
                                                    Complete = false
                                                };
            db.Notification.Add(newNotificationForStudent);
        }

        private void BuildNotificationForInstructor(Course course)
        {
            var newNotificationForInstructor = new Notification
                                                   {
                                                       Time = DateTime.Now,
                                                       Details =
                                                           "The Course titled: " + course.Title +
                                                           " that was instructed by you has been marked Complete by Username: " +
                                                           User.Identity.Name,
                                                       Link =
                                                           Url.Action("Details", "Course", new {id = course.CourseID}),
                                                       ViewableBy = course.Instructor.UserName,
                                                       Complete = false
                                                   };
            db.Notification.Add(newNotificationForInstructor);
        }

        private void BuildNotificationForAdmin(Course course)
        {
            var newNotificationForAdmin = new Notification
                                              {
                                                  Time = DateTime.Now,
                                                  Details =
                                                      "The Course titled: " + course.Title + " that was instructed by " +
                                                      course.Instructor.LastName +
                                                      " has been marked Complete by Username: " +
                                                      User.Identity.Name,
                                                  Link = Url.Action("Details", "Course", new {id = course.CourseID}),
                                                  ViewableBy = "Admin",
                                                  Complete = false
                                              };
            db.Notification.Add(newNotificationForAdmin);
        }

        public ActionResult ArchiveCourse(int id)
        {
            Course course = db.Courses.Find(id);
            course.Archived = true;
            BuildNotification(course);
            db.SaveChanges();
            return View("ArchivedCourse");
        }

        private void BuildNotification(Course course)
        {
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Administrator has Archived " + course.Title +
                                              " that was instructed by " +
                                              course.Instructor.LastName,
                                          Link = Url.Action("Details", "Course", new {id = course.CourseID}),
                                          ViewableBy = "Admin",
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);
        }
    }
}