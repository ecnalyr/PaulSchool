// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EducationalBackGround.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace PaulSchool.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// EducationalBackGround Class
    /// </summary>
    /// <remarks>Don't delete the serializable decorator over the class</remarks>
    [Serializable]
    public class EducationalBackGround
    {
        /// <summary>
        /// Gets or sets EducationalBackgroundID
        /// </summary>
        public int EducationalBackgroundID { get; set; }

        /// <summary>
        /// Gets or sets UniversityOrCollege
        /// </summary>
        [Required]
        [Display(Name = "University or College")]
        [StringLength(50)]
        public string UniversityOrCollege { get; set; }

        /// <summary>
        /// Gets or sets AreaOfStudy
        /// </summary>
        [Required]
        [Display(Name = "Area of Study")]
        [StringLength(50)]
        public string AreaOfStudy { get; set; }

        /// <summary>
        /// Gets or sets Degree
        /// </summary>
        [Required(ErrorMessage = "Example: Bachelor of Arts")]
        [Display(Name = "Degree")]
        [StringLength(50)]
        public string Degree { get; set; }

        /// <summary>
        /// Gets or sets YearReceived
        /// </summary>
        [Required(ErrorMessage = "Example: 2005")]
        [Display(Name = "Year Received")]
        [StringLength(4)]
        [Digits]
        public string YearReceived { get; set; }
    }
}