using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using LNF;
using LNF.Data;
using LNF.CommonTools;
using sselOnLine.AppCode;
using sselOnLine.AppCode.DAL;

namespace sselOnLine.Controls
{
    public partial class Survey : Applet
    {
        public override string AppletPrefix
        {
            get { return "SURVEY"; }
        }

        private DataRow _CurrentQuestion;
        private DataRow _ClientResponse;

        public string QuestionText
        {
            get{return litSurveyQuestion.Text;}
            set{litSurveyQuestion.Text = value;}
        }

        public int QuestionID
        {
            get
            {
                if (ViewState["CurrentSurveyQuestionID"] == null)
                {
                    //default
                    ViewState["CurrentSurveyQuestionID"] = -1;
                }
                return Convert.ToInt32(ViewState["CurrentSurveyQuestionID"]);
            }
            set
            {
                ViewState["CurrentSurveyQuestionID"] = value;
            }
        }

        public string Answers
        {
            get
            {
                if (ViewState["CurrentAnswers"] == null)
                {
                    //default
                    ViewState["CurrentAnswers"] = string.Empty;
                }

                return ViewState["CurrentAnswers"].ToString();
            }
            set
            {
                ViewState["CurrentAnswers"] = value;
            }
        }

        public bool Anonymous
        {
            get
            {
                bool result = false;

                DataRow[] rows = Properties.Select(string.Format("Property = '{0}_ANONYMOUS'", AppletPrefix));
                if (rows.Length > 0 && rows[0]["Value"].ToString() == "Yes")
                    result = true;

                return result;            
            }
            set
            {
                AppletDA.UpdateProperty(AppletPrefix + "_ANONYMOUS", (value) ? "Yes" : "No");
                _Properties = AppletDA.SelectProperties();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                SetState();

            Page.ClientScript.RegisterStartupScript(typeof(Page), "survey_watermark_script", "$(document).ready(function(){$('.applet .answer .answer_text').watermark('Optional answer/comments', { className: 'watermark' });});", true);
        }

        protected void btnAnswer_Click(object sender, EventArgs e)
        {
            litErrorMessage.Text = string.Empty;

            if (rblAnswer.SelectedIndex == -1)
            {
                litErrorMessage.Text = "Please select a response.";
                return;
            }

            if (QuestionID == -1) return;

            bool anon = true;
            if (Anonymous)
                anon = !chkNotAnonymous.Checked;
            else
                anon = false;

            SurveyDA.InsertAnswer(GetClientID(), QuestionID, rblAnswer.SelectedItem.Text, txtAnswer.Text, anon);

            SetState();
        }

        protected override void admin_Command(object sender, CommandEventArgs e)
        {
            lblAddErrorMsg.Text = string.Empty;
            switch(e.CommandName)
            {
                case "add_question":
                    if (string.IsNullOrEmpty(txtNewQuestion.Text))
                        lblAddErrorMsg.Text = "Question text is required.";
                    else if (string.IsNullOrEmpty(txtAnswer1.Text))
                        lblAddErrorMsg.Text = "Answer #1 is the default and cannot be left blank.";
                    else
                    {
                        string[] a = {txtAnswer1.Text, txtAnswer2.Text, txtAnswer3.Text, txtAnswer4.Text, txtAnswer5.Text};

                        SurveyDA.InsertQuestion(GetClientID(), string.Join("|", a), HttpUtility.HtmlEncode(txtNewQuestion.Text));
                        txtNewQuestion.Text = string.Empty;
                        txtAnswer1.Text = string.Empty;
                        txtAnswer2.Text = string.Empty;
                        txtAnswer3.Text = string.Empty;
                        txtAnswer4.Text = string.Empty;
                        txtAnswer5.Text = string.Empty;
                    }
                    LoadAdminCurrentQuestion();
                    break;
                case "save_properties":
                    Anonymous = chkAnonymous.Checked;
                    Enabled = chkEnabled.Checked;
                    break;
                case "save_current":
                    litAdminQuestionSaveError.Text = string.Empty;

                    if (!string.IsNullOrEmpty(txtAdminCurrentQuestionText.Text))
                    {
                        DateTime expdate;
                        if (!DateTime.TryParse(txtAdminCurrentQuestionExpirationDate.Text, out expdate))
                            expdate = DateTime.MinValue;
                        SurveyDA.UpdateQuestion(GetClientID(), QuestionID, txtAdminCurrentQuestionText.Text, expdate, chkAdminCurrentQuestionActive.Checked);
                        LoadAdminCurrentQuestion();
                    }
                    else
                        litAdminQuestionSaveError.Text = @"<div style=""color: #FF0000;"">The question text cannot be blank.</div>";
                    break;
            }
        }

        protected void report_Command(object sender, CommandEventArgs e)
        {
            ExportReport(e.CommandName, QuestionID);
        }

        private void ExportReport(string displayOption, int id)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;
            switch(displayOption)
            {
                case "all":
                    dt = SurveyDA.SelectReportAllQuestions();
                    break;
                case "byid":
                    dt = SurveyDA.SelectReportByQuestionID(id);
                    break;
            }

            sb.AppendLine(@"""ID"",""LName"",""FName"",""Email"",""Question"",""ClientAnswer"",""TextAnswer"",""AnswerDate"",""IsLabUser"",""IsStaff""");
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(string.Format(@"""{0}"",", dr.Field<int>("ClientSurveyQuestionID")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("LName")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("FName")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("Email")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("QuestionText")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("ClientAnswer").Replace(@"""", @"""""")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<string>("TextAnswer").Replace(@"""", @"""""")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<DateTime>("AnswerDate").ToString("yyyy-MM-dd HH:mm:ss")));
                sb.Append(string.Format(@"""{0}"",", dr.Field<bool>("IsLabUser")));
                sb.AppendLine(string.Format(@"""{0}""", dr.Field<bool>("IsStaff")));
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=SurveyQuestionResults_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv");
            Response.ContentType = "application/csv";
            Response.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected override void SetState()
        {
            litErrorMessage.Text = string.Empty;

            LoadAdminCurrentQuestion();

            LoadReportQuestions();

            rblAnswer.Items.Clear();
            string[] splitter = Answers.Split('|');
            foreach (string s in splitter)
                if (!string.IsNullOrEmpty(s)) rblAnswer.Items.Add(s);

            rblAnswer.SelectedIndex = -1;

            panAnonMessage.Visible = Anonymous;
            chkAnonymous.Checked = Anonymous;

            base.SetState();
        }

        private void LoadReportQuestions()
        {
            ddlReportSelectQuestion.DataSource = SurveyDA.SelectAllQuestions();
            ddlReportSelectQuestion.DataBind();
            ddlReportSelectQuestion.Items.Insert(0, new ListItem("-- Select --", "=x="));
        }

        private string ReplaceTags(string raw)
        {
            string result = raw;
            string[] allowedTags = {"div", "p", "ol", "ul", "li", "img"};

            foreach (string tag in allowedTags)
            {
                result = Regex.Replace(result, @"\[" + tag + @"(.*?)\]", @"<" + tag + @"$1>");
                result = Regex.Replace(result, @"\[/" + tag + @"\]", @"</" + tag + @">");
            }

            return result;
        }

        private void LoadAdminCurrentQuestion()
        {
            _CurrentQuestion = SurveyDA.SelectCurrentQuestion();

            if (_CurrentQuestion != null)
            {
                QuestionText = ReplaceTags(_CurrentQuestion["QuestionText"].ToString());
                QuestionID = Convert.ToInt32(_CurrentQuestion["SurveyQuestionID"]);
                Answers = _CurrentQuestion["Answers"].ToString();
            }

            panAdminCurrentQuestion.Visible = true;

            if (_CurrentQuestion == null)
            {
                trCurrentQuestion.Visible = false;
                trNoData.Visible = true;
            }
            else
            {
                trCurrentQuestion.Visible = true;
                trNoData.Visible = false;

                litAdminCurrentQuestionID.Text = _CurrentQuestion["SurveyQuestionID"].ToString();
                txtAdminCurrentQuestionText.Text = _CurrentQuestion["QuestionText"].ToString();
                litAdminCurrentQuestionCreatedDate.Text = Convert.ToDateTime(_CurrentQuestion["CreatedDate"]).ToString("MM/dd/yyyy HH:mm:ss");
                txtAdminCurrentQuestionExpirationDate.Text = GetExpirationDate(_CurrentQuestion);
                chkAdminCurrentQuestionActive.Checked = Convert.ToBoolean(_CurrentQuestion["Active"]);
            }
        }

        private string GetExpirationDate(DataRow dr)
        {
            string result = string.Empty;
            if (dr != null &&  dr["ExpirationDate"] != DBNull.Value)
                result = Convert.ToDateTime(dr["ExpirationDate"]).ToString("MM/dd/yyyy HH:mm:ss");
            return result;
        }

        protected override bool IsVisible()
        {
            if (Enabled)
            {
                //Check to see if there is a current question
                if (_CurrentQuestion == null) return false;

                //Check to see use user has already answered
                return !ClientHasResponded();
            }
            else
                return false;
        }

        public bool ClientHasResponded()
        {
            bool result = false;

            _ClientResponse = null;

            if (QuestionID != -1)
            {
                _ClientResponse = SurveyDA.SelectClientResponse(GetClientID(), QuestionID);
                result = _ClientResponse != null;
            }

            return result;
        }

        protected void ddlReportSelectQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlReportSelectQuestion.SelectedValue != "=x=")
            {
                int id = Convert.ToInt32(ddlReportSelectQuestion.SelectedValue);
                ExportReport("byid", id);
            }
        }
    }
}