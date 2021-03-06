﻿using LNF;
using LNF.CommonTools;
using LNF.Models.Mail;
using LNF.Repository;
using System;
using System.Configuration;
using System.Net;
using System.Text;

namespace sselOnLine.AppCode.BLL
{
    public static class TestManager
    {
        public enum QuestionRange
        {
            OneToTen,
            ElevenToTwenty,
            TwentyOneToThirty,
            ThirtyOneToForty,
        }

        public enum TestType
        {
            OSEH = 1,
            LNF = 2,
            Safety = 3,
            UserMeeting2011 = 4
        }

        public static int InsertNewTestHeader(UserTest th)
        {
            var command = DA.Command().Param("Action", "FirstTest");

            if (th.ClientID != 0) command.Param("ClientID", th.ClientID);
            if (!string.IsNullOrEmpty(th.FirstName)) command.Param("FirstName", th.FirstName);
            if (!string.IsNullOrEmpty(th.LastName)) command.Param("LastName", th.LastName);
            if (!string.IsNullOrEmpty(th.Answer)) command.Param("Answer", th.Answer);
            if (!string.IsNullOrEmpty(th.UMID)) command.Param("UMID", th.UMID);
            if (!string.IsNullOrEmpty(th.Email)) command.Param("Email", th.Email);
            if (!string.IsNullOrEmpty(th.Misc)) command.Param("Misc", th.Misc);
            if (th.TestDate != DateTime.MinValue) command.Param("TestDate", th.TestDate);
            if (th.TestID != 0) command.Param("TestID", th.TestID);
            if (!string.IsNullOrEmpty(th.IP.ToString())) command.Param("ClientIP", th.IP.ToString());
            int result = 0;

            try
            {
                using (var reader = command.ExecuteReader("dbo.SafetyTestUser_Insert"))
                {
                    if (reader.Read())
                        result = Convert.ToInt32(reader["UserTestID"]);
                    else
                        result = -1;
                }
            }
            catch
            {
                result = -2;
            }

            return result;
        }

        public static bool InsertAnswer(int userTestId, string answer, QuestionRange qr)
        {
            string result = string.Empty;
            if (qr != QuestionRange.OneToTen)
            {
                //The answer is stored in a comma separated format
                //First, get the current answer from database
                result = GetUserAnswer(userTestId);

                if (result == "Error")
                    return false;
            }

            result += answer;

            UpdateAnswer(userTestId, result);

            return true;
        }

        private static void UpdateAnswer(int userTestId, string answer)
        {
            DA.Command()
                .Param("UserTestID", userTestId)
                .Param("Answer", answer)
                .ExecuteNonQuery("dbo.SafetyTestUser_Update");
        }

        private static string GetUserAnswer(int userTestId)
        {
            string result = string.Empty;

            try
            {
                using (var reader = DA.Command().Param(new { Action = "GetAnswer", UserTestID = userTestId }).ExecuteReader("dbo.SafetyTestUser_Select"))
                {
                    if (reader.Read())
                    {
                        //only one column is returned
                        result = reader[0].ToString();
                    }
                }
            }
            catch
            {
                result = "Error";
            }

            return result;
        }

        private static UserTest GetUserTest(int userTestId)
        {
            UserTest result = null;

            try
            {
                using (var reader = DA.Command().Param(new { Action = "GetAll", UserTestID = userTestId }).ExecuteReader("dbo.SafetyTestUser_Select"))
                {
                    if (reader.Read())
                    {
                        result = new UserTest()
                        {
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            UMID = reader["UMID"].ToString(),
                            Email = reader["Email"].ToString(),
                            Misc = reader["Misc"].ToString(),
                            Answer = reader["Answer"].ToString(),
                            TestID = Convert.ToInt32(reader["TestID"]),
                            UserTestID = userTestId,
                            ClientID = Utility.ConvertTo(reader["ClientID"], 0),
                            IP = IPAddress.Parse(reader["ClientIP"].ToString()),
                            ClientData = new UserTest.Client(result.ClientID)
                        };
                    }
                }
            }
            catch
            {
                return null;
            }

            return result;
        }

