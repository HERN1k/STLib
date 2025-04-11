using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using STLib.Core.Testing;

namespace STLib.Tasks.Checkboxes
{
    /// <summary>
    /// Represents a multiple-answer checkbox-based task.
    /// </summary>
    public sealed class CheckboxesTask : CoreTask, IComparable, IComparable<CheckboxesTask>, IEquatable<CheckboxesTask>
    {
        #region Public properties
        /// <summary>
        /// Gets the list of possible answers for the task.
        /// </summary>
        public string[] Answers
        {
            get => m_answers.ToArray();
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Answers));
                }

                if (value.Length > m_maxAnswers)
                {
                    throw new ArgumentOutOfRangeException(nameof(Answers), $"The number of answers cannot exceed {m_maxAnswers}.");
                }

                m_answers.Clear();
                m_answers.AddRange(value);
            }
        }
        #endregion

        #region Private properties
        private readonly List<string> m_answers = new List<string>();
        private readonly int m_maxAnswers = 4;
        #endregion

        #region Constructors
        /// <summary>
        /// Private constructor for building a new <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="taskType">The type of the task.</param>
        private CheckboxesTask(TaskType taskType)
            : base(type: taskType)
        {
        }
        /// <summary>
        /// JSON constructor for deserializing a <see cref="CheckboxesTask"/> object.
        /// </summary>
        [JsonConstructor]
#pragma warning disable IDE0051
        private CheckboxesTask(Guid taskID, string name, string question, string[] answers, string correctAnswer, string answer, TaskType type, bool consider, bool isAnswered, int maxGrade, int grade)
