// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructorApplicationController.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    /// <summary>
    /// The instructor application controller.
    /// </summary>
    [Authorize]
    public class InstructorApplicationController : Controller
    {
        #region Fields

        /// <summary>
        /// The db.
        /// </summary>
        private readonly SchoolContext db = new SchoolContext();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Allows a registered user to apply to become an Instructor
        /// </summary>
        /// <returns>
        /// The view needed to register to become an Instructor
        /// </returns>
        public ActionResult ApplyToBecomeInstructor()
        {
            Student thisStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);
            IList<string> experiences = new List<string>
                                            {
                                                "0-1 year",
                                                "2-4 years",
                                                "5-7 years",
                                                "8-10 years",
                                                "Over 10 years"
                                            };
            var model = new InstructorApplicationViewModel
                            {
                                EducationalBackgrounds =
                                    new List<EducationalBackGround>
                                        {
                                            new EducationalBackGround
                                                {
                                                    AreaOfStudy = string.Empty,
                                                    Degree = string.Empty,
                                                    UniversityOrCollege = string.Empty,
                                                    YearReceived = string.Empty
                                                }
                                        },
                                CurrentUserId = thisStudent.StudentID,
                                ExperienceList = new SelectList(experiences),
                            };
            return View(model);
        }

        /// <summary>
        /// The apply to become instructor.
        /// </summary>
        /// <param name="applicationFromView">
        /// The application from view.
        /// </param>
        /// <returns>
        /// Index view
        /// </returns>
        [HttpPost]
        public ActionResult ApplyToBecomeInstructor(InstructorApplicationViewModel applicationFromView)
        {
            try
            {
                Student thisUser = db.Students.FirstOrDefault(o => o.StudentID == applicationFromView.CurrentUserId);
                InstructorApplication instructorApplication = BuildNewInstructorApplicationAndAddToDb(applicationFromView,
                                                                                                      thisUser);

                db.SaveChanges();

                BuildNewNotificationAndAddToDb(instructorApplication, thisUser);

                db.SaveChanges();
                TempData["message"] =
                    "Your application to become an Instructor has been submitted and is awaiting Administrative review.  You will be notified via your notifications page when your application has been reviewed, this typically takes 1 - 3 business days.  Contact the St. Paul School of Catechesis at 361-882-6191 if you have any questions.";
                return Redirect("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("",
                                         "Saving failed for some reason.  You may have left some information blank.  Please try again (several times in several different ways if possible (i.e. try using a different computer) - if the problem persists see your system administrator.");
            }
            TempData["message"] =
                "Your application to become an Instructor failed to submit.  A common reason for this error is blank spaces within the form.  Please fill all blanks and resubmit.  If the problem persists, contact the administrator.";
            return RedirectToAction("ApplyToBecomeInstructor");
            
        }

        private InstructorApplication BuildNewInstructorApplicationAndAddToDb(
            InstructorApplicationViewModel applicationFromView, Student thisUser)
        {
            InstructorApplication instructorApplication = BuildNewInstructorApplication(applicationFromView, thisUser);
            db.InstructorApplication.Add(instructorApplication);
            return instructorApplication;
        }

        private static InstructorApplication BuildNewInstructorApplication(
            InstructorApplicationViewModel applicationFromView, Student thisUser)
        {
            var educationList = new List<EducationalBackground>();
            foreach (EducationalBackGround educate in applicationFromView.EducationalBackgrounds)
            {
                var education = new EducationalBackground
                                    {
                                        YearReceived = educate.YearReceived,
                                        Degree = educate.Degree,
                                        AreaOfStudy = educate.AreaOfStudy,
                                        UniversityOrCollege = educate.UniversityOrCollege
                                    };
                educationList.Add(education);
            }
            var instructorApplication = new InstructorApplication
                                            {
                                                StudentID = thisUser.StudentID,
                                                EducationalBackground = new List<EducationalBackground>(),
                                                Experience = applicationFromView.Experience,
                                                WillingToTravel = applicationFromView.WillingToTravel,
                                                Approved = false
                                            };
            instructorApplication.EducationalBackground.AddRange(educationList);
            return instructorApplication;
        }

        private void BuildNewNotificationAndAddToDb(InstructorApplication instructorApplication, Student thisUser)
        {
            Notification newNotification = BuildNewNotification(instructorApplication, thisUser);
            db.Notification.Add(newNotification);
        }

        private Notification BuildNewNotification(InstructorApplication instructorApplication, Student thisUser)
        {
            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "A user by the name of " + thisUser.FirstMidName + " " + thisUser.LastName
                                              + " has applied to become an Instructor",
                                          Link =
                                              Url.Action(
                                                  "Details",
                                                  "InstructorApplication",
                                                  new {id = instructorApplication.InstructorApplicationID}),
                                          ViewableBy = "Admin",
                                          Complete = false
                                      };
            return newNotification;
        }

        /// <summary>
        /// The details.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        public ViewResult Details(int id)
        {
            InstructorApplication instructorApplication =
                db.InstructorApplication.Include("EducationalBackGround").FirstOrDefault(
                    m => m.InstructorApplicationID == id);
            return View(instructorApplication);
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult Delete(int id)
        {
            InstructorApplication application = db.InstructorApplication.Find(id);

            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Administrator has denied and deleted your application to become an Instructor. Contact the St. Paul School of Catechesis at 361-882-6191.",
                                          Link = Url.Action("ApplyToBecomeInstructor"),
                                          ViewableBy = application.Student.UserName,
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);
            db.SaveChanges();

            IEnumerable<EducationalBackground> listOfEducationalBackgrounds =
                db.EducationalBackground.Where(
                    o => o.InstructorApplication.InstructorApplicationID == application.InstructorApplicationID);
            foreach (EducationalBackground item in listOfEducationalBackgrounds)
            {
                db.EducationalBackground.Remove(item);
            }
            db.InstructorApplication.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index", "Notification");
        }

        /// <summary>
        /// The educational background.
        /// </summary>
        /// <returns>
        /// Educational background rows to the Instructor Application View Model
        /// </returns>
        public PartialViewResult EducationalBackground()
        {
            return PartialView("EducationalBackGround", new EducationalBackGround());
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult ApproveInstructorApplication(int id)
        {
            InstructorApplication instructorApplication = db.InstructorApplication.Find(id);
            instructorApplication.Approved = true;

            var instructor = new Instructor
                                 {
                                     UserName = instructorApplication.Student.UserName,
                                     LastName = instructorApplication.Student.LastName,
                                     FirstMidName = instructorApplication.Student.FirstMidName,
                                     Email = instructorApplication.Student.Email,
                                     EnrollmentDate = DateTime.Now
                                 };
            db.Instructors.Add(instructor);

            var newNotification = new Notification
                                      {
                                          Time = DateTime.Now,
                                          Details =
                                              "An Admin has approved your application to become an Instructor as of " +
                                              DateTime.Now,
                                          Link =
                                              Url.Action(
                                                  "Details",
                                                  "InstructorApplication",
                                                  new {id = instructorApplication.InstructorApplicationID}),
                                          ViewableBy = instructor.UserName,
                                          Complete = false
                                      };
            db.Notification.Add(newNotification);

            db.SaveChanges();

            if (!(Roles.IsUserInRole(instructor.UserName, "Instructor")))
            {
                Roles.AddUserToRole(instructor.UserName, "Instructor");
            }

            return View("Success");

            //Roles.AddUserToRole(, "Instructor");
            // make user an instructor
            // //  need to set user.role to "Instructor"
            // //  need to pull data from Student Table into Instructor table (actually need to rework the non-dry components of this feature,
            // //  but that is for another time).
            // Create notification for Instructor
            // visually notify Admin that the change has been made.
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// Returns View
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion
    }
}