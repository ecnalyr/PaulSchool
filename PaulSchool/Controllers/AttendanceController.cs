// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttendanceController.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace PaulSchool.Controllers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;

    using PaulSchool.Models;
    using PaulSchool.Resources;
    using PaulSchool.ViewModels;

    #endregion

    public class AttendanceController : Controller
    {
        #region Fields

        private readonly SchoolContext db = new SchoolContext();

        #endregion

        #region Public Methods and Operators

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult ArchiveCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            course.Archived = true;
            this.BuildNotification(course);
            this.db.SaveChanges();
            return this.RedirectToAction("Message", new { message = "You have added the course to the archive." });
        }

        public ActionResult AttendanceView(int id)
        {
            List<Attendance> attendanceItemsList = this.AttendanceItemsList(id);
            List<Student> studentList = this.StudentList(id);
            IEnumerable<Enrollment> enrollmentList = this.EnrollmentList(id);
            Course course = this.db.Courses.FirstOrDefault(p => p.CourseID == id);
            List<int> attDayList = AttDayList(course);

            var model = new AttendanceReportViewModel
                {
                    AttendanceDays = attDayList, 
                    Students = studentList, 
                    Enrollments = enrollmentList, 
                    Attendances = attendanceItemsList, 
                    CourseId = id, 
                    Course = course
                };
            this.ViewBag.CourseName = course.Title;
            return View(model);
        }

        public ActionResult CompleteCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            course.Completed = true;

            this.BuildNotificationForAdmin(course);
            this.BuildNotificationForInstructor(course);
            this.BuildNotificationsForStudents(course);
            this.db.SaveChanges();

            return this.RedirectToAction("Message", new { message = "You have completed the course." });
        }

        public ActionResult EditComment(int studentId, int courseId)
        {
            Enrollment enrollment =
                this.db.Enrollments.FirstOrDefault(s => s.StudentID == studentId && s.CourseID == courseId);
            if (enrollment != null && enrollment.Course.Completed)
            {
                RedirectToAction("Error");
            }
            else
            {
                var model = new EditCommentGradeViewModel
                    {
                        EnrollmentID = enrollment.EnrollmentID, 
                        Student = enrollment.Student, 
                        Grades =
                            new List<SelectListItem>
                                {
                                    new SelectListItem
                                        {
                                            Text = PaulSchoolResource.AttendanceController_EditComment_Incomplete, 
                                            Value = PaulSchoolResource.AttendanceController_EditComment_Incomplete
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

            return View("Error");
        }

        [HttpPost]
        public ActionResult EditComment(Enrollment enrollmentComment)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(enrollmentComment).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("AttendanceView", new { id = enrollmentComment.CourseID });
            }

            return View(enrollmentComment);
        }

        public ActionResult EditIndividualAttendance(int id, int studentId)
        {
            List<Attendance> attendanceItemsList = this.AttendanceItemsList(id);
            List<Student> studentList = this.SingleStudentList(id, studentId);
            IEnumerable<Enrollment> enrollmentList = this.SingleStudentEnrollmentList(id, studentId);
            Course course = this.db.Courses.FirstOrDefault(p => p.CourseID == id);
            List<int> attDayList = AttDayList(course);

            var model = new AttendanceReportViewModel
                {
                    AttendanceDays = attDayList, 
                    Students = studentList, 
                    Enrollments = enrollmentList, 
                    Attendances = attendanceItemsList, 
                    CourseId = id
                };

            if (course.Completed)
            {
                RedirectToAction("Error");
            }
            else
            {
                return View(model);
            }

            return View("Error");
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ViewResult Message(string message)
        {
            this.ViewBag.message = message;
            return this.View();
        }

        public ActionResult StudentDetails(int id)
        {
            //// Generates list of Attendances specifically for current Course
            IQueryable<Attendance> attendanceItems = this.db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();

            //// End of generating list of Attendances

            //// Generates list of Students (should be one only one student)
            IQueryable<Student> student = this.db.Students.Where(a => a.UserName == this.User.Identity.Name);
            List<Student> studentList = student.ToList();

            //// End of generating list of Students

            //// Generates list of AttendingDays specifically for current Course
            Course course = this.db.Courses.FirstOrDefault(p => p.CourseID == id);
            var attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            }

            //// End of generating list of AttendingDays
            Enrollment enrollment =
                this.db.Enrollments.FirstOrDefault(
                    q => q.Student.UserName == this.User.Identity.Name && q.CourseID == id);

            var model = new AttendanceReportViewModel
                {
                    AttendanceDays = attDayList, 
                    Students = studentList, 
                    
                    
                    ////should be only one student for this ActionResult
                    Attendances = attendanceItemsList, 
                    Comments = enrollment.Comments, 
                    Paid = enrollment.Paid, 
                    Course = course, 
                    SingleStudentEnrollment = enrollment
                };

            return View(model);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult UnArchiveCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            course.Archived = false;
            this.db.SaveChanges();
            return this.RedirectToAction("Message", new { message = "You have removed the course from the archive." });
        }

        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult UnCompleteCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            course.Completed = false;
            this.db.SaveChanges();
            return this.RedirectToAction("Message", new { message = "You have set the course to incomplete." });
        }

        [HttpPost]
        public bool UpdateAttendance(int userId, int attendanceDay, int courseId, int present)
        {
            //// Taking the attendence of corresponding userId
            Attendance attendance =
                this.db.Attendance.FirstOrDefault(
                    s => s.StudentID == userId && s.CourseID == courseId && s.AttendanceDay == attendanceDay);
            if (attendance != null)
            {
                attendance.Present = Convert.ToBoolean(present);
                this.db.Entry(attendance).State = EntityState.Modified;
            }

            this.db.SaveChanges();
            return true;
        }

        public ActionResult ViewComment(int studentId, int courseId)
        {
            Enrollment enrollment =
                this.db.Enrollments.FirstOrDefault(s => s.StudentID == studentId && s.CourseID == courseId);
            var model = new EditCommentGradeViewModel
                {
                    EnrollmentID = enrollment.EnrollmentID, 
                    Student = enrollment.Student, 
                    Grades =
                        new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Text = PaulSchoolResource.AttendanceController_EditComment_Incomplete, 
                                        Value = PaulSchoolResource.AttendanceController_EditComment_Incomplete
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

        #endregion

        #region Methods

        /// <summary>
        ///   The dispose.
        /// </summary>
        /// <param name="disposing"> The disposing. </param>
        protected override void Dispose(bool disposing)
        {
            this.db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        ///   Generates list of AttendingDays specifically for current Course
        /// </summary>
        /// <param name="course"> The Course object </param>
        /// <returns> A list of integers representing the each day of attendance </returns>
        private static List<int> AttDayList(Course course)
        {
            var attDayList = new List<int>();
            for (int i = 0; i < course.AttendingDays; i++)
            {
                attDayList.Add(i + 1);
            }

            return attDayList;
        }

        /// <summary>
        ///   Generates list of Attendances specifically for current Course
        /// </summary>
        /// <param name="id"> The CourseId </param>
        /// <returns> A List of Attendances </returns>
        private List<Attendance> AttendanceItemsList(int id)
        {
            IQueryable<Attendance> attendanceItems = this.db.Attendance.Where(s => s.CourseID == id);
            List<Attendance> attendanceItemsList = attendanceItems.ToList();
            return attendanceItemsList;
        }

        /// <summary>
        ///   The build notification.
        /// </summary>
        /// <param name="course"> The course. </param>
        private void BuildNotification(Course course)
        {
            var newNotification = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "An Administrator has Archived " + course.Title + " that was instructed by "
                        + course.Instructor.LastName, 
                    Link = this.Url.Action("Details", "Course", new { id = course.CourseID }), 
                    ViewableBy = "Admin", 
                    Complete = false
                };
            this.db.Notification.Add(newNotification);
        }

        /// <summary>
        ///   The build notification for admin.
        /// </summary>
        /// <param name="course"> The course. </param>
        private void BuildNotificationForAdmin(Course course)
        {
            var newNotificationForAdmin = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "The Course titled: " + course.Title + " that was instructed by " + course.Instructor.LastName
                        + " has been marked Complete by Username: " + this.User.Identity.Name, 
                    Link = this.Url.Action("Details", "Course", new { id = course.CourseID }), 
                    ViewableBy = "Admin", 
                    Complete = false
                };
            this.db.Notification.Add(newNotificationForAdmin);
        }

        /// <summary>
        ///   The build notification for instructor.
        /// </summary>
        /// <param name="course"> The course. </param>
        private void BuildNotificationForInstructor(Course course)
        {
            var newNotificationForInstructor = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "The Course titled: " + course.Title
                        + " that was instructed by you has been marked Complete by Username: " + this.User.Identity.Name, 
                    Link = this.Url.Action("Details", "Course", new { id = course.CourseID }), 
                    ViewableBy = course.Instructor.UserName, 
                    Complete = false
                };
            this.db.Notification.Add(newNotificationForInstructor);
        }

        /// <summary>
        ///   The build notification for student.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="course"> The course. </param>
        private void BuildNotificationForStudent(Enrollment item, Course course)
        {
            var newNotificationForStudent = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "The Course titled: " + course.Title + " and Instructed by " + course.Instructor.LastName
                        +
                        ", in which you were enrolled has been marked Complete.  Your grade and attendance values are final.", 
                    Link = this.Url.Action("StudentDetails", "Attendance", new { id = course.CourseID }), 
                    ViewableBy = item.Student.UserName, 
                    Complete = false
                };
            this.db.Notification.Add(newNotificationForStudent);
        }

        /// <summary>
        ///   The build notification for student regarding commissioning.
        /// </summary>
        /// <param name="course"> The course. </param>
        /// <param name="item"> The item. </param>
        /// <param name="totalElectivesPassed"> The total electives passed. </param>
        /// <param name="totalCoresPassed"> The total cores passed. </param>
        private void BuildNotificationForStudentRegardingCommissioning(
            Course course, Enrollment item, int totalElectivesPassed, int totalCoresPassed)
        {
            var newNotificationForStudentRegardingCommissioning = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "With " + totalCoresPassed + " core and " + totalElectivesPassed
                        + " elective courses complete, you meet the minimum requirements to apply for Commissioning.", 
                    Link = this.Url.Action("Index", "Commissioning", new { id = course.CourseID }), 
                    ViewableBy = item.Student.UserName, 
                    Complete = false
                };
            this.db.Notification.Add(newNotificationForStudentRegardingCommissioning);
        }

        /// <summary>
        ///   The build notifications for students.
        /// </summary>
        /// <param name="course"> The course. </param>
        private void BuildNotificationsForStudents(Course course)
        {
            int totalCoresNeeded = this.db.CommissioningRequirements.Find(1).CoreCoursesRequired;
            int totalElectivesNeeded = this.db.CommissioningRequirements.Find(1).ElectiveCoursesRequired;
            foreach (Enrollment item in course.Enrollments)
            {
                this.BuildNotificationForStudent(item, course);

                // Check if student now qualifies for Commissioning.  If so, sent notification stating that they meet the minimum requirements
                int totalCoresPassed = this.TotalCoresPassed(item);
                int totalElectivesPassed = this.TotalElectivesPassed(item);
                if (totalCoresPassed >= totalCoresNeeded && totalElectivesPassed >= totalElectivesNeeded)
                {
                    this.BuildNotificationForStudentRegardingCommissioning(
                        course, item, totalElectivesPassed, totalCoresPassed);
                }
            }
        }

        /// <summary>
        ///   Generates a list of all Enrollments for course - to be used to generate grades
        /// </summary>
        /// <param name="id"> The CourseId </param>
        /// <returns> A List of Enrollments </returns>
        private IEnumerable<Enrollment> EnrollmentList(int id)
        {
            IQueryable<Enrollment> enrollment = this.db.Enrollments.Where(i => i.CourseID == id);
            IEnumerable<Enrollment> enrollmentList = enrollment;
            return enrollmentList;
        }

        /// <summary>
        ///   Generates a list of all Enrollments for a single Student from a Course - to be used to generate grades
        /// </summary>
        /// <param name="id"> The CourseId </param>
        /// <param name="studentId"> The StudentId </param>
        /// <returns> A List of enrollment values representing the single Student in the Course </returns>
        private IEnumerable<Enrollment> SingleStudentEnrollmentList(int id, int studentId)
        {
            IQueryable<Enrollment> enrollment =
                this.db.Enrollments.Where(i => i.CourseID == id && i.StudentID == studentId);
            IEnumerable<Enrollment> enrollmentList = enrollment;
            return enrollmentList;
        }

        /// <summary>
        ///   Generates list of one Student in alphabetical order sorted by LastName (it is just one Student)
        /// </summary>
        /// <param name="id"> The CourseId </param>
        /// <param name="studentId"> The StudentId </param>
        /// <returns> A List of Students </returns>
        private List<Student> SingleStudentList(int id, int studentId)
        {
            IOrderedQueryable<Student> student =
                this.db.Enrollments.Where(e => e.CourseID == id && e.StudentID == studentId).Select(e => e.Student).
                    OrderBy(s => s.LastName);
            List<Student> studentList = student.ToList();
            return studentList;
        }

        /// <summary>
        ///   Generates list of Students in alphabetical order sorted by LastName
        /// </summary>
        /// <param name="id"> The CourseId </param>
        /// <returns> A List of Students </returns>
        private List<Student> StudentList(int id)
        {
            IOrderedQueryable<Student> student =
                this.db.Enrollments.Where(e => e.CourseID == id).Select(e => e.Student).OrderBy(s => s.LastName);
            List<Student> studentList = student.ToList();
            return studentList;
        }

        /// <summary>
        ///   The total cores passed.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The total cores passed. </returns>
        private int TotalCoresPassed(Enrollment item)
        {
            int totalCoresPassed =
                this.db.Enrollments.Count(
                    s =>
                    s.StudentID == item.Student.StudentID && s.Grade == "Credit Received" && s.Course.Elective == false
                    && s.Course.Title != "Day of Reflection");
            return totalCoresPassed;
        }

        /// <summary>
        ///   The total electives passed.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The total electives passed. </returns>
        private int TotalElectivesPassed(Enrollment item)
        {
            int totalElectivesPassed =
                this.db.Enrollments.Count(
                    s => s.StudentID == item.Student.StudentID && s.Grade == "Credit Received" && s.Course.Elective);
            return totalElectivesPassed;
        }

        #endregion
    }
}