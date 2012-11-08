using System;
using System.Collections.Generic;

namespace PaulSchool.Models
{
    public class Instructor
    {
        public int InstructorID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public string Email { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public string UserName { get; set; }
    }
}