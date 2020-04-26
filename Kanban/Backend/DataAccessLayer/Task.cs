using System;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Task : DalObject<Task>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastChangedDate { get; set; }

        /// <summary>
        /// Data Access Layer Task constructor that receives all necessary Business Layer Task parameters to ensure they are all saved properly.
        /// </summary>
        /// <param name="id">The ID associated with this task.</param>
        /// <param name="title">The current title of the task.</param>
        /// <param name="description">The current description of the task.</param>
        /// <param name="creationTime">The time the task was created on.</param>
        /// <param name="dueDate">The date the task is due on.</param>
        /// <param name="lastChangedDate">The last date the task was changed on.</param>
        public Task(int id, string title, string description, DateTime creationTime, DateTime dueDate, DateTime lastChangedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            CreationTime = creationTime;
            DueDate = dueDate;
            LastChangedDate = lastChangedDate;
        }

        /// <summary>
        /// Empty public constructor for use when loading .json files from memory.
        /// </summary>
        public Task() { }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path)
        {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + path);
            dc.WriteToFile(this.Id + "-" + this.Title, ToJson(), path);
        }

        /// <summary>
        /// The method to remove a task appearing in multiple columns (occurs when advancing tasks).
        /// </summary>
        /// <param name="fileName">The name of the file to be deleted from the disk.</param>
        /// <param name="path">The path the object will be saved to.</param>
        public void Delete(string fileName, string path)
        {
            DalController dc = new DalController();
            dc.RemoveFromFile(fileName, path);
        }        
    }
}
    