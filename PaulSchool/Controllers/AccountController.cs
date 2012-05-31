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
                    return logUserIn(model, returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private ActionResult logUserIn(LogOnModel model, string returnUrl)
        {
            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
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
                    return createAndSaveNewUserWithStudentProfileAndCookie(model);
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private ActionResult createAndSaveNewUserWithStudentProfileAndCookie(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                CustomProfile profile = CustomProfile.GetUserProfile(model.UserName);

                setDefaultStateOfUser(model, profile);

                saveNewProfile(model, profile);

                // check if already existing on the student table - update the table if needed
                Student isStudent = db.Students.FirstOrDefault(
                    o => o.UserName == User.Identity.Name);
                if (isStudent != null) // IF the user already exists in the Student Table . . .
                {
                    updateStudentsTableWithUpdatedProfileDataFromRegisterModel(model, isStudent);
                }
                else
                // Create student in student table if they have not been there before (everyone needs to be at least a student)
                {
                    createStudentTableDataWithNewProfileDataAndAssignStudentRole(model);
                }

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        private static void setDefaultStateOfUser(RegisterModel model, CustomProfile profile)
        {
            profile.IsTeacher = "no";
            profile.FilledStudentInfo = "no";
            profile.Save();
            Roles.AddUserToRole(model.UserName, "Default"); // Adds student to the "Default" role so we can force them to become a "Student" role.
        }

        private static void saveNewProfile(RegisterModel model, CustomProfile profile)
        {
            profile.LastName = model.LastName;
            profile.FirstMidName = model.FirstMidName;
            profile.StreetAddress = model.StreetAddress;
            profile.City = model.City;
            profile.State = model.State;
            profile.ZipCode = model.ZipCode;
            profile.Phone = model.Phone;
            profile.DateOfBirth = model.DateOfBirth;
            profile.ParishAffiliation = model.ParishAffiliation;
            profile.MinistryInvolvement = model.MinistryInvolvement;
            profile.Save();
        }

        private void updateStudentsTableWithUpdatedProfileDataFromRegisterModel(RegisterModel model, Student isStudent)
        {
            isStudent.LastName = model.LastName;
            isStudent.FirstMidName = model.FirstMidName;
            isStudent.StreetAddress = model.StreetAddress;
            isStudent.City = model.City;
            isStudent.State = model.State;
            isStudent.ZipCode = model.ZipCode;
            isStudent.Phone = model.Phone;
            isStudent.DateOfBirth = model.DateOfBirth;
            isStudent.ParishAffiliation = model.ParishAffiliation;
            isStudent.MinistryInvolvement = model.MinistryInvolvement;
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for isStudent.Email = u.Email;
            isStudent.Email = u.Email;
            db.SaveChanges();
        }

        private void createStudentTableDataWithNewProfileDataAndAssignStudentRole(RegisterModel model)
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for Email = u.Email;
            Student newStudent = new Student
            {
                LastName = model.LastName,
                FirstMidName = model.FirstMidName,
                Email = model.Email,
                UserName = model.UserName,
                EnrollmentDate = DateTime.Now,
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                ParishAffiliation = model.ParishAffiliation,
                MinistryInvolvement = model.MinistryInvolvement
            };
            db.Students.Add(newStudent);
            db.SaveChanges();
            if (!User.IsInRole("Student"))
            {
                Roles.AddUserToRole(model.UserName, "Student");
            }
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
                if (userName == null || (userName != null && userName == User.Identity.Name)) 
                {
                    updateProfileEmailInAllLocations(email);
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

        private void updateProfileEmailInAllLocations(string email)
        {
            // change email
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            u.Email = email;
            System.Web.Security.Membership.UpdateUser(u);

            //
            // Update Student Table, if needed
            updateCurrentUsersStudentTableEntrysEmailIfNeeded(email);

            //
            // Update Instructor Table, if needed
            updateCurrentUsersInstructorTableEntrysEmailIfNeeded(email);
        }

        private void updateCurrentUsersInstructorTableEntrysEmailIfNeeded(string email)
        {
            Instructor isInstructor = db.Instructors.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isInstructor != null) // IF the user already exists in the Instructor Table . . .
            {
                isInstructor.Email = email;
                db.SaveChanges();
            }
        }

        private void updateCurrentUsersStudentTableEntrysEmailIfNeeded(string email)
        {
            Student isStudent = db.Students.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isStudent != null) // IF the user already exists in the Student Table . . .
            {
                isStudent.Email = email;
                db.SaveChanges();
            }
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
            ProfileViewModel model = preloadProfileViewModelWithCurrentUsersProfileData();
            return View(model);
        }

        private ProfileViewModel preloadProfileViewModelWithCurrentUsersProfileData()
        {
            CustomProfile profile = CustomProfile.GetUserProfile(User.Identity.Name);
            ProfileViewModel model = new ProfileViewModel
            {
                LastName = profile.LastName,
                FirstMidName = profile.FirstMidName,
                StreetAddress = profile.StreetAddress,
                City = profile.City,
                State = profile.State,
                ZipCode = profile.ZipCode,
                Phone = profile.Phone,
                DateOfBirth = profile.DateOfBirth,
                ParishAffiliation = profile.ParishAffiliation,
                MinistryInvolvement = profile.MinistryInvolvement
            };
            return model;
        }

        //
        // POST: /Account/ChangeProfile

        [Authorize]
        [HttpPost]
        public ActionResult ChangeProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // validation succeeded => process the results
            // save the profile data
            saveNewProfileData(model);

            // check if already existing on the student table - update the table if needed
            Student isStudent = db.Students.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isStudent != null) // IF the user already exists in the Student Table . . .
            {
                updateStudentsTableWithUpdatedProfileDataFromProfileViewModel(model, isStudent);
            }
            else
            // Create student in student table if they have not been there before (everyone needs to be at least a student)
            {
                createStudentInStudentTableFromProfileViewModel(model);
            }

            // check if already existing on the instructor table - update the table if needed
            Instructor isInstructor = db.Instructors.FirstOrDefault(
                o => o.UserName == User.Identity.Name);
            if (isInstructor != null) // IF the user already exists in the Instructor Table . . .
            {
                updateInstructorsTableWithUpdatedProfileDataFromProfileViewModel(model, isInstructor);
            }

            return RedirectToAction("Profile");
        }

        private static void saveNewProfileData(ProfileViewModel model)
        {
            CustomProfile profile = CustomProfile.GetUserProfile();
            profile.LastName = model.LastName;
            profile.FirstMidName = model.FirstMidName;
            profile.StreetAddress = model.StreetAddress;
            profile.City = model.City;
            profile.State = model.State;
            profile.ZipCode = model.ZipCode;
            profile.Phone = model.Phone;
            profile.DateOfBirth = model.DateOfBirth;
            profile.ParishAffiliation = model.ParishAffiliation;
            profile.MinistryInvolvement = model.MinistryInvolvement;
            profile.Save();
        }

        private void updateStudentsTableWithUpdatedProfileDataFromProfileViewModel(ProfileViewModel model, Student isStudent)
        {
            isStudent.LastName = model.LastName;
            isStudent.FirstMidName = model.FirstMidName;
            isStudent.StreetAddress = model.StreetAddress;
            isStudent.City = model.City;
            isStudent.State = model.State;
            isStudent.ZipCode = model.ZipCode;
            isStudent.Phone = model.Phone;
            isStudent.DateOfBirth = model.DateOfBirth;
            isStudent.ParishAffiliation = model.ParishAffiliation;
            isStudent.MinistryInvolvement = model.MinistryInvolvement;
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for isStudent.Email = u.Email;
            isStudent.Email = u.Email;
            db.SaveChanges();
        }

        private void createStudentInStudentTableFromProfileViewModel(ProfileViewModel model)
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for Email = u.Email;
            Student newStudent = new Student
            {
                LastName = model.LastName,
                FirstMidName = model.FirstMidName,
                Email = u.Email,
                UserName = User.Identity.Name,
                EnrollmentDate = DateTime.Now,
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                ZipCode = model.ZipCode,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                ParishAffiliation = model.ParishAffiliation,
                MinistryInvolvement = model.MinistryInvolvement
            };
            db.Students.Add(newStudent);
            db.SaveChanges();
            if (!User.IsInRole("Student"))
            {
                Roles.AddUserToRole(User.Identity.Name, "Student");
            }
        }

        private void updateInstructorsTableWithUpdatedProfileDataFromProfileViewModel(ProfileViewModel model, Instructor isInstructor)
        {
            isInstructor.LastName = model.LastName;
            isInstructor.FirstMidName = model.FirstMidName;
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for isInstructor.Email = u.Email;
            isInstructor.Email = u.Email;
            db.SaveChanges();
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

        public class ProfileRequiredActionFilter : IActionFilter
        {
            #region Implementation of IActionFilter

            public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //TODO: Check if the Authenticated User has a profile.
            
            //var authorized = Roles.IsUserInRole(User, "Student");
            //var authorized = Roles.IsUserInRole(User.Identity.Name, "Student");

            //If Authenicated User doesn't have a profile...
            filterContext.Result = new RedirectResult("Path-To-Create-A-Profile");
        }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {
            }

            #endregion
        }
    }
}
