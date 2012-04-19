using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using PagedList;
using PaulSchool.ViewModels;
using System.Web.Security;

namespace PaulSchool.Controllers
{
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

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title desc" : "";
            ViewBag.CreditsSortParm = sortOrder == "Credits" ? "Credits desc" : "Credits";
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
        }

        //
        // GET: /Course/Details/5

        public ViewResult Details(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
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
                    Roles.AddUserToRole(User.Identity.Name, "Instructor");
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
                return RedirectToAction("Index");
            }
            return View(appliedCourse);
        }

    }
}
