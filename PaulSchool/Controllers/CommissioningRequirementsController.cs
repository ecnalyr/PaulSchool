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
    [Authorize(Roles = "SuperAdministrator")]
    public class CommissioningRequirementsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /CommissioningRequirements/

        public ViewResult Index()
        {
            return View(db.CommissioningRequirementse.ToList());
        }

        //
        // GET: /CommissioningRequirements/Details/5

        public ViewResult Details(int id)
        {
            CommissioningRequirements commissioningrequirements = db.CommissioningRequirementse.Find(id);
            return View(commissioningrequirements);
        }

        //
        // GET: /CommissioningRequirements/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /CommissioningRequirements/Create

        [HttpPost]
        public ActionResult Create(CommissioningRequirements commissioningrequirements)
        {
            if (ModelState.IsValid)
            {
                db.CommissioningRequirementse.Add(commissioningrequirements);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(commissioningrequirements);
        }
        
        //
        // GET: /CommissioningRequirements/Edit/5
 
        public ActionResult Edit(int id)
        {
            CommissioningRequirements commissioningrequirements = db.CommissioningRequirementse.Find(id);
            return View(commissioningrequirements);
        }

        //
        // POST: /CommissioningRequirements/Edit/5

        [HttpPost]
        public ActionResult Edit(CommissioningRequirements commissioningrequirements)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commissioningrequirements).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commissioningrequirements);
        }

        //
        // GET: /CommissioningRequirements/Delete/5
 
        public ActionResult Delete(int id)
        {
            CommissioningRequirements commissioningrequirements = db.CommissioningRequirementse.Find(id);
            return View(commissioningrequirements);
        }

        //
        // POST: /CommissioningRequirements/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            CommissioningRequirements commissioningrequirements = db.CommissioningRequirementse.Find(id);
            db.CommissioningRequirementse.Remove(commissioningrequirements);
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