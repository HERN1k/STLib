namespace STLib.Tasks.Text
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of the <see cref="TextTask"/> class.
    /// </summary>
    public static class TextTaskExtension
    {
        /// <summary>
        /// Adds or updates the name of the <see cref="TextTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="name">The name to assign to the task.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddName(this TextTask task, string name)
        {
            task.SetName(name);

            return task;
        }
        /// <summary>
        /// Adds or updates the question of the <see cref="TextTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="question">The question to assign to the task.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddQuestion(this TextTask task, string question)
        {
            task.SetQuestion(question);

            return task;
        }
        /// <summary>
        /// Sets whether the <see cref="TextTask"/> should be considered in the grading process.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="consider">A boolean indicating whether the task should be considered.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddConsider(this TextTask task, bool consider)
        {
            task.SetConsider(consider);

            return task;
        }
        /// <summary>
        /// Adds or updates the maximum grade for the <see cref="TextTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="maxGrade">The maximum grade to assign to the task.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddMaxGrade(this TextTask task, int maxGrade)
        {
            task.SetMaxGrade(maxGrade);

            return task;
        }
        /// <summary>
        /// Adds or updates the grade for the <see cref="TextTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="grade">The grade to assign to the task.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddGrade(this TextTask task, int grade)
        {
            task.SetGrade(grade);

            return task;
        }
        /// <summary>
        /// Adds or updates the correct answer for the <see cref="TextTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="correctAnswer">The correct answer to assign to the task.</param>
        /// <returns>The updated <see cref="TextTask"/> instance.</returns>
        public static TextTask AddCorrectAnswer(this TextTask task, string correctAnswer)
        {
            task.SetCorrectAnswer(correctAnswer);

            return task;
        }
    }
}