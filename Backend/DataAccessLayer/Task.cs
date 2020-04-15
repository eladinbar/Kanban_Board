using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Task : DalObject<Task>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public DateTime CreationTime { get; }
        public DateTime LastChangedDate { get; }

        public Task(int id, string title, string description, DateTime creationTime, DateTime dueDate, DateTime lastChangedDate)
        {
            Id = id;
            Title = title;
            Description = description;
            CreationTime = creationTime;
            DueDate = dueDate;
            LastChangedDate = lastChangedDate;
        }

        public Task() { }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path)
        {
            DalController dc = new DalController();
            dc.WriteToFile(this.Id + "", ToJson(), path);
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
    