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
            return DA.Command(CommandType.Text)
                .Param("ClientID", clientId)
                .Param("Answers", answers)
                .Param("QuestionText", text)
                .ExecuteNonQuery("INSERT SurveyQuestion (QuestionText, Answers, CreatedDate, ExpirationDate, Active, CreatedByClientID, UpdatedByClientID) VALUES (@QuestionText, @Answers, GETDATE(), NULL, 1, @ClientID, @ClientID)")
                .Value;
        }

        public static DataRow SelectCurrentQuestion()
        {
            var dt = DA.Command(CommandType.Text)
                .FillDataTable("SELECT * FROM SurveyQuestion WHERE ISNULL(ExpirationDate, DATEADD(dd, 1, GETDATE())) > GETDATE() AND Active = 1 ORDER BY CreatedDate DESC");

            DataRow dr = (dt.Rows.Count > 0) ? dt.Rows[0] : null;

            return dr;
        }

        public static int InsertAnswer(int clientId, int questionId, string answer, string text, bool anon)
        {
            return DA.Command(CommandType.Text)
                .Param("ClientID", clientId == 0, DBNull.Value, clientId)
                .Param("QuestionID", questionId)
                .Param("ClientAnswer", CleanInput(answer))
                .Param("TextAnswer", string.IsNullOrEmpty(text), DBNull.Value, text)
                .Param("IsAnonymous", anon)
                .ExecuteNonQuery("INSERT ClientSurveyQuestion (ClientID, SurveyQuestionID, ClientAnswer, TextAnswer, AnswerDate, IsAnonymous) VALUES (@ClientID, @QuestionID, @ClientAnswer, @TextAnswer, GETDATE(), @IsAnonymous)")
                .Value;
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
            var dt = DA.Command(CommandType.Text)
                .Param("ClientID", clientId)
                .Param("QuestionID", questionId)
                .FillDataTable("SELECT * FROM dbo.ClientSurveyQuestion WHERE ClientID = @ClientID AND SurveyQuestionID = @QuestionID");

            return dt.AsEnumerable().FirstOrDefault();
        }

        public static DataTable SelectAllClientResponses(int questionId)
        {
            return DA.Command(CommandType.Text)
                .Param("QuestionID", questionId)
                .FillDataTable("SELECT * FROM dbo.ClientSurveyQuestion WHERE SurveyQuestionID = @QuestionID");
        }

        public static DataTable SelectReportAllQuestions()
        {
            return DA.Command()
                .Param("Action", "SelectReportAllQuestions")
                .FillDataTable("dbo.Survey_Select");
        }

        public static DataTable SelectReportByQuestionID(int surveyQuestionId)
        {
            return DA.Command()
                .Param("Action", "SelectReportByQuestionID")
                .Param("SurveyQuestionID", surveyQuestionId)
                .FillDataTable("dbo.Survey_Select");
        }

        public static void UpdateQuestion(int clientId, int questionId, string text, DateTime expirationDate, bool active)
        {
            DA.Command(CommandType.Text)
                .Param("ClientID", clientId)
                .Param("QuestionID", questionId)
                .Param("QuestionText", CleanInput(text))
                .Param("ExpirationDate", expirationDate == DateTime.MinValue, DBNull.Value, expirationDate)
                .Param("Active", active)
                .ExecuteNonQuery("UPDATE SurveyQuestion SET QuestionText = @QuestionText, ExpirationDate = @ExpirationDate, Active = @Active, UpdatedByClientID = @ClientID WHERE SurveyQuestionID = @QuestionID");
        }

        public static DataTable SelectAllQuestions()
        {
            return DA.Command()
                .Param("Action", "SelectAllQuestions")
                .FillDataTable("dbo.Survey_Select");
        }
    }
}
