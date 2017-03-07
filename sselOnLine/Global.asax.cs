using LNF;
using sselOnLine.AppCode;
using System;
using System.Web;

namespace sselOnLine
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            string host = HttpContext.Current.Request.Url.Host;

            if (Providers.IsProduction())
            {
                if (!host.EndsWith(SiteSecurity.DefaultDomain))
                    host += "." + SiteSecurity.DefaultDomain;
            }

            if (host.EndsWith("/"))
                host = host.TrimEnd('/');

            Application["AppServer"] = string.Format("http://{0}/", host);
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Now that we are using AspNet Identity this is no longer needed.
        }

        void Session_Start(object sender, EventArgs e)
        {
            if (Providers.IsProduction())
            {
                if (!Request.Url.Host.EndsWith(SiteSecurity.DefaultDomain))
                {
                    Session.Abandon();
                    string redirectUrl = Application["AppServer"].ToString() + Request.Url.PathAndQuery.Substring(1);
                    Response.Redirect(redirectUrl);
                }
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
