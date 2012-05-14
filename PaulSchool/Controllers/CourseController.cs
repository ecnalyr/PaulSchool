using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using PagedList;
using PaulSchool.ViewModels;
using System.Web.Security;
using System.Diagnostics;

namespace PaulSchool.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        public PartialViewResult PreFillCourse(string selectedCourse) 
            // Note that this is searching by title only - there cannot be more than one class with the same title
        {
            CourseTemplates selectedTemplate = db.CourseTemplates.FirstOrDefault(
                                                o => o.Title == selectedCourse);

            ViewBag.selectedString = selectedCourse;
            ApplyCourseViewModel preFill = new ApplyCourseViewModel
            {
                Title = selectedTemplate.Title,
                Credits = selectedTemplate.Credits,
                Elective = selectedTemplate.Elective,
                AttendingDays = selectedTemplate.AttendingDays,
                AttendanceCap = selectedTemplate.AttendanceCap,
                Location = selectedTemplate.Location,
                Parish = selectedTemplate.Parish,
                Description = selectedTemplate.Description,
                StartDate = DateTime.Now
            };
            return PartialView("_CourseForm", preFill);
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

            var courses = from s in db.Courses
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
            int id = course.CourseID;
            if (course.Approved && !course.Completed)
            {
                Student thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);

                Enrollment nullIfStudentDoesNotHaveThisCourse = db.Enrollments.FirstOrDefault(
                    p => p.StudentID == thisStudent.StudentID && p.CourseID == course.CourseID );

                if ( nullIfStudentDoesNotHaveThisCourse == null)
                {
                    if ( course.Enrollments.Count < course.AttendanceCap )
                    {
                        Enrollment newEnrollment = new Enrollment
                        {
                            CourseID = course.CourseID,
                            StudentID = thisStudent.StudentID,
                            Grade = "incomplete"
                        };

                        for (int i = 0; i < course.AttendingDays; i++)
                        // Adds attendance rows for every day needed in the attendance table
                        {
                            Attendance newAttendance = new Attendance
                            {
                                CourseID = course.CourseID,
                                StudentID = thisStudent.StudentID,
                                AttendanceDay = i+1,
                                Present = false
                            };
                            db.Attendance.Add(newAttendance);
                        }

                        db.Enrollments.Add(newEnrollment);
                        db.SaveChanges();

                        Notification newNotification = new Notification
                        {
                            Time = DateTime.Now,
                            Details = "A Student by the name of " + thisStudent.FirstMidName + " " + thisStudent.LastName + " has signed up for " + course.Title,
                            Link = Url.Action("Details", "Student", new { id = thisStudent.StudentID }),
                            ViewableBy = "Admin",
                            Complete = false
                        };
                        db.Notification.Add(newNotification);
                        db.SaveChanges();


                        return Content("You have been added to the class");


                        //allow student to join class
                        //return Content("Student is allowed to join class");
                        //return RedirectToAction("Error", "Course", message);
                    }
                    else
                    {
                        // Class is full
                        return Content("Class is full");
                    }
                }
                else
                {
                    //  Student is already in class
                    return Content("Student is already in class");
                }
            }
            else
            {
                // Course is either not approved to begin by administrator or has already been completed
                return Content("Course is not ready to be joined");
            }
        }

        //
        // GET: /Course/ApplyToTeach

        public ActionResult ApplyToTeach()
        {
            var course = db.CourseTemplates;
            var model = new ApplyCourseViewModel
            {
                Courses = course.Select(x => new SelectListItem
                {
                    Value = x.Title,
                    Text = x.Title,
                }),
                StartDate = DateTime.Now
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

                    Instructor newInstructor = new Instructor
                    // Creates a new Instructor in the Instructor Table
                    // using the User's information
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
                    // sets the actual role of the user to Instructor
                }

                Instructor instructorAgain = db.Instructors.FirstOrDefault(
                 o => o.UserName == User.Identity.Name);
                // Have to check the instructor again because if we just created 
                // the instructor there will be "null" in the "instructor" variable
                Course newCourse = new Course
                {
                    Title = appliedCourse.Title,
                    Credits = appliedCourse.Credits,
                    Elective = appliedCourse.Elective,
                    InstructorID = instructorAgain.InstructorID, 
                    Year = appliedCourse.StartDate.Year,
                    AttendingDays = appliedCourse.AttendingDays,
                    AttendanceCap = appliedCourse.AttendanceCap,
                    StartDate = appliedCourse.StartDate,
                    Location = appliedCourse.Location,
                    Parish = appliedCourse.Parish,
                    Description = appliedCourse.Description,
                    Approved = false,
                    Completed = false,
                    Archived = false
                };
                db.Courses.Add(newCourse);
                db.SaveChanges();

                // Add the notification for the Admin that someone has applied to teach a Course
                
                Notification newNotification = new Notification
                {
                    Time = DateTime.Now,
                    Details = "An Instructor by the name of "+instructorAgain.LastName+" has applied to teach "+appliedCourse.Title,
                    Link = Url.Action("Details", "Course", new { id = newCourse.CourseID }),
                    ViewableBy = "Admin",
                    Complete = false
                };
                db.Notification.Add(newNotification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appliedCourse);
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
            ViewBag.cores = foreach(var completedcourse in thisStudent.Enrollments
            ViewBag.electives = 2;
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

            var courses = db.Courses.Where(s => s.Approved== true);

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
        public ViewResult Error (string message)
        {
            ViewBag.error = message;
            return View();
        }

        //
        // GET /Course/ApproveClass/5
        public ActionResult ApproveClass(int id)
        {
            Course course = db.Courses.Find(id);
            course.Approved = true;
            db.SaveChanges();
            return RedirectToAction("Details", new { id=course.CourseID });
        }
    }
}
