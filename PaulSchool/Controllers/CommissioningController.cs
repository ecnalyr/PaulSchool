using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using PaulSchool.Models;
using System.Web.Security;

namespace PaulSchool.Controllers
{
    using System.Web.Routing;

    public class CommissioningController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        [Authorize]
        public ViewResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ViewResult CommissioningApplications()
        {
            return View(db.ApplicationCommissionings.Where(o => o.Approved == false).ToList());
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ViewResult AllCommissioningApplications()
        {
            return View(db.ApplicationCommissionings.ToList());
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ViewResult ReCommissioningApplications()
        {
            return View(db.ApplicationCommissionings.Where(o => o.Approved == false && o.ReCommissioning == true).ToList());
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ViewResult AllReCommissioningApplications()
        {
            return View(db.ApplicationCommissionings.Where(o => o.ReCommissioning == true).ToList());
        }

        public ViewResult QualifyForCommissioning()
        {
            var minCoresNeeded = 0;//db.CommissioningRequirementse.Find(1).CoreCoursesRequired;
            var minElectivesNeeded = 0;//db.CommissioningRequirementse.Find(1).ElectiveCoursesRequired;

            IEnumerable<MembershipUser> users = Membership.GetAllUsers().Cast<MembershipUser>();
            ICollection<Student> qualifiedUsers = new List<Student>();
            foreach (var user in users)
            {
                var thisStudent = db.Students.FirstOrDefault( o => o.UserName == user.UserName);
                Debug.Write(user.UserName);
                Debug.Write(thisStudent.UserName);
                var coresPassed = TotalCoresPassed(thisStudent);
                var electivesPassed = TotalElectivesPassed(thisStudent);
                if (coresPassed >= minCoresNeeded && electivesPassed >= minElectivesNeeded)
                {
                    qualifiedUsers.Add(thisStudent);
                }
            }
            return View(qualifiedUsers);
        }

        public ViewResult Details(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);

            var totalCoresNeeded = db.CommissioningRequirements.Find(1).CoreCoursesRequired;
            var totalElectivesNeeded = db.CommissioningRequirements.Find(1).ElectiveCoursesRequired;
            ViewBag.totalCoresNeeded = totalCoresNeeded;
            ViewBag.totalElectivesNeeded = totalElectivesNeeded;

            var thisStudent = db.Students.FirstOrDefault(
                    o => o.StudentID == applicationcommissioning.StudentID);

            var doOrDoNotQualify = DoOrDoNotQualify(thisStudent, totalCoresNeeded, totalElectivesNeeded);

            ViewBag.doOrDoNotQualify = doOrDoNotQualify;

            return View(applicationcommissioning);
        }

        public ActionResult Create()
        {
            var totalCoresNeeded = db.CommissioningRequirements.Find(1).CoreCoursesRequired;
            var totalElectivesNeeded = db.CommissioningRequirements.Find(1).ElectiveCoursesRequired;
            ViewBag.totalCoresNeeded = totalCoresNeeded;
            ViewBag.totalElectivesNeeded = totalElectivesNeeded;

            var thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);

            var doOrDoNotQualify = DoOrDoNotQualify(thisStudent, totalCoresNeeded, totalElectivesNeeded);

            ViewBag.doOrDoNotQualify = doOrDoNotQualify;

            var applicationWithUserData = ApplicationWithUserData(thisStudent);

            var dayOfReflection = db.Enrollments.FirstOrDefault(s => s.StudentID == thisStudent.StudentID && s.Course.Title == "Day of Reflection" && s.Grade == "pass");
            ViewBag.completedDayOfReflection = "Incomplete, ask an Administrator for details on how to complete this.";
            if (dayOfReflection != null)
            {
                ViewBag.completedDayOfReflection = "Complete";
            }

            ViewBag.electivesPassed = TotalElectivesPassed(thisStudent);
            ViewBag.coresPassed = TotalCoresPassed(thisStudent);

            return View(applicationWithUserData);
        }

        public ViewResult RecommendationForm()
        {
            return View();
        }

        public ActionResult PrintableRecommendationForm()
        {
            return View();
        }

        private string DoOrDoNotQualify(Student thisStudent, int totalCoresNeeded, int totalElectivesNeeded)
        {
            int totalElectivesPassed;
            int totalCoresPassed;
            TotalCoresAndElectivesPassed(thisStudent, out totalElectivesPassed, out totalCoresPassed);

            var dayOfReflection = db.Enrollments.FirstOrDefault(s => s.StudentID == thisStudent.StudentID && s.Course.Title == "Day of Reflection" && s.Grade == "pass");

            string doOrDoNotQualify;
            if (totalCoresPassed >= totalCoresNeeded && totalElectivesPassed >= totalElectivesNeeded && dayOfReflection != null)
            {
                doOrDoNotQualify = "do";
            }
            else
            {
                doOrDoNotQualify = "do not";
            }
            return doOrDoNotQualify;
        }

        private static ApplicationCommissioning ApplicationWithUserData(Student thisStudent)
        {
            var applicationWithUserData = new ApplicationCommissioning
                                              {
                                                  StudentID = thisStudent.StudentID,
                                                  ReCommissioning = false,
                                                  RecommendationFiled = false,
                                                  PersonalStatement = "Type Personal Statement Here",
                                                  DayOfReflection = false,
                                                  ApplicationFeePaid = false,
                                                  MeetsMinimumRequirements = false
                                              };
            return applicationWithUserData;
        }

        private void TotalCoresAndElectivesPassed(Student thisStudent, out int totalElectivesPassed, out int totalCoresPassed)
        {
            totalElectivesPassed = TotalElectivesPassed(thisStudent);
            totalCoresPassed = TotalCoresPassed(thisStudent);
        }

        private int TotalCoresPassed(Student thisStudent)
        {
            var sevenYearsAgo = DateTime.Now.AddYears(-7);
            int totalCoresPassed = db.Enrollments.Count(s => s.StudentID == thisStudent.StudentID
                                                             && s.Grade == "pass"
                                                             && s.Course.Elective == false
                                                             && s.Course.Title != "Day of Reflection"
                                                             && s.Course.EndDate >= sevenYearsAgo);
            return totalCoresPassed;
        }

        private int TotalElectivesPassed(Student thisStudent)
        {
            var sevenYearsAgo = DateTime.Now.AddYears(-7);
            int totalElectivesPassed = db.Enrollments.Count(s => s.StudentID == thisStudent.StudentID
                                                                 && s.Grade == "pass"
                                                                 && s.Course.Elective
                                                                 && s.Course.EndDate >= sevenYearsAgo);
            return totalElectivesPassed;
        }

        [HttpPost]
        public ActionResult Create(ApplicationCommissioning applicationcommissioning)
        {
            if (ModelState.IsValid)
            {
                var thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                if (thisStudent != null) applicationcommissioning.StudentID = thisStudent.StudentID;
                applicationcommissioning.DateFiled = DateTime.Now;
                applicationcommissioning.Approved = false;
                db.ApplicationCommissionings.Add(applicationcommissioning);
                db.SaveChanges();

                var newNotification = new Notification
                {
                    Time = DateTime.Now,
                    Details =
                        "A student by the name of " + applicationcommissioning.Student.FirstMidName + " "
                        + applicationcommissioning.Student.LastName
                        + " has submitted an application for Commissioning",
                    Link = Url.Action("Details", "Commissioning", new { id = applicationcommissioning.Id }),
                    ViewableBy = "Admin",
                    Complete = false
                };
                db.Notification.Add(newNotification);
                db.SaveChanges();

                TempData["message"] =
                    "Please make sure you have completed all the steps for your application.  Contact the St. Paul School of Catechesis at 361-882-6191 if you have any questions.";
                return RedirectToAction("Index");  
            }

            return View(applicationcommissioning);
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Edit(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        [HttpPost]
        public ActionResult Edit(ApplicationCommissioning applicationcommissioning)
        {
            if (ModelState.IsValid)
            {
                db.Entry(applicationcommissioning).State = EntityState.Modified;
                db.SaveChanges();
                var thisStudent = db.Students.FirstOrDefault(
                    o => o.StudentID == applicationcommissioning.StudentID);

                var newNotification = new Notification
                                          {
                                              Time = DateTime.Now,
                                              Details =
                                                  "An Administrator has edited your application for Commissioning, filed on " +
                                                  applicationcommissioning.DateFiled,
                                              Link =
                                                  Url.Action("Details", "Commissioning",
                                                             new {id = applicationcommissioning.Id}),
                                              ViewableBy = thisStudent.UserName,
                                              Complete = false
                                          };
                db.Notification.Add(newNotification);
                db.SaveChanges();

                TempData["message"] = "Changes saved.";
                return this.RedirectToAction("CommissioningApplications");
            }
            return View(applicationcommissioning);
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Delete(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, ApplicationCommissioning hasReasonForDeletion)
        {            
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);

            var newNotification = new Notification
            {
                Time = DateTime.Now,
                Details =
                    "An Administrator has denied your application for Commissioning, filed on " + applicationcommissioning.DateFiled +
                    " citing the following reason: " + hasReasonForDeletion.AdminDenialReason,
                Link = Url.Action("Create", "Commissioning"),
                ViewableBy = applicationcommissioning.Student.UserName,
                Complete = false
            };
            db.Notification.Add(newNotification);

            db.ApplicationCommissionings.Remove(applicationcommissioning);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult ApproveCommissioningApplication(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);

            var newNotification = new Notification
            {
                Time = DateTime.Now,
                Details =
                    "Your application for Commissioning, filed on " + applicationcommissioning.DateFiled +
                    " has been approved.  Congratulations",
                Link = Url.Action("CertificateOfCommissioning", "Certificate", new {id = applicationcommissioning.Id}, null),
                ViewableBy = applicationcommissioning.Student.UserName,
                Complete = false
            };
            db.Notification.Add(newNotification);

            applicationcommissioning.Approved = true;
            applicationcommissioning.DateApproved = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("CertificateOfCommissioning", "Certificate", new { id = applicationcommissioning.Id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}