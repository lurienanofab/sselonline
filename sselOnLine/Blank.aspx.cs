using LNF;
using LNF.Cache;
using LNF.CommonTools;
using LNF.Models.Data;
using LNF.Web.Content;
using System;
using System.Configuration;
using System.Web.UI;

namespace sselOnLine
{
    public partial class Blank : LNFPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litDebug.Text = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["ThrowTestError"]))
                throw new Exception(Request.QueryString["ThrowTestError"]);

            //If there is a problem with the session but the user is sill authenticated this will restore the session variables.
            //If the user is not authenticated they will be redirected to the log in page by FormsAuthentication.
            CacheManager.Current.CheckSession();

            phStaging.Visible = CacheManager.Current.CurrentUser.HasPriv(ClientPrivilege.Staff | ClientPrivilege.Administrator | ClientPrivilege.Developer);

            phGoogleAnalytics.Visible = ServiceProvider.Current.IsProduction();

            SetupLoginRequirement();

            if (Utility.IsMobile() && bool.Parse(ConfigurationManager.AppSettings["UseMobileSite"]))
            {
                string prefview = Utility.PreferredMobileView();
                if (!string.IsNullOrEmpty(prefview))
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "mobile_redirect", string.Format("window.top.location = '/mobile/{0}';", prefview), true);
            }

            if (Page.User.Identity.Name == "jgett")
                litDebug.Text += @"<div style=""padding: 10px;""><a href=""Blank.aspx?ThrowTestError=testing"" class=""admin-link"">Throw a test error</a></div>";

            litCommonToolsVersion.Text = "<!-- CommonTools Version: " + Utility.Version() + " -->";
        }

        private void SetupLoginRequirement()
        {
            //LoginRequirement1.DisplayName = Session["DisplayName"].ToString();
            //LoginRequirement1.RedirectURL = Application["AppServer"].ToString() + KioskUtility.KioskRedirectUrl();
            //LoginRequirement1.IsKiosk = Utility.IsKiosk();

            //if (!Page.IsPostBack)
            //    LoginRequirement1.Check();
        }
    }
}