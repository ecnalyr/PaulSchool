// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// <summary>
//   The account controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// <summary>

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Security;
using PaulSchool.Models;
using PaulSchool.Resources;
using PaulSchool.ViewModels;

namespace PaulSchool.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : Controller
    {
        #region Fields

        /// <summary>
        /// The db.
        /// </summary>
        private readonly SchoolContext db = new SchoolContext();

        #endregion

        #region Public Methods and Operators

        private static SelectList StateList
        {
            get
            {
                return new SelectList(new[]
                    {
                        new {Value = "1", Text = "AK"},
                        new {Value = "2", Text = "AL"},
                        new {Value = "3", Text = "AR"},
                        new {Value = "4", Text = "AZ"},
                        new {Value = "5", Text = "CA"},
                        new {Value = "6", Text = "CO"},
                        new {Value = "7", Text = "CT"},
                        new {Value = "8", Text = "DC"},
                        new {Value = "9", Text = "DE"},
                        new {Value = "10", Text = "FL"},
                        new {Value = "11", Text = "GA"},
                        new {Value = "12", Text = "HI"},
                        new {Value = "13", Text = "IA"},
                        new {Value = "14", Text = "ID"},
                        new {Value = "15", Text = "IL"},
                        new {Value = "16", Text = "IN"},
                        new {Value = "17", Text = "KS"},
                        new {Value = "18", Text = "KY"},
                        new {Value = "19", Text = "LA"},
                        new {Value = "20", Text = "MA"},
                        new {Value = "21", Text = "MD"},
                        new {Value = "22", Text = "ME"},
                        new {Value = "23", Text = "MI"},
                        new {Value = "24", Text = "MN"},
                        new {Value = "25", Text = "MO"},
                        new {Value = "26", Text = "MS"},
                        new {Value = "27", Text = "MT"},
                        new {Value = "28", Text = "NC"},
                        new {Value = "29", Text = "ND"},
                        new {Value = "30", Text = "NE"},
                        new {Value = "31", Text = "NH"},
                        new {Value = "32", Text = "NJ"},
                        new {Value = "33", Text = "NM"},
                        new {Value = "34", Text = "NV"},
                        new {Value = "35", Text = "NY"},
                        new {Value = "36", Text = "OH"},
                        new {Value = "37", Text = "OK"},
                        new {Value = "38", Text = "OR"},
                        new {Value = "39", Text = "PA"},
                        new {Value = "40", Text = "RI"},
                        new {Value = "41", Text = "SC"},
                        new {Value = "42", Text = "SD"},
                        new {Value = "43", Text = "TN"},
                        new {Value = "44", Text = "TX"},
                        new {Value = "45", Text = "UT"},
                        new {Value = "46", Text = "VA"},
                        new {Value = "47", Text = "VT"},
                        new {Value = "48", Text = "WA"},
                        new {Value = "49", Text = "WI"},
                        new {Value = "50", Text = "WV"},
                        new {Value = "51", Text = "WY"}
                    }, "Value", "Text");
            }
        }

        /// <summary>
        /// Changes logged-in user's email
        /// </summary>
        /// <returns>
        /// ChangeEmailViewModel with current user's email.
        /// </returns>
        public ActionResult ChangeEmail()
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            ViewBag.Email1 = u.Email;
            var model = new ChangeEmailViewModel {Email = u.Email};
            return View(model);
        }

        // POST: /Account/ChangeEmail

        /// <summary>
        /// Changes logged-in user's email
        /// </summary>
        /// <param name="model">
        /// The model with the user's desired email.
        /// </param>
        /// <returns>
        /// Either the user's profile (with the updated email), or an error because the email was in use by someone else.
        /// </returns>
        [HttpPost]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = model.Email;
                string userName = Membership.GetUserNameByEmail(email);

                // checks if there is a duplicate email in the database
                if (userName == null || (userName == User.Identity.Name && userName != null))
                {
                    UpdateProfileEmailInAllLocations(email);
                    return RedirectToAction("Profile");
                }

                return RedirectToAction("failed");

                // don't allow email change as that email is already in use
            }

            return View(model);
        }

        /// <summary>
        /// Changes the current user's password
        /// </summary>
        /// <returns>
        /// ChangePassword view.
        /// </returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword

        /// <summary>
        /// Changes the current user's password
        /// </summary>
        /// <param name="model">
        /// The model with the user's desired password.
        /// </param>
        /// <returns>
        /// Password change success or fail screen.
        /// </returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded = false;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    if (currentUser != null)
                    {
                        changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                    }
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError(
                    string.Empty,
                    PaulSchoolResource.
                        AccountController_ChangePassword_The_current_password_is_incorrect_or_the_new_password_is_invalid_);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ChangePasswordSuccess

        /// <summary>
        /// Change password success view.
        /// </summary>
        /// <returns>
        /// Shows password change as a success.
        /// </returns>
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /// <summary>
        /// The change profile page allows a user to change their profile. 
        /// This page will be pre-loaded with the user's current profile data (if there is any to preload)
        /// </summary>
        /// <returns>
        /// ProfileViewModel pre-loaded with the current user's current profile data.
        /// </returns>
        [Authorize]
        public ActionResult ChangeProfile()
        {
            ProfileViewModel model = PreloadProfileViewModelWithCurrentUsersProfileData();
            model.State = StateList;
            model.StateInt = 44;
            return View(model);
        }

        /// <summary>
        /// The change profile page allows a user to change their profile. 
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The view of the user's profile with the updated profile data.
        /// </returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangeProfile(ProfileViewModel model, int stateInt)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string stringStateAbbreviation = StateList.First(m => m.Value == stateInt.ToString()).Text;
            SaveNewProfileData(model, stringStateAbbreviation);

            // check if already existing on the student table - update the table if needed
            Student isStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);
            if (isStudent != null)
            {
                // IF the user already exists in the Student Table . . .
                UpdateStudentsTableWithUpdatedProfileDataFromProfileViewModel(model, isStudent, stringStateAbbreviation);
            }
            else
            {
                // Create student in student table if they have not been there before (everyone needs to be at least a student)
                CreateStudentInStudentTableFromProfileViewModel(model, stringStateAbbreviation);
            }

            // check if already existing on the instructor table - update the table if needed
            Instructor isInstructor = db.Instructors.FirstOrDefault(o => o.UserName == User.Identity.Name);
            if (isInstructor != null)
            {
                // IF the user already exists in the Instructor Table . . .
                UpdateInstructorsTableWithUpdatedProfileDataFromProfileViewModel(model, isInstructor);
            }

            return RedirectToAction("Profile");
        }

        /// <summary>
        /// The log off.
        /// </summary>
        /// <returns>
        /// Home page for website.
        /// </returns>
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/LogOn

        /// <summary>
        /// The log on.
        /// </summary>
        /// <returns>
        /// User log on page.
        /// </returns>
        public ActionResult LogOn()
        {
            return View();
        }

        // POST: /Account/LogOn

        /// <summary>
        /// The log on.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The page the user was trying to go to before they were prompted to log on, or an error message.
        /// </returns>
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    Notification newNotifications =
                        db.Notification.FirstOrDefault(o => o.ViewableBy == model.UserName && o.PreviouslyRead == false);
                    if (newNotifications != null)
                    {
                        TempData["notificationMessage"] =
                            "You have an unchecked notification.  Please visit the Notification tab and tend to this notification.";
                    }
                    
                    return LogUserIn(model, returnUrl);
                }

                ModelState.AddModelError(
                    string.Empty,
                    PaulSchoolResource.AccountController_LogOn_The_user_name_or_password_provided_is_incorrect_);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// The profile.
        /// </summary>
        /// <returns>
        /// Shows the current user's profile view.
        /// </returns>
        [Authorize]
        public ActionResult Profile()
        {
            ProfileViewModel model = PreloadProfileViewModelWithCurrentUsersProfileData();
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            if (u != null)
            {
                ViewBag.Email = u.Email;
            }
            IOrderedQueryable<ApplicationCommissioning> usersCommissionings =
                db.ApplicationCommissionings.Where(o => o.Student.UserName == User.Identity.Name && o.Approved).
                    OrderByDescending(m => m.DateApproved);
            ApplicationCommissioning usersMostRecentCommissioning = usersCommissionings.FirstOrDefault();
            if (usersMostRecentCommissioning != null)
                ViewBag.mostRecentCommissioningCertificateID = usersMostRecentCommissioning.Id;
            return View(model);
        }

        // GET: /Account/Register

        /// <summary>
        /// Account registration
        /// </summary>
        /// <returns>
        /// Account registration View
        /// </returns>
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            model.State = StateList;
            model.StateInt = 44;
            return View(model);
        }

        // POST: /Account/Register

        /// <summary>
        /// Allows a user to fill in new registration information to create a new user account
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// or redisplays the register form view.
        /// </returns>
        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel, int stateInt)
        {
            string stringStateAbbreviation = StateList.First(m => m.Value == stateInt.ToString()).Text;

            if (ModelState.IsValid)
            {
                var model = new RegisterModel
                    {
                        UserName = viewModel.UserName,
                        Email = viewModel.Email,
                        FirstMidName = viewModel.FirstMidName,
                        LastName = viewModel.LastName,
                        StreetAddress = viewModel.StreetAddress,
                        City = viewModel.City,
                        State = stringStateAbbreviation,
                        ZipCode = viewModel.ZipCode,
                        Phone = viewModel.Phone,
                        DateOfBirth = viewModel.DateOfBirth,
                        ParishAffiliation = viewModel.ParishAffiliation,
                        MinistryInvolvement = viewModel.MinistryInvolvement,
                        Password = viewModel.Password,
                        ConfirmPassword = viewModel.ConfirmPassword
                    };
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(
                    model.UserName, model.Password, model.Email, null, null, false, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    //FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    CustomProfile profile = CustomProfile.GetUserProfile(model.UserName);

                    SetDefaultStateOfUser(model, profile);

                    SaveNewProfile(model, profile);

                    MembershipUser user = Membership.GetUser(model.UserName, false);


                    AccountMembershipService.SendConfirmationEmail(user);
                    return RedirectToAction("confirmation");
                }

                ModelState.AddModelError(string.Empty, ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            viewModel.State = StateList;
            return View(viewModel);
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult PasswordReset()
        {
            return View("PasswordReset");
        }

        [HttpPost]
        public ActionResult PasswordReset(PasswordResetViewModel model)
        {
            string emailAddress = model.email;
            string user = Membership.GetUserNameByEmail(emailAddress);
            AccountMembershipService.ChangePassword(user);
            TempData["tempMessage"] =
                "You have reset your password, please retrieve it from your email inbox and log on.";
            return RedirectToAction("LogOn");
        }

        public ActionResult Verify(string id)
        {
            if (string.IsNullOrEmpty(id) || (!Regex.IsMatch(id, @"[0-9a-f]{8}\-([0-9a-f]{4}\-){3}[0-9a-f]{12}")))
            {
                ViewBag.Message = "The user account is not valid.";
                return View();
            }

            else
            {
                MembershipUser user = Membership.GetUser(new Guid(id));
                if (!user.IsApproved)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    FormsAuthentication.SetAuthCookie(user.UserName, false /* createPersistentCookie */);

                    CustomProfile profile = CustomProfile.GetUserProfile(user.UserName);
                    // check if already existing on the student table - update the table if needed
                    Student isStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);
                    if (isStudent != null)
                    {
                        // IF the user already exists in the Student Table . . .
                        UpdateStudentsTableWithUpdatedProfileDataFromRegisterModel(profile, isStudent);
                    }
                    else
                    {
                        // Create student in student table if they have not been there before (everyone needs to be at least a student)
                        CreateStudentTableDataWithNewProfileDataAndAssignStudentRole(profile, user);
                    }

                    return RedirectToAction("Welcome");
                }
                else
                {
                    FormsAuthentication.SignOut();
                    TempData["tempMessage"] = "You have already confirmed your email address... please log in.";
                    return RedirectToAction("LogOn");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays error codes as strings
        /// </summary>
        /// <param name="createStatus">
        /// The create status.
        /// </param>
        /// <returns>
        /// The error code to string.
        /// </returns>
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

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
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        /// <summary>
        /// The save new profile.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="profile">
        /// The profile.
        /// </param>
        private static void SaveNewProfile(RegisterModel model, CustomProfile profile)
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

        /// <summary>
        /// The save new profile data.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private static void SaveNewProfileData(ProfileViewModel model, string state)
        {
            CustomProfile profile = CustomProfile.GetUserProfile();
            profile.LastName = model.LastName;
            profile.FirstMidName = model.FirstMidName;
            profile.StreetAddress = model.StreetAddress;
            profile.City = model.City;
            profile.State = state;
            profile.ZipCode = model.ZipCode;
            profile.Phone = model.Phone;
            profile.DateOfBirth = model.DateOfBirth;
            profile.ParishAffiliation = model.ParishAffiliation;
            profile.MinistryInvolvement = model.MinistryInvolvement;
            profile.Save();
        }

        /// <summary>
        /// The set default state of user.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="profile">
        /// The profile.
        /// </param>
        private static void SetDefaultStateOfUser(RegisterModel model, CustomProfile profile)
        {
            profile.IsTeacher = "no";
            profile.FilledStudentInfo = "no";
            profile.Save();
            Roles.AddUserToRole(model.UserName, "Default");

            // Adds student to the "Default" role so we can force them to become a "Student" role.
        }

        /// <summary>
        /// Creates Student in Student table from ProfileViewModel
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void CreateStudentInStudentTableFromProfileViewModel(ProfileViewModel model, string state)
        {
            MembershipUser u = Membership.GetUser(User.Identity.Name); // needed to get email for Email = u.Email;
            if (u == null)
            {
                throw new ArgumentNullException("model");
            }

            var newStudent = new Student
                {
                    LastName = model.LastName,
                    FirstMidName = model.FirstMidName,
                    Email = u.Email,
                    UserName = User.Identity.Name,
                    EnrollmentDate = DateTime.Now,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    State = state,
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

        /// <summary>
        /// The create student table data with new profile data and assign student role.
        /// </summary>
        /// <param name="profile">
        /// The model.
        /// </param>
        /// <param name="user">
        /// The user.
        ///  </param>
        private void CreateStudentTableDataWithNewProfileDataAndAssignStudentRole(CustomProfile profile,
                                                                                  MembershipUser user)
        {
            var newStudent = new Student
                {
                    LastName = profile.LastName,
                    FirstMidName = profile.FirstMidName,
                    Email = user.Email,
                    UserName = profile.UserName,
                    EnrollmentDate = DateTime.Now,
                    StreetAddress = profile.StreetAddress,
                    City = profile.City,
                    State = profile.State,
                    ZipCode = profile.ZipCode,
                    Phone = profile.Phone,
                    DateOfBirth = profile.DateOfBirth,
                    ParishAffiliation = profile.ParishAffiliation,
                    MinistryInvolvement = profile.MinistryInvolvement
                };
            db.Students.Add(newStudent);
            db.SaveChanges();
            if (!User.IsInRole("Student"))
            {
                Roles.AddUserToRole(profile.UserName, "Student");
            }
        }

        /// <summary>
        /// The log user in.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// Returns user to returnUrl
        /// </returns>
        private ActionResult LogUserIn(LogOnModel model, string returnUrl)
        {
            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

            var authorized = Roles.IsUserInRole(model.UserName, "Administrator");
            if (authorized)
            {
                return RedirectToAction("Index", "Notification");
            }

            if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The preload profile view model with current users profile data.
        /// </summary>
        /// <returns>
        /// Returns a ProfileViewModel with the current user's profile data
        /// </returns>
        private ProfileViewModel PreloadProfileViewModelWithCurrentUsersProfileData()
        {
            CustomProfile profile = CustomProfile.GetUserProfile(User.Identity.Name);
            var model = new ProfileViewModel
                {
                    LastName = profile.LastName,
                    FirstMidName = profile.FirstMidName,
                    StreetAddress = profile.StreetAddress,
                    City = profile.City,
                    StateString = profile.State,
                    ZipCode = profile.ZipCode,
                    Phone = profile.Phone,
                    DateOfBirth = profile.DateOfBirth,
                    ParishAffiliation = profile.ParishAffiliation,
                    MinistryInvolvement = profile.MinistryInvolvement
                };
            return model;
        }

        // GET: /Account/ChangePassword

        /// <summary>
        /// The update current users instructor table entrys email if needed.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        private void UpdateCurrentUsersInstructorTableEntrysEmailIfNeeded(string email)
        {
            Instructor isInstructor = db.Instructors.FirstOrDefault(o => o.UserName == User.Identity.Name);
            if (isInstructor != null)
            {
                // IF the user already exists in the Instructor Table . . .
                isInstructor.Email = email;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// The update current users student table entrys email if needed.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        private void UpdateCurrentUsersStudentTableEntrysEmailIfNeeded(string email)
        {
            Student isStudent = db.Students.FirstOrDefault(o => o.UserName == User.Identity.Name);
            if (isStudent != null)
            {
                // IF the user already exists in the Student Table . . .
                isStudent.Email = email;
                db.SaveChanges();
            }
        }

        // GET: /Account/Profile

        /// <summary>
        /// The update instructors table with updated profile data from profile view model.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="isInstructor">
        /// The is instructor.
        /// </param>
        private void UpdateInstructorsTableWithUpdatedProfileDataFromProfileViewModel(
            ProfileViewModel model, Instructor isInstructor)
        {
            isInstructor.LastName = model.LastName;
            isInstructor.FirstMidName = model.FirstMidName;
            MembershipUser u = Membership.GetUser(User.Identity.Name);

            // needed to get email for isInstructor.Email = u.Email;
            if (u != null)
            {
                isInstructor.Email = u.Email;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// The update profile email in all locations.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        private void UpdateProfileEmailInAllLocations(string email)
        {
            // change email
            MembershipUser u = Membership.GetUser(User.Identity.Name);
            if (u != null)
            {
                u.Email = email;
                Membership.UpdateUser(u);
            }

            // Update Student Table, if needed
            UpdateCurrentUsersStudentTableEntrysEmailIfNeeded(email);

            // Update Instructor Table, if needed
            UpdateCurrentUsersInstructorTableEntrysEmailIfNeeded(email);
        }

        /// <summary>
        /// The update students table with updated profile data from profile view model.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="isStudent">
        /// The is student.
        /// </param>
        private void UpdateStudentsTableWithUpdatedProfileDataFromProfileViewModel(
            ProfileViewModel model, Student isStudent, string state)
        {
            isStudent.LastName = model.LastName;
            isStudent.FirstMidName = model.FirstMidName;
            isStudent.StreetAddress = model.StreetAddress;
            isStudent.City = model.City;
            isStudent.State = state;
            isStudent.ZipCode = model.ZipCode;
            isStudent.Phone = model.Phone;
            isStudent.DateOfBirth = model.DateOfBirth;
            isStudent.ParishAffiliation = model.ParishAffiliation;
            isStudent.MinistryInvolvement = model.MinistryInvolvement;
            MembershipUser u = Membership.GetUser(User.Identity.Name);

            // needed to get email for isStudent.Email = u.Email;
            if (u != null)
            {
                isStudent.Email = u.Email;
            }

            db.SaveChanges();
        }

        /// <summary>
        /// The update students table with updated profile data from register model.
        /// </summary>
        /// <param name="profile">
        /// The model.
        /// </param>
        /// <param name="isStudent">
        /// The is student.
        /// </param>
        private void UpdateStudentsTableWithUpdatedProfileDataFromRegisterModel(CustomProfile profile, Student isStudent)
        {
            isStudent.LastName = profile.LastName;
            isStudent.FirstMidName = profile.FirstMidName;
            isStudent.StreetAddress = profile.StreetAddress;
            isStudent.City = profile.City;
            isStudent.State = profile.State;
            isStudent.ZipCode = profile.ZipCode;
            isStudent.Phone = profile.Phone;
            isStudent.DateOfBirth = profile.DateOfBirth;
            isStudent.ParishAffiliation = profile.ParishAffiliation;
            isStudent.MinistryInvolvement = profile.MinistryInvolvement;
            MembershipUser u = Membership.GetUser(User.Identity.Name);

            // needed to get email for isStudent.Email = u.Email;
            if (u != null)
            {
                isStudent.Email = u.Email;
            }

            db.SaveChanges();
        }

        #endregion
    }
}