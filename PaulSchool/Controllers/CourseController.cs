namespace PaulSchool.Controllers
{
    #region

    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using PagedList;

    using PaulSchool.Models;
    using PaulSchool.ViewModels;

    #endregion

    [Authorize]
    public class CourseController : Controller
    {
        #region Fields

        private readonly SchoolContext db = new SchoolContext();

        #endregion

        #region Public Methods and Operators

        public ViewResult ApplyToCourse(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "Title desc" : string.Empty;
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

            IQueryable<Course> courses = db.Courses.Where(s => s.Approved && s.Completed == false);

            if (!string.IsNullOrEmpty(searchString))
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

            int pageSize = 10;
            int pageNumber = page ?? 1;

            // Find out how many seats are available
            foreach (Course course in courses)
            {
                int count = this.db.Enrollments.Count(o => o.CourseID == course.CourseID);
                int seatsLeft = course.AttendanceCap - count;
                ViewBag.course = seatsLeft;
            }

            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Administrator, SuperAdministrator, Instructor")]
        public ActionResult ApplyToTeach()
        {
            DbSet<CourseTemplates> course = db.CourseTemplates;
            var model = new ApplyCourseViewModel
                {
                    Courses = course.Select(x => new SelectListItem { Value = x.Title, Text = x.Title, }), 
                    StartDate = DateTime.Now, 
                    EndDate = DateTime.Now
                };
            return View(model);
        }

        [HttpPost]
        public ActionResult ApplyToTeach(ApplyCourseViewModel appliedCourse)
        {
            if (ModelState.IsValid)
            {
                Debug.Write(appliedCourse.Cost);
                Instructor instructor = db.Instructors.FirstOrDefault(o => o.UserName == User.Identity.Name);

                if (instructor == null)
                {
                    // If the user is currently not an Instructor, make them one
                    Student currentUser = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);

                    // "currentUser" needed to have user's information ( every user should have data in the Student table, 
                    // so their information is stored there)

                    // Creates a new Instructor in the Instructor Table
                    // using the User's information
                    CreatesInstructorDataAndAssignsRole(currentUser);
                }

                Instructor instructorAgain = db.Instructors.FirstOrDefault(o => o.UserName == User.Identity.Name);

                // Have to check the instructor again because if we just created 
                // the instructor there will be "null" in the "instructor" variable
                Course newCourse = BuildCourseData(appliedCourse, instructorAgain);
                Debug.Write(appliedCourse.Cost);
                db.SaveChanges();
                BuildNotificationData(appliedCourse, instructorAgain, newCourse);
                db.SaveChanges();
                TempData["tempMessage"] =
                    "You have successfully applied to teach this course.  A notification has been sent to an administrator so they can review the application.  You will receive a notification when an administrator takes action on your application to teach.";
                return RedirectToAction("Details", new { id = newCourse.CourseID });
            }

            return View("Error");
        }

        [HttpPost]
        public ActionResult ApplyUsingDetails(Course idGetter)
        {
            // try to join course
            Course course = db.Courses.Find(idGetter.CourseID);
            if (course.Approved && !course.Completed)
            {
                Student thisStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);

                Enrollment nullIfStudentDoesNotHaveThisCourse =
                    db.Enrollments.FirstOrDefault(
                        p => p.StudentID == thisStudent.StudentID && p.CourseID == course.CourseID);

                if (nullIfStudentDoesNotHaveThisCourse == null)
                {
                    if (course.Enrollments.Count < course.AttendanceCap)
                    {
                        CreateEnrollmentAttendanceAndNotificationData(course, thisStudent);
                        TempData["tempMessage"] =
                            "You have successfully enrolled in this Course. (Please note that the below details are default values until your Instructor updates them)";
                        return RedirectToAction("StudentDetails", "Attendance", new { id = course.CourseID });
                    }

                    return RedirectToAction("Message", new { message = "This course is full." });
                }

                return RedirectToAction("Message", new { message = "This student is already enrolled in this course." });
            }

            return RedirectToAction(
                "Message", 
                new
                    {
                        message =
                            "Course is not ready to be joined, it has not been approved by an Administrator or has already concluded."
                    });
        }

        public ActionResult ApproveCourse(int id)
        {
            Course course = db.Courses.Find(id);
            course.Approved = true;

            BuildNotificationData(course);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = course.CourseID });
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Delete(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult DeleteConfirmed(int id, Course hasReasonForDeletion)
        {
            Course course = db.Courses.Find(id);

            // Add the notification for the Instructor that their Course has been unapproved
            var newNotification = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "An Administrator has denied your application to teach " + course.Title
                        + " citing the following reason: " + hasReasonForDeletion.AdminDenialReason, 
                    Link = Url.Action("ApplyToTeach", "Course", new { id = course.CourseID }), 
                    ViewableBy = course.Instructor.UserName, 
                    Complete = false
                };
            db.Notification.Add(newNotification);

            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index", "Notification");
        }

        public ActionResult Details(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        public ActionResult EditCourse(int id)
        {
            Course course = db.Courses.Find(id);
            return View(course);
        }

        [HttpPost]
        public ActionResult EditCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;

                Instructor thisInstructor = db.Instructors.FirstOrDefault(o => o.InstructorID == course.InstructorID);

                // Add a notification for the Instructor to see that the Course was modified
                var newNotification = new Notification
                    {
                        Time = DateTime.Now, 
                        Details =
                            "An Admin has edited the course you applied to teach: " + course.Title + " beginning "
                            + course.StartDate, 
                        Link = Url.Action("Details", "Course", new { id = course.CourseID }), 
                        // ViewableBy = course.Instructor.UserName,
                        ViewableBy = thisInstructor.UserName, 
                        Complete = false
                    };
                db.Notification.Add(newNotification);
                db.SaveChanges();
                TempData["tempMessage"] =
                    "You have successfully applied edited this course.  A notification has been sent to the instructor who applied to teach this course.";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult FullCourseList(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "Title desc" : string.Empty;
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
            ViewBag.CostSortParm = sortOrder == "Cost" ? "Cost desc" : "Cost";
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

            IQueryable<Course> courses = from s in db.Courses select s;
            if (!string.IsNullOrEmpty(searchString))
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
                case "Cost":
                    courses = courses.OrderBy(s => s.Cost);
                    break;
                case "Cost desc":
                    courses = courses.OrderByDescending(s => s.Cost);
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

            int pageSize = 10;
            int pageNumber = page ?? 1;
            return View(courses.ToPagedList(pageNumber, pageSize));

            // }
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "Title desc" : string.Empty;
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
            ViewBag.CostSortParm = sortOrder == "Cost" ? "Cost desc" : "Cost";
            ViewBag.ApprovedSortPArm = sortOrder == "Approved" ? "Approved desc" : "Approved";
            ViewBag.CompletedSortPArm = sortOrder == "Completed" ? "Completed desc" : "Completed";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            ViewBag.CurrentFilter = searchString;

            IQueryable<Course> courses = db.Courses.Where(s => s.Archived == false);

            if (!string.IsNullOrEmpty(searchString))
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
                case "Cost":
                    courses = courses.OrderBy(s => s.Cost);
                    break;
                case "Cost desc":
                    courses = courses.OrderByDescending(s => s.Cost);
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
                default:
                    courses = courses.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = page ?? 1;
            return View(courses.ToPagedList(pageNumber, pageSize));

            // }
        }

        public ViewResult Message(string message)
        {
            ViewBag.message = message;
            return View();
        }

        public PartialViewResult PreFillCourse(string selectedCourse)
        {
            CourseTemplates selectedTemplate = db.CourseTemplates.FirstOrDefault(o => o.Title == selectedCourse);

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
                        Cost = selectedTemplate.Cost, 
                        StartDate = DateTime.Now, 
                        EndDate = DateTime.Now.AddMonths(1)
                    };
                return PartialView("_CourseForm", preFill);
            }

            return null;
        }

        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult RemoveFromCourse(int id)
        {
            Enrollment enrollment = RemoveEnrollmentAndAttendanceData(id);
            db.SaveChanges();
            TempData["tempMessage"] =
                    "You have successfully removed " + enrollment.Student.FirstMidName + " " + enrollment.Student.LastName + " from this course.";
            return RedirectToAction("Details", new { id = enrollment.CourseID });
        }

        public ViewResult TeachingCourseList()
        {
            Instructor thisInstructor = db.Instructors.FirstOrDefault(o => o.UserName == User.Identity.Name);
            return View(thisInstructor);
        }

        public ViewResult UsersCourseList()
        {
            Student thisStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);

            int totalCoresPassed =
                db.Enrollments.Count(
                    s => s.StudentID == thisStudent.StudentID && s.Grade == "pass" && s.Course.Elective == false && s.Course.Title != "Day of Reflection");

            int totalElectivesPassed =
                db.Enrollments.Count(
                    s => s.StudentID == thisStudent.StudentID && s.Grade == "pass" && s.Course.Elective);

            var dayOfReflection = db.Enrollments.FirstOrDefault(s => s.StudentID == thisStudent.StudentID && s.Course.Title == "Day of Reflection" && s.Grade == "pass");
            ViewBag.coresPassed = totalCoresPassed;
            ViewBag.electivesPassed = totalElectivesPassed;
            ViewBag.completedDayOfReflection = "You have not completed the Commissioning requirement: Day of Reflection";
            if (dayOfReflection != null)
            {
                ViewBag.completedDayOfReflection = "You have completed the Commissioning requirement: Day of Reflection";
            }
            Enrollment nullIfAllCourseFeesArePaid = thisStudent.Enrollments.FirstOrDefault(o => o.Paid == false);

            if (nullIfAllCourseFeesArePaid != null)
            {
                TempData["paymentMessage"] =
                    "You have enrolled in at least one course with an application fee, but have not paid the fee.  Please view the details of your courses and pay online with Paypal or contact an Administrator to pay using another method.";
            }

            return View(thisStudent);
        }

        #endregion

        #region Methods

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void BuildAttendanceData(Course course, Student thisStudent)
        {
            for (int i = 0; i < course.AttendingDays; i++)
            {
                // Adds attendance rows for every day needed in the attendance table
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

        private Course BuildCourseData(ApplyCourseViewModel appliedCourse, Instructor instructorAgain)
        {
            Debug.Write(appliedCourse.Cost);
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
                    Cost = appliedCourse.Cost, 
                    Approved = false, 
                    Completed = false, 
                    Archived = false
                };
            db.Courses.Add(newCourse);
            return newCourse;
        }

        private void BuildEnrollmentData(Course course, Student thisStudent)
        {
            bool hasPaid = course.Cost == 0;
            var newEnrollment = new Enrollment
                {
                    CourseID = course.CourseID, 
                    StudentID = thisStudent.StudentID, 
                    Grade = "incomplete", 
                    Paid = hasPaid
                };
            db.Enrollments.Add(newEnrollment);
        }

        private void BuildNotificationData(Course course, Student thisStudent)
        {
            var newNotification = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "A Student by the name of " + thisStudent.FirstMidName + " " + thisStudent.LastName
                        + " has signed up for " + course.Title, 
                    Link = Url.Action("Details", "Student", new { id = thisStudent.StudentID }), 
                    ViewableBy = "Admin", 
                    Complete = false
                };
            db.Notification.Add(newNotification);
        }

        private void BuildNotificationData(
            ApplyCourseViewModel appliedCourse, Instructor instructorAgain, Course newCourse)
        {
            var newNotification = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "An Instructor by the name of " + instructorAgain.LastName + " has applied to teach "
                        + appliedCourse.Title, 
                    Link = Url.Action("Details", "Course", new { id = newCourse.CourseID }), 
                    ViewableBy = "Admin", 
                    Complete = false
                };
            db.Notification.Add(newNotification);
        }

        private void BuildNotificationData(Course course)
        {
            var newNotification = new Notification
                {
                    Time = DateTime.Now, 
                    Details =
                        "An Administrator has approved your application to teach " + course.Title + " beginning "
                        + course.StartDate, 
                    Link = Url.Action("AttendanceView", "Attendance", new { id = course.CourseID }), 
                    ViewableBy = course.Instructor.UserName, 
                    Complete = false
                };
            db.Notification.Add(newNotification);
        }

        private void CreateEnrollmentAttendanceAndNotificationData(Course course, Student thisStudent)
        {
            course.SeatsTaken = course.SeatsTaken + 1;
            BuildEnrollmentData(course, thisStudent);
            BuildAttendanceData(course, thisStudent);
            BuildNotificationData(course, thisStudent);
            db.SaveChanges();
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

        private Enrollment RemoveEnrollmentAndAttendanceData(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            Course course = db.Courses.First(o => o.CourseID == enrollment.CourseID);
            course.SeatsTaken = course.SeatsTaken - 1;
            db.Enrollments.Remove(enrollment);
            IQueryable<Attendance> attendanceItems =
                db.Attendance.Where(s => s.CourseID == enrollment.CourseID && s.StudentID == enrollment.StudentID);
            foreach (Attendance item in attendanceItems)
            {
                db.Attendance.Remove(item);
            }

            return enrollment;
        }

        #endregion

        public ActionResult MarkAsPaid(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            enrollment.Paid = true;
            db.SaveChanges();
            TempData["tempMessage"] =
                    "You have successfully marked " + enrollment.Student.FirstMidName + " " + enrollment.Student.LastName + "'s fee for this course as: Paid";
            return RedirectToAction("Details", new {id = enrollment.CourseID});
        }
    }
}