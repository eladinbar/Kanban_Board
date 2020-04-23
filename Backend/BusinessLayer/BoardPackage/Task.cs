using System;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Task : PersistedObject<DataAccessLayer.Task>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; private set; }
        public DateTime LastChangedDate { get; private set; }

        /// <summary>
        /// A public constructor that creates a new task and intializes all of its fields.
        /// </summary>
        /// <param name="title">The title the task will be created with.</param>
        /// <param name="description">The description the task will be created with.</param>
        /// <param name="dueDate">The date the task will be due on.</param>
        /// <param name="id">The unique ID that will be associated with this task.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the title or description given are invalid.</exception>
        public Task(string title, string description, DateTime dueDate, int id)
        {
            if (title.Length > 0 && title.Length <= 50)
                Title = title;
            else
                throw new ArgumentOutOfRangeException("The title cannot be empty or exceed 50 charecters");
            if (description.Length <= 300)
                Description = description;
            else
                throw new ArgumentOutOfRangeException("The description can not exceed 300 charecters");

            DueDate = dueDate.ToLocalTime();
            CreationTime = DateTime.Now.ToLocalTime();
            LastChangedDate = DateTime.Now.ToLocalTime();
            Id = id;
            log.Info("New task was created with " + id + " ID");
        }

        /// <summary>
        /// An internal constructor that initializes all of the required fields upon loading an exisiting task from memory.
        /// </summary>
        /// <param name="title">The title the task will be created with.</param>
        /// <param name="description">The description the task will be created with.</param>
        /// <param name="dueDate">The date the task will be due on.</param>
        /// <param name="id">The unique ID that will be associated with this task.</param>
        /// <param name="creationTime">The time in which this loaded task was created on.</param>
        /// <param name="lastChangedDate">The last date this task was changed.</param>
        internal Task (string title, string description, DateTime dueDate, int id, DateTime creationTime, DateTime lastChangedDate) {
            Title = title;
            Description = description;
            DueDate = dueDate.ToLocalTime();
            Id = id;
            CreationTime = creationTime.ToLocalTime();
            LastChangedDate = lastChangedDate.ToLocalTime();
            log.Info("Task " + id + " was Loaded from memory");
        }

        /// <summary>
        /// Changes the task's title.
        /// </summary>
        /// <param name="title">The new title to be given to the task.</param>
        /// <exception cref="ArgumentException">Thrown if the new title is empty or is more than 50 characters long.</exception>
        public void UpdateTaskTitle(string title)
        {
            if (title.Length > 0 && title.Length <= 50)
            {
                Title = title;
                LastChangedDate = DateTime.Now.ToLocalTime();
            }
            else
                throw new ArgumentException("The title cannot be empty or exceed 50 characters");
        }
       
        /// <summary>
        /// Changes the task's description.
        /// </summary>
        /// <param name="description">The new description the task will be given.</param>
        /// <exception cref="ArgumentException">Thrown when the description is more than 300 characters long.</exception>
        public void UpdateTaskDescription(string description)
        {
            if (description.Length <= 300)
            {
                Description = description;
                LastChangedDate = DateTime.Now.ToLocalTime();
            }
            else
                throw new ArgumentException("The description can not exceed 300 charecters");
        }
      
        /// <summary>
        /// Changes the task's due date to a new one.
        /// </summary>
        /// <param name="duedate">The new due date for the task.</param>
        /// <exception cref="ArgumentException">Thrown when the new due date is earlier than the current time.</exception>
        public void UpdateTaskDuedate(DateTime duedate)
        {
            if (duedate.CompareTo(DateTime.Now) < 0)
                throw new ArgumentException("Due date cannot be set to past time");
            else
            {
                DueDate = duedate.ToLocalTime();
                LastChangedDate = DateTime.Now.ToLocalTime();
            }
        }

        /// <summary>
        /// Transforms the task to its corresponding DalObject.
        /// </summary>
        /// <returns>Returns a Data Access Layer Task.</returns>
        public DataAccessLayer.Task ToDalObject()
        {
            return new DataAccessLayer.Task(Id, Title, Description, CreationTime, DueDate, LastChangedDate);
        }

        /// <summary>
        /// The method in the BusinessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public void Save(string path)
        {
            ToDalObject().Save(path);
            log.Info("Task.save was called");
        }

        /// <summary>
        /// Removes a task from memory (used when a task is advanced to a new column).
        /// </summary>
        /// <param name="fileName">The name the task will be saved with.</param>
        /// <param name="path">The path the task will be saved to.</param>
        internal void Delete(string fileName, string path)
        {
            ToDalObject().Delete(fileName, path);
            log.Info("Task " + Id + "-" + Title + "deleted");
        }
    }
}
