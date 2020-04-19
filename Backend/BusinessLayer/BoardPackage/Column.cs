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

        /// <summary>
        /// limits the number of task the column can hold
        /// </summary>
        /// <param name="limit">the desierd limit</param>
        /// <exception cref="ArgumentException">theown when tring to set limit to s number less or equal to 0.
        /// alternetevly trown if there is more tasks then the specified limit.</exception>
        public void LimitColumnTasks(int limit)
        {
            if (limit == 0)
            {
                log.Error("Attempt to set limit to 0");
                throw new ArgumentOutOfRangeException("Cannot use negative number to limit number of tasks");
            }
            else if (limit < Tasks.Count)
            {
                log.Error("number of tasks in the column was greater then limit set attempt");
                throw new ArgumentOutOfRangeException("Number of tasks is more then the desired limit");
            }
            else
                Limit = limit;
        }
        /// <summary>
        /// insert a task to the Tasks of the column
        /// </summary>
        /// <param name="t">the Task object to insert</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// trown if the column is full
        /// </exception>
        internal void InsertTask(Task t)
        {
            if (!CheckLimit())
                throw new ArgumentOutOfRangeException(Name + " column is full");
            else
                Tasks.Add(t);
        }

        /// <summary>
        /// Removes the Task of the Column
        /// </summary>
        /// <param name="id">the ID of the task to remove</param>
        /// <returns>
        /// returns the task that has been removed.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if the task do not exist in the column.</exception>
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
        
        /// <summary>
        /// Gets Spesified Task
        /// </summary>
        /// <param name="taskId">The ID of the task to get</param>
        /// <returns>
        /// Returns the task with the task id if exist. else return null.
        /// </returns>
        public Task GetTask(int taskId)
        {
            if (Tasks.Exists(x => x.Id == taskId))
                return Tasks.Find(x => x.Id == taskId);
            else
                throw new ArgumentException("Task #"+taskId+" does not exist in column");
        }
        ///>inheritdoc/>
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

        ///>inheritdoc/>
        public void Save(string path)
        {
            log.Info("Column.save was called");
            ToDalObject().Save(path);
        }
        /// <summary>
        /// Checks if the column is full
        /// </summary>
        /// <returns>
        /// return true if it's not full. else, return folse.
        /// </returns>
        internal bool CheckLimit()
        {
            if (Tasks.Count() < Limit)
                return true;
            else
                return false;
        }

    }
}
