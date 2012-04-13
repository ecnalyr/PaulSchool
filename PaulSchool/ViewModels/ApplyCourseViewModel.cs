using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaulSchool.ViewModels
{
    public class ApplyCourseViewModel
    {
        [Display(Name = "selected course")]
        public string SelectedCourse { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }
    }
}