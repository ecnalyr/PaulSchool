// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EducationalBackGround.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
        public string UniversityOrCollege { get; set; }

        /// <summary>
        /// Gets or sets AreaOfStudy
        /// </summary>
        public string AreaOfStudy { get; set; }

        /// <summary>
        /// Gets or sets Degree
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// Gets or sets YearReceived
        /// </summary>
        public string YearReceived { get; set; }
    }
}