using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace PaulSchool.ViewModels
{
    public class EmailViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [Email]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Subject")]
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Body")]
        [StringLength(2000)]
        public string Body { get; set; }
    }
}