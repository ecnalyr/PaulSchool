using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaulSchool.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        
        public DateTime Time { get; set; }
        
        public string Details { get; set; }
        
        public string Link { get; set; }
        
        public string ViewableBy { get; set; } // "Admin" or 'UserName/ID'
        
        public bool Complete { get; set; }
    }
}