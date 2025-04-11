namespace STLib.Tasks.MultipleChoice
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of the <see cref="MultipleChoiceTask"/> class.
    /// </summary>
    public static class MultipleChoiceTaskExtension
    {
        /// <summary>
        /// Adds or updates the name of the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="name">The name to assign to the task.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddName(this MultipleChoiceTask task, string name)
        {
            task.SetName(name);

            return task;
        }
        /// <summary>
        /// Adds or updates the question of the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="question">The question to assign to the task.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddQuestion(this MultipleChoiceTask task, string question)
        {
            task.SetQuestion(question);

            return task;
        }
        /// <summary>
        /// Sets whether the <see cref="MultipleChoiceTask"/> should be considered in the grading process.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="consider">A boolean indicating whether the task should be considered.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddConsider(this MultipleChoiceTask task, bool consider)
        {
            task.SetConsider(consider);

            return task;
        }
        /// <summary>
        /// Adds or updates the maximum grade for the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="maxGrade">The maximum grade to assign to the task.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddMaxGrade(this MultipleChoiceTask task, int maxGrade)
        {
            task.SetMaxGrade(maxGrade);

            return task;
        }
        /// <summary>
        /// Adds or updates the grade for the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="grade">The grade to assign to the task.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddGrade(this MultipleChoiceTask task, int grade)
        {
            task.SetGrade(grade);

            return task;
        }
        /// <summary>
        /// Adds or updates the correct answer for the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="correctAnswer">The correct answer to assign to the task.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddCorrectAnswer(this MultipleChoiceTask task, string correctAnswer)
        {
            task.SetCorrectAnswer(correctAnswer);

            return task;
        }
        /// <summary>
        /// Adds a single answer to the list of possible answers for the <see cref="MultipleChoiceTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="answer">The answer to add to the list of possible answers.</param>
        /// <returns>The updated <see cref="MultipleChoiceTask"/> instance.</returns>
        public static MultipleChoiceTask AddAnswerItem(this MultipleChoiceTask task, string answer)
        {
            task.SetAnswersItem(answer);

            return task;
        }
    }
}