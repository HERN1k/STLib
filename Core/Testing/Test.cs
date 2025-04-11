using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.IO;
using System.Text.Json;
using System.IO.Compression;

namespace STLib.Core.Testing
{
    /// <summary>
    /// Represents a test containing tasks, metadata, and functionality for managing test lifecycle, grading, and serialization.
    /// </summary>
    public sealed class Test : IComparable, IComparable<Test>, IEquatable<Test>
    {
        #region Public properties
        /// <summary>
        /// Gets the unique identifier for the test.
        /// </summary>
        public Guid TestID
        {
            get => m_testID;
            private set
            {
                if (value.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(TestID));
                }

                m_testID = value;
            }
        }
        /// <summary>
        /// Gets the date and time when the test was created.
        /// </summary>
        public DateTime Created
        {
            get => m_created;
            private set
            {
                if (value.Equals(DateTime.MinValue) || value.Equals(DateTime.MaxValue))
                {
                    throw new ArgumentNullException(nameof(Created));
                }

                m_created = value;
            }
        }
        /// <summary>
        /// Gets the list of tasks associated with the test.
        /// </summary>
        public List<CoreTask> Tasks
        {
            get => new List<CoreTask>(m_tasks);
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Tasks));
                }

                if (value.Count == 0)
                {
                    throw new ArgumentException("Tasks cannot be empty.", nameof(Tasks));
                }

                if (value.Count > MaxTasks)
                {
                    throw new ArgumentOutOfRangeException(nameof(Tasks), $"Tasks cannot exceed {MaxTasks}.");
                }

                m_tasks.Clear();

                foreach (var task in value)
                {
                    if (task == null)
                    {
                        throw new ArgumentNullException(nameof(Tasks), "Task cannot be null.");
                    }

                    m_tasks.Add(task);
                }
            }
        }
        /// <summary>
        /// Gets the maximum number of tasks allowed in the test.
        /// </summary>
        public int MaxTasks
        {
            get => m_maxTasks;
        }
        /// <summary>
        /// Gets the maximum grade achievable for the test.
        /// </summary>
        public int MaxGrade
        {
            get => m_maxGrade;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxGrade), "MaxGrade cannot be negative.");
                }

                m_maxGrade = value;
            }
        }
        /// <summary>
        /// Gets the grade achieved for the test.
        /// </summary>
        public int Grade
        {
            get => m_grade;
            private set
            {
                if (value < 0 || value > MaxGrade)
                {
                    throw new ArgumentOutOfRangeException(nameof(Grade), "Grade must be between 0 and MaxGrade.");
                }

                m_grade = value;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the test is finished.
        /// </summary>
        public bool IsFinished
        {
            get => m_isFinished;
            private set => m_isFinished = value;
        }
        /// <summary>
        /// Gets the total allowed time for the test.
        /// </summary>
        public TimeSpan TestTime
        {
            get => m_testTime;
            private set
            {
                if (value < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(nameof(TestTime), "TestTime cannot be negative.");
                }

                if (value > TimeSpan.FromHours(6))
                {
                    throw new ArgumentOutOfRangeException(nameof(TestTime), "TestTime cannot exceed 6 hours.");
                }

                m_testTime = value;
            }
        }
        /// <summary>
        /// Gets the start time of the test.
        /// </summary>
        public DateTime StartTime
        {
            get => m_startTime;
            private set
            {
                if (value == default)
                {
                    m_startTime = value;

                    return;
                }

                if (value.Equals(DateTime.MinValue) || value.Equals(DateTime.MaxValue))
                {
                    throw new ArgumentNullException(nameof(StartTime));
                }

                if (value < Created)
                {
                    throw new ArgumentOutOfRangeException(nameof(StartTime), "StartTime cannot be earlier than Created.");
                }

                m_startTime = value;
            }
        }
        /// <summary>
        /// Gets the end time of the test.
        /// </summary>
        public DateTime EndTime
        {
            get => m_endTime;
            private set
            {
                if (value == default)
                {
                    m_endTime = value;

                    return;
                }

                if (value.Equals(DateTime.MinValue) || value.Equals(DateTime.MaxValue))
                {
                    throw new ArgumentNullException(nameof(EndTime));
                }

                if (value < StartTime)
                {
                    throw new ArgumentOutOfRangeException(nameof(EndTime), "EndTime cannot be earlier than StartTime.");
                }

                m_endTime = value;
            }
        }
        /// <summary>
        /// Gets the last modification date and time of the test.
        /// </summary>
        public DateTime LastModified
        {
            get => m_lastModified;
            private set
            {
                if (value == default)
                {
                    m_lastModified = value;

                    return;
                }

                if (value.Equals(DateTime.MinValue) || value.Equals(DateTime.MaxValue))
                {
                    throw new ArgumentNullException(nameof(LastModified));
                }

                if (value < Created)
                {
                    throw new ArgumentOutOfRangeException(nameof(LastModified), "LastModified cannot be earlier than Created.");
                }

                m_lastModified = value;
            }
        }
        /// <summary>
        /// Gets the list of user identifiers who modified the test.
        /// </summary>
        public List<Guid> Modifiers
        {
            get => new List<Guid>(m_modifiers);
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Modifiers));
                }

                var modifiers = m_modifiers.ToArray();

                m_modifiers.Clear();
                m_modifiers.AddRange(modifiers);
                m_modifiers.AddRange(value);
            }
        }
        /// <summary>
        /// Gets the list of attentions (warnings or notes) associated with the test.
        /// </summary>
        public List<Attention> Attentions
        {
            get => new List<Attention>(m_attentions);
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Attentions));
                }

                var attentions = m_attentions.ToArray();

                m_attentions.Clear();
                m_attentions.AddRange(attentions);
                m_attentions.AddRange(value);
            }
        }
        /// <summary>
        /// Gets the name of the test.
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
        /// Gets the description of the test.
        /// </summary>
        public string Description
        {
            get => m_description;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Description));
                }

                if (value.Length > 512)
                {
                    throw new ArgumentOutOfRangeException(nameof(Description), "Description cannot exceed 512 characters.");
                }

                if (value.Contains("NULL"))
                {
                    throw new ArgumentException("Description cannot contain \"NULL\".", nameof(Description));
                }

                m_description = value;
            }
        }
        /// <summary>
        /// Gets the instructions for the test.
        /// </summary>
        public string Instructions
        {
            get => m_instructions;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(nameof(Instructions));
                }

                if (value.Length > 512)
                {
                    throw new ArgumentOutOfRangeException(nameof(Instructions), "Instructions cannot exceed 512 characters.");
                }

                if (value.Contains("NULL"))
                {
                    throw new ArgumentException("Instructions cannot contain \"NULL\".", nameof(Instructions));
                }

                m_instructions = value;
            }
        }
        /// <summary>
        /// Gets the unique identifier of the test creator.
        /// </summary>
        public Guid Creator
        {
            get => m_creator;
            private set
            {
                if (value.Equals(Guid.Empty))
                {
                    throw new ArgumentNullException(nameof(Creator));
                }

                m_creator = value;
            }
        }
        /// <summary>
        /// Gets the list of subjects associated with the test.
        /// </summary>
        public List<Guid> Subjects
        {
            get => new List<Guid>(m_subjects);
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(Subjects));
                }

                if (value.Count == 0)
                {
                    throw new ArgumentException("Subjects cannot be empty.", nameof(Subjects));
                }

                var subjects = m_subjects.ToArray();

                m_subjects.Clear();
                m_subjects.AddRange(subjects);
                m_subjects.AddRange(value);
            }
        }
        /// <summary>
        /// Gets a value indicating whether the test is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get => m_isReadOnly;
            private set => m_isReadOnly = value;
        }
        #endregion

        #region Private properties
        private Guid m_testID = Guid.NewGuid();
        private DateTime m_created = DateTime.UtcNow;
        private readonly HashSet<CoreTask> m_tasks = new HashSet<CoreTask>();
        private readonly int m_maxTasks = 100;                                      // Dont change this value
        private int m_maxGrade = default;
        private int m_grade = default;
        private bool m_isFinished = default;
        private TimeSpan m_testTime = TimeSpan.Zero;
        private DateTime m_startTime = default;
        private DateTime m_endTime = default;
        private DateTime m_lastModified = DateTime.UtcNow;
        private readonly List<Guid> m_modifiers = new List<Guid>();
        private readonly List<Attention> m_attentions = new List<Attention>();
        private string m_name = "NULL";
        private string m_description = "NULL";
        private string m_instructions = "NULL";
        private Guid m_creator = Guid.Empty;
        private readonly List<Guid> m_subjects = new List<Guid>();
        private bool m_isReadOnly = default;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class with the specified creator.
        /// </summary>
        /// <param name="creator">The unique identifier of the test creator.</param>
        private Test(Guid creator)
        {
            Creator = creator;
            
            AddLastModifier(Creator);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class with the specified parameters.
        /// </summary>
        /// <param name="testID">The unique identifier for the test.</param>
        /// <param name="created">The creation date and time of the test.</param>
        /// <param name="tasks">The list of tasks in the test.</param>
        /// <param name="maxGrade">The maximum grade for the test.</param>
        /// <param name="grade">The grade achieved for the test.</param>
        /// <param name="isFinished">A value indicating whether the test is finished.</param>
        /// <param name="testTime">The total allowed time for the test.</param>
        /// <param name="startTime">The start time of the test.</param>
        /// <param name="endTime">The end time of the test.</param>
        /// <param name="lastModified">The last modification date and time of the test.</param>
        /// <param name="modifiers">The list of modifiers for the test.</param>
        /// <param name="attentions">The list of attentions associated with the test.</param>
        /// <param name="name">The name of the test.</param>
        /// <param name="description">The description of the test.</param>
        /// <param name="instructions">The instructions for the test.</param>
        /// <param name="creator">The unique identifier of the test creator.</param>
        /// <param name="subjects">The list of subjects associated with the test.</param>
        [JsonConstructor]
#pragma warning disable IDE0051
        private Test(Guid testID, DateTime created, List<CoreTask> tasks, int maxGrade, int grade, bool isFinished, TimeSpan testTime, DateTime startTime, DateTime endTime, DateTime lastModified, List<Guid> modifiers, List<Attention> attentions, string name, string description, string instructions, Guid creator, List<Guid> subjects)
#pragma warning restore IDE0051
        {
            TestID = testID;
            Created = created;
            Tasks = tasks;
            MaxGrade = maxGrade;
            Grade = grade;
            IsFinished = isFinished;
            TestTime = testTime;
            StartTime = startTime;
            EndTime = endTime;
            LastModified = lastModified;
            Modifiers = modifiers;
            Attentions = attentions;
            Name = name;
            Description = description;
            Instructions = instructions;
            Creator = creator;
            Subjects = subjects;
        }
        #endregion

        #region Logic methods
        /// <summary>
        /// Creates a new instance of the <see cref="Test"/> class with the given creator.
        /// </summary>
        /// <param name="creator">The unique identifier of the test creator.</param>
        /// <returns>A new instance of the <see cref="Test"/> class.</returns>
        public static Test Build(Guid creator) => new Test(creator);
        /// <summary>
        /// Validates whether the test has correct and valid data.
        /// </summary>
        /// <returns><c>true</c> if the test is valid; otherwise, <c>false</c>.</returns>
        public bool IsCorrect()
        {
            if (m_testID.Equals(Guid.Empty))
            {
                return false;
            }

            if (m_created.Equals(DateTime.MinValue) || m_created.Equals(DateTime.MaxValue))
            {
                return false;
            }

            if (m_tasks.Count == 0)
            {
                return false;
            }

            if (m_tasks.Count > m_maxTasks)
            {
                return false;
            }

            if (m_testTime <= TimeSpan.Zero)
            {
                return false;
            }

            if (m_modifiers.Count <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(m_name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(m_description))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(m_instructions))
            {
                return false;
            }

            if (m_creator.Equals(Guid.Empty))
            {
                return false;
            }

            if (m_subjects.Count == 0)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Starts the test and initializes all tasks. 
        /// Throws exceptions if the test cannot be started due to invalid state or configuration.
        /// </summary>
        /// <param name="modifier">The GUID of the user attempting to start the test.</param>
        public void OnStart(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            if (!Subjects.Contains(modifier))
            {
                throw new ArgumentException("You is not a subject of the test.", nameof(modifier));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Test is read-only.");
            }

            if (IsFinished)
            {
                throw new InvalidOperationException("Test is already finished.");
            }

            if (m_tasks.Count == 0)
            {
                throw new InvalidOperationException("Test has no tasks.");
            }

            foreach (var task in m_tasks)
            {
                task.Reset();

                if (!task.IsCorrectTask())
                {
                    throw new InvalidProgramException("Task is not valid.");
                }
            }

            StartTime = DateTime.UtcNow;

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Saves the test in a development state. 
        /// Validates the test configuration and resets all tasks for further modifications.
        /// </summary>
        public void OnDevSave()
        {
            if (!IsCorrect())
            {
                throw new InvalidProgramException("Test is not valid.");
            }

            foreach (var task in m_tasks)
            {
                if (task.Consider)
                {
                    MaxGrade += task.MaxGrade;
                }

                task.Reset();

                if (!task.IsCorrectTask())
                {
                    throw new InvalidProgramException("Task is not valid.");
                }
            }

            Reset();

            AddLastModifier(Creator);
        }
        /// <summary>
        /// Starts an intermediate version of the test, allowing modifications or updates.
        /// Ensures the test is still active and valid for editing.
        /// </summary>
        /// <param name="modifier">The GUID of the user attempting to start the intermediate state.</param>
        public void OnIntermediateStart(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            if (!Subjects.Contains(modifier))
            {
                throw new ArgumentException("You is not a subject of the test.", nameof(modifier));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Test is read-only.");
            }

            if (IsFinished)
            {
                throw new InvalidOperationException("Test is already finished.");
            }

            if (m_tasks.Count == 0)
            {
                throw new InvalidOperationException("Test has no tasks.");
            }

            if (DateTime.UtcNow >= StartTime + TestTime)
            {
                OnTimeOut(modifier);

                return;
            }

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Saves the intermediate progress of the test.
        /// </summary>
        /// <param name="modifier">The GUID of the user saving the intermediate state.</param>
        public void OnIntermediateSave(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Finalizes and saves the test, marking it as completed. 
        /// Calculates the grade and updates the test state.
        /// </summary>
        /// <param name="modifier">The GUID of the user saving the final state of the test.</param>
        public void OnSave(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            if (!Subjects.Contains(modifier))
            {
                throw new ArgumentException("You is not a subject of the test.", nameof(modifier));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Test is read-only.");
            }

            if (IsFinished)
            {
                throw new InvalidOperationException("Test is already finished.");
            }

            if (m_tasks.Count == 0)
            {
                throw new InvalidOperationException("Test has no tasks.");
            }

            EndTime = DateTime.UtcNow;

            Grade = CalculateGrade();

            IsFinished = true;

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Ends the test due to a timeout. 
        /// Marks the test as finished and generates a critical attention note.
        /// </summary>
        /// <param name="modifier">The GUID of the user associated with the timeout.</param>
        public void OnTimeOut(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            if (!Subjects.Contains(modifier))
            {
                throw new ArgumentException("You is not a subject of the test.", nameof(modifier));
            }

            if (IsReadOnly)
            {
                throw new InvalidOperationException("Test is read-only.");
            }

            if (IsFinished)
            {
                throw new InvalidOperationException("Test is already finished.");
            }

            if (m_tasks.Count == 0)
            {
                throw new InvalidOperationException("Test has no tasks.");
            }

            EndTime = DateTime.UtcNow;

            Grade = CalculateGrade();

            IsFinished = true;

            Attentions.Add(new Attention("Test time is over / Час тестування закінчився", AttentionType.Critical));

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Adds the last modifier to the test and updates the last modified time.
        /// Prevents duplicate consecutive modifications by the same user.
        /// </summary>
        /// <param name="modifier">The GUID of the modifier to add.</param>
        public void AddLastModifier(Guid modifier)
        {
            if (modifier.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(modifier));
            }

            var lastModifier = Modifiers.ElementAtOrDefault(m_modifiers.Count - 1);

            if (lastModifier.Equals(modifier))
            {
                return;
            }

            LastModified = DateTime.UtcNow;

            Modifiers = new List<Guid>() { modifier };
        }
        /// <summary>
        /// Resets the test to its initial state, clearing grades, times, and attentions.
        /// </summary>
        public void Reset()
        {
            Grade = default;
            IsFinished = default;
            StartTime = default;
            EndTime = default;
            Attentions = new List<Attention>();
            IsReadOnly = default;
        }
        /// <summary>
        /// Calculates the total grade of the test based on the grades of individual tasks.
        /// </summary>
        /// <returns>The total grade of the test.</returns>
        private int CalculateGrade()
        {
            if (m_tasks.Count == 0)
            {
                return default;
            }

            var grade = 0;

            foreach (var task in m_tasks)
            {
                if (task.Consider)
                {
                    grade += task.Grade;
                }
            }

            return grade;
        }
        /// <summary>
        /// Creates and configures JSON serialization options for the <see cref="Test"/> class.
        /// </summary>
        /// <returns>A configured instance of <see cref="JsonSerializerOptions"/>.</returns>
        private static JsonSerializerOptions SerializerOptions()
        {
            return new JsonSerializerOptions
            {
#if DEBUG
                WriteIndented = true,
#else
                WriteIndented = false,
#endif
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                IncludeFields = true,
                Converters =
                {
                    new CoreTaskConverter(),
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
        }
        /// <summary>
        /// Serializes the current instance of the <see cref="Test"/> class to a JSON string.
        /// </summary>
        /// <returns>A JSON string representation of the current <see cref="Test"/> instance.</returns>
        /// <exception cref="JsonException">Thrown when serialization fails.</exception>
        public string SerializeToJson()
        {
            try
            {
                return JsonSerializer.Serialize(this, SerializerOptions());
            }
            catch (Exception ex)
            {
                throw new JsonException("Serialization failed.", ex);
            }
        }
        /// <summary>
        /// Serializes the current instance of the <see cref="Test"/> class to a compressed byte array.
        /// </summary>
        /// <returns>A compressed byte array containing the serialized <see cref="Test"/> object.</returns>
        /// <exception cref="JsonException">Thrown when serialization fails.</exception>
        public byte[] SerializeToByteArray()
        {
            try
            {
                using (var serializeStream = new MemoryStream())
                using (var compressedStream = new MemoryStream())
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    JsonSerializer.Serialize(serializeStream, this, SerializerOptions());

                    byte[] bytes = serializeStream.ToArray();

                    gzipStream.Write(bytes, 0, bytes.Length);
                    gzipStream.Close();

                    return compressedStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new JsonException("Serialization failed.", ex);
            }
        }
        /// <summary>
        /// Deserializes a JSON string into an instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An instance of the <see cref="Test"/> class, or <c>null</c> if the input is invalid.</returns>
        /// <exception cref="JsonException">Thrown when deserialization fails.</exception>
        public static Test? DeserializeFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            try
            {
                return JsonSerializer.Deserialize<Test>(json, SerializerOptions());
            }
            catch (Exception ex)
            {
                throw new JsonException("Deserialization failed.", ex);
            }
        }
        /// <summary>
        /// Deserializes a compressed byte array into an instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="bytes">The compressed byte array to deserialize.</param>
        /// <returns>An instance of the <see cref="Test"/> class, or <c>null</c> if the input is invalid.</returns>
        /// <exception cref="JsonException">Thrown when deserialization fails.</exception>
        public static Test? DeserializeFromByteArray(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            try
            {
                using (var compressedStream = new MemoryStream(bytes))
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var decompressedStream = new MemoryStream())
                {
                    gzipStream.CopyTo(decompressedStream);

                    return JsonSerializer.Deserialize<Test>(decompressedStream.ToArray(), SerializerOptions());
                }
            }
            catch (Exception ex)
            {
                throw new JsonException("Deserialization failed.", ex);
            }
        }
        /// <summary>
        /// Sets the name of the test.
        /// </summary>
        /// <param name="name">The new name of the test.</param>
        public void SetName(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Sets the description of the test.
        /// </summary>
        /// <param name="description">The new description of the test.</param>
        public void SetDescription(string description)
        {
            Description = description;
        }
        /// <summary>
        /// Sets the instructions for the test.
        /// </summary>
        /// <param name="instructions">The new instructions for the test.</param>
        public void SetInstructions(string instructions)
        {
            Instructions = instructions;
        }
        /// <summary>
        /// Updates the grade for the test.
        /// </summary>
        /// <param name="grade">The new grade for the test.</param>
        public void SetGrade(int grade)
        {
            Grade = grade;
        }
        /// <summary>
        /// Updates the subject of the test with a single subject identifier.
        /// </summary>
        /// <param name="subject">The GUID representing the new subject.</param>
        public void SetSubject(Guid subject)
        {
            Subjects = new List<Guid>() { subject };
        }
        /// <summary>
        /// Updates the total allowed time for the test.
        /// </summary>
        /// <param name="time">The new time span for the test duration.</param>
        public void SetTestTime(TimeSpan time)
        {
            TestTime = time;
        }
        /// <summary>
        /// Adds attention to the test and updates the last modifier.
        /// </summary>
        /// <param name="attention">The attention object to add.</param>
        /// <param name="modifier">The GUID of the modifier making the change.</param>
        public void SetAttention(Attention attention, Guid modifier)
        {
            Attentions = new List<Attention>() { attention };

            AddLastModifier(modifier);
        }
        /// <summary>
        /// Adds a task to the test. Validates that the task does not already exist and the maximum task limit is not exceeded.
        /// </summary>
        /// <param name="task">The task to add to the test.</param>
        /// <exception cref="ArgumentNullException">Thrown when the task is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the task already exists in the test.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the task count exceeds the maximum limit.</exception>
        public void SetTask(CoreTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            if (m_tasks.Contains(task))
            {
                throw new ArgumentException("Task already exists in the test.", nameof(task));
            }

            if (m_tasks.Count >= MaxTasks)
            {
                throw new InvalidOperationException("Cannot add more tasks to the test. Maximum limit reached.");
            }

            m_tasks.Add(task);
        }
        /// <summary>
        /// Removes a task from the test by its unique identifier.
        /// </summary>
        /// <param name="taskID">The unique identifier of the task to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the task ID is empty.</exception>
        public void UnSetTask(Guid taskID)
        {
            if (taskID.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(taskID));
            }

            var task = m_tasks.FirstOrDefault(t => t.TaskID.Equals(taskID));

            if (task == null)
            {
                return;
            }

            m_tasks.Remove(task);
        }
        /// <summary>
        /// Converts a UTC date to a local date string in the format "dd:MM:yyyy".
        /// </summary>
        /// <param name="date"></param>
        public string ToLocalDate(DateTime date) => date.ToLocalTime().ToString("dd.MM.yyyy");
        /// <summary>
        /// Converts a UTC date to a local time string in the format "HH:mm:ss".
        /// </summary>
        /// <param name="date"></param>
        public string ToLocalTime(DateTime date) => date.ToLocalTime().ToString("HH:mm:ss");
        /// <summary>
        /// Converts a UTC date to a local date and time string in the format "dd:MM:yyyy HH:mm:ss".
        /// </summary>
        /// <param name="date"></param>
        public string ToLocalDateTime(DateTime date) => date.ToLocalTime().ToString("dd:MM:yyyy HH:mm:ss");
        /// <summary>
        /// Trims a string to a specified length and appends ellipsis if the string exceeds that length.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string TrimToLength(string str, int length = 32)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(str) || length == 0)
            {
                return string.Empty;
            }

            str = str.Trim();

            if (str.Length <= length)
            {
                return str;
            }

            return string.Create(length + 3, (str), (span, state) =>
            {
                state.AsSpan(0, length).CopyTo(span);
                span[length] = '.';
                span[length + 1] = '.';
                span[length + 2] = '.';
            });
        }
        #endregion

        #region Base methods
        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Test test)
            {
                return TestID.Equals(test.TestID);
            }

            return false;
        }
        /// <inheritdoc />
        public bool Equals(Test other)
        {
            return TestID.Equals(other.TestID);
        }
        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Test otherTask)
            {
                return LastModified.CompareTo(otherTask.LastModified);
            }
            else
            {
                throw new ArgumentException("Object is not a Test");
            }
        }
        /// <inheritdoc />
        public int CompareTo(Test other)
        {
            if (other == null)
            {
                return 1;
            }

            return LastModified.CompareTo(other.LastModified);
        }
        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"TestID: {TestID}, ");
            sb.AppendLine($"Tasks count: {Tasks.Count}, ");
            sb.AppendLine($"MaxTasks: {MaxTasks}, ");
            sb.AppendLine($"MaxGrade: {MaxGrade}, ");
            sb.AppendLine($"Grade: {Grade}, ");
            sb.AppendLine($"IsFinished: {IsFinished}, ");
            sb.AppendLine($"TestTime: {TestTime}, ");
            sb.AppendLine($"StartTime: {StartTime}, ");
            sb.AppendLine($"EndTime: {EndTime}, ");
            sb.AppendLine($"LastModified: {LastModified}, ");
            sb.AppendLine($"Modifiers count: {Modifiers.Count}, ");
            sb.AppendLine($"Attentions count: {Attentions.Count}, ");
            sb.AppendLine($"Name: {Name}, ");
            sb.AppendLine($"Description: {Description}, ");
            sb.AppendLine($"Instructions: {Instructions}, ");
            sb.AppendLine($"Creator: {Creator}, ");
            sb.AppendLine($"Subjects count: {Subjects.Count}, ");
            sb.AppendLine($"IsReadOnly: {IsReadOnly}");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"Tasks: ");

            foreach (var task in Tasks)
            {
                sb.AppendLine(task.ToString());
            }

            return sb.ToString();
        }
        /// <inheritdoc />
        public override int GetHashCode()
        {
            return TestID.GetHashCode();
        }
        #endregion
    }
}