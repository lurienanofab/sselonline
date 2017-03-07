using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Configuration;
using LNF;

namespace sselOnLine.AppCode
{
    public class SiteSecurity
    {
        public static string DefaultUrl
        {
            get { return VirtualPathUtility.ToAbsolute("~"); }
        }

        public static string DefaultDomain
        {
            get { return ConfigurationManager.AppSettings["DefaultDomain"]; }
        }

        public static string AuthDomain
        {
            get { return ConfigurationManager.AppSettings["AuthDomain"]; }
        }

        public static string SecurePath
        {
            get { return ConfigurationManager.AppSettings["SecurePath"]; }
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Request.Cookies.Remove("ASP.NET_SessionId");
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName);
            authCookie.Domain = AuthDomain;
            authCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public static bool EmailIsValid(string test)
        {
            try
            {
                MailAddress addr = new MailAddress(test);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
