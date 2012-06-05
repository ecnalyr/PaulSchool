using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.ViewModels
{
    public class InstructorApplicationViewModel
    {
        public IEnumerable<SelectListItem> EducationalBackground { get; set; }

        public Student BasicInfoGatheredFromProfile { get; set; }

        [Required]
        [StringLength(100,
            ErrorMessage = "Example: 1 year, 5-7 years, etc. . .")]
        public string Experience { get; set; }

        [Required]
        public bool WillingToTravel { get; set; }

        public string UniversityOrCollege { get; set; }

        public string AreaOfStudy { get; set; }

        public string Degree { get; set; }

        [StringLength(4,
            ErrorMessage = "Year must be formatted similar to: 1999")]
        public int YearReceived { get; set; }
    }
}