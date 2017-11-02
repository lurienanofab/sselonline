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
    public class Loader : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string redirectUrl = "~/Blank.aspx";
            string room = GetRoom(context);
            bool isOnKiosk = !string.IsNullOrEmpty(room) || CacheManager.Current.IsOnKiosk();

            if (isOnKiosk)
                redirectUrl = "/" + KioskUtility.KioskRedirectUrl();

            context.Response.Redirect(redirectUrl);
        }

        private string GetRoom(HttpContext context)
        {
            string result = string.Empty;
            if (context.Session["Room"] != null)
            {
                result = context.Session["Room"].ToString();
                context.Session.Remove("Room");
            }
            return result;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}