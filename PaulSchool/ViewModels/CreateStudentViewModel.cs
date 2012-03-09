using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaulSchool.ViewModels
{
    public class CreateStudentViewModel
    {
        [Display(Name = "select user")]
        public string SelectedUser { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public string Email { get; set; }
    }
}