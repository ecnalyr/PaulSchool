namespace PaulSchool.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public int AttendanceDay { get; set; }

        public bool Present { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}