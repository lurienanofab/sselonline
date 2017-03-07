using LNF.CommonTools;
using LNF.Repository;
using System;
using System.Data;
using System.Linq;

namespace sselOnLine.AppCode.DAL
{
    public static class SurveyDA
    {
        public static int InsertQuestion(int clientId, string answers, string text)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                return dba.SelectCommand
                    .CommandTypeText()
                    .AddParameter("@ClientID", clientId)
                    .AddParameter("@Answers", answers)
                    .AddParameter("@QuestionText", text)
                    .ExecuteNonQuery("INSERT SurveyQuestion (QuestionText, Answers, CreatedDate, ExpirationDate, Active, CreatedByClientID, UpdatedByClientID) VALUES (@QuestionText, @Answers, GETDATE(), NULL, 1, @ClientID, @ClientID)");
            }
        }

        public static DataRow SelectCurrentQuestion()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                DataTable dt = dba.CommandTypeText().FillDataTable("SELECT * FROM SurveyQuestion WHERE ISNULL(ExpirationDate, DATEADD(dd, 1, GETDATE())) > GETDATE() AND Active = 1 ORDER BY CreatedDate DESC");
                DataRow dr = (dt.Rows.Count > 0) ? dt.Rows[0] : null;
                return dr;
            }
        }

        public static int InsertAnswer(int clientId, int questionId, string answer, string text, bool anon)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                return dba.SelectCommand
                    .CommandTypeText()
                    .AddParameterIf("@ClientID", clientId == 0, clientId, DBNull.Value)
                    .AddParameter("@QuestionID", questionId)
                    .AddParameter("@ClientAnswer", SurveyDA.CleanInput(answer))
                    .AddParameter("@TextAnswer", Utility.DBNullCheck(text, string.IsNullOrEmpty(text)))
                    .AddParameter("@IsAnonymous", anon)
                    .ExecuteNonQuery("INSERT ClientSurveyQuestion (ClientID, SurveyQuestionID, ClientAnswer, TextAnswer, AnswerDate, IsAnonymous) VALUES (@ClientID, @QuestionID, @ClientAnswer, @TextAnswer, GETDATE(), @IsAnonymous)");
            }
        }

        private static string CleanInput(string input)
        {
            string result = input;

            //get rid of all html tags
            result = result.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");

            //allow <br /> tag
            result = result.Replace("&lt;br&gt;", "<br />");
            result = result.Replace("&lt;br /&gt;", "<br />");

            return result;
        }

        public static DataRow SelectClientResponse(int clientId, int questionId)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                DataTable dt = dba.CommandTypeText()
                    .AddParameter("@ClientID", clientId)
                    .AddParameter("@QuestionID", questionId)
                    .FillDataTable("SELECT * FROM ClientSurveyQuestion WHERE ClientID = @ClientID AND SurveyQuestionID = @QuestionID");
                return dt.AsEnumerable().FirstOrDefault();
            }
        }

        public static DataTable SelectAllClientResponses(int questionId)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                return dba.CommandTypeText()
                    .AddParameter("@QuestionID", questionId)
                    .FillDataTable("SELECT * FROM ClientSurveyQuestion WHERE SurveyQuestionID = @QuestionID");
            }
        }

        public static DataTable SelectReportAllQuestions()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                dba.AddParameter("@Action", "SelectReportAllQuestions");
                DataTable dt = dba.FillDataTable("Survey_Select");
                return dt;
            }
        }

        public static DataTable SelectReportByQuestionID(int surveyQuestionId)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                dba.AddParameter("@Action", "SelectReportByQuestionID");
                dba.AddParameter("@SurveyQuestionID", surveyQuestionId);
                DataTable dt = dba.FillDataTable("Survey_Select");
                return dt;
            }
        }

        public static void UpdateQuestion(int clientId, int questionId, string text, DateTime expirationDate, bool active)
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                dba.CommandTypeText()
                    .AddParameter("@ClientID", clientId)
                    .AddParameter("@QuestionID", questionId)
                    .AddParameter("@QuestionText", CleanInput(text))
                    .AddParameter("@ExpirationDate", Utility.DBNullCheck(expirationDate, expirationDate == DateTime.MinValue))
                    .AddParameter("@Active", active)
                    .ExecuteNonQuery("UPDATE SurveyQuestion SET QuestionText = @QuestionText, ExpirationDate = @ExpirationDate, Active = @Active, UpdatedByClientID = @ClientID WHERE SurveyQuestionID = @QuestionID");
            }
        }

        public static DataTable SelectAllQuestions()
        {
            using (SQLDBAccess dba = new SQLDBAccess("cnSselData"))
            {
                dba.AddParameter("@Action", "SelectAllQuestions");
                DataTable dt = dba.FillDataTable("Survey_Select");
                return dt;
            }
        }
    }
}
