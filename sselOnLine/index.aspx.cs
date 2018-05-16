using LNF.Cache;
using LNF.CommonTools;
using LNF.Repository;
using LNF.Web.Content;
using System;

namespace sselOnLine
{
    public partial class Index : LNFPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CacheManager.Current.CheckSession();

            if (!Page.IsPostBack)
            {
                Session.Remove("SiteMenu");

                ValidPasswordCheck();

                //DropDownMenu1.LogoImageUrl = "/static/images/lnfbanner.jpg";
                //DropDownMenu1.DataSource = GetSiteMenu();
                //DropDownMenu1.DataBind();

                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("<div>Current User: " + CacheManager.Current.CurrentUser.DisplayName + "</div>");
                //sb.AppendLine(@"<div id=""jclock""></div>");
                //DropDownMenuItem ddmi = new DropDownMenuItem(sb.ToString());
                //ddmi.CssClass = "menu-clock";
                //ddmi.Enabled = false;
                //DropDownMenu1.Items.Add(ddmi);

                // pass this info along to Loader.ashx
                Session["Room"] = Request.QueryString["room"];

                HandleViewUrl();
            }
        }

        //private LNF.SiteMenu GetSiteMenu()
        //{
        //    var siteMenu = new LNF.SiteMenu(CacheManager.Current.CurrentUser);
        //    var logout = siteMenu.First(x => x.IsLogout);

        //    if (!string.IsNullOrEmpty(Request.QueryString["timeout"]) && !string.IsNullOrEmpty(Request.QueryString["room"]))
        //    {
        //        if (int.TryParse(Request.QueryString["timeout"], out int timeout))
        //        {
        //            string room = Request.QueryString["room"];
        //            logout.MenuURL += "?ReturnUrl=" + Server.UrlEncode(string.Format("/sselonline?timeout={0}&room={1}", timeout, room));
        //        }
        //    }

        //    return siteMenu;
        //}

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