using System;
using System.ComponentModel.DataAnnotations;

namespace PaulSchool.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }

        public DateTime Time { get; set; }

        public string Details { get; set; }

        public string Link { get; set; }

        public string ViewableBy { get; set; } // "Admin" or 'UserName/ID'

        [UIHint("IsChecked")]
        public bool Complete { get; set; }

        [UIHint("IsChecked")]
        public bool PreviouslyRead { get; set; }

        public Notification()
        {
            PreviouslyRead = false;
        }
    }
}