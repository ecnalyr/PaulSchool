using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Profile;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaulSchool.Models
{
    public class CustomProfile : ProfileBase
    {
        public static CustomProfile GetUserProfile(string username)
        {
            return Create(username) as CustomProfile;
        }
        public static CustomProfile GetUserProfile()
        {
            return Create(Membership.GetUser().UserName) as CustomProfile;
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "FilledStudentInfo must be yes or no")]
        [DisplayName("FilledstudentInfo")]
        [StringLength(5)]
        public string FilledStudentInfo // I used strings (instead of bool) because I was having difficulty using bool with ASP.NET's default Membership Provider's profile feature
        {
            get { return base["FilledStudentInfo"] as string; }
            set { base["FilledStudentInfo"] = value; }
        }

        [SettingsAllowAnonymous(false)]
        [Required(ErrorMessage = "IsTeacher must be yes or no")]
        [DisplayName("IsTeacher")]
        [StringLength(5)]
        public string IsTeacher // I used strings (instead of bool) because I was having difficulty using bool with ASP.NET's default Membership Provider's profile feature
        {
            get { return base["IsTeacher"] as string; }
            set { base["IsTeacher"] = value; }
        }


        
    }
}