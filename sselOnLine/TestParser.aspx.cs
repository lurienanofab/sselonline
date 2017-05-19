using HtmlAgilityPack;
using sselOnLine.AppCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace sselOnLine
{
    public partial class TestParser : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadTests();
            }
        }

        private void LoadTests()
        {
            ddlTests.DataSource = TestUtility.GetTests();
            ddlTests.DataBind();
        }

        protected void btnParse_Click(object sender, EventArgs e)
        {
            ShowError();

            if (fuTest.HasFile)
            {
                if (Path.GetExtension(fuTest.FileName) == ".html")
                {
                    ParseFile(fuTest.PostedFile);
                }
                else
                {
                    ShowError("Only html files are allowed.");
                }
            }
            else
            {
                ShowError("Please select a file to upload.");
            }
        }

        private void ShowError()
        {
            phError.Visible = false;
            litErrorMessage.Text = string.Empty;
        }

        private void ShowError(string msg)
        {
            phError.Visible = true;
            litErrorMessage.Text = msg;
        }

        private void ParseFile(HttpPostedFile file)
        {
            var root = TestUtility.GetTests().FirstOrDefault(x => x.TestType == int.Parse(ddlTests.SelectedValue));

            root.QuestionsList = new List<Question>();

            using (StreamReader reader = new StreamReader(file.InputStream))
            {
                string content = reader.ReadToEnd();

                HtmlDocument hdoc = new HtmlDocument();

                hdoc.LoadHtml(content);

                var nodes = hdoc.DocumentNode.SelectNodes("//ol");

                Question q = null;

                foreach (var n in nodes)
                {
                    //n.InnerText.Dump();
                    if (q == null)
                    {
                        var li = n.Descendants("li").First();
                        q = new Question();
                        q.QuestionText = HttpUtility.HtmlDecode(li.InnerText).Trim().Replace("&", "&amp;");
                        q.QuestionNumber = GetQuestionNumber(n);
                        q.Required = IsRequired(li);
                    }
                    else
                    {
                        string correctAnswer;
                        q.Choices = GetChoices(n, out correctAnswer);
                        q.CorrectAnswer = correctAnswer;
                        root.QuestionsList.Add(q);
                        q = null;
                    }
                }

                string xml = TestUtility.SerializeTest(root);

                litOutput.Text = xml;
            }
        }

        private bool IsRequired(HtmlNode node)
        {
            var spans = node.SelectNodes("span[contains(@class, 'c0')]");

            if (spans == null)
                return false;

            if (spans.Count == 0)
                return false;

            return true;
        }

        private int GetQuestionNumber(HtmlNode node)
        {
            return node.GetAttributeValue("start", 0);
        }

        private List<Choice> GetChoices(HtmlNode node, out string correctAnswer)
        {
            var result = new List<Choice>();
            var items = node.Descendants("li");

            var index = 97;

            correctAnswer = null;

            foreach(var li in items)
            {
                var c = new Choice();
                c.ChoiceText = HttpUtility.HtmlDecode(li.InnerText).Trim().Replace("&", "&amp;");
                c.ChoiceIndex = ((char)index).ToString();
                if (c.ChoiceText.EndsWith("*"))
                { 
                    correctAnswer = c.ChoiceIndex;
                    c.ChoiceText = c.ChoiceText.TrimEnd('*').Trim();
                }
                result.Add(c);
                index++;
            }

            return result;
        }
    }
}