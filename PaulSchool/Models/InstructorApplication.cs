using System.Collections.Generic;

namespace PaulSchool.Models
{
    public class InstructorApplication
    {
        public int InstructorApplicationID { get; set; }

        public int StudentID { get; set; }

        public virtual Student Student { get; set; }

        public List<EducationalBackground> EducationalBackground { get; set; }

        public string Experience { get; set; }

        public bool WillingToTravel { get; set; }

        public bool Approved { get; set; }
    }
}