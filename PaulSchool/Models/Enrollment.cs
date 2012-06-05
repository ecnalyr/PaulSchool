namespace PaulSchool.Models
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public string Grade { get; set; } // pass, fail, null

        public string Comments { get; set; } // Instructors can set comments for student to read.

        public virtual Course Course { get; set; }
        // The CourseID property is a foreign key, and the corresponding navigation property is Course.

        public virtual Student Student { get; set; }
        // The StudentID property is a foreign key, and the corresponding navigation property is Student.
    }
}