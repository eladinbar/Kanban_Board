using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    /// <summary>
    /// Represents the Kanban Board
    /// </summary>
    internal class Board : PersistedObject<DataAccessLayer.Board>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public List<Column> Columns { get; }
        public string UserEmail { get; }
        public int TaskCounter { get; set; }

        /// <summary>
        /// A public constructor that creates a new board and initializes all of its fields.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        public Board(string email)
        {
            UserEmail = email;
            TaskCounter = 0;
            Columns = new List<Column>();
            Columns.Add(newColumn("backlog"));
            Columns.Add(newColumn("in progress"));
            Columns.Add(newColumn("done"));
            log.Info("New board created");
        }

        /// <summary>
        /// An internal constructor that initializes all of the required fields upon loading an existing board from memory.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="taskCounter">The amount of tasks that the board contains.</param>
        /// <param name="columns">The list of columns the board contains.</param>
        internal Board(string email, int taskCounter, List<Column> columns)
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
            log.Info("load - Board " + email + " was loaded from memory");
        }

        /// <summary>
        /// Creates a new column and returns it.
        /// </summary>
        /// <param name="name">The name of the column to be created.</param>
        /// <returns>Returns the created column.</returns>
        private Column newColumn(string name) {
            Column newColumn = new Column(name);
            newColumn.Save("Boards\\" + UserEmail + "\\" + Columns.Count + "-");
            return newColumn;
        }

        /// <summary>
        /// Get the column with the specified name.
        /// </summary>
        /// <param name="columnName">The column name to return.</param>
        /// <returns>Returns the column with the given name.</returns>
        /// <exception cref="ArgumentException">Thrown when the Column with the given name does not exist.</exception>
        public Column GetColumn(string columnName)
        {
            if (Columns.Exists(c => c.Name.Equals(columnName)))
            {
                log.Debug(UserEmail + ": returned column " + columnName);
                return Columns.Find(x => x.Name.Equals(columnName));
            }
            else
                throw new ArgumentException("'" + columnName + "' column does not exist.");
        }
        /// <summary>
        /// Get the column with the specified Index.
        /// </summary>
        /// <param name="columnOrdinal">The index of the column to return.</param>
        /// <returns>Returns the column with the given ordinal.</returns>
        /// <exception cref="ArgumentException">Thrown when the ordinal given doesn't point to any valid Column.</exception>
        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal >= Columns.Count || columnOrdinal<0)
            {
                log.Warn("columnOrdinal was out of range");
                throw new ArgumentOutOfRangeException("Column index out of range");
            }
            log.Debug(UserEmail + ": returned column no." + columnOrdinal);
               return Columns[columnOrdinal];
        }
        /// <summary>
        /// Get the names of the board's columns as a list.
        /// </summary>
        /// <returns>Returns a List of strings with the column names.</returns>
        public List<string> getColumnNames()
        {
            List<string> columnNames = new List<string>();
            foreach(Column c in Columns)
            {
                columnNames.Add(c.Name);
            }
            log.Debug("Returned columns' names");
            return columnNames;
        }

        /// <summary>
        /// Checks whether a task with the given ID exists.
        /// </summary>
        /// <returns>Returns true if the task exists, otherwise returns false.</returns>
        internal bool TaskIdExistenceCheck(int id)
        {
            return id <= TaskCounter & id >= 1;
        }

        /// <summary>
        /// The method in the BusinessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public void Save(string path)
        {
            log.Info("Board.save was called");
            ToDalObject().Save(path);         
        }

        /// <summary>
        /// Transforms the Board to its corresponding DalObject.
        /// </summary>
        /// <returns>Returns a Data Access Layer Board.</returns>
        public DataAccessLayer.Board ToDalObject()
        {
            log.Debug("Creating DalObject<Board>");
            List<DataAccessLayer.Column> dalColumns = new List<DataAccessLayer.Column>();
            foreach(Column c in Columns)
            {
                dalColumns.Add(c.ToDalObject());
            }
            return new DataAccessLayer.Board(UserEmail, TaskCounter, dalColumns);
        }
    }
}
