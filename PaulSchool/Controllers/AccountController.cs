// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="LostTie LLC">
//   Copyright (c) Lost Tie LLC.  All rights reserved.
//   This code and information is provided "as is" without warranty of any kind, either expressed or implied, including but not limited to the implied warranties of merchantability and fitness for a particular purpose.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PaulSchool.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using PaulSchool.Models;
    using PaulSchool.Resources;
    using PaulSchool.ViewModels;

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

        /// <summary>
        /// Changes logged-in user's email
        /// </summary>
        /// <returns>
        /// ChangeEmailViewModel with current user's email.
        /// </returns>
        public ActionResult ChangeEmail()
        {
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);
            this.ViewBag.Email1 = u.Email;
            var model = new ChangeEmailViewModel { Email = u.Email };
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
            if (this.ModelState.IsValid)
            {
                string email = model.Email;
                string userName = Membership.GetUserNameByEmail(email);

                // checks if there is a duplicate email in the database
                if (userName == null || (userName == this.User.Identity.Name && userName != null))
                {
                    this.UpdateProfileEmailInAllLocations(email);
                    return this.RedirectToAction("Profile");
                }

                return this.RedirectToAction("failed");

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
            return this.View();
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
            if (this.ModelState.IsValid)
            {
                bool changePasswordSucceeded = false;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(this.User.Identity.Name, true /* userIsOnline */);
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
                    return this.RedirectToAction("ChangePasswordSuccess");
                }

                this.ModelState.AddModelError(
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
            return this.View();
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
            ProfileViewModel model = this.PreloadProfileViewModelWithCurrentUsersProfileData();
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
        public ActionResult ChangeProfile(ProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            // validation succeeded => process the results
            // save the profile data
            SaveNewProfileData(model);

            // check if already existing on the student table - update the table if needed
            Student isStudent = this.db.Students.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
            if (isStudent != null)
            {
                // IF the user already exists in the Student Table . . .
                this.UpdateStudentsTableWithUpdatedProfileDataFromProfileViewModel(model, isStudent);
            }
            else
            {
                // Create student in student table if they have not been there before (everyone needs to be at least a student)
                this.CreateStudentInStudentTableFromProfileViewModel(model);
            }

            // check if already existing on the instructor table - update the table if needed
            Instructor isInstructor = this.db.Instructors.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
            if (isInstructor != null)
            {
                // IF the user already exists in the Instructor Table . . .
                this.UpdateInstructorsTableWithUpdatedProfileDataFromProfileViewModel(model, isInstructor);
            }

            return this.RedirectToAction("Profile");
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

            return this.RedirectToAction("Index", "Home");
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
            return this.View();
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
            if (this.ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    return this.LogUserIn(model, returnUrl);
                }

                this.ModelState.AddModelError(
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
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);
            if (u != null)
            {
                this.ViewBag.Email = u.Email;
            }

            return this.View();
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
            return this.View();
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
        public ActionResult Register(RegisterModel model)
        {
            if (this.ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(
                    model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return this.CreateAndSaveNewUserWithStudentProfileAndCookie(model);
                }

                this.ModelState.AddModelError(string.Empty, ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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
        private static void SaveNewProfileData(ProfileViewModel model)
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
        /// Creates and saves new user with student profile and cookie.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// Returns the home page of the website after confirming successful user registration.
        /// </returns>
        private ActionResult CreateAndSaveNewUserWithStudentProfileAndCookie(RegisterModel model)
        {
            if (this.ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                CustomProfile profile = CustomProfile.GetUserProfile(model.UserName);

                SetDefaultStateOfUser(model, profile);

                SaveNewProfile(model, profile);

                // check if already existing on the student table - update the table if needed
                Student isStudent = this.db.Students.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
                if (isStudent != null)
                {
                    // IF the user already exists in the Student Table . . .
                    this.UpdateStudentsTableWithUpdatedProfileDataFromRegisterModel(model, isStudent);
                }
                else
                {
                    // Create student in student table if they have not been there before (everyone needs to be at least a student)
                    this.CreateStudentTableDataWithNewProfileDataAndAssignStudentRole(model);
                }

                return this.RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        /// <summary>
        /// Creates Student in Student table from ProfileViewModel
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void CreateStudentInStudentTableFromProfileViewModel(ProfileViewModel model)
        {
            MembershipUser u = Membership.GetUser(this.User.Identity.Name); // needed to get email for Email = u.Email;
            if (u == null)
            {
                throw new ArgumentNullException("model");
            }

            var newStudent = new Student
                {
                    LastName = model.LastName, 
                    FirstMidName = model.FirstMidName, 
                    Email = u.Email, 
                    UserName = this.User.Identity.Name, 
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
            this.db.Students.Add(newStudent);
            this.db.SaveChanges();
            if (!this.User.IsInRole("Student"))
            {
                Roles.AddUserToRole(this.User.Identity.Name, "Student");
            }
        }

        /// <summary>
        /// The create student table data with new profile data and assign student role.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        private void CreateStudentTableDataWithNewProfileDataAndAssignStudentRole(RegisterModel model)
        {
            var newStudent = new Student
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
            this.db.Students.Add(newStudent);
            this.db.SaveChanges();
            if (!this.User.IsInRole("Student"))
            {
                Roles.AddUserToRole(model.UserName, "Student");
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

            // FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            if (this.Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The preload profile view model with current users profile data.
        /// </summary>
        /// <returns>
        /// Returns a ProfileViewModel with the current user's profile data
        /// </returns>
        private ProfileViewModel PreloadProfileViewModelWithCurrentUsersProfileData()
        {
            CustomProfile profile = CustomProfile.GetUserProfile(this.User.Identity.Name);
            var model = new ProfileViewModel
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

        // GET: /Account/ChangePassword

        /// <summary>
        /// The update current users instructor table entrys email if needed.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        private void UpdateCurrentUsersInstructorTableEntrysEmailIfNeeded(string email)
        {
            Instructor isInstructor = this.db.Instructors.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
            if (isInstructor != null)
            {
                // IF the user already exists in the Instructor Table . . .
                isInstructor.Email = email;
                this.db.SaveChanges();
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
            Student isStudent = this.db.Students.FirstOrDefault(o => o.UserName == this.User.Identity.Name);
            if (isStudent != null)
            {
                // IF the user already exists in the Student Table . . .
                isStudent.Email = email;
                this.db.SaveChanges();
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
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);

            // needed to get email for isInstructor.Email = u.Email;
            if (u != null)
            {
                isInstructor.Email = u.Email;
            }

            this.db.SaveChanges();
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
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);
            if (u != null)
            {
                u.Email = email;
                Membership.UpdateUser(u);
            }

            // Update Student Table, if needed
            this.UpdateCurrentUsersStudentTableEntrysEmailIfNeeded(email);

            // Update Instructor Table, if needed
            this.UpdateCurrentUsersInstructorTableEntrysEmailIfNeeded(email);
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
            ProfileViewModel model, Student isStudent)
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
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);

            // needed to get email for isStudent.Email = u.Email;
            if (u != null)
            {
                isStudent.Email = u.Email;
            }

            this.db.SaveChanges();
        }

        /// <summary>
        /// The update students table with updated profile data from register model.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="isStudent">
        /// The is student.
        /// </param>
        private void UpdateStudentsTableWithUpdatedProfileDataFromRegisterModel(RegisterModel model, Student isStudent)
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
            MembershipUser u = Membership.GetUser(this.User.Identity.Name);

            // needed to get email for isStudent.Email = u.Email;
            if (u != null)
            {
                isStudent.Email = u.Email;
            }

            this.db.SaveChanges();
        }

        #endregion
    }
}