using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Column : PersistedObject<DataAccessLayer.Column>
    {
        private string _name;
        private int _limit;
        private List<Task> _tasks;

        public Column(string name)
        {
            _name = name;
            _limit = Int32.MaxValue;
            _tasks = new List<Task>();

        }

        public Column(string name, List<Task> tasks, int limit)
        {
            _name = name;
            _limit = limit;
            _tasks = tasks;

        }

        public void LimitColumnTasks(int limit)
        {
            if (limit == 0)
                throw new ArgumentOutOfRangeException("Cannot use negative number to limit number of tasks");
            else if (limit > Tasks.Count)
                throw new ArgumentOutOfRangeException("Number of tasks is more then the desired limit");
            else
                _limit = limit;
        }

        public void InsertTask(Task t)
        {
            if (!CheckLimit())
                throw new ArgumentOutOfRangeException(Name + " column is full");
            else
                Tasks.Add(t);
        }
        public Task RemoveTask(int id)
        {
            Task toRemove = Tasks.Find(x => x.Id.Equals(id));
            if (Tasks.Remove(toRemove))
                return toRemove;
            else
                throw new ArgumentException("Task #" + id + " is not in " + Name);
        }
        
        public Task GetTask(int taskId)
        {
            if (Tasks.Exists(x => x.Id == taskId))
                return Tasks.Find(x => x.Id == taskId);
            else
                return null;
        }

        public DataAccessLayer.Column ToDalObject()
        {
            List<DataAccessLayer.Task> dalTasks = new List<DataAccessLayer.Task>();
            foreach(Task t in Tasks)
            {
                dalTasks.Add(t.ToDalObject());
            }
            return new DataAccessLayer.Column(Name, Limit, dalTasks);
        }

        public void Save(string path)
        {
            ToDalObject().Save(path);
        }

        public bool CheckLimit()
        {
            if (Tasks.Count() < Limit)
                return true;
            else
                return false;
        }
        
        //getters
        public string Name { get;}
        public int Limit { get; }
        public List<Task> Tasks { get; }


    }
}
