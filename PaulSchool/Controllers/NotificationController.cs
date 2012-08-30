using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();

        public ViewResult Index()
        {
            if (User.IsInRole("Administrator"))
                // returns notifications viewable by Administrators
            {
                IQueryable<Notification> incompleteNotifications = db.Notification.Where(
                    o => o.Complete == false &&
                         o.ViewableBy == "Admin");
                return View(incompleteNotifications.ToList());
            }

            else
                // returns notifications by user name
            {
                IQueryable<Notification> incompleteNotifications = db.Notification.Where(
                    o => o.Complete == false &&
                         o.ViewableBy == User.Identity.Name);
                return View(incompleteNotifications.ToList());
            }
        }

        public ActionResult MarkPreviouslyReadTrue(int id)
        {
            Notification notification = db.Notification.Find(id);
            notification.PreviouslyRead = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MarkPreviouslyReadFalse(int id)
        {
            Notification notification = db.Notification.Find(id);
            notification.PreviouslyRead = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ViewResult Details(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Notification.Add(notification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notification);
        }

        public ActionResult Edit(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        [HttpPost]
        public ActionResult Edit(Notification notification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notification);
        }

        public ActionResult Delete(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Notification notification = db.Notification.Find(id);
            db.Notification.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Complete(int id)
        {
            Notification notification = db.Notification.Find(id);
            notification.Complete = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, SuperAdministrator")]
        public ActionResult EmailList()
        {
            IEnumerable<MembershipUser> users = Membership.GetAllUsers().Cast<MembershipUser>();
            ICollection<string> emailList = new List<string>();
            foreach (MembershipUser user in users)
            {
                string thisEmail = user.Email + ", ";
                emailList.Add(thisEmail);
            }
            return View(emailList);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}