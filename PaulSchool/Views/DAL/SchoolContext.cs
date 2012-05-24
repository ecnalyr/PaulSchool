using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PaulSchool.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PaulSchool.Models
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; } // In Entity Framework terminology, an entity set typically corresponds to a database table, and an entity corresponds to a row in the table.
        
        public DbSet<Enrollment> Enrollments { get; set; }
        
        public DbSet<Course> Courses { get; set; }
        
        public DbSet<Instructor> Instructors { get; set; }
        
        public DbSet<CourseTemplates> CourseTemplates { get; set; }
        
        public DbSet<Attendance> Attendance { get; set; }

        public DbSet<Notification> Notification { get; set; }

        // Removes pluralization convention from database names
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}