﻿namespace PaulSchool.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public string Grade { get; set; } // pass, fail, null

        public string Comments { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}