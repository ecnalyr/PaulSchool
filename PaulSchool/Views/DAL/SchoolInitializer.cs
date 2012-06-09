using System;
using System.Collections.Generic;
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
                new Student { FirstMidName = "Carson", UserName="TestUser", LastName = "Alexander", Email = "123@aol.com",     EnrollmentDate = DateTime.Parse("2005-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso", Email = "345@aol.com",        EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { FirstMidName = "Arturo",   LastName = "Anand", Email = "567@aol.com",         EnrollmentDate = DateTime.Parse("2003-09-01") },
                new Student { FirstMidName = "Gytis",    LastName = "Barzdukas", Email = "456@aol.com",     EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { FirstMidName = "Yan",      LastName = "Li", Email = "123@gmail.com",          EnrollmentDate = DateTime.Parse("2002-09-01") },
                new Student { FirstMidName = "Peggy",    LastName = "Justice", Email = "1765@gmail.com",    EnrollmentDate = DateTime.Parse("2001-09-01") },
                new Student { FirstMidName = "Laura",    LastName = "Norman", Email = "123423@gmail.com",   EnrollmentDate = DateTime.Parse("2003-09-01") },
                new Student { FirstMidName = "Nino",     LastName = "Olivetto", Email = "122343@gmail.com", EnrollmentDate = DateTime.Parse("2005-09-01") }
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();

            var instructors = new List<Instructor>
            {
                new Instructor { FirstMidName = "Mister", LastName = "Instructor", Email = "mrinstruc@email.com", EnrollmentDate = DateTime.Parse("2001-04-02"), UserName = "Instructor1" },
                new Instructor { FirstMidName = "Miss", LastName = "Teacher", Email = "instructmail@email.com", EnrollmentDate = DateTime.Parse("2002-06-03"), UserName = "Instructor2" }
            };
            instructors.ForEach(s => context.Instructors.Add(s));
            context.SaveChanges();

            var courses = new List<Course>
            {
                new Course { Title = "Prayer",          InstructorID = 1, Credits = 3, Elective = false, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Spirituality",    InstructorID = 1, Credits = 3, Elective = false, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Macroeconomics",  InstructorID = 2, Credits = 3, Elective = true, Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Calculus",        InstructorID = 1, Credits = 4, Elective = true,Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Trigonometry",    InstructorID = 1, Credits = 4, Elective = true,Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Composition",     InstructorID = 2, Credits = 3, Elective = false,Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false },
                new Course { Title = "Literature",      InstructorID = 1, Credits = 4, Elective = true,Year = 2011, AttendingDays = 10, AttendanceCap = 30, StartDate = DateTime.Parse("2012-01-01"), EndDate = DateTime.Parse("2012-02-01"), DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "AliceParish", Description = "Very Cool Course", Approved = true, Completed = false, Archived = false }
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            var courseTemplates = new List<CourseTemplates>
            {
                new CourseTemplates { Title = "Custom (edit title)", Credits = 3, Elective = false, AttendingDays = 10, AttendanceCap = 30 },
                new CourseTemplates { Title = "Prayer", Credits = 3, Elective = false, AttendingDays = 10,  AttendanceCap = 30, DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "GoodParish", Description = "A course taken in Corpus Christi" },
                new CourseTemplates { Title = "Spirituality", Credits = 3, Elective = false, AttendingDays = 10, AttendanceCap = 30, DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "GoodParish", Description = "A course taken in Corpus Christi" },
                new CourseTemplates { Title = "Macroeconomics", Credits = 3, Elective = true, AttendingDays = 8, AttendanceCap = 30, DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "GoodParish", Description = "A course taken in Corpus Christi" },
                new CourseTemplates { Title = "Calculus", Credits = 1, Elective = true, AttendingDays = 3, AttendanceCap = 25, DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "GoodParish", Description = "A course taken in Corpus Christi" },
                new CourseTemplates { Title = "Trigonometry", Credits = 1, Elective = true, AttendingDays = 8, AttendanceCap = 25, DurationHours = 1, DurationMins = 30, Location = "Corpus Christi", Parish = "GoodParish", Description = "A course taken in Corpus Christi" },
            };
            courseTemplates.ForEach(s => context.CourseTemplates.Add(s));
            context.SaveChanges();

            /*var enrollments = new List<Enrollment>
            {
                new Enrollment { StudentID = 1, CourseID = 1, Grade = "incomplete"},
                new Enrollment { StudentID = 1, CourseID = 2, Grade = "incomplete"},
                new Enrollment { StudentID = 1, CourseID = 3, Grade = "incomplete"},
                new Enrollment { StudentID = 2, CourseID = 4, Grade = "incomplete"},
                new Enrollment { StudentID = 2, CourseID = 5, Grade = "incomplete"},
                new Enrollment { StudentID = 2, CourseID = 6, Grade = "incomplete"},
                new Enrollment { StudentID = 3, CourseID = 1, Grade = "incomplete"},
                new Enrollment { StudentID = 4, CourseID = 1, Grade = "incomplete"},
                new Enrollment { StudentID = 4, CourseID = 2, Grade = "incomplete"},
                new Enrollment { StudentID = 5, CourseID = 3, Grade = "incomplete"},
                new Enrollment { StudentID = 6, CourseID = 4, Grade = "incomplete"},
                new Enrollment { StudentID = 7, CourseID = 5, Grade = "incomplete"},
            };
            enrollments.ForEach(s => context.Enrollments.Add(s));
            context.SaveChanges();*/
        }
    }
}
