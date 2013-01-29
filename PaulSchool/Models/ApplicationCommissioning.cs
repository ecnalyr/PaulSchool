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

        [UIHint("IsChecked")]
        public bool ReCommissioning { get; set; }

        [Display(Name = "Recommendation Filed")]
        [UIHint("IsChecked")]
        public bool RecommendationFiled { get; set; }

        public RecommendationForCommissioning Recommendation { get; set; }

        [Required]
        [Display(Name = "Personal Statement")]
        [StringLength(1000)]
        public string PersonalStatement { get; set; }

        [Display(Name = "Day of Reflection")]
        [UIHint("IsChecked")]
        public bool DayOfReflection { get; set; }

        [Display(Name = "Application Fee Paid?")]
        [UIHint("IsChecked")]
        public bool ApplicationFeePaid { get; set; }

        [Display(Name = "Meets Minimum Requirements?")]
        [UIHint("IsChecked")]
        public bool MeetsMinimumRequirements { get; set; }

        [Display(Name = "Date Filed")]
        public DateTime DateFiled { get; set; }

        [Display(Name = "Date Approved")]
        public DateTime? DateApproved { get; set; }

        [Display(Name = "Admin Denial Reason")]
        public string AdminDenialReason { get; set; }

        public virtual Student Student { get; set; }

        [UIHint("IsChecked")]
        public bool Approved { get; set; }

    }
}