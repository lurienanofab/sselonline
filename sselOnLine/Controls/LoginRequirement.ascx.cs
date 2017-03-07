using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;
using LNF;
using sselOnLine.AppCode.BLL;

namespace sselOnLine.Controls
{
    public partial class LoginRequirement : System.Web.UI.UserControl
    {
        private bool _RequiresResponse;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string DisplayName { get; set; }

        [UrlProperty]
        public string RedirectURL { get; set; }

        public bool IsKiosk { get; set; }

        public bool RequiresResponse
        {
            get { return _RequiresResponse; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Check()
        {
            Check(true);
        }

        public void Check(bool login)
        {
            RequiredQuestions.Visible = false;
            RequiredInput.Visible = false;
            KioskMessage.Visible = false;

            //get the active requirement, will be -1 if there isn't one
            Dictionary<string, int> ids = RequirementDB.GetID(Convert.ToInt32(Session["ClientID"]), IsKiosk);
            int clrID = ids["ClientLoginRequirementID"];
            int lrID = ids["LoginRequirementID"];
            RequirementDB.LoginRequirement logreq = RequirementDB.GetLoginRequirement(lrID);
            hidClientLoginRequirementID.Value = clrID.ToString();

            _RequiresResponse = false;

            string modalScript = string.Empty;

            if (clrID != -1)
            {
                RequirementDB req = new RequirementDB(clrID);

                if (login) req.HandleLogin();

                if (req.Locked)
                    modalScript = ModalOnScript();
                else
                    modalScript = ModalOffScript();

                FindControl(logreq.Name).Visible = true;
                if (logreq.Name == "RequiredInput")
                {
                    chkAcknowledge.Checked = !req.Locked;
                    chkAcknowledge.Enabled = req.Locked;
                    litConfirmCheckbox.Text = @"<input type=""checkbox"" onchange=""confirm_change(this);""" + (req.Locked ? " disabled" : string.Empty) + " />";
                }
                else if (logreq.Name == "KioskMessage")
                {
                    litRemainingLoginAttempts.Text = Math.Max((logreq.MaxLoginAttempts - req.LoginAttempts), 0).ToString();
                    btnKioskOK.Enabled = !req.Locked;
                }

                _RequiresResponse = true;
            }
            else
                modalScript = ModalOffScript();

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "modal-script", modalScript);
        }

        protected void btnRequiredOK_Click(object sender, EventArgs e)
        {
            bool q1Answer = false;
            bool q2Answer = false;

            q1Answer = rblQ1.Items[0].Selected;
            q2Answer = rblQ2.Items[0].Selected;

            RequirementDB req = new RequirementDB(Convert.ToInt32(hidClientLoginRequirementID.Value));
            req.Locked = false;
            req.RequiredAcknowledgement = true;
            req.Complete = true;
            req.AddResponseDataNode("q1", (q1Answer ? "yes" : "no"));
            req.AddResponseDataNode("q2", (q2Answer ? "yes" : "no"));
            req.Active = false;

            if (!q1Answer && q2Answer)
            {
                //flag account complete

                req.Save();

                RequirementDB nextReq;
                int cID = req.ClientID;
                int tID = req.ToggleID;
                while (tID != -1)
                {
                    int clrID = RequirementDB.GetID(cID, tID)["ClientLoginRequirementID"];
                    nextReq = new RequirementDB(clrID);
                    nextReq.Active = false;
                    nextReq.Locked = false;
                    nextReq.Complete = true;
                    nextReq.Save();
                    cID = nextReq.ClientID;
                    tID = nextReq.ToggleID;
                }
            }
            else
                req.Save(true);

            Check();
        }

        protected void chkAcknowledge_CheckChanged(object sender, EventArgs e)
        {
            if (chkAcknowledge.Checked)
            {
                RequirementDB req = new RequirementDB(Convert.ToInt32(hidClientLoginRequirementID.Value));
                req.Locked = false;
                req.AddResponseDataNode("acknowledgement", "yes");
                req.Save(true, true);
                Check(false);
            }
        }

        protected void btnConfirmOK_Click(object sender, EventArgs e)
        {
            MailMessage email = new MailMessage("admin@lnf.umich.edu", ConfigurationManager.AppSettings["LoginRequirementEmail"]);
            SmtpClient smtp = new SmtpClient("127.0.0.1");

            int uploaded = 0;
            string filePath;
            DateTime uploadDateTime = DateTime.Now;
            Attachment attach;

            if (fupResponse1.HasFile)
            {
                filePath = Server.MapPath("./files/user/") + Session["ClientID"].ToString() + "_" + uploadDateTime.ToString("yyyyMMddHHmmss") + "_" + fupResponse1.FileName;
                fupResponse1.SaveAs(filePath);
                attach = new Attachment(filePath);
                attach.ContentDisposition.FileName = fupResponse1.FileName;
                email.Attachments.Add(attach);
                uploaded += 1;
            }

            if (fupResponse2.HasFile)
            {
                filePath = Server.MapPath("./files/user/") + Session["ClientID"].ToString() + "_" + uploadDateTime.ToString("yyyyMMddHHmmss") + "_" + fupResponse2.FileName;
                fupResponse1.SaveAs(filePath);
                attach = new Attachment(filePath);
                attach.ContentDisposition.FileName = fupResponse2.FileName;
                email.Attachments.Add(attach);
                uploaded += 1;
            }

            RequirementDB req = new RequirementDB(Convert.ToInt32(hidClientLoginRequirementID.Value));
            req.Complete = true;
            req.Active = false;

            req.RequiredMessageInput = txtResponse.Text;
            req.RequiredFileUploaded = (uploaded > 0);
            req.RequiredAcknowledgement = true;
            req.Save();

            string subject = "NNIN Information Request Submitted";
            string body = "NNIN information request completed." + Environment.NewLine + Environment.NewLine;
            body += "User: " + DisplayName + Environment.NewLine + Environment.NewLine;
            body += "Text:" + Environment.NewLine + txtResponse.Text + Environment.NewLine + Environment.NewLine;
            body += "Files uploaded: " + uploaded.ToString() + (uploaded > 0 ? " (see attachment" + (uploaded > 1 ? "s" : "") + ")" : "");

            email.Subject = subject;
            email.Body = body;
            email.IsBodyHtml = false;

            smtp.Send(email);

            Check();
        }

        protected void btnKioskOK_Click(object sender, EventArgs e)
        {
            Response.Redirect(RedirectURL);
        }

        private string ModalOnScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(@"  $(document).ready(function(){");
            sb.AppendLine(@"    var obj = (parent.$) ? parent.$(top.window) : $(top.window);");
            sb.AppendLine(@"    $('.modal_overlay', top.document).remove();");
            sb.AppendLine(@"    obj.unbind('resize');");
            sb.AppendLine(@"    $('body').css('background-color', '#FFFFFF');");
            sb.AppendLine(@"    $('#ps', top.document).prepend('<div class=""modal_overlay""></div>');");
            sb.AppendLine(@"    $('.modal_overlay', top.document).css({ 'top': '0px', 'left': '0px', 'width': $(top.window).width() + 'px', 'height': $(top.window).height() + 'px', 'position': 'absolute', 'z-index': '5', 'background-color': '#CCCCCC', 'filter': 'alpha(opacity=75)', '-moz-opacity': '0.75', '-khtml-opacity': '0.75', 'opacity': '0.75' });");
            sb.AppendLine(@"    $('body').css('background-color', '#D9D9D9');");
            sb.AppendLine(@"    $('#p2', top.document).css({ 'position': 'relative', 'z-index': '10' });");
            sb.AppendLine(@"    obj.resize(function(){ $('body', top.document).css('overflow', 'hidden'); $('.modal_overlay', top.document).css({ 'width': $(top.window).width() + 'px', 'height': $(top.window).height() + 'px' }); $('body', top.document).css('overflow', 'auto'); });");
            sb.AppendLine(@"  });");
            sb.AppendLine(@"</script>");
            return sb.ToString();
        }

        private string ModalOffScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<script type=""text/javascript"">");
            sb.AppendLine(@"  $(document).ready(function(){");
            sb.AppendLine(@"    var obj = (parent.$) ? parent.$(top.window) : $(top.window);");
            sb.AppendLine(@"    $('.modal_overlay', top.document).remove();");
            sb.AppendLine(@"    $('body').css('background-color', '#FFFFFF');");
            sb.AppendLine(@"    //obj.unbind('resize');");
            sb.AppendLine(@"});");
            sb.AppendLine(@"</script>");
            return sb.ToString();
        }
    }
}