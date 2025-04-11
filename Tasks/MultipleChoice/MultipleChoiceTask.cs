using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Linq;
using STLib.Core.Testing;

namespace STLib.Tasks.MultipleChoice
{
    /// <summary>
    /// Represents a multiple-choice task, where the user selects a single correct answer from a set of options.
    /// </summary>
    public sealed class MultipleChoiceTask : CoreTask, IComparable, IComparable<MultipleChoiceTask>, IEquatable<MultipleChoiceTask>
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
        /// Private constructor for building a new <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="taskType">The type of the task.</param>
        private MultipleChoiceTask(TaskType taskType)
            : base(type: taskType)
        {
        }
        /// <summary>
        /// JSON constructor for deserializing a <see cref="MultipleChoiceTask"/> object.
        /// </summary>
        [JsonConstructor]
#pragma warning disable IDE0051
        private MultipleChoiceTask(Guid taskID, string name, string question, string[] answers, string correctAnswer, string answer, TaskType type, bool consider, bool isAnswered, int maxGrade, int grade)
#pragma warning restore IDE0051
            : base(taskID, name, question, correctAnswer, answer, type, consider, isAnswered, maxGrade, grade)
        {
            Answers = answers;
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Factory method to create a new instance of <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <returns>A new <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask Build() => new MultipleChoiceTask(TaskType.MultipleChoice);
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

            this.Answer = answer.Trim();

            this.Grade = CalculateGrade(Answer);

            this.IsAnswered = true;
        }
        /// <inheritdoc />
        protected override int CalculateGrade(string answer)
        {
            if (!this.Consider)
            {
                return default;
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                return default;
            }

            if (answer.Equals(this.CorrectAnswer, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.MaxGrade;
            }
            else
            {
                return default;
            }
        }
        /// <inheritdoc />
        public override void SetCorrectAnswer(string correctAnswer)
        {
            correctAnswer = correctAnswer.Trim();

            if (!m_answers.Contains(correctAnswer))
            {
                throw new ArgumentException($"The answer \"{correctAnswer}\" is not in the list of answers.");
            }

            this.CorrectAnswer = correctAnswer;
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
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is MultipleChoiceTask task)
            {
                return this.TaskID.Equals(task.TaskID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(MultipleChoiceTask other)
        {
            return this.TaskID.Equals(other.TaskID);
        }
        /// <inheritdoc />
        public int CompareTo(MultipleChoiceTask other)
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

            sb.AppendLine(string.Concat(base.ToString(), ", "));
            sb.AppendLine($"MaxAnswers: {m_maxAnswers}, ");
            sb.AppendLine($"Answers: ");

            for (int i = 0; i < m_maxAnswers; i++)
            {
                sb.AppendLine($"\t{i + 1}: {m_answers.ElementAtOrDefault(i)}");
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