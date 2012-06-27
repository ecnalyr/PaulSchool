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

        public ActionResult PaypalTest()
        {
            return View();
        }

    }
}