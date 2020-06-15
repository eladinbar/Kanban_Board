using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Task
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private const int MAXIMUM_TITLE_LENGTH = 50;
        private const int MINIMUM_TITLE_LENGTH = 0;
        private const int MAXIMUM_DESCRIPTION_LENGTH = 300; 

        public virtual int Id { get; internal set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string EmailAssignee { get; private set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; private set; }
        public DateTime LastChangedDate { get; private set; }
        public virtual DalTask DalCopyTask { get; internal set; }

        /// <summary>
        /// A public constructor that creates a new task and intializes all of its fields.
        /// </summary>
        /// <param name="title">The title the task will be created with.</param>
        /// <param name="description">The description the task will be created with.</param>
        /// <param name="dueDate">The date the task will be due on.</param>
        /// <param name="id">The unique ID that will be associated with this task.</param>
        /// <param name="email">The email of current board user.</param>
        /// <param name="columnName">The ordinal of the column the task should be added to.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the title or description given are invalid.</exception>
        public Task(string title, string description, DateTime dueDate, int id, string email, string columnName, string emailAssignee) 
        {
            if (title.Length > MINIMUM_TITLE_LENGTH && title.Length <= MAXIMUM_TITLE_LENGTH)
                Title = title;
            else
                throw new ArgumentOutOfRangeException("The title cannot be empty or exceed 50 characters");
            if (description == null)
                Description = "";
            else if (description.Length <= MAXIMUM_DESCRIPTION_LENGTH)
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
            EmailAssignee = emailAssignee;
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
        /// <param name="dalTask">The DAL appearance of the current board.</param>
        internal Task (string title, string description, DateTime dueDate, int id, DateTime creationTime, DateTime lastChangedDate, string emailAsignee, DalTask dalTask) 
        { 
            Title = title;
            Description = description;
            DueDate = dueDate;
            Id = id;
            CreationTime = creationTime;
            LastChangedDate = lastChangedDate;
            EmailAssignee = emailAsignee;
            DalCopyTask = dalTask;
            log.Info("Task " + id + " was Loaded from memory");
        }

        /// <summary>
        /// For testing only.
        /// </summary>
        internal Task() { }

        /// <summary>
        /// Changes the task's title.
        /// </summary>
        /// <param name="title">The new title to be given to the task.</param>
        /// <exception cref="ArgumentException">Thrown if the new title is empty or is more than 50 characters long.</exception>
        public void UpdateTaskTitle(string title) 
        {
            if(title == null)
            {
                throw new ArgumentNullException("Title cannot be null.");
            }
            else if (title.Length > 0 && title.Length <= MAXIMUM_TITLE_LENGTH)
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
        public void UpdateTaskDescription(string description) 
        {
            if (description == null)
            {
                Description = "";
                LastChangedDate = DateTime.Now;             
            }
            else if(description.Length <= MAXIMUM_DESCRIPTION_LENGTH)
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
        public void UpdateTaskDuedate(DateTime duedate) 
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

        /// <summary>
        /// Transforms the task to its data access layer variant.
        /// </summary>
        /// <param name="email">The email that is to be persisted with the new DalTask.</param>
        /// <param name="columnName">The column name that is to be persisted with the new DalTask.</param>
        /// <returns>Returns a DalTask with all necessary elements to be persisted.</returns>
        internal DalTask ToDalObject(string email, string columnName) {
            DalCopyTask = new DalTask(email, columnName, Id, Title, Description, EmailAssignee, DueDate, CreationTime, LastChangedDate);
            return DalCopyTask;
        }

        /// <summary>
        /// The method in the BusinessLayer to save a task to the database.
        /// </summary>
        /// <param name="email">The email that is to be persisted with the new DalTask.</param>
        /// <param name="columnName">The columnName that is to be persisted with the new DalTask.</param>
        internal void Save(string email, string columnName) {
            ToDalObject(email, columnName);
            DalCopyTask.Save();
        }

        /// <summary>
        /// Chacks if the current user is the Assignee of the task.
        /// </summary>
        /// <param name="currentUserEmail">vurrent loggedIn use email.</param>
        /// <returns>Returns true if the currentUserEmail is the TaskAssignee, otherwise returns false.</returns>
        public bool AssigneeCheck(string currentUserEmail)
        {
            if (this.EmailAssignee.Equals(currentUserEmail))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        internal void UpdateTaskAssignee(string emailAsignee)
        {
            EmailAssignee = emailAsignee;
            DalCopyTask.EmailAssignee = EmailAssignee;
        }

        /// <summary>
        /// Deletes the task from the database.
        /// </summary>
        internal void Delete()
        {
            DalCopyTask.Delete();
        }
    }
}
