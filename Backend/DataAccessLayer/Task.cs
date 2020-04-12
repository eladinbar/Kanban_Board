using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Task : DalObject<Task>
    {
        private readonly int _id;
        private readonly string _title;
        private readonly string _description;
        private readonly DateTime _creationDate;
        private readonly DateTime _dueDate;
        private readonly DateTime _lastChangedDate;

        public Task (int id, string title, string description, DateTime creationDate,DateTime dueDate, DateTime lastChangedDate)
        {
            _id = id;
            _title = title;
            _description = description;
            _creationDate = creationDate;
            _dueDate = dueDate;
            _lastChangedDate = lastChangedDate;
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

        //getters
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public DateTime CreationTime { get; }
        public DateTime LastChangedDate { get; }

    }
}
    