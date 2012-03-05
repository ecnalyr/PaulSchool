using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaulSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "St. Paul School of Catechesis";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
