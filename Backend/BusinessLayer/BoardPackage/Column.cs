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
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string Name { get; }
        public int Limit { get; private set; }
        public List<Task> Tasks { get; }

        public Column(string name)
        {
            Name = name;
            Limit = Int32.MaxValue;
            Tasks = new List<Task>();
            log.Info("New column " + name + "created");

        }

        public Column(string name, List<Task> tasks, int limit)
        {
            Name = name;
            Limit = limit;
            Tasks = tasks;
            log.Debug("load - Board " + name + "was loaded from memory");

        }

        public void LimitColumnTasks(int limit)
        {
            if (limit == 0)
            {
                log.Error("Attempt to set limit to 0");
                throw new ArgumentOutOfRangeException("Cannot use negative number to limit number of tasks");
            }
            else if (limit > Tasks.Count)
            {
                log.Error("number of tasks in the column was greater then limit set attempt");
                throw new ArgumentOutOfRangeException("Number of tasks is more then the desired limit");
            }
            else
                Limit = limit;
        }

        internal void InsertTask(Task t)
        {
            if (!CheckLimit())
                throw new ArgumentOutOfRangeException(Name + " column is full");
            else
                Tasks.Add(t);
        }

        internal Task RemoveTask(int id)
        {
            Task toRemove = Tasks.Find(x => x.Id.Equals(id));
            if (Tasks.Remove(toRemove))
            {
                log.Debug("task " + id + " was removed from " + Name);
                return toRemove;
            }
            else
            {
                log.Error("Removal attempt to non existing task");
                throw new ArgumentException("Task #" + id + " is not in " + Name);
            }
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
            log.Debug("Creating DalObject<Column>");
            List<DataAccessLayer.Task> dalTasks = new List<DataAccessLayer.Task>();
            foreach(Task t in Tasks)
            {
                dalTasks.Add(t.ToDalObject());
            }
            return new DataAccessLayer.Column(Name, Limit, dalTasks);
        }

        public void Save(string path)
        {
            log.Info("Column.save was called");
            ToDalObject().Save(path);
        }

        internal bool CheckLimit()
        {
            if (Tasks.Count() < Limit)
                return true;
            else
                return false;
        }

    }
}
