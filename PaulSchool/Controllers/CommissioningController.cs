using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{
    using System.Web.Routing;
    using System.Diagnostics;

    public class CommissioningController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Commissioning/

        public ViewResult Index()
        {
            return View(db.ApplicationCommissionings.ToList());
        }

        //
        // GET: /Commissioning/Details/5

        public ViewResult Details(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

        //
        // GET: /Commissioning/Create

        public ActionResult Create()
        {
            var thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
            Debug.Write(thisStudent.StudentID);
            var applicationWithUserData = new ApplicationCommissioning
                {
                    StudentID = thisStudent.StudentID,
                    RecommendationFiled = false,
                    PersonalStatement = "Type Personal Statement Here",
                    DayOfReflection = false,
                    ApplicationFeePaid = false,
                    MeetsMinimumRequirements = false
                };
            Debug.Write(thisStudent.LastName);

            return View(applicationWithUserData);
        } 

        //
        // POST: /Commissioning/Create

        [HttpPost]
        public ActionResult Create(ApplicationCommissioning applicationcommissioning)
        {
            if (ModelState.IsValid)
            {
                var thisStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                applicationcommissioning.StudentID = thisStudent.StudentID;
                applicationcommissioning.DateFiled = DateTime.Now;
                db.ApplicationCommissionings.Add(applicationcommissioning);
                db.SaveChanges();

                var newNotification = new Notification
                {
                    Time = DateTime.Now,
                    Details =
                        "A student by the name of " + applicationcommissioning.Student.FirstMidName + " "
                        + applicationcommissioning.Student.LastName
                        + " has submitted an application for Commissioning",
                    Link = this.Url.Action("Details", "Commissioning", new { id = applicationcommissioning.Id }),
                    ViewableBy = "Admin",
                    Complete = false
                };
                this.db.Notification.Add(newNotification);
                this.db.SaveChanges();

                return RedirectToAction("Index");  
            }

            return View(applicationcommissioning);
        }
        
        //
        // GET: /Commissioning/Edit/5
 
        public ActionResult Edit(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

        //
        // POST: /Commissioning/Edit/5

        [HttpPost]
        public ActionResult Edit(ApplicationCommissioning applicationcommissioning)
        {
            if (this.ModelState.IsValid)
            {
                this.db.Entry(applicationcommissioning).State = EntityState.Modified;
                this.db.SaveChanges();

                return this.RedirectToAction("Index");
            }
            return View(applicationcommissioning);
        }

        //
        // GET: /Commissioning/Delete/5
        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Delete(int id)
        {
            ApplicationCommissioning applicationcommissioning = db.ApplicationCommissionings.Find(id);
            return View(applicationcommissioning);
        }

        //
        // POST: /Commissioning/Delete/5
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}