using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulSchool.Models
{
    public class CourseTemplates
    {
        public int CourseTemplatesID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public bool Elective { get; set; }
        public int AttendingDays { get; set; } // 10, 8, 3, or custom
        public int AttendanceCap { get; set; } // default of 30
        public string Location { get; set; }
        public string Parish { get; set; }
        public string Description { get; set; }
    }
}