        public static string GradeTest(int userTestId, TestType testType)
        {
            if (userTestId < 1)
                return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [Invalid UserTestID]</div>";

            UserTest userTest = TestManager.GetUserTest(userTestId);
            if (userTest == null)
                return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [No test found for specified UserTestID]</div>";

            if (userTest.TestID != (int)testType)
                return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [TestTypeID mismatch]</div>";

            string[] userAnswerSplitter = userTest.Answer.Split(',');

            //Get correct answer from db
            string correctAnswer = string.Empty;
            string adminEmail = string.Empty;
            string userEmail = userTest.ClientData.Email();

            try
            {
                using (var reader = DA.Command().Param("TestID", testType).ExecuteReader("dbo.SafetyTestAnswers_Select"))
                {
                    if (reader.Read())
                    {
                        //wtf? Using index here is a really bad idea. What are the column names??
                        correctAnswer = reader[3].ToString();
                        adminEmail = reader[4].ToString();
                    }
                    else
                        return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [No answers found for specified TestTypeID]</div>";
                }
            }
            catch
            {
                return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [Database error: unable to retrieve answers]</div>";
            }

            adminEmail = (string.IsNullOrEmpty(adminEmail)) ? ConfigurationManager.AppSettings["EmailSafetyTestAdmin"] : adminEmail;
            userEmail = (string.IsNullOrEmpty(userEmail)) ? userTest.Email : userEmail;
            StringBuilder wrongAnswers = new StringBuilder(); //keep a record of all questions that have wrong answers
            string[] correctAnswerSplitter = correctAnswer.Split(',');

            //now compare the answers 
            int wrongAnswerCount = 0;
            for (int index = 0; index < correctAnswerSplitter.Length; index++)
            {
                if (index >= userAnswerSplitter.Length)
                {
                    wrongAnswerCount += 1;
                    wrongAnswers.AppendLine("Q" + (index + 1).ToString() + ": [blank]<br />");
                }
                else if (correctAnswerSplitter[index].Trim() != userAnswerSplitter[index].Trim())
                {
                    wrongAnswerCount += 1;
                    wrongAnswers.AppendLine("Q" + (index + 1).ToString() + ": " + userAnswerSplitter[index] + "<br />");
                }
            }

            int correctAnswerCount = correctAnswerSplitter.Length - wrongAnswerCount;
            double score = Convert.ToDouble(correctAnswerCount) / Convert.ToDouble(correctAnswerSplitter.Length);

            double minscore = GetMinScoreByTestType(testType);

            string result = string.Empty;

            if (score >= minscore)
            {
                string tester = userTest.ClientData.DisplayName();
                tester = (string.IsNullOrEmpty(tester)) ? userTest.FirstName + " " + userTest.LastName : tester;

                string subj = string.Empty;
                StringBuilder body = new StringBuilder();
                body.AppendLine("Name: " + tester + "<br />");

                bool sendClientEmail = true;

                switch (testType)
                {
                    case TestType.OSEH:
                        subj = "OSEH Safety test result by " + tester;
                        body.AppendLine("UMID: " + userTest.UMID + "<br /><br />");
                        body.AppendLine("The minimum score for this test is 17/20. Please see below for questions with wrong answers.<br /><br />");
                        body.AppendLine("Score: " + (correctAnswerSplitter.Length - wrongAnswerCount).ToString() + "/" + correctAnswerSplitter.Length.ToString() + "<br /><br />");
                        if (wrongAnswerCount > 0)
                        {
                            body.AppendLine("Questions with wrong or empty answer(s)<br />");
                            body.AppendLine(wrongAnswers.ToString());
                        }
                        result = "<div>Your test results have been sent to the LNF User Services Manager and to your email inbox.</div>";
                        break;
                    case TestType.LNF:
                        subj = "LNF Safety test result by " + tester;
                        body.AppendLine("Referred by: " + userTest.Misc + "<br /><br />");
                        body.AppendLine("The minimum score for this test is 36/40. Please see below for questions with wrong answers.<br /><br />");
                        body.AppendLine("Score: " + (correctAnswerSplitter.Length - wrongAnswerCount).ToString() + "/" + correctAnswerSplitter.Length.ToString() + "<br /><br />");
                        if (wrongAnswerCount > 0)
                        {
                            body.AppendLine("Questions with wrong or empty answer(s)<br />");
                            body.AppendLine(wrongAnswers.ToString());
                        }
                        result = "<div>Your test results have been sent to the LNF User Services Manager and to your email inbox.</div>";
                        break;
                    case TestType.UserMeeting2011:
                        subj = "2011 LNF User Meeting Test";
                        body.AppendLine("<br />");
                        body.AppendLine("User email: " + userEmail + "<br /><br />");
                        body.AppendLine("Minimum score: " + minscore.ToString("#0%") + "<br /><br />");
                        body.AppendLine("User score: " + score.ToString("#0%") + "<br /><br />");
                        if (wrongAnswerCount > 0)
                        {
                            body.AppendLine("Questions with wrong or empty answer(s):<br />");
                            body.AppendLine(wrongAnswers.ToString());
                        }
                        result = "<div>You have successfully completed the online quiz.</div>";
                        sendClientEmail = false;
                        break;
                    default:
                        return @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [Invalid TestTypeID]</div>";
                }

                //Send out email to tester and LNF User Services manager
                try
                {
                    if (sendClientEmail)
                    {
                        //send to both
                        SendEmail(adminEmail, userEmail, subj, body.ToString());
                    }
                    else
                    {
                        //only send to admin
                        SendEmail(adminEmail, subj, body.ToString());
                    }
                }
                catch (Exception ex)
                {
                    string err = @"<div style=""color: #FF0000; font-weight: bold;"">An error occurred while grading your test, please contact the LNF IT administrator. [Send email failed]<div>" + ex.Message + "</div>";
                    if (ex.InnerException != null)
                        err += "<div>" + ex.InnerException.Message + "</div>";
                    err += "</div>";
                    return err;
                }

                return result;
            }
            else
            {
                string testStartPage;
                switch (testType)
                {
                    case TestType.OSEH:
                        testStartPage = "/sselOnLine/TestOSEH.aspx";
                        result = string.Format(@"<div>Your score was less than {0}. Please take the test again. [<a href=""{1}"">Start Over</a>]</div>", minscore.ToString("#0%"), testStartPage);
                        break;
                    case TestType.LNF:
                        testStartPage = "/sselOnLine/TestLNF.aspx";
                        result = string.Format(@"<div>Your score was less than {0}. Please take the test again. [<a href=""{1}"">Start Over</a>]</div>", minscore.ToString("#0%"), testStartPage);
                        break;
                    case TestType.UserMeeting2011:
                        testStartPage = "/sselOnLine/TestUserMeeting2011.aspx";
                        result = "You did not satisfactorily complete the quiz. The deadline for successfully completing the quiz is July 15.";
                        break;
                    default:
                        testStartPage = "http://lnf.umich.edu/MNF/Prospective/OnsiteProcessing/index.aspx";
                        result = string.Format(@"<div>Your score was less than {0}. Please take the test again. [<a href=""{1}"">Start Over</a>]</div>", minscore.ToString("#0%"), testStartPage);
                        break;
                }

                return result;
            }
        }

