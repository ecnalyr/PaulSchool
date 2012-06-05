using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaulSchool.ViewModels
{
    public class ApplyCourseViewModel
    {
        [Display(Name = "selected course")]
        public string SelectedCourse { get; set; }

        public IEnumerable<SelectListItem> Courses { get; set; }

        public int? CourseTemplatesID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Range(1, 100.00,
            ErrorMessage = "Credits must be a positive value")]
        public int Credits { get; set; }

        [Required]
        public bool Elective { get; set; }

        [Required]
        [Range(1, 100.00,
            ErrorMessage = "Total Attending Days must be a positive value (often 10, 8, or 3)")]
        public int AttendingDays { get; set; }

        // 10, 8, 3, or custom

        [Required]
        [Range(1, 100.00,
            ErrorMessage = "Attendance Cap must be a positive value (typically 30)")]
        public int AttendanceCap { get; set; }

        // default of 30

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, 24, ErrorMessage = "Hours must be a value (you can use 0 hours 45 mins, for example")]
        public int DurationHours { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "0 - 59 mins")]
        public int DurationMins { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Parish { get; set; }

        [Required]
        public string Description { get; set; }
    }
}