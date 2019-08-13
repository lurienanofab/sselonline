using LNF.Scheduler;
using LNF.Web;
using System.Web;
using System.Web.SessionState;

namespace sselOnLine
{
    /// <summary>
    /// Loads the appropriate page based on if the client is a kiosk or not
    /// </summary>
    public class Loader : IHttpHandler, IRequiresSessionState
    {
        public HttpContextBase ContextBase { get; private set; }

        public void ProcessRequest(HttpContext context)
        {
            ContextBase = new HttpContextWrapper(context);

            string redirectUrl = "~/Blank.aspx";
            string room = GetRoom();
            string currentIp = ContextBase.CurrentIP();

            bool isOnKiosk = !string.IsNullOrEmpty(room) || KioskUtility.IsOnKiosk(currentIp);

            if (isOnKiosk)
            {
                
                redirectUrl = "/" + KioskUtility.KioskRedirectUrl(currentIp);
            }

            context.Response.Redirect(redirectUrl);
        }

        private string GetRoom()
        {
            string result = string.Empty;
            if (ContextBase.Session["Room"] != null)
            {
                result = ContextBase.Session["Room"].ToString();
                ContextBase.Session.Remove("Room");
            }
            return result;
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}