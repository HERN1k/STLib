using System;
using System.Text;
using System.Text.Json.Serialization;

namespace STLib.Core.Testing
{
    /// <summary>
    /// Represents an abstract base class for managing core task functionality.
    /// Provides properties and methods for handling task-related data, such as ID, name, question, and grading.
    /// </summary>
    public abstract class CoreTask : IComparable, IComparable<CoreTask>, IEquatable<CoreTask>
    {
        #region Public properties
        /// <summary>
        /// Gets the unique identifier for the task.
        /// </summary>
        public Guid TaskID
        {
            get => m_taskID;
            private set
            {
                if (value.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(TaskID));
                }

                m_taskID = value;
            }
        }
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string Name
        {
            get => m_name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Name));
                }

                if (value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException(nameof(Name), "Name cannot exceed 100 characters.");
                }

                if (value.Contains("NULL"))
                {
                    throw new ArgumentException("Name cannot contain \"NULL\".", nameof(Name));
                }

                m_name = value;
            }
        }
        /// <summary>
        /// Gets or sets the question associated with the task.
        /// </summary>
        public string Question
        {
            get => m_question;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Question));
                }

                if (value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException(nameof(Question), "Question cannot exceed 256 characters.");
                }

                if (value.Contains("NULL"))
                {
                    throw new ArgumentException("Question cannot contain \"NULL\".", nameof(Question));
                }

                m_question = value;
            }
        }
        /// <summary>
        /// Gets or sets the correct answer for the task.
        /// </summary>
        public string CorrectAnswer
        {
            get => m_correctAnswer;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(CorrectAnswer));
                }

                m_correctAnswer = value;
            }
        }
        /// <summary>
        /// Gets or sets the answer provided for the task.
        /// </summary>
        public string Answer
        {
            get => m_answer;
            protected set
            {
                m_answer = value ?? throw new ArgumentNullException(nameof(Answer));
            }
        }
        /// <summary>
        /// Gets the type of the task.
        /// </summary>
        public TaskType Type
        {
            get => m_type;
            private set
            {
                if (value == TaskType.Unknown)
                {
                    throw new ArgumentException("The type of question cannot be unknown");
                }

                m_type = value;
            }
        }                                              // Dont change this value
        /// <summary>
        /// Gets or sets a value indicating whether the task should be considered for grading.
        /// </summary>
        public bool Consider
        {
            get => m_consider;
            protected set => m_consider = value;
        }
        /// <summary>
        /// Gets or sets a value indicating whether the task has been answered.
        /// </summary>
        public bool IsAnswered
        {
            get => m_isAnswered;
            protected set => m_isAnswered = value;
        }
        /// <summary>
        /// Gets or sets the maximum grade for the task.
        /// </summary>
        public int MaxGrade
        {
            get => m_maxGrade;
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxGrade));
                }

                m_maxGrade = value;
            }
        }
        /// <summary>
        /// Gets or sets the grade achieved for the task.
        /// </summary>
        public int Grade
        {
            get => m_grade;
            protected set
            {
                if (value < 0 || value > MaxGrade)
                {
                    throw new ArgumentOutOfRangeException(nameof(Grade));
                }

                m_grade = value;
            }
        }
        #endregion

        #region Private properties
        private Guid m_taskID = Guid.NewGuid();
        private string m_name = "NULL";
        private string m_question = "NULL";
        private string m_correctAnswer = "NULL";
        private string m_answer = string.Empty;
        private TaskType m_type = TaskType.Unknown;                         // Dont change this value
        private bool m_consider = true;
        private bool m_isAnswered = default;
        private int m_maxGrade = 1;
        private int m_grade = default;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTask"/> class with the specified task type.
        /// </summary>
        /// <param name="type">The type of the task.</param>
        protected CoreTask(TaskType type)
        {
            Type = type;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTask"/> class with the specified parameters.
        /// </summary>
        /// <param name="taskID">The unique identifier for the task.</param>
        /// <param name="name">The name of the task.</param>
        /// <param name="question">The question associated with the task.</param>
        /// <param name="correctAnswer">The correct answer for the task.</param>
        /// <param name="answer">The answer provided for the task.</param>
        /// <param name="type">The type of the task.</param>
        /// <param name="consider">A value indicating whether the task should be considered for grading.</param>
        /// <param name="isAnswered">A value indicating whether the task has been answered.</param>
        /// <param name="maxGrade">The maximum grade for the task.</param>
        /// <param name="grade">The grade achieved for the task.</param>
        [JsonConstructor]
        protected CoreTask(Guid taskID, string name, string question, string correctAnswer, string answer, TaskType type, bool consider, bool isAnswered, int maxGrade, int grade)
        {
            TaskID = taskID;
            Name = name;
            Question = question;
            Type = type;
            Consider = consider;
            IsAnswered = isAnswered;
            MaxGrade = maxGrade;
            Grade = grade;
            CorrectAnswer = correctAnswer;
            Answer = answer;
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Abstract method to set the answer for the task.
        /// </summary>
        /// <param name="answer">The answer to set.</param>
        public abstract void SetAnswer(string answer);
        /// <summary>
        /// Abstract method to calculate the grade for the task based on the provided answer.
        /// </summary>
        /// <param name="answer">The provided answer.</param>
        /// <returns>The calculated grade.</returns>
        protected abstract int CalculateGrade(string answer);
        /// <summary>
        /// Determines whether the task is configured correctly.
        /// </summary>
        /// <returns><c>true</c> if the task is correct; otherwise, <c>false</c>.</returns>
        public virtual bool IsCorrectTask()
        {
            if (string.IsNullOrWhiteSpace(m_name) ||
                m_name.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(m_question) ||
                m_question.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(m_correctAnswer) ||
                m_correctAnswer.Equals("NULL", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            if (m_type == TaskType.Unknown)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Determines whether the task has been answered correctly.
        /// </summary>
        /// <returns><c>true</c> if the task is answered correctly; otherwise, <c>false</c>.</returns>
        public virtual bool IsCorrect()
        {
            if (IsAnswered)
            {
                if (Grade > 0)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Sets the name of the task.
        /// </summary>
        /// <param name="name">The name to set.</param>
        public virtual void SetName(string name)
        {
            Name = name.Trim();
        }
        /// <summary>
        /// Sets the question associated with the task.
        /// </summary>
        /// <param name="question">The question to set.</param>
        public virtual void SetQuestion(string question)
        {
            Question = question.Trim();
        }
        /// <summary>
        /// Sets the correct answer for the task.
        /// </summary>
        /// <param name="correctAnswer">The correct answer to set.</param>
        public virtual void SetCorrectAnswer(string correctAnswer)
        {
            CorrectAnswer = correctAnswer.Trim();
        }
        /// <summary>
        /// Sets whether the task should be considered for grading.
        /// </summary>
        /// <param name="consider">A value indicating whether to consider the task.</param>
        public virtual void SetConsider(bool consider)
        {
            Consider = consider;
        }
        /// <summary>
        /// Sets the maximum grade for the task.
        /// </summary>
        /// <param name="maxGrade">The maximum grade to set.</param>
        public virtual void SetMaxGrade(int maxGrade)
        {
            MaxGrade = maxGrade;
        }
        /// <summary>
        /// Sets the achieved grade for the task.
        /// </summary>
        /// <param name="grade">The grade to set.</param>
        public virtual void SetGrade(int grade)
        {
            Grade = grade;
        }
        /// <summary>
        /// Resets the task to its initial state.
        /// </summary>
        public virtual void Reset()
        {
            Answer = string.Empty;
            IsAnswered = false;
            Grade = 0;

        }
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is CoreTask task)
            {
                return TaskID.Equals(task.TaskID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(CoreTask other)
        {
            return TaskID.Equals(other.TaskID);
        }
        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is CoreTask otherTask)
            {
                return Grade.CompareTo(otherTask.Grade);
            }
            else
            {
                throw new ArgumentException("Object is not a CoreTask");
            }
        }
        /// <inheritdoc />
        public int CompareTo(CoreTask other)
        {
            if (other == null)
            {
                return 1;
            }

            return Grade.CompareTo(other.Grade);
        }
        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"TaskID: {TaskID}, ");
            sb.AppendLine($"Name: {Name}, ");
            sb.AppendLine($"Question: {Question}, ");
            sb.AppendLine($"Type: {Type}, ");
            sb.AppendLine($"Consider: {Consider}, ");
            sb.AppendLine($"IsAnswered: {IsAnswered}, ");
            sb.AppendLine($"MaxGrade: {MaxGrade}, ");
            sb.AppendLine($"Grade: {Grade}, ");
            sb.AppendLine($"CorrectAnswer: {CorrectAnswer}, ");
            sb.Append($"Answer: {Answer}");

            return sb.ToString();
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return TaskID.GetHashCode();
        }
        #endregion
    }
}