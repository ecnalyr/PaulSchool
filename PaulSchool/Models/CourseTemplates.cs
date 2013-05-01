using System;
using System.ComponentModel.DataAnnotations;

namespace PaulSchool.Models
{
    public class CourseTemplates
    {
        public int CourseTemplatesID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Credit hours")]
        [Range(1, 100.00, ErrorMessage = "Credits must be a positive value")]
        public int Credits { get; set; }

        [Required]
        [UIHint("IsChecked")]
        public bool Elective { get; set; }

        [Range(1, 100.00, ErrorMessage = "Total Attending Days must be a positive value (often 10, 8, or 3)")]
        [Display(Name = "Attending Days")]
        public int AttendingDays { get; set; }

        // 10, 8, 3, or custom

        [Range(1, 100.00, ErrorMessage = "Attendance Cap must be a positive value (typically 30)")]
        [Display(Name = "Attendance Cap")]
        public int AttendanceCap { get; set; }

        public string Location { get; set; }

        public string Parish { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }
    }
}