using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sselOnLine.AppCode
{
    public class TestRoot
    {
        /* TestType's OSEH = 1, LNF = 2, Safety = 3, UserMeeting2011 = 4 */
        public int TestType { get; set; }
        public string TestTitle { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public List<Question> QuestionsList { get; set; }
        public string TestManualLink { get; set; }
        public int ExamTime { get; set; }
        public int PassingScore { get; set; }

        /// <summary>
        /// Either 0 for all questions on 1 page, or greater than 0 for a certain number of questions per page.
        /// </summary>
        public int QuestionsPerPage { get; set; }

        /// <summary>
        /// Either 0 for all questions in QuestionsList, or greater than 0 for a subset of QuestionsList.
        /// </summary>
        public int MaxQuestions { get; set; }

        /// <summary>
        /// Indicates whether or not the list of questions (based on QuestionsList and MaxQuestions) should be randomly ordered each time the test is taken.
        /// </summary>
        public bool Randomize { get; set; }

        /// <summary>
        /// Indicates whether or not to display the Recaptcha control (for preventing spam, bots, etc.) when the user is starting the test.
        /// </summary>
        public bool UseRecaptcha { get; set; }

        public TestRoot()
        {
            QuestionsList = new List<Question>();
            ExamTime = 20;
            PassingScore = 10;
        }

        /// <summary>
        /// Gets a list of questions from QuestionsList based on MaxQuestions and Randomize.
        /// </summary>
        public IList<Question> GetQuestions()
        {
            // need to separate out required questions
            var required = QuestionsList.Where(x => x.Required).ToList();
            var notreq = QuestionsList.Where(x => !x.Required).ToList();

            var rnd = new Random();

            if (Randomize)
            {    
                Shuffle(required, rnd);
                Shuffle(notreq, rnd);
            }

            var list = new List<Question>();

            // first add the required questions
            list.AddRange(required);

            // now add not required questions
            list.AddRange(notreq);

            IList<Question> result;

            if (MaxQuestions > 0)
                result = list.Take(MaxQuestions).ToList();
            else
                result = new List<Question>(list);

            if (Randomize)
            {
                // need to reshuffle so that the required questions aren't always at the start of the test
                Shuffle(result, rnd);
            }

            return result;
        }

        private void Shuffle<T>(IList<T> list, Random rnd)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class Choice
    {
        public string ChoiceIndex { get; set; }
        public string ChoiceText { get; set; }

        public string ChoiceDisplayText
        {
            get
            {
                if (ChoiceIndex == ChoiceText)
                    return ChoiceIndex;
                else
                    return ChoiceIndex + ") " + ChoiceText;
            }
        }
    }

    public class Question
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public List<Choice> Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string UserSelectedAnswer { get; set; }

        /// <summary>
        /// Indicates whether or not this question must appear on the test when MaxQuestions and Randomize are used.
        /// </summary>
        public bool Required { get; set; }

        public Question()
        {
            QuestionNumber = 0;
            QuestionText = string.Empty;
            Choices = new List<Choice>();
            CorrectAnswer = string.Empty;
        }

        public string GetUserSelectedAnswerText()
        {
            foreach (Choice c in Choices)
            {
                if (c.ChoiceIndex == UserSelectedAnswer)
                    return c.ChoiceText;
            }
            return string.Empty;
        }

        public Choice GetCorrectAnswer()
        {
            return Choices.FirstOrDefault(x => x.ChoiceText == CorrectAnswer);
        }

        public string GetAllAnswers()
        {
            string result = string.Empty;
            string space = string.Empty;

            foreach (Choice c in Choices)
            {
                result += space + "(" + c.ChoiceIndex + ") " + c.ChoiceText;
                space = " ";
            }

            return result;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool showCorrectAnswer)
        {
            string result = QuestionText + " " + GetAllAnswers();

            string userAnswer = string.IsNullOrEmpty(UserSelectedAnswer) ? "(left blank)" : UserSelectedAnswer;

            result += " [Your Answer: " + userAnswer;

            if (showCorrectAnswer)
                result += ", Correct Answer: " + CorrectAnswer;

            result += "]";

            return result;
        }

        public string ToHtml(bool showCorrectAnswer)
        {
            string result = "<div class=\"question\">";

            result += string.Format("<div class=\"question-text\">{0}</div>", QuestionText);

            foreach (var c in Choices)
            {
                bool isCorrectAnswer = c.ChoiceIndex == CorrectAnswer;
                bool userIsIncorrect = CorrectAnswer != UserSelectedAnswer && UserSelectedAnswer == c.ChoiceIndex;

                string className = "choice" + (userIsIncorrect ? " incorrect" : string.Empty);

                if (showCorrectAnswer && isCorrectAnswer)
                    className += " correct";

                result += string.Format("<div class=\"{0}\">{1}</div>", className, c.ChoiceDisplayText);
            }

            string userAnswer = string.IsNullOrEmpty(UserSelectedAnswer) ? "(left blank)" : UserSelectedAnswer;

            result += string.Format("<div class=\"answer\">[Your Answer: {0}", userAnswer);

            if (showCorrectAnswer)
                result += string.Format(", Correct Answer: {0}", CorrectAnswer);

            result += "]</div>";

            result += "</div>";

            return result;
        }
    }
}
