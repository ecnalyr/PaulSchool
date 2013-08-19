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
            var newNotifications = db.Notification.FirstOrDefault(o => o.ViewableBy == User.Identity.Name && o.PreviouslyRead == false);
            if (newNotifications != null)
            {
                TempData["notificationMessage"] =
                    "You have an unchecked notification.  Please visit the Notification tab and tend to this notification.";
            }

            return View();
        }

        [RequireHttps]
        public ActionResult SslTest()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}