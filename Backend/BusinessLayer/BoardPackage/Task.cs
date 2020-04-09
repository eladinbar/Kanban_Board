using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Task : PresistedObject<DataAccessLayer.Task>
    {
        private int _id;
        private string _title;
        private string _description;
        private DateTime _creationDate;
        private DateTime _dueDate;
        private DateTime _LastChangedDate;

        public Task(string title, string description, DateTime dueDate, int id)
        {
            if (title.Length > 0 && title.Length <= 50)
                this._title = title;
            else
                throw new ArgumentOutOfRangeException("title can not exceed 50 charecters");
            if (description.Length <= 300)
                this._description = description;
            else
                throw new ArgumentOutOfRangeException("description can not exceed 300 charecters");

            this._dueDate = dueDate;
            this._creationDate = DateTime.Now;
            this._LastChangedDate = DateTime.Now;
            this._id = id;

        }

        public void UpdateTaskTitle(string title)
        {
            if (title.Length > 0 && title.Length <= 50)
                Title = title;
            //update lastChangeddate
            else
                throw new ArgumentException("title can not exceed 50 charecters");
        }
        private string Title { get; set; }

        public void UpdateTaskDescription(string description)
        {
            if (description.Length <= 300)
                Description = description;
            //update last Changed date
            else
                throw new ArgumentException("description can not exceed 300 charecters");
        }
        private string Description { get; set; }

        public void UpdateDuedate(DateTime duedate)
        {
            if (duedate < _dueDate)
                throw new ArgumentException();
            else
                DueDate = duedate;
        }
        private DateTime DueDate { get; set; }

        public int Id { get;}
    }
}
