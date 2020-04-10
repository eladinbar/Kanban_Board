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
        private int _id;
        private string _title;
        private string _description;
        private DateTime _creationDate;
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
            _creationDate = DateTime.Now;
            _lastChangedDate = DateTime.Now;
            _id = id;

            Save();

        }

        public void UpdateTaskTitle(string title)
        {
            if (title.Length > 0 && title.Length <= 50)
            {
                _title = title;
                _lastChangedDate = new DateTime().ToLocalTime();
                Save();
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
                Save();
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
                Save();
            }
        }

        public DataAccessLayer.Task ToDalObject()
        {
            return new DataAccessLayer.Task(Id, Title, Description, CreationDate, DueDate, LastChangedDate);
        }

        public void Save()
        {
            ToDalObject().Save();
        }

        
        //getters
        public int Id { get;}
        public string Title { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public DateTime CreationDate { get; }
        public DateTime LastChangedDate { get; }
    }
}
