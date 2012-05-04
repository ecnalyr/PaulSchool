using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PaulSchool.Models;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    public class AccountController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    // Need to identify the user because the user is not officially 'logged in' yet.
                    MembershipUser u = Membership.GetUser(model.UserName);
                    var authorized = Roles.IsUserInRole(model.UserName, "Student");
                    if (authorized)
                        // If the user already has the Student role, they can be redirected as normal, 
                        // if not, they need to fill out their profile details.
                    {
                        //FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Profile", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    CustomProfile profile = CustomProfile.GetUserProfile(model.UserName);
                    //Set default state of user
                    profile.IsTeacher = "no";
                    profile.FilledStudentInfo = "no";
                    profile.Save();
                    Roles.AddUserToRole(model.UserName, "Default"); // Adds student to the "Default" role so we can force them to become a "Student" role.
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/ChangeEmail

        public ActionResult ChangeEmail()
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            ViewBag.Email1 = u.Email;
            var model = new ChangeEmailViewModel
            {
                Email = u.Email
            };
            return View(model);
        }

        //
        // POST: /Account/ChangeEmail

        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email;
                string userName = Membership.GetUserNameByEmail(email);
                // checks if there is a duplicate email in the database
                // the following code also executes if user attmps to change their email to the same email that they were already using
                if (userName == null || (userName != null && userName == User.Identity.Name)) 
                {
                    // change email
                    MembershipUser u = Membership.GetUser(User.Identity.Name);
                    u.Email = email;
                    System.Web.Security.Membership.UpdateUser(u);

                    //
                    // Update Student Table, if needed
                    Student isStudent = db.Students.FirstOrDefault(
                        o => o.UserName == User.Identity.Name);
                    if (isStudent != null) // IF the user already exists in the Student Table . . .
                    {
                        isStudent.Email = email;
                        db.SaveChanges();
                    }

                    //
                    // Update Instructor Table, if needed
                    Instructor isInstructor = db.Instructors.FirstOrDefault(
                        o => o.UserName == User.Identity.Name);
                    if (isInstructor != null) // IF the user already exists in the Instructor Table . . .
                    {
                        isInstructor.Email = email;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Profile");
                }
                else
                {
                    return RedirectToAction("failed");
                    // don't allow email change as that email is already in use
                }

            }

            return View(model);
        }


        //
        // GET: /Account/Profile

        [Authorize]
        public ActionResult Profile()
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            ViewBag.Email = u.Email;
            return View();
        }

        //
        // Get /Account/ChangeProfile

        [Authorize]
        public ActionResult ChangeProfile()
        {
            CustomProfile profile = CustomProfile.GetUserProfile(User.Identity.Name);
            ProfileViewModel model = new ProfileViewModel
            {
                LastName = profile.LastName,
                FirstMidName = profile.FirstMidName
            };
            return View(model);
        }

        //
        // POST: /Account/ChangeProfile

        [Authorize]
        [HttpPost]
        public ActionResult ChangeProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // there were validation errors => redisplay the view
                return View(model);
            }

            // validation succeeded => process the results
            // save the profile data
            CustomProfile profile = CustomProfile.GetUserProfile();
            profile.LastName = model.LastName;
            profile.FirstMidName = model.FirstMidName;
            profile.Save();

            // check if already existing on the student table - update the table if needed
            Student isStudent = db.Students.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isStudent != null) // IF the user already exists in the Student Table . . .
            {
                isStudent.LastName = model.LastName;
                isStudent.FirstMidName = model.FirstMidName;
                MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for isStudent.Email = u.Email;
                isStudent.Email = u.Email;
                db.SaveChanges();
            }
            else
            // Create student in student table if they have not been there before (everyone needs to be at least a student)
            {
                MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for Email = u.Email;
                Student newStudent = new Student
                {
                    LastName = model.LastName,
                    FirstMidName = model.FirstMidName,
                    Email = u.Email,
                    UserName = User.Identity.Name,
                    EnrollmentDate = DateTime.Now
                };
                db.Students.Add(newStudent);
                db.SaveChanges();
                if (!User.IsInRole("Student"))
                {
                    Roles.AddUserToRole(User.Identity.Name, "Student");
                }
            }

            // check if already existing on the instructor table - update the table if needed
            Instructor isInstructor = db.Instructors.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isInstructor != null) // IF the user already exists in the Instructor Table . . .
            {
                isInstructor.LastName = model.LastName;
                isInstructor.FirstMidName = model.FirstMidName;
                MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for isInstructor.Email = u.Email;
                isInstructor.Email = u.Email;
                db.SaveChanges();
            }

            return RedirectToAction("Profile");
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
