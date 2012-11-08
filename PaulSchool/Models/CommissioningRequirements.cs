using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulSchool.Models
{
    public class CommissioningRequirements
    {
        public int Id { get; set; }

        public int CoreCoursesRequired { get; set; }

        public int ElectiveCoursesRequired { get; set; }
    }
}