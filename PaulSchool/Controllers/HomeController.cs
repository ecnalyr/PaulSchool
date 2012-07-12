using System.Web.Mvc;
using PaulSchool.Models;
using System.Linq;

namespace PaulSchool.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public ActionResult Index()
        {
            ViewBag.Message = "St. Paul School of Catechesis";

            var newNotifications = db.Notification.FirstOrDefault(o => o.ViewableBy == User.Identity.Name && o.Complete == false);
            if (newNotifications != null)
            {
                TempData["notificationMessage"] =
                    "You have an unchecked notification.  Please visit the Notification tab and tend to this notification.";
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}