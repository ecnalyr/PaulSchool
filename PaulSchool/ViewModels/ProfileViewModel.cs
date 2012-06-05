using System.ComponentModel.DataAnnotations;

namespace PaulSchool.ViewModels
{
    public class ProfileViewModel
    {
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
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Must have a Phone Number")]
        [Display(Name = "Phone Number")]
        [StringLength(10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Must have a Date of Birth (MM/DD/YYYY)")]
        [Display(Name = "Date of Birth")]
        [StringLength(10)]
        public string DateOfBirth { get; set; }

        [Display(Name = "Parish Affiliation")]
        [StringLength(30)]
        public string ParishAffiliation { get; set; }

        [Display(Name = "Ministry Involvement")]
        [StringLength(50)]
        public string MinistryInvolvement { get; set; }
    }
}