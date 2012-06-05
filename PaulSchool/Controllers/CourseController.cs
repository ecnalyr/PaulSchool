using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public PartialViewResult PreFillCourse(string selectedCourse)
            // Note that this is searching by title only - there cannot be more than one class with the same title
        {
            CourseTemplates selectedTemplate = db.CourseTemplates.FirstOrDefault(
                o => o.Title == selectedCourse);

            ViewBag.selectedString = selectedCourse;
            if (selectedTemplate != null)
            {
                var preFill = new ApplyCourseViewModel
                                  {
                                      Title = selectedTemplate.Title,
                                      Credits = selectedTemplate.Credits,
                                      Elective = selectedTemplate.Elective,
                                      AttendingDays = selectedTemplate.AttendingDays,
                                      AttendanceCap = selectedTemplate.AttendanceCap,
                                      DurationHours = selectedTemplate.DurationHours,
                                      DurationMins = selectedTemplate.DurationMins,
                                      Location = selectedTemplate.Location,
                                      Parish = selectedTemplate.Parish,
                                      Description = selectedTemplate.Description,
                                      StartDate = DateTime.Now,
                                      EndDate = DateTime.Now.AddMonths(1)
                                  };
                return PartialView("_CourseForm", preFill);
            }
            return null;
        }


        //
        // GET: /Course/

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title desc" : "";
            ViewBag.CreditsSortParm = sortOrder == "Credits" ? "Credits desc" : "Credits";
            ViewBag.ElectiveSortParm = sortOrder == "Elective" ? "Elective desc" : "Elective";
            ViewBag.InstructorSortParm = sortOrder == "Instructor" ? "Instructor desc" : "Instructor";
            ViewBag.YearSortParm = sortOrder == "Year" ? "Year desc" : "Year";
            ViewBag.AttendingDaysSortParm = sortOrder == "AttendingDays" ? "AttendingDays desc" : "AttendingDays";
            ViewBag.AttendanceCapSortParm = sortOrder == "AttendanceCap" ? "AttendanceCap desc" : "AttendanceCap";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate desc" : "StartDate";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "Location desc" : "Location";
            ViewBag.ParishSortParm = sortOrder == "Parish" ? "Parish desc" : "Parish";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description desc" : "Description";
            ViewBag.ApprovedSortPArm = sortOrder == "Approved" ? "Approved desc" : "Approved";
            ViewBag.CompletedSortPArm = sortOrder == "Completed" ? "Completed desc" : "Completed";
            ViewBag.ArchivedSortPArm = sortOrder == "Archived" ? "Archived desc" : "Archived";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            IQueryable<Course> courses = from s in db.Courses
                                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Title desc":
                    courses = courses.OrderByDescending(s => s.Title);
                    break;
                case "Credits":
                    courses = courses.OrderBy(s => s.Credits);
                    break;
                case "Credits desc":
                    courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "Elective":
                    courses = courses.OrderBy(s => s.Credits);
                    break;
                case "Elective desc":
                    courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "Instructor":
                    courses = courses.OrderBy(s => s.Instructor.LastName);
                    break;
                case "Instructor desc":
                    courses = courses.OrderByDescending(s => s.Instructor.LastName);
                    break;
                case "Year":
                    courses = courses.OrderBy(s => s.Year);
                    break;
                case "Year desc":
                    courses = courses.OrderByDescending(s => s.Year);
                    break;
                case "AttendingDays":
                    courses = courses.OrderBy(s => s.AttendingDays);
                    break;
                case "AttendingDays desc":
                    courses = courses.OrderByDescending(s => s.AttendingDays);
                    break;
                case "AttendanceCap":
                    courses = courses.OrderBy(s => s.AttendanceCap);
                    break;
                case "AttendanceCap desc":
                    courses = courses.OrderByDescending(s => s.AttendanceCap);
                    break;
                case "StartDate":
                    courses = courses.OrderBy(s => s.StartDate);
                    break;
                case "StartDate desc":
                    courses = courses.OrderByDescending(s => s.StartDate);
                    break;
                case "Location":
                    courses = courses.OrderBy(s => s.Location);
                    break;
                case "Location desc":
                    courses = courses.OrderByDescending(s => s.Location);
                    break;
                case "Parish":
                    courses = courses.OrderBy(s => s.Parish);
                    break;
                case "Parish desc":
                    courses = courses.OrderByDescending(s => s.Parish);
                    break;
                case "Description":
                    courses = courses.OrderBy(s => s.Description);
                    break;
                case "Description desc":
                    courses = courses.OrderByDescending(s => s.Description);
                    break;
                case "Approved":
                    courses = courses.OrderBy(s => s.Approved);
                    break;
                case "Approved desc":
                    courses = courses.OrderByDescending(s => s.Approved);
                    break;
                case "Completed":
                    courses = courses.OrderBy(s => s.Completed);
                    break;
                case "Completed desc":
                    courses = courses.OrderByDescending(s => s.Completed);
                    break;
                case "Archived":
                    courses = courses.OrderBy(s => s.Archived);
                    break;
                case "Archived desc":
                    courses = courses.OrderByDescending(s => s.Archived);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
            //}
        }

        //
        // GET: /Course/Details/5

        public ActionResult Details(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        //
        // POST: /Course/Details/5
        [HttpPost]
        public ActionResult ApplyUsingDetails(Course idGetter)
        {
            //try to join course
            Course course = db.Courses.Find(idGetter.CourseID);
            if (course.Approved && !course.Completed)
            {
                Student thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);

                Enrollment nullIfStudentDoesNotHaveThisCourse = db.Enrollments.FirstOrDefault(
                    p => p.StudentID == thisStudent.StudentID && p.CourseID == course.CourseID);

                if (nullIfStudentDoesNotHaveThisCourse == null)
                {
                    if (course.Enrollments.Count < course.AttendanceCap)
                    {
                        CreateEnrollmentAttendanceAndNotificationData(course, thisStudent);
                        return Content("You have been added to the class");
                    }
                    return Content("Class is full");
                }
                return Content("Student is already in class");
            }
            return Content("Course is not ready to be joined");
        }

        private void CreateEnrollmentAttendanceAndNotificationData(Course course, Student thisStudent)
        {
            BuildEnrollmentData(course, thisStudent);
            BuildAttendanceData(course, thisStudent);
            BuildNotificationData(course, thisStudent);
            db.SaveChanges();
        }

        private void BuildNotificationData(Course course, Student thisStudent)
        {
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "A Student by the name of " + thisStudent.FirstMidName + " " +
                                              thisStudent.LastName + " has signed up for " + course.Title,
                                          Link = Url.Action("Details", "Student", new {id = thisStudent.StudentID}),
                                          ViewableBy = "Admin",
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);
        }

        private void BuildAttendanceData(Course course, Student thisStudent)
        {
            for (int i = 0; i < course.AttendingDays; i++)
                // Adds attendance rows for every day needed in the attendance table
            {
                var newAttendance = new Attendance
                                        {
                                            CourseID = course.CourseID,
                                            StudentID = thisStudent.StudentID,
                                            AttendanceDay = i + 1,
                                            Present = false
                                        };
                db.Attendance.Add(newAttendance);
            }
        }

        private void BuildEnrollmentData(Course course, Student thisStudent)
        {
            var newEnrollment = new Enrollment
                                    {
                                        CourseID = course.CourseID,
                                        StudentID = thisStudent.StudentID,
                                        Grade = "incomplete"
                                    };
            db.Enrollments.Add(newEnrollment);
        }

        //
        // GET: /Course/ApplyToTeach

        public ActionResult ApplyToTeach()
        {
            DbSet<CourseTemplates> course = db.CourseTemplates;
            var model = new ApplyCourseViewModel
                            {
                                Courses = course.Select(x => new SelectListItem
                                                                 {
                                                                     Value = x.Title,
                                                                     Text = x.Title,
                                                                 }),
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now
                            };
            return View(model);
        }

        //
        // POST: /Course/ApplyToTeach

        [HttpPost]
        public ActionResult ApplyToTeach(ApplyCourseViewModel appliedCourse)
        {
            if (ModelState.IsValid)
            {
                Instructor instructor = db.Instructors.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);

                if (instructor == null)
                    // If the user is currently not an Instructor, make them one
                {
                    Student currentUser = db.Students.FirstOrDefault(
                        o => o.UserName == User.Identity.Name);
                    // "currentUser" needed to have user's information ( every user should have data in the Student table, 
                    // so their information is stored there)

                    // Creates a new Instructor in the Instructor Table
                    // using the User's information
                    CreatesInstructorDataAndAssignsRole(currentUser);
                }

                Instructor instructorAgain = db.Instructors.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                // Have to check the instructor again because if we just created 
                // the instructor there will be "null" in the "instructor" variable

                Course newCourse = BuildCourseData(appliedCourse, instructorAgain);
                db.SaveChanges();
                BuildNotificationData(appliedCourse, instructorAgain, newCourse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appliedCourse);
        }

        private void BuildNotificationData(ApplyCourseViewModel appliedCourse, Instructor instructorAgain,
                                           Course newCourse)
        {
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Instructor by the name of " + instructorAgain.LastName +
                                              " has applied to teach " + appliedCourse.Title,
                                          Link = Url.Action("Details", "Course", new {id = newCourse.CourseID}),
                                          ViewableBy = "Admin",
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);
        }

        private Course BuildCourseData(ApplyCourseViewModel appliedCourse, Instructor instructorAgain)
        {
            var newCourse = new Course
                                {
                                    Title = appliedCourse.Title,
                                    Credits = appliedCourse.Credits,
                                    Elective = appliedCourse.Elective,
                                    InstructorID = instructorAgain.InstructorID,
                                    Year = appliedCourse.StartDate.Year,
                                    AttendingDays = appliedCourse.AttendingDays,
                                    AttendanceCap = appliedCourse.AttendanceCap,
                                    StartDate = appliedCourse.StartDate,
                                    EndDate = appliedCourse.EndDate,
                                    DurationHours = appliedCourse.DurationHours,
                                    DurationMins = appliedCourse.DurationMins,
                                    Location = appliedCourse.Location,
                                    Parish = appliedCourse.Parish,
                                    Description = appliedCourse.Description,
                                    Approved = false,
                                    Completed = false,
                                    Archived = false
                                };
            db.Courses.Add(newCourse);
            return newCourse;
        }

        private void CreatesInstructorDataAndAssignsRole(Student currentUser)
        {
            var newInstructor = new Instructor
                                    {
                                        UserName = User.Identity.Name,
                                        EnrollmentDate = DateTime.Now,
                                        LastName = currentUser.LastName,
                                        FirstMidName = currentUser.FirstMidName,
                                        Email = currentUser.Email
                                    };
            db.Instructors.Add(newInstructor);
            db.SaveChanges();
            if (!User.IsInRole("Instructor"))
            {
                Roles.AddUserToRole(User.Identity.Name, "Instructor");
            }
        }


        //
        // GET: /Course/TeachingCourseList

        public ViewResult TeachingCourseList()
        {
            Instructor thisInstructor = db.Instructors.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            return View(thisInstructor);
        }


        //
        // GET: /Course/UsersCourseList

        public ViewResult UsersCourseList()
        {
            Student thisStudent = db.Students.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            /*ViewBag.cores = foreach(var completedcourse in thisStudent.Enrollments
            ViewBag.electives = 2;*/
            // This section being worked on.  Deals with displaying total core and elective credits
            return View(thisStudent);
        }

        //
        // GET: /Course/ApplyToCourse

        public ViewResult ApplyToCourse(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title desc" : "";
            ViewBag.StartDateSortParm = sortOrder == "StartDate" ? "StartDate desc" : "StartDate";
            ViewBag.CreditsSortParm = sortOrder == "Credits" ? "Credits desc" : "Credits";
            ViewBag.InstructorSortParm = sortOrder == "Instructor" ? "Instructor desc" : "Instructor";
            ViewBag.AttendingDaysSortParm = sortOrder == "AttendingDays" ? "AttendingDays desc" : "AttendingDays";
            ViewBag.AttendanceCapSortParm = sortOrder == "AttendanceCap" ? "AttendanceCap desc" : "AttendanceCap";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "Location desc" : "Location";
            ViewBag.ParishSortParm = sortOrder == "Parish" ? "Parish desc" : "Parish";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            IQueryable<Course> courses = db.Courses.Where(s => s.Approved); // used to read s=> s.approved == true

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Title.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Title desc":
                    courses = courses.OrderByDescending(s => s.Title);
                    break;
                case "StartDate":
                    courses = courses.OrderBy(s => s.StartDate);
                    break;
                case "StartDate desc":
                    courses = courses.OrderByDescending(s => s.StartDate);
                    break;
                case "Credits":
                    courses = courses.OrderBy(s => s.Credits);
                    break;
                case "Credits desc":
                    courses = courses.OrderByDescending(s => s.Credits);
                    break;
                case "Instructor":
                    courses = courses.OrderBy(s => s.Instructor.LastName);
                    break;
                case "Instructor desc":
                    courses = courses.OrderByDescending(s => s.Instructor.LastName);
                    break;
                case "AttendingDays":
                    courses = courses.OrderBy(s => s.AttendingDays);
                    break;
                case "AttendingDays desc":
                    courses = courses.OrderByDescending(s => s.AttendingDays);
                    break;
                case "AttendanceCap":
                    courses = courses.OrderBy(s => s.AttendanceCap);
                    break;
                case "AttendanceCap desc":
                    courses = courses.OrderByDescending(s => s.AttendanceCap);
                    break;
                case "Location":
                    courses = courses.OrderBy(s => s.Location);
                    break;
                case "Location desc":
                    courses = courses.OrderByDescending(s => s.Location);
                    break;
                case "Parish":
                    courses = courses.OrderBy(s => s.Parish);
                    break;
                case "Parish desc":
                    courses = courses.OrderByDescending(s => s.Parish);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET /Course/Error
        public ViewResult Error(string message)
        {
            ViewBag.error = message;
            return View();
        }

        //
        // GET /Course/ApproveCourse/5
        public ActionResult ApproveCourse(int id)
        {
            Course course = db.Courses.Find(id);
            course.Approved = true;

            // Add the notification for the Instructor stating that their Course has been approved
            BuildNotificationData(course);
            db.SaveChanges();
            return RedirectToAction("Details", new {id = course.CourseID});
        }

        private void BuildNotificationData(Course course)
        {
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Administrator has approved your application to teach " + course.Title +
                                              " beginning " + course.StartDate,
                                          Link = Url.Action("AttendanceView", "Attendance", new {id = course.CourseID}),
                                          ViewableBy = course.Instructor.UserName,
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);
        }

        //
        // POST /Course/RemoveFromCourse/5
        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult RemoveFromCourse(int id)
        {
            Enrollment enrollment = RemoveEnrollmentAndAttendanceData(id);
            db.SaveChanges();
            return RedirectToAction("Details", new {id = enrollment.CourseID});
        }

        private Enrollment RemoveEnrollmentAndAttendanceData(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            IQueryable<Attendance> attendanceItems = db.Attendance.Where(s => s.CourseID == enrollment.CourseID &&
                                                                              s.StudentID == enrollment.StudentID);
            foreach (Attendance item in attendanceItems)
            {
                db.Attendance.Remove(item);
            }
            return enrollment;
        }

        //
        // Get /Course/EditClass/5

        public ActionResult EditCourse(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        //
        // POST: /Course/EditClass/5

        [HttpPost]
        public ActionResult EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;

                // Add a notification for the Instructor to see that the Course was modified
                var newNotification = new Notification
                                          {
                                              Time = DateTime.Now,
                                              Details =
                                                  "An Admin has edited the course you applied to teach: " + course.Title +
                                                  " beginning " + course.StartDate,
                                              Link = Url.Action("Details", "Course", new {id = course.CourseID}),
                                              ViewableBy = course.Instructor.UserName,
                                              Complete = false
                                          };
                db.Notification.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        //
        // GET: /Course/Delete/5

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Delete(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, Course hasReasonForDeletion)
        {
            Course course = db.Courses.Find(id);

            // Add the notification for the Instructor that their Course has been approved
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Administrator has denied your application to teach " + course.Title +
                                              " citing the following reason: " + hasReasonForDeletion.AdminDenialReason,
                                          Link = Url.Action("ApplyToTeach", "Course", new {id = course.CourseID}),
                                          ViewableBy = course.Instructor.UserName,
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);

            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}