using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PaulSchool.Models;

namespace PaulSchool.ViewModels
{
    public class AttendanceViewModel
    {
        public IEnumerable<Attendance> AttendanceItems { get; set; }
    }
}