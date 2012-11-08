using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
namespace PaulSchool.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [Email]
        public string Email { get; set; }
    }
}