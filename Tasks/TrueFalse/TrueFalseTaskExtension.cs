namespace STLib.Tasks.TrueFalse
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of the <see cref="TrueFalseTask"/> class.
    /// </summary>
    public static class TrueFalseTaskExtension
    {
        /// <summary>
        /// Adds or updates the name of the <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="name">The name to assign to the task.</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask AddName(this TrueFalseTask task, string name)
        {
            task.SetName(name);

            return task;
        }
        /// <summary>
        /// Adds or updates the question of the <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="question">The question to assign to the task.</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask AddQuestion(this TrueFalseTask task, string question)
        {
            task.SetQuestion(question);

            return task;
        }
        /// <summary>
        /// Sets whether the <see cref="TrueFalseTask"/> should be considered in the grading process.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="consider">A boolean indicating whether the task should be considered.</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask AddConsider(this TrueFalseTask task, bool consider)
        {
            task.SetConsider(consider);

            return task;
        }
        /// <summary>
        /// Adds or updates the maximum grade for the <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="maxGrade">The maximum grade to assign to the task.</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask AddMaxGrade(this TrueFalseTask task, int maxGrade)
        {
            task.SetMaxGrade(maxGrade);

            return task;
        }
        /// <summary>
        /// Adds or updates the grade for the <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="grade">The grade to assign to the task.</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        public static TrueFalseTask AddGrade(this TrueFalseTask task, int grade)
        {
            task.SetGrade(grade);

            return task;
        }
        /// <summary>
        /// Adds or updates the correct answer for the <see cref="TrueFalseTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="correctAnswer">The correct answer to assign to the task, must be either "True" or "False".</param>
        /// <returns>The updated <see cref="TrueFalseTask"/> instance.</returns>
        /// <exception cref="ArgumentException">Thrown if the correct answer is not "True" or "False".</exception>
        public static TrueFalseTask AddCorrectAnswer(this TrueFalseTask task, string correctAnswer)
        {
            task.SetCorrectAnswer(correctAnswer);

            return task;
        }
    }
}