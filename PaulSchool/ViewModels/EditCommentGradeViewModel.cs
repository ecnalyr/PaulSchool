using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.ViewModels
{
    public class EditCommentGradeViewModel
    {
        [Display(Name = "Grade")]
        public IEnumerable<SelectListItem> Grades { get; set; }

        public int EnrollmentID { get; set; }

        public Student Student { get; set; }

        public string Grade { get; set; }

        public string Comments { get; set; }      
    }
}