using LNF;
using LNF.Repository;
using System.Configuration;
using System.Text;

namespace sselOnLine.AppCode.BLL
{
    public static class HFSafetyTestManager
    {
        public static bool InsertNewAnswer(int clientId, string answer)
        {
            DA.Command()
                .Param("Action", "HFSafetyTest")
                .Param("ClientID", clientId)
                .Param("Answer", answer)
                .ExecuteNonQuery("dbo.SafetyTestUser_Insert");

            return true;
        }

        private static string GetUserAnswer(int clientId)
        {
            string result = string.Empty;

            try
            {
                using (var reader = DA.Command().Param(new { Action = "GetAnswerForHFSafety", ClientID = clientId }).ExecuteReader("dbo.SafetyTestUser_Select"))
                {
                    if (reader.Read())
                        result = reader[0].ToString();
                }
            }
            catch
            {
                result = "Error";
            }

            return result;
        }

        public static bool GradeTest(int clientId)
        {
            string userTest = GetUserAnswer(clientId);
            string[] userAnswerSplitter = userTest.Split(',');

            //Get correct answer from db
            string correctAnswer = string.Empty;

            try
            {
                using (var reader = DA.Command().Param("TestID", TestManager.TestType.Safety).ExecuteReader("dbo.SafetyTestAnswers_Select"))
                {
                    if (reader.Read())
                    {
                        //the third column has the answers
                        correctAnswer = reader[3].ToString();
                    }
                }
            }
            catch
            {
                correctAnswer = "Error";
                return false;
            }

            string wrongAnswers = string.Empty;    //keep a record of all questions that have wrong answers
            string[] correctAnswerSplitter = correctAnswer.Split(',');
            //now compare the answers
            int wrongAnswerCount = 0;
            for (int index = 0; index < correctAnswerSplitter.Length; index++)
            {
                if (correctAnswerSplitter[index] != userAnswerSplitter[index])
                {
                    wrongAnswerCount += 1;
                    wrongAnswers += "Q" + (index + 1).ToString() + ": " + userAnswerSplitter[index] + "<br />";
                }
            }

            string testerName = ServiceProvider.Current.Context.GetSessionValue("DisplayName").ToString();
            string subj = "HF Safety Test result by " + testerName;

            StringBuilder body = new StringBuilder();
            body.AppendLine("Name: " + testerName + "<br />");
            body.AppendLine("Score: " + (correctAnswerSplitter.Length - wrongAnswerCount).ToString() + "/" + correctAnswerSplitter.Length.ToString() + "<br /><br />");
            if (wrongAnswerCount > 3)
            {
                body.AppendLine("Questions with wrong or empty answer(s)<br />");
                body.AppendLine(wrongAnswers);
                body.AppendLine("<br /><br />You have to retake the test again because you didn't pass the minimum score (9/12).");
            }
            else
            {
                body.AppendLine("Congratulations! You've passed the HF Safety Test.<br />");
                if (wrongAnswerCount > 0)
                {
                    body.AppendLine("Questions with wrong or empty answer(s)<br />");
                    body.AppendLine(wrongAnswers);
                }

                MarkTestPass(clientId);
            }

            //Send out email to tester and LNF User Services manager
            string adminEmail = ConfigurationManager.AppSettings["EmailSafetyTestAdmin"];
            string userEmail = ServiceProvider.Current.Context.GetSessionValue("Email").ToString();

            try
            {
                TestManager.SendEmail(adminEmail, userEmail, subj, body.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void MarkTestPass(int clientId)
        {
            DA.Command()
                .Param("Action", "HFSafetyTestPass")
                .Param("ClientID", clientId)
                .ExecuteNonQuery("dbo.SafetyTestUser_Insert");
        }
    }
}
