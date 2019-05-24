using LNF;
using LNF.CommonTools;
using LNF.Hooks;
using LNF.Models.Data;
using LNF.Scheduler;
using LNF.Web.Content;
using sselOnLine.AppCode;
using sselOnLine.AppCode.BLL;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace sselOnLine
{
    public partial class Login : LNFPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string loginUrl = GetLoginUrl();

            if (!string.IsNullOrEmpty(loginUrl))
            {
                Response.Redirect(loginUrl);
                return;
            }

            bool isKiosk = IsKiosk();

            if (!isKiosk && bool.Parse(ConfigurationManager.AppSettings["RequireSSL"]))
            {
                if (!Request.Url.GetLeftPart(UriPartial.Scheme).StartsWith("https"))
                    Response.Redirect(string.Format("https://{0}{1}", Request.Url.Host, Request.Url.PathAndQuery));
            }

            phGoogleAnalytics.Visible = ServiceProvider.Current.IsProduction();

            if (!Page.IsPostBack)
            {
                SiteSecurity.SignOut();

                if (Utility.IsMobile())
                    Response.Redirect("/mobile");

                diwater_meter.Visible = ConfigurationManager.AppSettings["ShowMetersOnLoginPage"] == "Yes";
                AjaxDataSource1.Enabled = ConfigurationManager.AppSettings["ShowMetersOnLoginPage"] == "Yes";
                chkKiosk.Visible = !ServiceProvider.Current.IsProduction();
                if (!chkKiosk.Visible)
                    chkKiosk.Visible = ConfigurationManager.AppSettings["ShowKioskLoginOptionIpList"].Contains(Request.UserHostAddress);

                SetFocus(txtUsername);

                if (isKiosk)
                {
                    string[] ipParts = Request.UserHostAddress.Split('.');
                    lblKiosk.Visible = true;
                    lblKiosk.Text = "This is Kiosk " + ipParts[3];
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("MeterTag", typeof(string));
                dt.Columns.Add("MeterName", typeof(string));
                dt.Rows.Add("wetchem", "Wet Chem");
                diwater_meter.DataSource = dt;
                diwater_meter.DataBind();
            }

            lblError.Visible = false;
        }

        private string GetLoginUrl()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoginUrl"]))
            {
                string url = ConfigurationManager.AppSettings["LoginUrl"];
                string amp = "?";

                if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    url += amp + "ReturnUrl=" + Request.QueryString["ReturnUrl"];
                    amp = "&";
                }

                if (!string.IsNullOrEmpty(Request.QueryString["ReturnServer"]))
                {
                    url += amp + "ReturnServer=" + Request.QueryString["ReturnServer"];
                    amp = "&";
                }

                return url;
            }

            return string.Empty;
        }

        protected void btnEnter_Click(object sender, ImageClickEventArgs e)
        {
            CheckLogin();
        }

        protected void btnLoginErrorOK_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        private void CheckLogin()
        {
            string un = txtUsername.Text;
            string pw = txtPassword.Text;
            HookManager.RunHooks(new BeforeLogInHookContext(un, pw), new BeforeLogInHookResult(), (a, b) => HandleBeforeLogInHook(a, b));
        }

        private void HandleBeforeLogInHook(BeforeLogInHookContext context, BeforeLogInHookResult result)
        {
            //2007-08-07 Added the special code here so administrator has universal password that works for everyone user
            //2011-05-13 now the universal password code is handled in ClientDB.Login
            string debug = string.Empty;

            IClient c = null;
            if (result.IsLoggedIn)
            {
                debug += "result.IsLoggedIn = true";
                c = result.Client;
            }
            else
            {
                c = ServiceProvider.Current.Data.Client.Login(context.Username, context.Password);
                debug += "result.IsLoggedIn = false, c " + (c == null ? "is null" : "is not null, username = " + context.Username + ", password = " + context.Password);
            }

            //client will be "empty" if login fails
            if (c != null)
            {
                if (!c.ClientActive)
                {
                    //litLoginErrorMessage.Text = @"<div style=""font-weight: bold; font-size: 16pt; color: #FF0000; padding-top: 40px; padding-bottom: 40px;"">Your account is inactive.</div>";
                    panLoginError.Visible = true;
                    return;
                }

                bool isKiosk = chkKiosk.Checked || KioskUtility.IsOnKiosk(Request.UserHostAddress);

                HttpCookie authCookie = FormsAuthentication.GetAuthCookie(c.UserName, false);
                FormsAuthenticationTicket formInfoTicket = FormsAuthentication.Decrypt(authCookie.Value);
                FormsAuthenticationTicket myTicket = new FormsAuthenticationTicket(formInfoTicket.Version, formInfoTicket.Name, formInfoTicket.IssueDate, formInfoTicket.Expiration, formInfoTicket.IsPersistent, string.Join("|", c.Roles()), formInfoTicket.CookiePath);

                authCookie.Value = FormsAuthentication.Encrypt(myTicket);
                authCookie.Domain = SiteSecurity.AuthDomain;
                authCookie.Expires = formInfoTicket.Expiration;

                Response.Cookies.Add(authCookie);
                Session["UserName"] = c.UserName;
                Session["DisplayName"] = c.DisplayName;
                Session["ClientID"] = c.ClientID;
                Session["Email"] = c.Email;
                Session["IsKiosk"] = isKiosk;

                EventLogger.WriteToSystemLog(c.ClientID, Guid.NewGuid(), EventLogger.LogMessageTypes.Info, string.Format("User {0} has logged in.", c.UserName));

                HookManager.RunHooks(new AfterLogInHookContext(c, isKiosk), new AfterLogInHookResult(), (a, b) => HandleAfterLogInHook(a, b));
            }
            else
            {
                lblError.Text += "[" + debug + "]";
                lblError.Visible = true;
            }
        }

        private void HandleAfterLogInHook(AfterLogInHookContext context, AfterLogInHookResult result)
        {
            if (result.Redirect)
                Response.Redirect(result.RedirectUrl);
            else
            {
                string returnServer = GetReturnServer();
                string returnUrl = GetReturnUrl();
                string redirectUrl = string.Format("{0}{1}", returnServer, returnUrl);
                Response.Redirect(redirectUrl);
            }
        }

        private string GetReturnServer()
        {
            string result = Request.QueryString["ReturnServer"];

            if (string.IsNullOrEmpty(result))
                result = Application["AppServer"].ToString();

            if (!result.StartsWith("http"))
                result = "http://" + result;

            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        private string GetReturnUrl()
        {
            string result = Request.QueryString["ReturnUrl"];

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(GetReturnServer()))
                result = SiteSecurity.DefaultUrl;

            if (result == null)
                return string.Empty;

            if (result.StartsWith("/"))
                result = result.Substring(1);

            return result;
        }

        private bool IsKiosk()
        {
            bool result = KioskDB.IsKiosk(Request.UserHostAddress);
            return result;
        }
    }
}