#pragma warning restore IDE0051
            : base(taskID, name, question, correctAnswer, answer, type, consider, isAnswered, maxGrade, grade)
        {
            Answers = answers;
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Factory method to create a new instance of <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <returns>A new <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask Build() => new CheckboxesTask(TaskType.Checkboxes);
        /// <inheritdoc />
        public override bool IsCorrectTask()
        {
            if (m_answers.Count == 0)
            {
                return false;
            }

            if (m_answers.Count > m_maxAnswers)
            {
                return false;
            }

            var correctAnswersList = GetCorrectAnswers();

            if (correctAnswersList.Count == 0)
            {
                return false;
            }

            if (correctAnswersList.Count > m_maxAnswers)
            {
                return false;
            }

            foreach (var answer in correctAnswersList)
            {
                if (!m_answers.Contains(answer))
                {
                    return false;
                }
            }

            if (this.MaxGrade != correctAnswersList.Count)
            {
                return false;
            }

            return base.IsCorrectTask();
        }
        /// <inheritdoc />
        public override bool IsCorrect()
        {
            return base.IsCorrect();
        }
        /// <inheritdoc />
        public override void SetAnswer(string answer)
        {
            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentNullException(nameof(answer));
            }

            answer = answer.Trim();

            var userAnswersList = GetUserAnswers();

            if (userAnswersList.Contains(answer))
            {
                throw new ArgumentException($"The answer \"{answer}\" is already selected.");
            }

            if (userAnswersList.Count >= m_maxAnswers)
            {
                throw new ArgumentOutOfRangeException(nameof(answer), $"The number of answers cannot exceed {m_maxAnswers}.");
            }

            if (!m_answers.Contains(answer))
            {
                throw new ArgumentException($"The answer \"{answer}\" is not in the list of answers.");
            }

            userAnswersList.Add(answer);

            SetUserAnswers(userAnswersList);

            this.Grade = CalculateGrade(null);

            this.IsAnswered = true;
        }
        /// <inheritdoc />
        protected override int CalculateGrade(string? _)
        {
            if (!this.Consider)
            {
                return default;
            }

            var userAnswersList = GetUserAnswers();
            var correctAnswersList = GetCorrectAnswers();

            if (userAnswersList.Count == 0 || correctAnswersList.Count == 0)
            {
                return default;
            }

            int grade = 0;

            foreach (var answer in userAnswersList)
            {
                if (correctAnswersList.Contains(answer))
                {
                    grade += 1;
                }
            }

            return grade;
        }
        /// <inheritdoc />
        public override void SetCorrectAnswer(string correctAnswer)
        {
            base.SetCorrectAnswer(correctAnswer);
        }
        /// <summary>
        /// Sets the correct answers for the task.
        /// </summary>
        /// <param name="correctAnswersList">A list of correct answers.</param>
        public void SetCorrectAnswers(List<string> correctAnswersList)
        {
            if (correctAnswersList == null)
            {
                throw new ArgumentNullException(nameof(correctAnswersList));
            }

            foreach (var answer in correctAnswersList)
            {
                if (!m_answers.Contains(answer))
                {
                    throw new ArgumentException($"The answer \"{answer}\" is not in the list of answers.");
                }
            }

            if (correctAnswersList.Count > m_maxAnswers)
            {
                throw new ArgumentOutOfRangeException(nameof(correctAnswersList), $"The number of correct answers cannot exceed {m_maxAnswers}.");
            }

            this.MaxGrade = correctAnswersList.Count;

            SetCorrectAnswer(JsonSerializer.Serialize<List<string>>(correctAnswersList));
        }
        /// <summary>
        /// Adds an answer to the list of possible answers.
        /// </summary>
        /// <param name="answer">The answer to add.</param>
        public void SetAnswersItem(string answer)
        {
            answer = answer.Trim();

            if (m_answers.Contains(answer))
            {
                throw new ArgumentException($"The answer \"{answer}\" is already in the list of answers.");
            }

            if (m_answers.Count == m_maxAnswers)
            {
                throw new ArgumentOutOfRangeException(nameof(Answers), $"The number of answers cannot exceed {m_maxAnswers}.");
            }

            m_answers.Add(answer);
        }

        private List<string> GetUserAnswers()
        {
            var answer = string.IsNullOrEmpty(this.Answer) ? "[]" : this.Answer;
            var userAnswersList = JsonSerializer.Deserialize<List<string>>(answer);

            if (userAnswersList == null)
            {
                userAnswersList = new List<string>();

                SetUserAnswers(userAnswersList);
            }

            return userAnswersList;
        }

        private void SetUserAnswers(List<string> userAnswersList)
        {
            if (userAnswersList == null)
            {
                throw new ArgumentNullException(nameof(userAnswersList));
            }

            this.Answer = JsonSerializer.Serialize<List<string>>(userAnswersList);
        }

        private List<string> GetCorrectAnswers()
        {
            var correctAnswersList = JsonSerializer.Deserialize<List<string>>(this.CorrectAnswer);

            if (correctAnswersList == null)
            {
                correctAnswersList = new List<string>();

                SetCorrectAnswers(correctAnswersList);
            }

            return correctAnswersList;
        }
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is CheckboxesTask task)
            {
                return this.TaskID.Equals(task.TaskID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(CheckboxesTask other)
        {
            return this.TaskID.Equals(other.TaskID);
        }
        /// <inheritdoc />
        public int CompareTo(CheckboxesTask other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Grade.CompareTo(other.Grade);
        }
        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            var userAnswersList = GetUserAnswers();
            var correctAnswersList = GetCorrectAnswers();

            sb.AppendLine(string.Concat(base.ToString(), ", "));
            sb.AppendLine($"MaxAnswers: {m_maxAnswers}, ");
            sb.AppendLine($"Answers: ");

            for (int i = 0; i < m_maxAnswers; i++)
            {
                sb.AppendLine($"\t{i + 1}: {m_answers.ElementAtOrDefault(i)}");
            }

            sb.AppendLine($"CorrectAnswers: ");

            for (int i = 0; i < correctAnswersList.Count; i++)
            {
                sb.AppendLine($"\t{i + 1}: {correctAnswersList.ElementAtOrDefault(i)}");
            }

            sb.AppendLine($"UserAnswers: ");

            for (int i = 0; i < userAnswersList.Count; i++)
            {
                sb.AppendLine($"\t{i + 1}: {userAnswersList.ElementAtOrDefault(i)}");
            }

            return sb.ToString();
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}