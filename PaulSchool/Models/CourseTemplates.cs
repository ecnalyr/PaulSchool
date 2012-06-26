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
        [Range(1, 100.00, ErrorMessage = "Credits must be a positive value")]
        public int Credits { get; set; }

        [Required]
        public bool Elective { get; set; }

        [Range(1, 100.00, ErrorMessage = "Total Attending Days must be a positive value (often 10, 8, or 3)")]
        public int AttendingDays { get; set; }

        // 10, 8, 3, or custom

        [Range(1, 100.00, ErrorMessage = "Attendance Cap must be a positive value (typically 30)")]
        public int AttendanceCap { get; set; }

        // default of 30

        [Required]
        [Range(0, 24, ErrorMessage = "Hours must be a value (you can use 0 hours then input 45 mins below, for example")
        ]
        public int DurationHours { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "0 - 59 mins")]
        public int DurationMins { get; set; }


        public string Location { get; set; }

        public string Parish { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }
    }
}