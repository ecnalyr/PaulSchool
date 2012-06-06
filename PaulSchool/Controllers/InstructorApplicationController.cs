using System.Linq;
using System.Web.Mvc;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    public class InstructorApplicationController : Controller
    {
        private readonly SchoolContext db = new SchoolContext();
        //
        // GET: /InstructorApplication/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /InstructorApplication/ApplyToBecomeInstructor

        public ActionResult ApplyToBecomeInstructor()
        {
            Student thisStudent = db.Students.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            var model = new InstructorApplicationViewModel
                            {
                                BasicInfoGatheredFromProfile = thisStudent,
                                Experience = "1 year? 2 years?",
                                WillingToTravel = false,
                            };
            return View(model);
        }

        public PartialViewResult EducationalBackground()
        {
            return PartialView("_EducationalBackground", new InstructorApplicationViewModel());
        }
    }
}