using LNF.Web.Content;
using System;
using System.Web;
using System.Web.Security;

namespace sselOnLine
{
    public partial class Test : LNFPage
    {
        public string TestName { get { return Request.QueryString["testname"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TestName))
            {
                divfull.Visible = false;
                phErrorMessage.Visible = true;
            }
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }

            string testsuburl = VirtualPathUtility.ToAbsolute(string.Format("~/TestPage.aspx?testname={0}", TestName));

            Response.Redirect(testsuburl);
        }

        protected void btnExistingUser_Click(object sender, EventArgs e)
        {
            string testurl = VirtualPathUtility.ToAbsolute(string.Format("~/TestPage.aspx?testname={0}", TestName));
            if (Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect(testurl);
            }
            else
            {
                string testsuburl = string.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, testurl);

                Response.Redirect(testsuburl);
            }
        }
    }
}