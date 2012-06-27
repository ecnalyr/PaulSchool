using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{
    [HandleError]
    public class CertificateController : PdfController
    {
        private readonly SchoolContext db = new SchoolContext();

        //
        // GET: /Certificate/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrdersInHtml()
        {
            Student c = (from stud in db.Students where stud.StudentID == 1 select stud).First();
            return View(c);
        }

        public ActionResult OrdersInPdf()
        {
            Student c = (from stud in db.Students where stud.StudentID == 1 select stud).First();
            // To render a PDF instead of an HTML, all we need to do is call ViewPdf instead of View. This
            // requires the controller to be inherited from MyController instead of MVC's Controller.
            return ViewPdf(c);
        }

    }
}
