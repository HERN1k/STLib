using System;
using System.Text.Json.Serialization;

using STLib.Core.Testing;

namespace STLib.Tasks.Text
{
    /// <summary>
    /// Represents a text-based task where the user provides a written answer to a question.
    /// </summary>
    public sealed class TextTask : CoreTask, IComparable, IComparable<TextTask>, IEquatable<TextTask>
    {
        #region Constructors
        /// <summary>
        /// Private constructor for initializing a new <see cref="TextTask"/> instance.
        /// </summary>
        /// <param name="taskType">The type of the task.</param>
        private TextTask(TaskType taskType)
            : base(type: taskType)
        {
        }
        /// <summary>
        /// JSON constructor for deserializing a <see cref="TextTask"/> object.
        /// </summary>
        [JsonConstructor]
#pragma warning disable IDE0051
        private TextTask(Guid taskID, string name, string question, string correctAnswer, string answer, TaskType type, bool consider, bool isAnswered, int maxGrade, int grade)
#pragma warning restore IDE0051
            : base(taskID, name, question, correctAnswer, answer, type, consider, isAnswered, maxGrade, grade)
        {
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Factory method to create a new instance of <see cref="TextTask"/>.
        /// </summary>
        /// <returns>A new <see cref="TextTask"/> instance.</returns>
        public static TextTask Build() => new TextTask(TaskType.Text);
        /// <inheritdoc />
        public override bool IsCorrectTask()
        {
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
        /// <param name="answer">The user's answer.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided answer is null or empty.</exception>
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

            if (answer.Equals(this.CorrectAnswer, StringComparison.InvariantCultureIgnoreCase))
            {
                return this.MaxGrade;
            }
            else
            {
                return default;
            }
        }
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is TextTask task)
            {
                return this.TaskID.Equals(task.TaskID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(TextTask other)
        {
            return this.TaskID.Equals(other.TaskID);
        }
        /// <inheritdoc />
        public int CompareTo(TextTask other)
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