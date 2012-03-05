using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PaulSchool.Models;

namespace PaulSchool.DAL
{
    public class SchoolInitializer : DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student { FirstMidName = "Carson",   LastName = "Alexander", Email = "123@aol.com",     EnrollmentDate = DateTime.Parse("2005-09-01")},
                new Student { FirstMidName = "Meredith", LastName = "Alonso", Email = "345@aol.com",        EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student { FirstMidName = "Arturo",   LastName = "Anand", Email = "567@aol.com",         EnrollmentDate = DateTime.Parse("2003-09-01")},
                new Student { FirstMidName = "Gytis",    LastName = "Barzdukas", Email = "456@aol.com",     EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student { FirstMidName = "Yan",      LastName = "Li", Email = "123@gmail.com",          EnrollmentDate = DateTime.Parse("2002-09-01")},
                new Student { FirstMidName = "Peggy",    LastName = "Justice", Email = "1765@gmail.com",    EnrollmentDate = DateTime.Parse("2001-09-01")},
                new Student { FirstMidName = "Laura",    LastName = "Norman", Email = "123423@gmail.com",   EnrollmentDate = DateTime.Parse("2003-09-01")},
                new Student { FirstMidName = "Nino",     LastName = "Olivetto", Email = "122343@gmail.com", EnrollmentDate = DateTime.Parse("2005-09-01")}
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course { Title = "Prayer",         Credits = 3, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Spirituality",   Credits = 3, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Macroeconomics", Credits = 3, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Calculus",       Credits = 4, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Trigonometry",   Credits = 4, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Composition",    Credits = 3, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false},
                new Course { Title = "Literature",     Credits = 4, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false}
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentID = 1, CourseID = 1, Grade = "pass" },
                new Enrollment { StudentID = 1, CourseID = 2, Grade = "fail" },
                new Enrollment { StudentID = 1, CourseID = 3, Grade = "incomplete" },
                new Enrollment { StudentID = 2, CourseID = 4, Grade = "fail" },
                new Enrollment { StudentID = 2, CourseID = 5, Grade = "pass" },
                new Enrollment { StudentID = 2, CourseID = 6, Grade = "fail" },
                new Enrollment { StudentID = 3, CourseID = 1            },
                new Enrollment { StudentID = 4, CourseID = 1,           },
                new Enrollment { StudentID = 4, CourseID = 2, Grade = "incomplete" },
                new Enrollment { StudentID = 5, CourseID = 3, Grade = "pass" },
                new Enrollment { StudentID = 6, CourseID = 4            },
                new Enrollment { StudentID = 7, CourseID = 5, Grade = "pass" },
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();
        }
    }
}
