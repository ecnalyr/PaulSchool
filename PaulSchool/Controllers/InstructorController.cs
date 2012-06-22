using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{
    //[Authorize(Roles = "Administrator, SuperAdministrator, Instructor")]
    public class InstructorController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        //
        // GET: /Instructor/

        [Authorize(Roles = "Administrator, SuperAdministrator, Instructor")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("SuperAdministrator"))
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
                ViewBag.FNameSortParm = sortOrder == "FName" ? "FName desc" : "FName";
                ViewBag.EmailSortParm = sortOrder == "Email" ? "Email desc" : "Email";
                ViewBag.UserNameSortParm = sortOrder == "UserName" ? "UserName desc" : "UserName";

                if (Request.HttpMethod == "GET")
                {
                    searchString = currentFilter;
                }
                else
                {
                    page = 1;
                }
                ViewBag.CurrentFilter = searchString;

                IQueryable<Instructor> instructors = from s in db.Instructors
                                                     select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    instructors = instructors.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                                         || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
                }
                switch (sortOrder)
                {
                    case "Name desc":
                        instructors = instructors.OrderByDescending(s => s.LastName);
                        break;
                    case "Date":
                        instructors = instructors.OrderBy(s => s.EnrollmentDate);
                        break;
                    case "Date desc":
                        instructors = instructors.OrderByDescending(s => s.EnrollmentDate);
                        break;
                    case "FName":
                        instructors = instructors.OrderBy(s => s.FirstMidName);
                        break;
                    case "FName desc":
                        instructors = instructors.OrderByDescending(s => s.FirstMidName);
                        break;
                    case "Email":
                        instructors = instructors.OrderBy(s => s.Email);
                        break;
                    case "Email desc":
                        instructors = instructors.OrderByDescending(s => s.Email);
                        break;
                    case "UserName":
                        instructors = instructors.OrderBy(s => s.UserName);
                        break;
                    case "UserName desc":
                        instructors = instructors.OrderByDescending(s => s.UserName);
                        break;
                    default:
                        instructors = instructors.OrderBy(s => s.LastName);
                        break;
                }
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                return View(instructors.ToPagedList(pageNumber, pageSize));
            }

            if (User.IsInRole("Instructor"))
            {
                Instructor instructor = db.Instructors.FirstOrDefault(
                    n => n.UserName == User.Identity.Name);

                if (instructor != null)
                {
                    int id = instructor.InstructorID;

                    Course testCourse = db.Courses.FirstOrDefault(
                        o => o.InstructorID == id &&
                             o.Approved);
                    if (testCourse == null)
                    {
                        //This is not an instructor with an approved course
                        ViewBag.FailAllCheck = true;
                        ViewBag.Fail = "User does not have an Admin-Approved class";
                        return View();
                    }
                }
                //This is an instructor with an approved course
                //Display the instructor's page
                Instructor thisInstructor = db.Instructors.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                if (thisInstructor != null) return RedirectToAction("Details", new {id = thisInstructor.InstructorID});
            }
            // This user is not an "Administrator", "SuperAdministrator", or "Instructor"
            return RedirectToAction("UserNotAuthorized");
            //return View();
        }

        //
        // GET: /Instructor/Details/5

        [Authorize(Roles = "Administrator, SuperAdministrator, Instructor")]
        public ActionResult Details(int id)
        {
            if (User.IsInRole("Administrator") || User.IsInRole("SuperAdministrator"))
            {
                Instructor instructor = db.Instructors.Find(id);
                return View(instructor);
            }
            else
            {
                Instructor instructor = db.Instructors.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                if (instructor != null && id == instructor.InstructorID)
                    // if the current user's id matches the instructor's id
                {
                    return View(instructor);
                }
                // the current user and the selected instructor's id do not match
                return RedirectToAction("failedtomatch");
            }
        }


        //UNNECESSARY
        /*
        //
        // GET: /Instructor/Create
        [Authorize]
        public ActionResult Create() // This entire method is likely without necessary use.  Likely to remove.
        {
            if (User.IsInRole("Administrator"))
            {
                var users = Roles.GetUsersInRole("Student");
                var model = new CreateStudentViewModel
                {
                    Users = users.OfType<MembershipUser>().Select(x => new SelectListItem // does not actually return any users because we are using Roles.GetUserInRole instead of Membership.GetAllUsers()
                    {
                        Value = x.UserName,
                        Text = x.UserName,
                    })
                };
                return View(model);
                /*ViewData["Users"] = Roles.GetUsersInRole("Student");
                return View();*/
        /*
            }
            else // we do not check for another role, because we want only Administrator to be able to explicitly add a Teacher.
            {
                return View(/*not an administrator, no reason to be here, I think*/
        /*);
            }
        } 

        //
        // POST: /Instructor/Create

        [HttpPost]
        public ActionResult Create(CreateStudentViewModel studentModel, string selectedUser, string lastName, string firstMidName, string email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Instructor instructor = new Instructor();

                    //Establish the student data
                    instructor.UserName = selectedUser;
                    instructor.LastName = lastName;
                    instructor.FirstMidName = firstMidName;
                    instructor.Email = email;
                    instructor.EnrollmentDate = DateTime.Now;

                    db.Instructors.Add(instructor);//inputs student data into database (is not saved yet)
                    db.SaveChanges();//saves the instructor to database

                    var user = System.Web.Security.Membership.GetUser(instructor.UserName);//gets the actual user
                    Roles.AddUserToRole(user.UserName, "Instructor");//takes the user and sets role to instructor

                    // assigns Instructor data to the profile of the user (so the user is associated with this specified Instructor data)
                    CustomProfile profile = CustomProfile.GetUserProfile(instructor.UserName);
                    profile.IsTeacher = "yes";
                    profile.Save();

                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Saving failed for some reason.  You may have left some information blank.  Please try again (several times in several different ways if possible (i.e. try using a different computer) - if the problem persists see your system administrator.");
            }

            // This code block is here to allow the page to render in case we get a DataException and have to re-display the screen.
            var users = Membership.GetAllUsers();
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
        */

        //
        // GET: /Instructor/Edit/5

        public ActionResult Edit(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // POST: /Instructor/Edit/5

        [HttpPost]
        public ActionResult Edit(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        //
        // GET: /Instructor/Delete/5

        public ActionResult Delete(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            return View(instructor);
        }

        //
        // POST: /Instructor/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}