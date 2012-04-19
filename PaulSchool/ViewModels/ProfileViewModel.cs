using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaulSchool.ViewModels
{
    public class ProfileViewModel
    {
        public string IsTeacher { get; set; }
        public string FilledStudentInfo { get; set; }
        public string LastName {get; set; }
        public string FirstMidName { get; set; }
    }
}
