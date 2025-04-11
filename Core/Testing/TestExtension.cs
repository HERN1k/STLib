using System;
using System.Collections.Generic;

namespace STLib.Core.Testing
{
    /// <summary>
    /// Provides extension methods to enhance the functionality of the <see cref="Test"/> class.
    /// </summary>
    public static class TestExtension
    {
        /// <summary>
        /// Adds or updates the name of the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="name">The name to assign to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddName(this Test test, string name)
        {
            test.SetName(name);

            return test;
        }
        /// <summary>
        /// Adds or updates the description of the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="description">The description to assign to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddDescription(this Test test, string description)
        {
            test.SetDescription(description);

            return test;
        }
        /// <summary>
        /// Adds or updates the instructions for the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="instructions">The instructions to assign to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddInstructions(this Test test, string instructions)
        {
            test.SetInstructions(instructions);

            return test;
        }
        /// <summary>
        /// Adds or updates the grade for the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="grade">The grade to assign to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddGrade(this Test test, int grade)
        {
            test.SetGrade(grade);

            return test;
        }
        /// <summary>
        /// Adds or updates a subject for the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="subject">The subject GUID to associate with the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddSubject(this Test test, Guid subject)
        {
            test.SetSubject(subject);

            return test;
        }
        /// <summary>
        /// Adds or updates the test time.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="time">The time span to assign to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddTestTime(this Test test, TimeSpan time)
        {
            test.SetTestTime(time);

            return test;
        }
        /// <summary>
        /// Adds an attention item to the test and registers the modifier.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="attention">The attention to add to the test.</param>
        /// <param name="modifier">The GUID of the user modifying the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddAttention(this Test test, Attention attention, Guid modifier)
        {
            test.SetAttention(attention, modifier);

            return test;
        }
        /// <summary>
        /// Adds a single task to the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="task">The task to add to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddTask(this Test test, CoreTask task)
        {
            test.SetTask(task);

            return test;
        }
        /// <summary>
        /// Adds multiple tasks to the test.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="tasks">A collection of tasks to add to the test.</param>
        /// <returns>The updated test object.</returns>
        public static Test AddTasks(this Test test, IEnumerable<CoreTask> task)
        {
            foreach (var t in task)
            {
                test.SetTask(t);
            }

            return test;
        }
        /// <summary>
        /// Removes a single task from the test by its unique identifier.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="taskID">The unique identifier of the task to remove.</param>
        /// <returns>The updated test object.</returns>
        public static Test RemoveTask(this Test test, Guid taskID)
        {
            test.UnSetTask(taskID);

            return test;
        }
        /// <summary>
        /// Removes multiple tasks from the test using their unique identifiers.
        /// </summary>
        /// <param name="test">The test object to modify.</param>
        /// <param name="IDs">A collection of task unique identifiers to remove.</param>
        /// <returns>The updated test object.</returns>
        public static Test RemoveTasks(this Test test, IEnumerable<Guid> IDs)
        {
            foreach (var t in IDs)
            {
                test.UnSetTask(t);
            }

            return test;
        }
    }
}