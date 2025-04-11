using System;
using System.Text.Json.Serialization;

using STLib.Core.Testing;

namespace STLib.Tasks.TrueFalse
{
    /// <summary>
    /// Represents a true/false task where the user must choose between "True" or "False" as the answer.
    /// </summary>
    public sealed class TrueFalseTask : CoreTask, IComparable, IComparable<TrueFalseTask>, IEquatable<TrueFalseTask>
    {
        #region Private properties
        private readonly string m_trueAnswer = "True";
        private readonly string m_falseAnswer = "False";
        #endregion

        #region Constructors
        /// <summary>
        /// Private constructor for creating an instance of <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="taskType">The type of the task.</param>
        private TrueFalseTask(TaskType taskType)
            : base(type: taskType)
        {
        }
        /// <summary>
        /// JSON constructor for deserializing a <see cref="TrueFalseTask"/> object.
        /// </summary>
        [JsonConstructor]
#pragma warning disable IDE0051
        private TrueFalseTask(Guid taskID, string name, string question, string correctAnswer, string answer, TaskType type, bool consider, bool isAnswered, int maxGrade, int grade)
#pragma warning restore IDE0051
            : base(taskID, name, question, correctAnswer, answer, type, consider, isAnswered, maxGrade, grade)
        {
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Factory method to create a new instance of <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <returns>A new <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask Build() => new TrueFalseTask(TaskType.TrueFalse);
        /// <inheritdoc />
        public override bool IsCorrectTask()
        {
            if (!m_trueAnswer.Equals(this.CorrectAnswer, StringComparison.InvariantCulture) &&
                !m_falseAnswer.Equals(this.CorrectAnswer, StringComparison.InvariantCulture))
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
        /// <summary>
        /// Sets the user's answer to the task and calculates the grade.
        /// </summary>
        /// <param name="answer">The user's answer, which must be either "True" or "False".</param>
        /// <exception cref="ArgumentNullException">Thrown if the answer is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the answer is not "True" or "False".</exception>
        public override void SetAnswer(string answer)
        {
            if (string.IsNullOrEmpty(answer))
            {
                throw new ArgumentNullException(nameof(answer));
            }

            if (!m_trueAnswer.Equals(answer, StringComparison.InvariantCulture) &&
                !m_falseAnswer.Equals(answer, StringComparison.InvariantCulture))
            {
                throw new ArgumentException("Answer must be either \"True\" or \"False\".", nameof(answer));
            }

            this.Answer = answer.Trim();

            this.Grade = CalculateGrade(this.Answer);

            this.IsAnswered = true;
        }
        /// <summary>
        /// Calculates the grade for the task based on the user's answer.
        /// </summary>
        /// <param name="answer">The user's answer.</param>
        /// <returns>The calculated grade.</returns>
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

            if (answer.Equals(this.CorrectAnswer, StringComparison.InvariantCulture))
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

            if (!m_trueAnswer.Equals(correctAnswer, StringComparison.InvariantCulture) &&
                !m_falseAnswer.Equals(correctAnswer, StringComparison.InvariantCulture))
            {
                throw new ArgumentException("Correct answer must be either \"True\" or \"False\".", nameof(correctAnswer));
            }

            this.CorrectAnswer = correctAnswer;
        }
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is TrueFalseTask task)
            {
                return this.TaskID.Equals(task.TaskID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(TrueFalseTask other)
        {
            return this.TaskID.Equals(other.TaskID);
        }
        /// <inheritdoc />
        public int CompareTo(TrueFalseTask other)
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
            return base.ToString();
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}