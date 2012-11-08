using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PaulSchool.ViewModels
{
    public class CreateStudentViewModel
    {
        [Display(Name = "selected user")]
        public string SelectedUser { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstMidName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}