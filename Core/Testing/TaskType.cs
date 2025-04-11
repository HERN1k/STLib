namespace STLib.Core.Testing
{
    /// <summary>
    /// Represents the types of tasks that can be associated with a test or activity.
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// Indicates an unknown type of task. This is the default value.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Represents a text-based task, where users provide written answers.
        /// </summary>
        Text = 2,
        /// <summary>
        /// Represents a multiple-choice task, where users select one answer from several options.
        /// </summary>
        MultipleChoice = 4,
        /// <summary>
        /// Represents a checkbox-based task, where users can select multiple answers.
        /// </summary>
        Checkboxes = 8,
        /// <summary>
        /// Represents a true/false task, where users select between two options: true or false.
        /// </summary>
        TrueFalse = 16
    }
}