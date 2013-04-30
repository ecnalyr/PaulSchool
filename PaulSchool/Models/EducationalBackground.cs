using System.ComponentModel.DataAnnotations;
using System;
using DataAnnotationsExtensions;

namespace PaulSchool.Models
{
    public class EducationalBackground
    {
        public int EducationalBackgroundID { get; set; }

        [Required]
        [Display(Name = "University or Collegeasdfasdf")]
        [StringLength(50)]
        public string UniversityOrCollege { get; set; }

        [Required]
        [Display(Name = "Area of Study")]
        [StringLength(50)]
        public string AreaOfStudy { get; set; }

        [Required(ErrorMessage = "Example: Bachelor of Arts")]
        [Display(Name = "Degree")]
        [StringLength(50)]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Example: 2005")]
        [Display(Name = "Year Received")]
        [StringLength(4)]
        [Digits]
        public string YearReceived { get; set; }

        public virtual InstructorApplication InstructorApplication { get; set; }
    }
}