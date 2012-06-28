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

        public ViewResult Index()
        {
            return View(db.ApplicationCommissionings.Where(o => o.Approved == false).ToList());
        }

        public ViewResult AllApplications()
        {
            return View(db.ApplicationCommissionings.ToList());
        }

        public ViewResult QualifyForCommissioning()
        {
            var minCoresNeeded = 0;//db.CommissioningRequirementse.Find(1).CoreCoursesRequired;
            var minElectivesNeeded = 0;//db.CommissioningRequirementse.Find(1).ElectiveCoursesRequired;

            IEnumerable<MembershipUser> users = Membership.GetAllUsers().Cast<MembershipUser>();
            ICollection<Student> qualifiedUsers = new List<Student>();
            foreach (var user in users)
            {
                Debug.Write("we got into foreach loop");
                var thisStudent = db.Students.FirstOrDefault( o => o.UserName == user.UserName);
                var coresPassed = TotalCoresPassed(thisStudent);
                var electivesPassed = TotalElectivesPassed(thisStudent);
                if (coresPassed >= minCoresNeeded && electivesPassed >= minElectivesNeeded)
                {
                    qualifiedUsers.Add(thisStudent);
                }
                Debug.Write("we got to the end of the foreach loop");
            }
            return View(qualifiedUsers);
        }

        public ViewResult Details(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);

            var totalCoresNeeded = db.CommissioningRequirementse.Find(1).CoreCoursesRequired;
            var totalElectivesNeeded = db.CommissioningRequirementse.Find(1).ElectiveCoursesRequired;
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
            var totalCoresNeeded = db.CommissioningRequirementse.Find(1).CoreCoursesRequired;
            var totalElectivesNeeded = db.CommissioningRequirementse.Find(1).ElectiveCoursesRequired;
            ViewBag.totalCoresNeeded = totalCoresNeeded;
            ViewBag.totalElectivesNeeded = totalElectivesNeeded;

            var thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);

            var doOrDoNotQualify = DoOrDoNotQualify(thisStudent, totalCoresNeeded, totalElectivesNeeded);

            ViewBag.doOrDoNotQualify = doOrDoNotQualify;

            var applicationWithUserData = ApplicationWithUserData(thisStudent);

            return View(applicationWithUserData);
        }

        private string DoOrDoNotQualify(Student thisStudent, int totalCoresNeeded, int totalElectivesNeeded)
        {
            int totalElectivesPassed;
            int totalCoresPassed;
            TotalCoresAndElectivesPassed(thisStudent, out totalElectivesPassed, out totalCoresPassed);

            string doOrDoNotQualify;
            if (totalCoresPassed >= totalCoresNeeded && totalElectivesPassed >= totalElectivesNeeded)
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
            int totalCoresPassed = db.Enrollments.Count(s => s.StudentID == thisStudent.StudentID
                                                             && s.Grade == "pass"
                                                             && s.Course.Elective == false);
            return totalCoresPassed;
        }

        private int TotalElectivesPassed(Student thisStudent)
        {
            int totalElectivesPassed = db.Enrollments.Count(s => s.StudentID == thisStudent.StudentID
                                                                 && s.Grade == "pass"
                                                                 && s.Course.Elective);
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

                return RedirectToAction("Index");  
            }

            return View(applicationcommissioning);
        }
        
        public ActionResult Edit(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

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

                return this.RedirectToAction("Index");
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