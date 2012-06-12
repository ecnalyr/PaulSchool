using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataAnnotationsExtensions;


namespace PaulSchool.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [Email]
        public string Email { get; set; }

        // new
        [Required(ErrorMessage = "Must have a First Name (you may include a Middle Name if you choose)")]
        [Display(Name = "First and Middle Name")]
        [StringLength(20)]
        public string FirstMidName { get; set; }

        [Required(ErrorMessage = "Must have a Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Must have a Street Address")]
        [Display(Name = "Street Address")]
        [StringLength(30)]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Must have a City")]
        [Display(Name = "City")]
        [StringLength(30)]
        public string City { get; set; }

        [Required(ErrorMessage = "Must have a State")]
        [Display(Name = "State")]
        [StringLength(15)]
        public string State { get; set; }

        [Required(ErrorMessage = "Must have a Zip Code")]
        [Display(Name = "Zip Code")]
        [StringLength(5)]
        [Digits]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Must have a Phone Number")]
        [Display(Name = "Phone Number")]
        [StringLength(10)]
        [Digits]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Must have a Date of Birth (MM/DD/YYYY)")]
        [Display(Name = "Date of Birth")]
        [StringLength(10)]
        [Date]
        public string DateOfBirth { get; set; }

        [Display(Name = "Parish Affiliation")]
        [StringLength(30)]
        public string ParishAffiliation { get; set; }

        [Display(Name = "Ministry Involvement")]
        [StringLength(50)]
        public string MinistryInvolvement { get; set; }

        // old
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}