using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PaulSchool.Models;

namespace PaulSchool.ViewModels
{
    public class AttendanceReportViewModel
    {
        public List<int> AttendanceDays { get; set; }
        
        public List<Student> Students { get; set; }
        
        public List<Attendance> Attendances { get; set; }

        public string IsPresent(Student student, int attendanceDay)
        {
            return Attendances.Single(a => a.StudentID == student.StudentID && a.AttendanceDay == attendanceDay).Present ? "present" : "absent";
        }
    }
}