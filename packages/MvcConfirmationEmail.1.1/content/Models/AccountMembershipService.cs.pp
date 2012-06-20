using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Net.Mail;
using System.Configuration;

namespace $rootnamespace$.Models
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
                Subject = "Please confirm your email",
                Body = verifyUrl
            };

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Send(message);

        }

        public static string GetPublicUrl()
        {
            var request = HttpContext.Current.Request;

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
    }
}