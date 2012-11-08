using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PagedList;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    [Authorize(Roles = "Administrator, SuperAdministrator, Instructor")]
    public class StudentController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName desc" : "FName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email desc" : "Email";
            ViewBag.CitySortParm = sortOrder == "City" ? "City desc" : "City";
            ViewBag.ZipCodeSortParm = sortOrder == "ZipCode" ? "ZipCode desc" : "ZipCode";
            ViewBag.ParishAffiliationSortParm = sortOrder == "ParishAffiliation" ? "ParishAffiliation desc" : "ParishAffiliation";
            ViewBag.MinistryInvolvementSortParm = sortOrder == "MinistryInvolvement" ? "MinistryInvolvement desc" : "MinistryInvolvement";
            ViewBag.UserNameSortParm = sortOrder == "UserName" ? "UserName desc" : "UserName";
            ViewBag.EnrollmentSortParm = sortOrder == "EnrollmentDate" ? "EnrollmentDate desc" : "EnrollmentDate";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            IQueryable<Student> students = from s in db.Students
                                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                               || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            //            city, zipcode, ministryinvolvement, parishaffiliation, username, enrollmentdate
            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "FName":
                    students = students.OrderBy(s => s.FirstMidName);
                    break;
                case "FName desc":
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Email":
                    students = students.OrderBy(s => s.Email);
                    break;
                case "Email desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                case "City":
                    students = students.OrderBy(s => s.City);
                    break;
                case "City desc":
                    students = students.OrderByDescending(s => s.City);
                    break;
                case "ZipCode":
                    students = students.OrderBy(s => s.ZipCode);
                    break;
                case "ZipCode desc":
                    students = students.OrderByDescending(s => s.ZipCode);
                    break;
                case "ParishAffiliation":
                    students = students.OrderBy(s => s.ParishAffiliation);
                    break;
                case "ParishAffiliation desc":
                    students = students.OrderByDescending(s => s.ParishAffiliation);
                    break;
                case "MinistryInvolvement":
                    students = students.OrderBy(s => s.MinistryInvolvement);
                    break;
                case "MinistryInvolvement desc":
                    students = students.OrderByDescending(s => s.MinistryInvolvement);
                    break;
                case "UserName":
                    students = students.OrderBy(s => s.UserName);
                    break;
                case "UserName desc":
                    students = students.OrderByDescending(s => s.UserName);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult StudentListWithAllDetails(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.FNameSortParm = sortOrder == "FName" ? "FName desc" : "FName";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email desc" : "Email";
            ViewBag.UserNameSortParm = sortOrder == "City" ? "City desc" : "City";
            ViewBag.UserNameSortParm = sortOrder == "ZipCode" ? "ZipCode desc" : "ZipCode";
            ViewBag.UserNameSortParm = sortOrder == "ParishAffiliation" ? "ParishAffiliation desc" : "ParishAffiliation";
            ViewBag.UserNameSortParm = sortOrder == "MinistryInvolvement" ? "MinistryInvolvement desc" : "MinistryInvolvement";
            ViewBag.UserNameSortParm = sortOrder == "UserName" ? "UserName desc" : "UserName";
            ViewBag.UserNameSortParm = sortOrder == "EnrollmentDate" ? "EnrollmentDate desc" : "EnrollmentDate";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            IQueryable<Student> students = from s in db.Students
                                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                               || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            //            city, zipcode, ministryinvolvement, parishaffiliation, username, enrollmentdate
            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                case "FName":
                    students = students.OrderBy(s => s.FirstMidName);
                    break;
                case "FName desc":
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Email":
                    students = students.OrderBy(s => s.Email);
                    break;
                case "Email desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                case "City":
                    students = students.OrderBy(s => s.City);
                    break;
                case "City desc":
                    students = students.OrderByDescending(s => s.City);
                    break;
                case "ZipCode":
                    students = students.OrderBy(s => s.ZipCode);
                    break;
                case "ZipCode desc":
                    students = students.OrderByDescending(s => s.ZipCode);
                    break;
                case "ParishAffiliation":
                    students = students.OrderBy(s => s.ParishAffiliation);
                    break;
                case "ParishAffiliation desc":
                    students = students.OrderByDescending(s => s.ParishAffiliation);
                    break;
                case "MinistryInvolvement":
                    students = students.OrderBy(s => s.MinistryInvolvement);
                    break;
                case "MinistryInvolvement desc":
                    students = students.OrderByDescending(s => s.MinistryInvolvement);
                    break;
                case "UserName":
                    students = students.OrderBy(s => s.UserName);
                    break;
                case "UserName desc":
                    students = students.OrderByDescending(s => s.UserName);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 25;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult Details(int id)
        {
            Student student = db.Students.Find(id);
            //ViewBag.testCompletedCoreCourses = (from o in student where o.enrollment)

            int totalCoresPassed = db.Enrollments.Count(s => s.StudentID == id
                                                             && s.Grade == "pass"
                                                             && s.Course.Elective == false);

            int totalElectivesPassed = db.Enrollments.Count(s => s.StudentID == id
                                                                 && s.Grade == "pass"
                                                                 && s.Course.Elective);

            int totalCoresNeeded = db.CommissioningRequirementse.Find(1).CoreCoursesRequired;
            int totalElectivesNeeded = db.CommissioningRequirementse.Find(1).ElectiveCoursesRequired;

            ViewBag.coresPassed = totalCoresPassed;
            ViewBag.electivesPassed = totalElectivesPassed;
            ViewBag.coresNeeded = totalCoresNeeded;
            ViewBag.electivesNeeded = totalElectivesNeeded;

            ViewBag.testCompletedElectiveCourses = 1;
            return View(student);
        }

        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                MembershipUserCollection users = Membership.GetAllUsers();
                var model = new CreateStudentViewModel
                                {
                                    Users = users.OfType<MembershipUser>().Select(x => new SelectListItem
                                                                                           {
                                                                                               Value = x.UserName,
                                                                                               Text = x.UserName,
                                                                                           })
                                };
                return View(model);
            }
            if (User.IsInRole("Default"))
            {
                MembershipUserCollection users = Membership.FindUsersByName(User.Identity.Name);
                var model = new CreateStudentViewModel
                                {
                                    Users = users.OfType<MembershipUser>().Select(x => new SelectListItem
                                                                                           {
                                                                                               Value = x.UserName,
                                                                                               Text = x.UserName,
                                                                                           })
                                };
                return View(model);
            }
            return View( /*user has a role that is not "Default" or "Administrator"  Needs error message*/);
        }

        [HttpPost]
        public ActionResult Create(CreateStudentViewModel studentModel, string selectedUser, string lastName,
                                   string firstMidName, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var student = new Student();

                    //Establish the student data
                    student.UserName = selectedUser;
                    student.LastName = lastName;
                    student.FirstMidName = firstMidName;
                    student.Email = email;
                    student.EnrollmentDate = DateTime.Now;

                    db.Students.Add(student); //inputs student data into database (is not saved yet)
                    db.SaveChanges(); //saves the student to database

                    MembershipUser user = Membership.GetUser(student.UserName); //gets the actual user
                    Roles.AddUserToRole(user.UserName, "Student"); //takes the user and sets role to student

                    // assigns Student data to the profile of the user (so the user is associated with this specified Student data)
                    CustomProfile profile = CustomProfile.GetUserProfile(student.UserName);
                    profile.FilledStudentInfo = "yes";
                    profile.Save();

                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("",
                                         "Saving failed for some reason.  You may have left some information blank.  Please try again (several times in several different ways if possible (i.e. try using a different computer) - if the problem persists see your system administrator.");
            }

            // This code block is here to allow the page to render in case we get a DataException and have to re-display the screen.
            MembershipUserCollection users = Membership.GetAllUsers();
            var model = new CreateStudentViewModel
                            {
                                Users = users.OfType<MembershipUser>().Select(x => new SelectListItem
                                                                                       {
                                                                                           Value = x.UserName,
                                                                                           Text = x.UserName,
                                                                                       })
                            };
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("",
                                         "Saving failed for some reason. Please try again (several times in several different ways if possible (i.e. try using a different computer) - if the problem persists see your system administrator.");
            }
            return View(student);
        }


        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage =
                    "Saving failed for some reason. Please try again (several times in several different ways if possible (i.e. try using a different computer) - if the problem persists see your system administrator.";
            }
            return View(db.Students.Find(id));
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("Delete",
                                        new RouteValueDictionary
                                            {
                                                {"id", id},
                                                {"saveChangesError", true}
                                            });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult MakeStudent()
        {
            ViewBag.Message = "You just made this account into a student";

            return View();
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult SendStudentAnEmail(string emailAddress)
        {
            var emailViewModel = new EmailViewModel
                                     {
                                         Email = emailAddress,
                                         Subject = "Put Subject Here",
                                         Body = "Put Body Here"
                                     };
            return View(emailViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult SendStudentAnEmail(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                AccountMembershipService.SendCustomEmail(model);
                return RedirectToAction("Message", "Course", new {message = "Your email has been sent."});
            }
            return View("Error");
        }
    }
}