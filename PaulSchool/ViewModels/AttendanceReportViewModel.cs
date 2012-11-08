using System.Collections.Generic;
using System.Linq;
using PaulSchool.Models;
using PaulSchool.Resources;

namespace PaulSchool.ViewModels
{
    public class AttendanceReportViewModel
    {
        public List<int> AttendanceDays { get; set; }

        public List<Student> Students { get; set; }

        public List<Attendance> Attendances { get; set; }

        public IEnumerable<Enrollment> Enrollments { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string Comments { get; set; }

        public bool Paid { get; set; }

        public Enrollment SingleStudentEnrollment { get; set; }

        public string IsPresent(Student student, int attendanceDay)
        {
            return Attendances.Single(a => a.StudentID == student.StudentID && a.AttendanceDay == attendanceDay).Present
                       ? PaulSchoolResource.Present_Text
                       : PaulSchoolResource.Absent_Text;
        }

    }
}