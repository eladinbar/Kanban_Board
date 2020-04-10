using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Task : DalObject<Task>
    {
        private int _id;
        private string _title;
        private string _description;
        private DateTime _creationDate;
        private DateTime _dueDate;
        private DateTime _lastChangedDate;

        public Task (int id, string title, string description, DateTime creationDate,DateTime dueDate, DateTime lastChangedDate)
        {
            _id = id;
            _title = title;
            _description = description;
            _creationDate = creationDate;
            _dueDate = dueDate;
            _lastChangedDate = lastChangedDate;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public User FromJson()
        {
            throw new NotImplementedException();
        }
    }
}
    