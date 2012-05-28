using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulSchool.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public string LastName { get; set; }

        public string FirstMidName { get; set; }

        public string Email { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string DateOfBirth { get; set; }

        public string ParishAffiliation { get; set; }

        public string MinistryInvolvement { get; set; } // drop-down including a blank customizable field

        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } //if a given Student row in the database has two related Enrollment rows (rows that contain that student's primary key value in their StudentID foreign key column), that Student entity's Enrollments navigation property will contain those two Enrollment entities.
        
        public string UserName { get; set; }
    }
}