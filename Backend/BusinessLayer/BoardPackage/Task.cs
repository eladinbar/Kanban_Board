using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Task
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public int Id { get; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; private set; }
        public DateTime LastChangedDate { get; private set; }
        public DalTask DalCopyTask { get; private set; } 

        /// <summary>
        /// A public constructor that creates a new task and intializes all of its fields.
        /// </summary>
        /// <param name="title">The title the task will be created with.</param>
        /// <param name="description">The description the task will be created with.</param>
        /// <param name="dueDate">The date the task will be due on.</param>
        /// <param name="id">The unique ID that will be associated with this task.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the title or description given are invalid.</exception>
        public Task(string title, string description, DateTime dueDate, int id, string email, string columnName) //checked
        {
            if (title.Length > 0 && title.Length <= 50)
                Title = title;
            else
                throw new ArgumentOutOfRangeException("The title cannot be empty or exceed 50 characters");
            if (description == null)
                Description = "";
            else if (description.Length <= 300)
                Description = description;
            else
                throw new ArgumentOutOfRangeException("The description cannot exceed 300 characters");

            if (dueDate.CompareTo(DateTime.Now) < 0)
                throw new ArgumentException("Due date cannot be set to past time.");
            else
                DueDate = dueDate;

            CreationTime = DateTime.Now;
            LastChangedDate = DateTime.Now;
            Id = id;
            DalCopyTask = new DalTask(email, columnName, id, title, description, dueDate, CreationTime, LastChangedDate);
            log.Info("New task #" + id + " was created");
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
        internal Task (string title, string description, DateTime dueDate, int id, DateTime creationTime, DateTime lastChangedDate, DalTask dalTask) //checked
        { 
            Title = title;
            Description = description;
            DueDate = dueDate;
            Id = id;
            CreationTime = creationTime;
            LastChangedDate = lastChangedDate;
            DalCopyTask = dalTask;
            log.Info("Task " + id + " was Loaded from memory");
        }

        /// <summary>
        /// Changes the task's title.
        /// </summary>
        /// <param name="title">The new title to be given to the task.</param>
        /// <exception cref="ArgumentException">Thrown if the new title is empty or is more than 50 characters long.</exception>
        public void UpdateTaskTitle(string title) //checked
        {
            if(title == null)
            {
                throw new ArgumentNullException("Title cannot be null.");
            }
            else if (title.Length > 0 && title.Length <= 50)
            {
                Title = title;
                LastChangedDate = DateTime.Now;
                DalCopyTask.Title = this.Title;
                DalCopyTask.LastChangedDate = this.LastChangedDate;

            }
            else
                throw new ArgumentException("The title cannot be empty or exceed 50 characters");
        }
       
        /// <summary>
        /// Changes the task's description.
        /// </summary>
        /// <param name="description">The new description the task will be given.</param>
        /// <exception cref="ArgumentException">Thrown when the description is more than 300 characters long.</exception>
        public void UpdateTaskDescription(string description) //checked
        {
            if (description == null)
            {
                Description = "";
                LastChangedDate = DateTime.Now;             
            }
            else if(description.Length <= 300)
            {
                Description = description;
                LastChangedDate = DateTime.Now;
            }
            else
                throw new ArgumentException("The description can not exceed 300 charecters");
            DalCopyTask.Description = this.Description;
            DalCopyTask.LastChangedDate = this.LastChangedDate;
        }

        /// <summary>
        /// Changes the task's due date to a new one.
        /// </summary>
        /// <param name="duedate">The new due date for the task.</param>
        /// <exception cref="ArgumentException">Thrown when the new due date is earlier than the current time.</exception>
        public void UpdateTaskDuedate(DateTime duedate) //checked
        {
            if (duedate == null)
                throw new ArgumentNullException("Due date cannot be null.");
            else if (duedate.CompareTo(DateTime.Now) < 0)
                throw new ArgumentException("Due date cannot be set to past time.");
            else
            {
                DueDate = duedate;
                LastChangedDate = DateTime.Now;
                DalCopyTask.DueDate = duedate;
                DalCopyTask.LastChangedDate = this.LastChangedDate;
            }
        }

    }
}
