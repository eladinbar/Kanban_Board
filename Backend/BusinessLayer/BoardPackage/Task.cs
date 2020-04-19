using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

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

        public Task(string title, string description, DateTime dueDate, int id)
        {
            if (title.Length > 0 && title.Length <= 50)
                Title = title;
            else
                throw new ArgumentOutOfRangeException("title can not be empty or exceed 50 charecters");
            if (description.Length <= 300)
                Description = description;
            else
                throw new ArgumentOutOfRangeException("description can not exceed 300 charecters");

            DueDate = dueDate.ToLocalTime();
            CreationTime = DateTime.Now.ToLocalTime();
            LastChangedDate = DateTime.Now.ToLocalTime();
            Id = id;
            log.Info("New task was created with " + id + " ID");

        }

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
        /// changes the task title.
        /// </summary>
        /// <param name="title">the new title of the task</param>
        /// <exception cref="ArgumentException">Trown if the new title is empty or more th 50 charecters</exception>
        public void UpdateTaskTitle(string title)
        {
            if (title.Length > 0 && title.Length <= 50)
            {
                Title = title;
                LastChangedDate = DateTime.Now.ToLocalTime();
            }
            else
                throw new ArgumentException("title can not exceed 50 charecters or be empty");
        }
       
        /// <summary>
        /// changes the task description
        /// </summary>
        /// <param name="description">the new description of the task</param>
        /// <exception cref="ArgumentException">Thrown when description is more then 300 characters</exception>
        public void UpdateTaskDescription(string description)
        {
            if (description.Length <= 300)
            {
                Description = description;
                LastChangedDate = DateTime.Now.ToLocalTime();
            }
            else
                throw new ArgumentException("description can not exceed 300 charecters");
        }
      
        /// <summary>
        /// changes the task duedate to a new one
        /// </summary>
        /// <param name="duedate">The new duedate.</param>
        /// <exception cref="ArgumentException">Thrown when the new duedate is erlier then the current time of the change.</exception>
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
        ///>inheritdoc/>
        public DataAccessLayer.Task ToDalObject()
        {
            return new DataAccessLayer.Task(Id, Title, Description, CreationTime, DueDate, LastChangedDate);
        }

        ///>inheritdoc/>
        public void Save(string path)
        {
            ToDalObject().Save(path);
            log.Info("Task.save was called");
        }

        /// <summary>
        /// prevents foe double saving the same task
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        internal void Delete(string fileName, string path) //Removes tasks appearing in multiple columns (occurs when advancing tasks)
        {
            ToDalObject().Delete(fileName, path);
            log.Info("Task " + Id + "-" + Title + "deleted");
        }
    }
}
