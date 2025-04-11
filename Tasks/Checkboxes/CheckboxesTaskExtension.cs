using System.Collections.Generic;

namespace STLib.Tasks.Checkboxes
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of the <see cref="CheckboxesTask"/> class.
    /// </summary>
    public static class CheckboxesTaskExtension
    {
        /// <summary>
        /// Adds or updates the name of the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="name">The name to assign to the task.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddName(this CheckboxesTask task, string name)
        {
            task.SetName(name);

            return task;
        }
        /// <summary>
        /// Adds or updates the question of the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="question">The question to assign to the task.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddQuestion(this CheckboxesTask task, string question)
        {
            task.SetQuestion(question);

            return task;
        }
        /// <summary>
        /// Sets whether the <see cref="CheckboxesTask"/> should be considered in the grading process.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="consider">A boolean indicating whether the task should be considered.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddConsider(this CheckboxesTask task, bool consider)
        {
            task.SetConsider(consider);

            return task;
        }
        /// <summary>
        /// Adds or updates the maximum grade for the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="maxGrade">The maximum grade to assign to the task.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddMaxGrade(this CheckboxesTask task, int maxGrade)
        {
            task.SetMaxGrade(maxGrade);

            return task;
        }
        /// <summary>
        /// Adds or updates the grade for the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="grade">The grade to assign to the task.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddGrade(this CheckboxesTask task, int grade)
        {
            task.SetGrade(grade);

            return task;
        }
        /// <summary>
        /// Adds or updates the list of correct answers for the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="correctAnswers">A list of correct answers to assign to the task.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddCorrectAnswers(this CheckboxesTask task, List<string> correctAnswers)
        {
            task.SetCorrectAnswers(correctAnswers);

            return task;
        }
        /// <summary>
        /// Adds a single answer to the list of possible answers for the <see cref="CheckboxesTask"/>.
        /// </summary>
        /// <param name="task">The task to modify.</param>
        /// <param name="answer">The answer to add to the list of possible answers.</param>
        /// <returns>The updated <see cref="CheckboxesTask"/> instance.</returns>
        public static CheckboxesTask AddAnswerItem(this CheckboxesTask task, string answer)
        {
            task.SetAnswersItem(answer);

            return task;
        }
    }
}