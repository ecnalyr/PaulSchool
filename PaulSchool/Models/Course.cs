using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PaulSchool.Models
{
    public class Course
    {
        public Course()
        {
            this.SeatsTaken = 0;
        }

        public int CourseID { get; set; }

        public string Title { get; set; }

        public int Credits { get; set; }

        [UIHint("IsChecked")]
        public bool Elective { get; set; }

        // public string Instructor { get; set; }
        public int InstructorID { get; set; }

        public virtual Instructor Instructor { get; set; }

        public int Year { get; set; }

        public int AttendingDays { get; set; } // 10, 8, 3, or custom

        public int AttendanceCap { get; set; } // default of 30

        public int SeatsTaken { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [DisplayFormat(DataFormatString = "{0: h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime StartTime
        {
            get { return StartDate; }
        }

        [DisplayFormat(DataFormatString = "{0: h:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime EndTime
        {
            get { return EndDate; }
        }

        public string Location { get; set; }

        public string Parish { get; set; }

        public string Description { get; set; }

        public decimal Cost { get; set; }

        public string AdminDenialReason { get; set; }

        [UIHint("IsChecked")]
        public bool Approved { get; set; } // yes, no - needs to be a drop down

        [UIHint("IsChecked")]
        public bool Completed { get; set; } // yes, no - needs to be a drop down

        [UIHint("IsChecked")]
        public bool Archived { get; set; } // yes, no - needs to be a drop down

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }

}