using System;

namespace PaulSchool.Models
{
    public class EducationalBackground
    {
        public int EducationalBackgroundID { get; set; }

        public string UniversityOrCollege { get; set; }

        public string AreaOfStudy { get; set; }

        public string Degree { get; set; }

        public string YearReceived { get; set; }

        public virtual InstructorApplication InstructorApplication { get; set; }
    }
}