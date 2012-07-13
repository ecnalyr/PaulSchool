using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;
using System.Globalization;

namespace PaulSchool.Controllers
{
    using System.Diagnostics;

    [HandleError]
    public class CertificateController : PdfController
    {
        private readonly SchoolContext db = new SchoolContext();

        public ActionResult CertificateOfAttendance(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            var dateWithOrdinals = AddOrdinal(enrollment.Course.EndDate.Day);
            ViewBag.dateWithOrdinal = dateWithOrdinals;
            return View(enrollment);
        }

        public ActionResult CertificateOfCommissioning(int id)
        {
            ApplicationCommissioning commissioning = db.ApplicationCommissionings.Find(id);
            return View(commissioning);
        }

        public string AddOrdinal(int num)
        {
            switch (num % 100)
            {
                case 11:
                case 12:
                case 13:
                    return num.ToString() + "th";
            }

            switch (num % 10)
            {
                case 1:
                    return num.ToString() + "st";
                case 2:
                    return num.ToString() + "nd";
                case 3:
                    return num.ToString() + "rd";
                default:
                    return num.ToString() + "th";
            }

        }

    }
}
