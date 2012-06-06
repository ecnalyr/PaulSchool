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

        // GET: /InstructorApplication/

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
            var model = new InstructorApplicationViewModel
                {
                    BasicInfoGatheredFromProfile = thisStudent, 
                    Experience = "1 year? 2 years?", 
                    WillingToTravel = false, 
                };
            return View(model);
        }

        /// <summary>
        /// The apply to become instructor.
        /// </summary>
        /// <param name="applicationFromView">
        /// The application from view.
        /// </param>
        /// <param name="listFromView">
        /// The list from view.
        /// </param>
        /// <returns>
        /// </returns>
        [HttpPost]
        public ActionResult ApplyToBecomeInstructor(
            InstructorApplicationViewModel applicationFromView, IEnumerable<InstructorApplicationViewModel> listFromView)
        {
            var instructorApplication = new InstructorApplication
                {
                    BasicInfoGatheredFromProfile = applicationFromView.BasicInfoGatheredFromProfile, 
                    // EducationalBackground = listFromView.,
                    Experience = applicationFromView.Experience, 
                    WillingToTravel = applicationFromView.WillingToTravel
                };
            return this.View(applicationFromView);
        }

        /// <summary>
        /// The educational background.
        /// </summary>
        /// <returns>
        /// </returns>
        public PartialViewResult EducationalBackground()
        {
            return this.PartialView("_EducationalBackground", new InstructorApplicationViewModel());
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// </returns>
        public ActionResult Index()
        {
            return this.View();
        }

        #endregion
    }
}