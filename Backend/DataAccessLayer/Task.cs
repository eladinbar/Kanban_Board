using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Task : DalObject<Task>
    {
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

        public override void Save(string path)
        {
            DalController dc = new DalController();
            dc.WriteToFile(this.Id + "", ToJson(), path);
        }

        public void Delete(string fileName, string path) //Removes tasks appearing in multiple columns (occurs when advancing tasks)
        {
            DalController dc = new DalController();
            dc.RemoveFromFile(fileName, path);
        }        
    }
}
    