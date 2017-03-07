using LNF;
using sselOnLine.AppCode.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sselOnLine.AppCode
{
    public abstract class TestPage : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack && !Providers.IsProduction())
                SetCorrectAnswers();

            base.OnLoad(e);
        }

        protected void Next_Click(object sender, EventArgs e)
        {
            OnNext(e);
        }

        protected virtual void OnNext(EventArgs e)
        {
            int id = -1;
            try
            {
                id = Convert.ToInt32(Request.QueryString["id"]);
            }
            catch
            {
                RedirectToStartPage();
                return;
            }

            TestManager.InsertAnswer(id, GetAnswers(), QuestionRange);

            RedirectToNextPage(id);
        }

        protected virtual void SetCorrectAnswers()
        {
            Dictionary<RadioButtonList, string> answers = GetQuestions();
            foreach (KeyValuePair<RadioButtonList, string> kvp in answers)
                kvp.Key.SelectedValue = kvp.Value;
        }

        protected virtual string GetAnswers()
        {
            string result = string.Join(", ", GetQuestions().Select(x => x.Key.SelectedValue));
            result += ", ";
            return result;
        }

        protected abstract Dictionary<RadioButtonList, string> GetQuestions();

        protected abstract TestManager.QuestionRange QuestionRange { get; }

        protected abstract void RedirectToStartPage();

        protected abstract void RedirectToNextPage(int id);
    }
}
