using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using LNF.Cache;
using LNF.Scheduler;

namespace sselOnLine
{
    /// <summary>
    /// Loads the appropriate page based on if the client is a kiosk or not
    /// </summary>
    public class Loader : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string redirectUrl = "~/Blank.aspx";

            if (CacheManager.Current.IsOnKiosk())
            {
                redirectUrl = KioskUtility.KioskRedirectUrl();

                if (!redirectUrl.StartsWith("/"))
                    redirectUrl = "/" + redirectUrl;   
            }

            context.Response.Redirect(redirectUrl);
        }

        public bool IsReusable
        {
            get  { return false; }
        }
    }
}