using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using PaulSchool.ViewModels;

namespace PaulSchool.Models
{
    public class AccountMembershipService
    {
        public static void SendConfirmationEmail(MembershipUser user)
        {
            string confirmationGuid = user.ProviderUserKey.ToString();
            string verifyUrl = GetPublicUrl() + "account/verify?ID=" + confirmationGuid;
            string from = ConfigurationManager.AppSettings["MvcConfirmationEmailFromAccount"];

            var message = new MailMessage(from, user.Email)
                              {
                                  Subject =
                                      "Please confirm your email address for regisration for the Diocese of Corpus Christi St. Paul School of Catechism.",
                                  Body = verifyUrl + " <-- Click that link to verify your account for the St. Paul School of Catechism."
                              };

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Send(message);
        }

        public static void SendCustomEmail(EmailViewModel email)
        {
            string to = email.Email;
            string from = ConfigurationManager.AppSettings["MvcConfirmationEmailFromAccount"];

            var message = new MailMessage(from, to)
            {
                Subject = email.Subject,
                Body = email.Body
            };

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Send(message);
        }

        public static string GetPublicUrl()
        {
            HttpRequest request = HttpContext.Current.Request;

            var uriBuilder = new UriBuilder
                                 {
                                     Host = request.Url.Host,
                                     Path = "/",
                                     Port = 80,
                                     Scheme = "http",
                                 };

            if (request.IsLocal)
            {
                uriBuilder.Port = request.Url.Port;
            }

            return new Uri(uriBuilder.Uri.ToString()).AbsoluteUri;
        }

        public static void ChangePassword(string currentUser)
        {
            MembershipUser user = Membership.GetUser(currentUser);
            string password = user.ResetPassword();
            string logOnUrl = GetPublicUrl() + "account/logon";
            string from = ConfigurationManager.AppSettings["MvcConfirmationEmailFromAccount"];

            var message = new MailMessage(from, user.Email)
                              {
                                  Subject = "School of Catechesis Password Reset",
                                  Body =
                                      "Hello " + user.UserName + ", this is your new password: " + password +
                                      "     You can log on here: " + logOnUrl
                              };
            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Send(message);
        }
    }
}