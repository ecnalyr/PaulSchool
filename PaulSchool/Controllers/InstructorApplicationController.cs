// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructorApplicationController.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PaulSchool.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using PaulSchool.Models;
    using PaulSchool.ViewModels;

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

        // GET: /InstructorApplication/ApplyToBecomeInstructor
        #region Public Methods and Operators

        /// <summary>
        /// Allows a registered user to apply to become an Instructor
        /// </summary>
        /// <returns>
        /// The view needed to register to become an Instructor
        /// </returns>
        public ActionResult ApplyToBecomeInstructor()
        {
            Student thisStudent = this.db.Students.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
            IList<string> experiences = new List<string>() { "0-1 year", "2-4 years", "5-7 years", "8-10 years", "Over 10 years" };
            var model = new InstructorApplicationViewModel
                {
                    EducationalBackgrounds = new List<EducationalBackGround>
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
                    ExperienceList = new SelectList(experiences)
                };
            return View(model);
        }

        /// <summary>
        /// The apply to become instructor.
        /// </summary>
        /// <param name="applicationFromView">The application from view.</param>
        /// <returns>Index view</returns>
        [HttpPost]
        public ActionResult ApplyToBecomeInstructor(InstructorApplicationViewModel applicationFromView)
        {
            Student thisStudent = this.db.Students.FirstOrDefault(o => o.StudentID == applicationFromView.CurrentUserId);
            var instructorApplication = new InstructorApplication
                {
                    BasicInfoGatheredFromProfile = thisStudent,
                    EducationalBackground = applicationFromView.EducationalBackgrounds as ICollection<EducationalBackground>,
                    Experience = applicationFromView.Experience,
                    WillingToTravel = applicationFromView.WillingToTravel
                };
            this.db.InstructorApplication.Add(instructorApplication);
            this.db.SaveChanges();
            return this.Redirect("Index");
        }

        /// <summary>
        /// The educational background.
        /// </summary>
        /// <returns>
        /// Educational background rows to the Instructor Application View Model
        /// </returns>
        public PartialViewResult EducationalBackground()
        {
            return this.PartialView("EducationalBackGround", new EducationalBackGround());
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>Returns View</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        #endregion
    }
}