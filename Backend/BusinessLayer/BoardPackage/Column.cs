using System;
using System.Collections.Generic;
using System.Linq;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    internal class Column
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string Name { get; }
        public int Limit { get; private set; }
        public List<Task> Tasks { get; }
        public DalColumn DalCopyColumn { get; private set; }



        /// <summary>
        /// A public contructor that creates a new column and initializes its fields.
        /// </summary>
        /// <param name="name">The name the column will be created with.</param>
        /// <param name="email">The email of the board user.</param>
        /// <param name="columnOrdinal">Ordinal the column will be created with.</param>
        public Column(string name, string email, int columnOrdinal) //checked
        {
            Name = name;
            Limit = Int32.MaxValue;
            Tasks = new List<Task>();
            log.Info("New column " + name + "created");
        }

        /// <summary>
        /// An internal constructor that initializes all of the required fields upon loading an exisiting column from memory.
        /// </summary>
        /// <param name="name">The name the column will be created with.</param>
        /// <param name="limit">The maximum amount of tasks to be allowed in this column.</param>
        /// <param name="tasks">The list of tasks the column contains.</param>
        /// <param name="dalColumn">The DAL appearance of the current column.</param>
        internal Column(string name, int limit, List<Task> tasks, DalColumn dalColumn) //checked
        {
            Name = name;
            Limit = limit;
            Tasks = tasks;
            DalCopyColumn = dalColumn;
            log.Debug("load - Board " + name + " was loaded from memory");
        }

        /// <summary>
        /// Limits the amount of tasks the column may contain.
        /// </summary>
        /// <param name="limit">The desired column limit.</param>
        /// <exception cref="ArgumentException">Thrown when trying to set the limit to a number less or equal to 0.
        /// Alternatively thrown if there are more tasks than the specified limit.</exception>
        public void LimitColumnTasks(int limit) //checked 
        {
            if (limit == 0)
            {
                log.Error("Attempt to set limit to 0");
                throw new ArgumentOutOfRangeException("Cannot use negative numbers to limit the number of tasks");
            }
            else if (limit < Tasks.Count)
            {
                log.Error("The number of tasks in the column is greater than the limit given");
                throw new ArgumentOutOfRangeException("The number of tasks in the column: " + Tasks.Count + ", is more than the desired limit: " + limit);
            }
            else
            {
                Limit = limit;
                DalCopyColumn.Limit = limit;
            }
        }

        /// <summary>
        /// Inserts a task to this column.
        /// </summary>
        /// <param name="t">The task to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the column is full.</exception>
        internal void InsertTask(Task t) //checked
        {
            if (!CheckLimit())
            {
                log.Warn("The column '" + Name + "' was full - task insert failed.");
                throw new ArgumentOutOfRangeException(Name + " column is full");
            }
            else
            {
                Tasks.Add(t);
                log.Debug("The task " + t.Id + " was added to '" + Name + "' column");
            }
        }

        /// <summary>
        /// Removes a task from the column.
        /// </summary>
        /// <param name="taskId">The ID of the task to remove.</param>
        /// <returns>Returns the task that has been removed.</returns>
        /// <exception cref="ArgumentException">Thrown if the task does not exist in the column.</exception>
        internal Task RemoveTask(int taskId) //checked
        {
            Task toRemove = Tasks.Find(x => x.Id.Equals(taskId));
            if (Tasks.Remove(toRemove))
            {
                log.Debug("The task " + taskId + " was removed from '" + Name+"' column");
                return toRemove;
            }
            else
            {
                log.Error("Removal attempt to non existing task");
                throw new ArgumentException("Task #" + taskId + " is not in '" + Name+"' column");
            }
        }
        
        /// <summary>
        /// Gets the specified task.
        /// </summary>
        /// <param name="taskId">The ID of the task to return.</param>
        /// <returns>Returns the task with the task ID if it exists, otherwise returns null.</returns>
        public Task GetTask(int taskId) //checked
        {
            if (Tasks.Exists(x => x.Id == taskId))
                return Tasks.Find(x => x.Id == taskId);
            else
                throw new ArgumentException("Task #"+taskId+" does not exist in '" + Name + "' column");
        }

        /// <summary>
        /// Checks if the column is full.
        /// </summary>
        /// <returns>Returns true if the column is not full, otherwise returns false.</returns>
        internal bool CheckLimit() //checked
        {
            if (Tasks.Count() < Limit)
                return true;
            else
                return false;
        }

        internal DalColumn ToDalObject(string email, int columnOrdinal)
        {
            DalCopyColumn = new DalColumn(email, columnOrdinal, Name, Limit);
            return DalCopyColumn;
        }

        internal void Save(string email, int columnOrdinal)
        {
            ToDalObject(email, columnOrdinal);
            DalCopyColumn.Save();
        }
    }
}
