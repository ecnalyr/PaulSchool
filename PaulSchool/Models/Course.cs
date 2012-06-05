using System;
using System.Collections.Generic;

namespace PaulSchool.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

        public bool Elective { get; set; }

        // public string Instructor { get; set; }
        public int InstructorID { get; set; }

        public virtual Instructor Instructor { get; set; }

        public int Year { get; set; }

        public int AttendingDays { get; set; } // 10, 8, 3, or custom

        public int AttendanceCap { get; set; } // default of 30

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DurationHours { get; set; } // i.e. 1 hour, or 0 hour and 45 mins

        public int DurationMins { get; set; } // i.e. 0 mins, or 0 hour and 45 mins

        public string Location { get; set; }

        public string Parish { get; set; }

        public string Description { get; set; }

        public string AdminDenialReason { get; set; }

        public bool Approved { get; set; } // yes, no - needs to be a drop down

        public bool Completed { get; set; } // yes, no - needs to be a drop down

        public bool Archived { get; set; } // yes, no - needs to be a drop down
        // end of modified part*/
        //

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        // The Enrollments property is a navigation property. A Course entity can be related to any number of Enrollment entities.
    }
}