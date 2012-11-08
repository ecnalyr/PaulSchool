using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PaulSchool.Controllers
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text;

    using PaulSchool.Models;

    public class PaypalController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public ActionResult IPN()
        {
            Debug.Write("entering ipn action ");
            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");

            string response = GetPayPalResponse(formVals, true);
            Debug.Write("IPN Response received: " + response + " <-- That was response. . . ");

            if (response == "VERIFIED")
            {
                Debug.Write("VERIFIED RESPONSE");
                string stringAmountPaid = Request["mc_gross"];
                string stringEnrollmentID = Request["item_number"];


                //validate the transaction
                Decimal amountPaid = 0;
                Decimal.TryParse(stringAmountPaid, out amountPaid);

                int enrollmentID = Convert.ToInt32(stringEnrollmentID);
                Debug.Write("This is stringEnrollmentID: "+ stringEnrollmentID+ " <-- ");
                Debug.Write("This is enrollmentID: " + enrollmentID + " <-- ");
                Debug.Write("This is The amount paid" + amountPaid + " <-- ");


                Enrollment enrollment = db.Enrollments.FirstOrDefault(p => p.EnrollmentID == enrollmentID);


                Course course = db.Courses.First(p => p.CourseID == enrollment.CourseID);

                if (AmountPaidIsValid(course, amountPaid))
                {
                    enrollment.Paid = true;
                    db.SaveChanges();

                    // the enrollment should now be marked as paid.
                }
            }

            else
            {
                Debug.Write("RESPONSE WAS NOT VERIFIED");
            }

            return this.View();
        }

        private bool AmountPaidIsValid(Course course, decimal amountPaid)
        {
            bool result = true;

            if (course != null)
            {
                if (course.Cost > amountPaid)
                {
                    result = false;
                }
            }
            return result;
        }

        string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
        {
            string paypalUrl = useSandbox
                                   ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
                                   : "https://www.paypal.com/cgi-bin/webscr";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);
            
            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] param = Request.BinaryRead(Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);

            StringBuilder sb = new StringBuilder();
            sb.Append(strRequest);

            foreach (string key in formVals.Keys)
            {
                sb.AppendFormat("&{0}={1}", key, formVals[key]);
            }
            strRequest += sb.ToString();
            req.ContentLength = strRequest.Length;

            string response = "";
            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                streamOut.Write(strRequest);
                streamOut.Close();
                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    response = streamIn.ReadToEnd();
                }
            }
            return response;
        }
    }
}
