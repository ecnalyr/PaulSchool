using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PaulSchool.Models
{
    public class ApplicationCommissioning
    {
        public int Id { get; set; }

        public int StudentID { get; set; }

        public bool RecommendationFiled { get; set; }

        public RecommendationForCommissioning Recommendation { get; set; }

        [Required]
        [Display(Name = "Personal Statement")]
        [StringLength(1000)]
        public string PersonalStatement { get; set; }

        public bool DayOfReflection { get; set; }

        public bool ApplicationFeePaid { get; set; }

        public bool MeetsMinimumRequirements { get; set; }

        public DateTime DateFiled { get; set; }

        public string AdminDenialReason { get; set; }

        public virtual Student Student { get; set; }

        public bool Approved { get; set; }

    }
}