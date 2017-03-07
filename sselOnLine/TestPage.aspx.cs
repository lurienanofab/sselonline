using LNF.Models.Data;
using LNF.Repository;
using LNF.Web.Content;
using Newtonsoft.Json;
using sselOnLine.AppCode;
using sselOnLine.AppCode.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace sselOnLine
{
    public partial class TestPage : LNFPage
    {
        private string _LastName;
        private string _FirstName;
        private string _Email;
        private string _GroupName;
        private int _PageNumber;
        //private string _Answer;
        private DateTime _StartDate;
        private List<Question> _WrongAnswers = null;
        private List<Question> _CorrectAnswers = null;
        private IList<Question> _QuestionsList = null;
        private TestRoot xmlTestRoot = null;

        public override ClientPrivilege AuthTypes
        {
            get { return 0; }
        }

        public string TestName { get { return Request.QueryString["testname"]; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TestName))
            {
                Response.Redirect("~/Test.aspx");
            }
            else
            {
                var test = GetXMLTestRoot();

                Page.Title = test.TestTitle;
                hlinkManual.NavigateUrl = GetXMLTestRoot().TestManualLink;
                hypStartOver1.NavigateUrl = "~/Test.aspx?testname=" + TestName;
                hypStartOver2.NavigateUrl = "~/Test.aspx?testname=" + TestName;
                panRecaptcha.Visible = test.UseRecaptcha;

                string testsuburl = VirtualPathUtility.ToAbsolute(string.Format("~/Test.aspx?testname={0}", TestName));

                if (Page.User.Identity.IsAuthenticated)
                {
                    string furl = string.Format("{0}?ReturnUrl={1}", VirtualPathUtility.ToAbsolute("~/SignOut.aspx"), Server.UrlEncode(testsuburl));
                    litUserName.Text = string.Format("{0} [<a href=\"{1}\">log out</a>]", CurrentUser.UserName, furl);
                    txtFirstName.Text = CurrentUser.FName;
                    txtLastName.Text = CurrentUser.LName;
                    txtEmail.Text = CurrentUser.Email;
                    txtGroupName.Text = CurrentUser.OrgName;

                    txtFirstName.Enabled = false;
                    txtLastName.Enabled = false;
                }
                else
                {
                    trUserName.Visible = false;
                }
            }
        }

        private TestRoot GetXMLTestRoot()
        {
            if (null == xmlTestRoot)
            {
                if (!string.IsNullOrEmpty(TestName))
                {
                    StreamReader reader = new StreamReader(GetTestFilePath(TestName + ".xml"));
                    XmlSerializer deserializer = new XmlSerializer(typeof(TestRoot));
                    xmlTestRoot = (TestRoot)deserializer.Deserialize(reader);
                    reader.Close();
                }
            }

            if (null == xmlTestRoot)
                Response.Redirect(string.Format("~/TestPage.aspx?testname={0}", TestName), false);

            return xmlTestRoot;
        }

        public static string GetTestFilePath(string testFileName)
        {
            return ConfigurationManager.AppSettings["SecurePath"] + "\\testfiles\\" + testFileName;
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(StartTestAsync));
        }

        protected async Task StartTestAsync()
        {
            var test = GetXMLTestRoot();

            Session.Remove("lname");
            Session.Remove("fname");
            Session.Remove("email");
            Session.Remove("groupname");
            Session.Remove("page");
            Session.Remove("answers");
            Session.Remove("start");
            Session.Remove("questions");

            lblMsg.Text = string.Empty;
            lblCaptchaError.Text = string.Empty;

            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtGroupName.Text))
            {
                lblMsg.Text = "All data is required";
                return;
            }

            if (!SiteSecurity.EmailIsValid(txtEmail.Text))
            {
                lblMsg.Text = "Your email address is invalid";
                return;
            }

            if (test.UseRecaptcha)
            {
                var response = await GoogleRecaptcha1.Verify();

                if (!response.Success)
                {
                    lblCaptchaError.Text = "The CAPTCHA test failed. Please try again.";
                    return;
                }
            }

            panTestStart.Visible = false;
            panTestPage.Visible = true;

            _LastName = txtLastName.Text;
            Session["test.lname"] = _LastName;

            _FirstName = txtFirstName.Text;
            Session["test.fname"] = _FirstName;

            _Email = txtEmail.Text;
            Session["test.email"] = _Email;

            _GroupName = txtGroupName.Text;
            Session["test.groupname"] = _GroupName;

            _PageNumber = 1;
            Session["test.page"] = _PageNumber;

            //_Answer = string.Empty;
            //Session["test.answer"] = _Answer;

            _StartDate = DateTime.Now;
            Session["test.start"] = _StartDate;

            _QuestionsList = test.GetQuestions();
            Session["test.questions"] = _QuestionsList;

            rptQuestions.DataSource = GetPageQuestions();
            rptQuestions.DataBind();

            // if exam time is longer than session time(20) then extend session timeout
            if (Session.Timeout < test.ExamTime)
            {
                Session.Timeout = test.ExamTime;
            }
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Question q = (Question)e.Item.DataItem;

                RadioButtonList rblChoices = (RadioButtonList)e.Item.FindControl("rblChoices");
                rblChoices.DataSource = q.Choices;
                rblChoices.DataBind();

                HiddenField hidQuestionNumber = (HiddenField)e.Item.FindControl("hidQuestionNumber");
                hidQuestionNumber.Value = q.QuestionNumber.ToString();
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (CheckSession())
            {
                var test = GetXMLTestRoot();

                //check the time limit
                TimeSpan ts = DateTime.Now - _StartDate;

                if (ts.Minutes > test.ExamTime)
                {
                    panTimeLimit.Visible = true;
                    panTestPage.Visible = false;
                    return;
                }

                _PageNumber += 1;

                //get the current user answers
                foreach (RepeaterItem item in rptQuestions.Items)
                {
                    RadioButtonList rblChoices = (RadioButtonList)item.FindControl("rblChoices");
                    HiddenField hidQuestionNumber = (HiddenField)item.FindControl("hidQuestionNumber");
                    int qnum = int.Parse(hidQuestionNumber.Value);
                    var q = _QuestionsList.First(x => x.QuestionNumber == qnum);
                    q.UserSelectedAnswer = rblChoices.SelectedValue;
                }

                var page = GetPageQuestions();

                if (page.Count() > 0)
                {
                    //load the next page of questions
                    rptQuestions.DataSource = page;
                    rptQuestions.DataBind();
                    Session["test.page"] = _PageNumber;
                    Session["test.start"] = DateTime.Now;
                }
                else
                {
                    GradeTest();

                    int totalQuestions = _QuestionsList.Count;
                    int userScore = totalQuestions - _WrongAnswers.Count;

                    if (userScore >= test.PassingScore)
                    {
                        SaveCorrectAnswers();
                        string results = EmailAnswers(true);

                        results += WrongAnswersHTML(q => q.ToHtml(true));

                        litPassedMessage.Text = results;
                        litTestAdminEmail.Text = string.Format(@"<a href=""mailto:{0}"">{0}</a>", ConfigurationManager.AppSettings["EmailSafetyTestAdmin"]);
                        panPassedMessage.Visible = true;
                        panTestPage.Visible = false;
                    }
                    else
                    {
                        if (userScore >= (totalQuestions / 2)) // if right anwers are above 50% but not above pass marks
                        {
                            EmailAnswers(false);
                        }

                        ShowWrongAnswers();
                    }
                }
            }
            else
            {
                panTestStart.Visible = true;
                panTestPage.Visible = false;
            }
        }

        private IEnumerable<Question> GetPageQuestions()
        {
            var value = GetXMLTestRoot().QuestionsPerPage;

            var questionsPerPage = value > 0 ? value : _QuestionsList.Count;
            var skip = 0;
            var take = 0;

            skip = (_PageNumber - 1) * questionsPerPage;
            take = questionsPerPage;

            return _QuestionsList.Skip(skip).Take(take);
        }

        private int SaveCorrectAnswers()
        {
            int clientid = 0;

            if (Page.User.Identity.IsAuthenticated)
                clientid = CurrentUser.ClientID;

            // @Answer was a comma separated string but now a subset of questions might be used, and also the order might be random, so we also need to save the QuestionNumber along with the user answer.
            var answers =  _QuestionsList.Select(x => new { x.QuestionNumber, x.UserSelectedAnswer }).ToArray();
            string answersJson = JsonConvert.SerializeObject(answers);

            using (var dba = DA.Current.GetAdapter())
            {
                string sql = "INSERT sselData.dbo.SafetyTestUser (FirstName, LastName, UMID, GroupName, Email, Misc, Answer, TestDate, TestID, ClientID, ClientIP) VALUES (@FirstName, @LastName, @UMID, @GroupName, @Email, NULL, @Answer, GETDATE(), @TestID, @ClientID, @ClientIP)";
                return dba.CommandTypeText()
                    .AddParameter("@FirstName", _FirstName)
                    .AddParameter("@LastName", _LastName)
                    .AddParameter("@UMID", "-") // UMID is not used anymore
                    .AddParameter("@GroupName", _GroupName)
                    .AddParameter("@Email", _Email)
                    .AddParameter("@Answer", answersJson)
                    .AddParameter("@TestID", GetXMLTestRoot().TestType)
                    .AddParameter("@ClientID", clientid)
                    .AddParameter("@ClientIP", Request.ServerVariables["REMOTE_ADDR"])
                    .ExecuteNonQuery(sql);
            }
        }

        private string EmailAnswers(bool passed)
        {
            var test = GetXMLTestRoot();

            double score = Convert.ToDouble(_CorrectAnswers.Count) / Convert.ToDouble(_QuestionsList.Count);

            var sb = new StringBuilder();

            if (passed)
            {
                sb.AppendLine("<h2>Congratulations! You've passed the " + test.TestTitle + "</h2>");
            }
            else
            {
                sb.AppendLine("<h2>Sorry, you have not reached the minimum score for the " + test.TestTitle + ". Please review the manual and take the test again.</h2>");
            }

            sb.AppendLine("<ul>");
            sb.AppendLine("<li>Last Name: " + _LastName + "</li>");
            sb.AppendLine("<li>First Name: " + _FirstName + "</li>");
            sb.AppendLine("<li>Email: " + _Email + "</li>");
            sb.AppendLine("<li>Group Name: " + _GroupName + "</li>");
            sb.AppendLine("<li>Score: " + score.ToString("0.00%") + " (" + _CorrectAnswers.Count.ToString() + "/" + _QuestionsList.Count.ToString() + ")</li>");
            sb.AppendLine("</ul>");

            string subj = string.Format("{0} results for {1} {2}", test.TestTitle, _FirstName, _LastName);

            if (Page.User.Identity.IsAuthenticated)
            {
                subj = string.Format("--Recertification-- {0} ({1})", subj, CurrentUser.UserName);
            }

            string body = sb.ToString();

            body += WrongAnswersHTML(q => q.ToString(passed));

            SendEmail(_Email, ConfigurationManager.AppSettings["EmailSafetyTestAdmin"], subj, body);

            return sb.ToString();
        }

        private void SendEmail(string toAddr, string ccAddr, string subj, string body)
        {
            TestManager.SendEmail(ccAddr, toAddr, subj, body);
        }

        private void ShowWrongAnswers()
        {
            panTestPage.Visible = false;
            panWrongAnswers.Visible = true;
            litWrongAnswers.Text = WrongAnswersHTML(q => q.ToHtml(false));
        }

        private string WrongAnswersHTML(Func<Question, string> fn)
        {
            string html = "<h4>Questions with incorrect answers:</h4>";
            html += "<ul>";
            if (_WrongAnswers != null && _WrongAnswers.Count > 0)
            {
                foreach (var q in _WrongAnswers)
                {
                    html += string.Format("<li>{0}</li>", fn(q));
                    //string allAnswers = q.GetAllAnswers();
                    //string userSelAnswer = q.GetUserSelectedAnswerText();
                    //string correctAnswer = FindCorrectAnswer(q);

                    //if (showCorrectAnswer)
                    //    html += string.Format("<li>{0}{1}[Correct Answer: {2}. Your Answer: {3}]</li>", q.QuestionText, allAnswers, correctAnswer, userSelAnswer);
                    //else
                    //    html += string.Format("<li>{0}{1}[Your Answer: {2}]</li>", q.QuestionText, allAnswers, userSelAnswer);
                }
            }
            else
                html += "<li>No wrong answers were found.</li>";

            html += "</ul>";

            return html;
        }

        private string FindCorrectAnswer(Question question)
        {
            var choice = question.GetCorrectAnswer();

            if (choice == null)
                return string.Empty;
            else
                return choice.ChoiceText;
        }

        private void GradeTest()
        {
            _WrongAnswers = new List<Question>();
            _CorrectAnswers = new List<Question>();

            foreach (var q in _QuestionsList)
            {
                if (q.CorrectAnswer == q.UserSelectedAnswer)
                    _CorrectAnswers.Add(q);
                else
                    _WrongAnswers.Add(q);
            }
        }

        public bool CheckSession()
        {
            if (Session["test.lname"] != null)
                _LastName = Session["test.lname"].ToString();
            else
                return false;

            if (string.IsNullOrEmpty(_LastName))
                return false;

            if (Session["test.fname"] != null)
                _FirstName = Session["test.fname"].ToString();
            else
                return false;

            if (string.IsNullOrEmpty(_FirstName))
                return false;

            if (Session["test.email"] != null)
                _Email = Session["test.email"].ToString();
            else
                return false;

            if (string.IsNullOrEmpty(_Email))
                return false;

            if (Session["test.groupname"] != null)
                _GroupName = Session["test.groupname"].ToString();
            else
                return false;

            if (string.IsNullOrEmpty(_GroupName))
                return false;

            if (Session["test.page"] != null)
                _PageNumber = Convert.ToInt32(Session["test.page"]);
            else
                return false;

            if (Session["test.start"] != null)
                _StartDate = Convert.ToDateTime(Session["test.start"]);
            else
                return false;

            if (Session["test.questions"] != null)
                _QuestionsList = (IList<Question>)Session["test.questions"];
            else
                return false;

            return true;
        }

        protected void btnSendTestEmail_Click(object sender, EventArgs e)
        {
            SendEmail("jgett@umich.edu", "jgett@lnf.umich.edu", "This is a test", "Hello World!");
            litSendTestEmailMessage.Text = "<span><strong>Test email was sent!</strong></span>";
        }

        protected string GetExamTime()
        {
            return GetXMLTestRoot().ExamTime.ToString();
        }

        protected string GetTestTitle()
        {
            return GetXMLTestRoot().TestTitle;
        }

        protected int GetQuestionStartNumber()
        {
            var questionsPerPage = GetXMLTestRoot().QuestionsPerPage;

            if (questionsPerPage == 0)
                return 1;

            return _PageNumber * questionsPerPage - 1;
        }
    }
}