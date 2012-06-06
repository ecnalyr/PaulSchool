﻿using System.Collections.Generic;

namespace PaulSchool.Models
{
    public class InstructorApplication
    {
        public int InstructorApplicationID { get; set; }

        public Student BasicInfoGatheredFromProfile { get; set; }

        public virtual ICollection<EducationalBackground> EducationalBackground { get; set; }

        public string Experience { get; set; }

        public bool WillingToTravel { get; set; }
    }
}