        public static double GetMinScoreByTestType(TestType testType)
        {
            double result = 0;
            switch (testType)
            {
                case TestType.OSEH:
                    result = 0.5;
                    break;
                case TestType.LNF:
                    result = 0.5;
                    break;
                case TestType.Safety:
                    result = 0.5;
                    break;
                case TestType.UserMeeting2011:
                    result = 0.7;
                    break;
                default:
                    throw new ArgumentException("testType");
            }
            return result;
        }

        public static string GetFromAddress()
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings["TestingEmail"])
                ? "lnf-testing@umich.edu"
                : ConfigurationManager.AppSettings["TestingEmail"];
        }

        public static void SendEmail(string adminToAddr, string subj, string body)
        {
            ServiceProvider.Current.Mail.SendMessage(new SendMessageArgs
            {
                ClientID = 0,
                Caller = "seslOnLine.AppCode.BLL.TestManager.SendEmail[1]",
                Subject = subj + " [Admin Notice]",
                Body = body,
                From = GetFromAddress(),
                To = new string[] { adminToAddr },
                IsHtml = true
            });
        }

        public static void SendEmail(string adminToAddr, string userToAddr, string subject, string body)
        {
            ServiceProvider.Current.Mail.SendMessage(new SendMessageArgs
            {
                ClientID = 0,
                Caller = "seslOnLine.AppCode.BLL.TestManager.SendEmail[2]",
                Subject = subject,
                Body = body,
                From = GetFromAddress(),
                To = new string[] { userToAddr },
                Cc = new string[] { adminToAddr },
                IsHtml = true
            });
        }
    }
}
