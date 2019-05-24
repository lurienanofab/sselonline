using LNF;
using LNF.Models.Mail;
using sselOnLine.AppCode;
using sselOnLine.AppCode.BLL;
using System;
using System.Configuration;
using System.Web.UI;

namespace sselOnLine
{
    public partial class Unsubscribe : Page
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
                ServiceProvider.Current.Mail.SendMessage(new SendMessageArgs
                {
                    ClientID = 0,
                    Caller = "sselOnLine.Unsubscribe.btnSubmit_Click",
                    Subject = $"LNF User List unsubscribe request from {user.DisplayName}",
                    Body = bodytext,
                    From = "system@lnf.umich.edu",
                    To = new string[] { ConfigurationManager.AppSettings["EmailUnsubscribe"] }
                });

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