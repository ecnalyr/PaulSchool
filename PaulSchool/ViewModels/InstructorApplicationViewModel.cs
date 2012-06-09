// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstructorApplicationViewModel.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PaulSchool.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using PaulSchool.Models;

    /// <summary>
    /// InstructorApplicationViewModel Class
    /// </summary>
    public class InstructorApplicationViewModel
    {
        /// <summary>
        /// Gets or sets EducationBackground
        /// </summary>
        public IList<EducationalBackGround> EducationalBackgrounds { get; set; }

        /// <summary>
        /// Gets or sets CurrentUserId
        /// </summary>
        public int CurrentUserId { get; set; }

        /// <summary>
        /// Gets or sets ExperienceList
        /// </summary>
        public SelectList ExperienceList { get; set; }

        /// <summary>
        /// Gets or sets Experience
        /// </summary>
        public string Experience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether WillingToTravel
        /// </summary>
        public bool WillingToTravel { get; set; }
    }
}