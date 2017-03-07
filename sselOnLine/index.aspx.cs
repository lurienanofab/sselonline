using LNF;
using LNF.Cache;
using LNF.CommonTools;
using LNF.Repository;
using LNF.Web.Content;
using LNF.Web.Controls.Navigation;
using System;
using System.Text;

namespace sselOnLine
{
    public partial class index : LNFPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hidSeedTime.Value = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");

            CacheManager.Current.CheckSession();

            if (!Page.IsPostBack)
            {
                ValidPasswordCheck();

                DropDownMenu1.LogoImageUrl = GetStaticUrl("images/lnfbanner.jpg");
                DropDownMenu1.DataSource = SiteMenu.Create(CacheManager.Current.CurrentUser);
                DropDownMenu1.DataBind();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div>Current User: " + CacheManager.Current.CurrentUser.DisplayName + "</div>");
                sb.AppendLine(@"<div id=""jclock""></div>");
                DropDownMenuItem ddmi = new DropDownMenuItem(sb.ToString());
                ddmi.CssClass = "menu-clock";
                ddmi.Enabled = false;
                DropDownMenu1.Items.Add(ddmi);

                HandleViewUrl();
            }
        }

        private void ValidPasswordCheck()
        {
            // this code enforces the rule that the password cannot be the same as the username
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                string pwd = dba.ApplyParameters(new { Action = "GetPassword", UserName = Context.User.Identity.Name }).ExecuteScalar<string>("Client_CheckAuth");
                if (new Encryption().EncryptText(User.Identity.Name) == pwd)
                {
                    hidPasswordRedirect.Value = "true";
                }
            }
        }

        private void HandleViewUrl()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["view"]))
            {
                if (Request.QueryString["view"] != "/")
                    Session["view"] = Request.QueryString["view"];

                Response.Redirect("~");
                return;
            }

            hidViewUrl.Value = string.Empty;

            string view = Session["view"] == null ? string.Empty : Session["view"].ToString();
            if (!string.IsNullOrEmpty(view))
            {
                Session["view"] = null;
                hidViewUrl.Value = view;
            }
        }
    }
}