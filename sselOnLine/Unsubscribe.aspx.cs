using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using sselOnLine.AppCode;
using sselOnLine.AppCode.BLL;
using LNF;

namespace sselOnLine
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string reason = txtReason.Text.Trim();
            string addr = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(reason) || string.IsNullOrEmpty(addr))
            {
                lblMsg.Text = "Data missing! Both fields are required.";
                return;
            }

            if (!SiteSecurity.EmailIsValid(addr))
            {
                lblMsg.Text = "Invalid email address.";
                return;
            }

            UnsubscribeDB.UserInfo user = UnsubscribeDB.GetUserByEmail(txtEmail.Text.Trim());
            if (user.ClientID == -1)
            {
                lblMsg.Text = "The address you provided did not match our records. Please be sure to use the correct address.";
                return;
            }

            string bodytext = "LNF User List: Unsubscribe Request" + Environment.NewLine;
            bodytext += "--------------------------------------------------" + Environment.NewLine;
            bodytext += "Email:         " + user.Email + Environment.NewLine;
            bodytext += "Name:          " + user.DisplayName + Environment.NewLine;
            bodytext += "Organization:  " + user.Organization + Environment.NewLine;
            bodytext += "Reason:        " + txtReason.Text + Environment.NewLine;

            try
            {
                Providers.Email.SendMessage(0, "sselOnLine.Unsubscribe.btnSubmit_Click(object sender, EventArgs e)", "LNF User List unsubscribe request from " + user.DisplayName, bodytext, "system@lnf.umich.edu", new string[] { ConfigurationManager.AppSettings["EmailUnsubscribe"] });
                lblMsg.Text = "Your request to unsubscribe has been sent to the administrator. It might take several days for the change to take effect.";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Something is wrong, please contact the LNF administrator" + Environment.NewLine;
                lblMsg.Text += "<!--" + Environment.NewLine;
                lblMsg.Text += ex.Message + Environment.NewLine;
                lblMsg.Text += "-->" + Environment.NewLine;
            }
        }
    }
}