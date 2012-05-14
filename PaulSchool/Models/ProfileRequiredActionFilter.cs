using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PaulSchool.Models;


namespace PaulSchool.Models
{
    public class ProfileRequiredActionFilter : IActionFilter
    {
        #region Implementation of IActionFilter

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //TODO: Check if the Authenticated User has a profile.
            
            //var authorized = Roles.IsUserInRole(User, "Student");


            //If Authenicated User doesn't have a profile...
            filterContext.Result = new RedirectResult("Path-To-Create-A-Profile");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        #endregion
    }
}