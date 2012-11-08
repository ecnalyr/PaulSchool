using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Profile;
using System.Web.Security;

namespace PaulSchool.Models
{
    public class CustomProfile : ProfileBase
    {
        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "FilledStudentInfo must be yes or no")]
        [DisplayName("FilledstudentInfo")]
        [StringLength(5)]
        public string FilledStudentInfo
            // I used strings (instead of bool) because I was having difficulty using bool with ASP.NET's default Membership Provider's profile feature
        {
            get { return base["FilledStudentInfo"] as string; }
            set { base["FilledStudentInfo"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "IsTeacher must be yes or no")]
        [DisplayName("IsTeacher")]
        [StringLength(5)]
        public string IsTeacher
            // I used strings (instead of bool) because I was having difficulty using bool with ASP.NET's default Membership Provider's profile feature
        {
            get { return base["IsTeacher"] as string; }
            set { base["IsTeacher"] = value; }
        }

        [Required(ErrorMessage = "Must have a Last Name")]
        [DisplayName("Last Name")]
        [StringLength(20)]
        public string LastName
        {
            get { return base["LastName"] as string; }
            set { base["LastName"] = value; }
        }

        [Required(ErrorMessage = "Must have a First Name (you may include a Middle Name if you choose)")]
        [DisplayName("First and Middle Name")]
        [StringLength(20)]
        public string FirstMidName
        {
            get { return base["FirstMidName"] as string; }
            set { base["FirstMidName"] = value; }
        }

        [Required(ErrorMessage = "Must have a Street Address")]
        [DisplayName("Street Address")]
        [StringLength(30)]
        public string StreetAddress
        {
            get { return base["StreetAddress"] as string; }
            set { base["StreetAddress"] = value; }
        }

        [Required(ErrorMessage = "Must have a City")]
        [DisplayName("City")]
        [StringLength(30)]
        public string City
        {
            get { return base["City"] as string; }
            set { base["City"] = value; }
        }

        [Required(ErrorMessage = "Must have a State")]
        [DisplayName("State")]
        [StringLength(15)]
        public string State
        {
            get { return base["State"] as string; }
            set { base["State"] = value; }
        }

        [Required(ErrorMessage = "Must have a Zip Code")]
        [DisplayName("Zip Code")]
        [StringLength(5)]
        public string ZipCode
        {
            get { return base["ZipCode"] as string; }
            set { base["ZipCode"] = value; }
        }

        [Required(ErrorMessage = "Must have a Phone Number")]
        [DisplayName("Phone Number")]
        [StringLength(10)]
        public string Phone
        {
            get { return base["Phone"] as string; }
            set { base["Phone"] = value; }
        }

        [Required(ErrorMessage = "Must have a Date of Birth (MM/DD/YYYY)")]
        [DisplayName("Date of Birth")]
        [StringLength(10)]
        public string DateOfBirth
        {
            get { return base["DateOfBirth"] as string; }
            set { base["DateOfBirth"] = value; }
        }

        [DisplayName("Parish Affiliation")]
        [StringLength(30)]
        public string ParishAffiliation
        {
            get { return base["ParishAffiliation"] as string; }
            set { base["ParishAffiliation"] = value; }
        }

        [DisplayName("Ministry Involvement")]
        [StringLength(50)]
        public string MinistryInvolvement
        {
            get { return base["MinistryInvolvement"] as string; }
            set { base["MinistryInvolvement"] = value; }
        }

        public static CustomProfile GetUserProfile(string username)
        {
            return Create(username) as CustomProfile;
        }

        public static CustomProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as CustomProfile;
        }
    }
}