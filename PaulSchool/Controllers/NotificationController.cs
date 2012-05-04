using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaulSchool.Models;

namespace PaulSchool.Controllers
{ 
    public class NotificationController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Notification/

        public ViewResult Index()
        {
            return View(db.Notification.ToList());
        }

        //
        // GET: /Notification/Details/5

        public ViewResult Details(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        //
        // GET: /Notification/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Notification/Create

        [HttpPost]
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
        
        //
        // GET: /Notification/Edit/5
 
        public ActionResult Edit(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        //
        // POST: /Notification/Edit/5

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

        //
        // GET: /Notification/Delete/5
 
        public ActionResult Delete(int id)
        {
            Notification notification = db.Notification.Find(id);
            return View(notification);
        }

        //
        // POST: /Notification/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Notification notification = db.Notification.Find(id);
            db.Notification.Remove(notification);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}