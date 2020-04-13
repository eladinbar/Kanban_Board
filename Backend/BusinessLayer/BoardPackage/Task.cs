using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Task : PersistedObject<DataAccessLayer.Task>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private int _id;
        private string _title;
        private string _description;
        private DateTime _creationTime;
        private DateTime _dueDate;
        private DateTime _lastChangedDate;

        public Task(string title, string description, DateTime dueDate, int id)
        {
            if (title.Length > 0 && title.Length <= 50)
                _title = title;
            else
                throw new ArgumentOutOfRangeException("title can not exceed 50 charecters");
            if (description.Length <= 300)
                _description = description;
            else
                throw new ArgumentOutOfRangeException("description can not exceed 300 charecters");

            _dueDate = dueDate;
            _creationTime = DateTime.Now;
            _lastChangedDate = DateTime.Now;
            _id = id;

        }

        internal Task (string title, string description, DateTime dueDate, int id, DateTime creationTime, DateTime lastChangedDate) {
            _title = title;
            _description = description;
            _dueDate = dueDate;
            _id = id;
            _creationTime = creationTime;
            _lastChangedDate = lastChangedDate;
        }

        public void UpdateTaskTitle(string title)
        {
            if (title.Length > 0 && title.Length <= 50)
            {
                _title = title;
                _lastChangedDate = new DateTime().ToLocalTime();
            }
            else
                throw new ArgumentException("title can not exceed 50 charecters");
        }
       

        public void UpdateTaskDescription(string description)
        {
            if (description.Length <= 300)
            {
                _description = description;
                _lastChangedDate = new DateTime().ToLocalTime();
            }
            else
                throw new ArgumentException("description can not exceed 300 charecters");
        }
      
              
        public void UpdateTaskDuedate(DateTime duedate)
        {
            if (duedate.CompareTo(new DateTime()) < 0)
                throw new ArgumentException("Due date cannot be set to past time");
            else
            {
                _dueDate = duedate;
                _lastChangedDate = new DateTime().ToLocalTime();
            }
        }

        public DataAccessLayer.Task ToDalObject()
        {
            return new DataAccessLayer.Task(Id, Title, Description, CreationTime, DueDate, LastChangedDate);
        }

        public void Save(string path)
        {
            ToDalObject().Save(path);
        }

        public void Delete(string fileName, string path) //Removes tasks appearing in multiple columns (occurs when advancing tasks)
        {
            ToDalObject().Delete(Id + "", path);
        }

        //getters
        public int Id { get;}
        public string Title { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public DateTime CreationTime { get; }
        public DateTime LastChangedDate { get; }
    }
}
