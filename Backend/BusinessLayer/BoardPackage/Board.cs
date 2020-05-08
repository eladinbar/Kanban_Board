using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;


namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    /// <summary>
    /// Represents the Kanban Board
    /// </summary>
    internal class Board
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public List<Column> Columns { get; }
        public string UserEmail { get; }
        public int TaskCounter { get; set; }
        public DalBoard DalCopyBoard { get; private set; }


        /// <summary>
        /// A public constructor that creates a new board and initializes all of its fields.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        public Board(string email) //checked
        {
            UserEmail = email;
            TaskCounter = 0;
            Columns = new List<Column>();
            Columns.Add(newColumn("backlog"));
            Columns.Add(newColumn("in progress"));
            Columns.Add(newColumn("done"));
            DalCopyBoard = new DalBoard(email, TaskCounter);
            log.Info("New board created");
        }

        /// <summary>
        /// An internal constructor that initializes all of the required fields upon loading an existing board from memory.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="taskCounter">The amount of tasks that the board contains.</param>
        /// <param name="columns">The list of columns the board contains.</param>
        internal Board(string email, int taskCounter, List<Column> columns, DalBoard dalBoard) //checked
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
            DalCopyBoard = dalBoard;
            log.Info("load - Board " + email + " was loaded from memory");
        }

        /// <summary>
        /// Creates a new column and returns it.
        /// </summary>
        /// <param name="name">The name of the column to be created.</param>
        /// <returns>Returns the created column.</returns>
        private Column newColumn(string name) { //not needed???
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
                return Columns.Find(c => c.Name.Equals(columnName));          
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
        public void Save(string path) //not needed???
        {
            log.Info("Board.save was called");
            ToDalObject().Save(path);         
        }

        /// <summary>
        /// Transforms the Board to its corresponding DalObject.
        /// </summary>
        /// <returns>Returns a Data Access Layer Board.</returns>
        public DataAccessLayer.Board ToDalObject() //not needed???
        {
            log.Debug("Creating DalObject<Board>");
            List<DataAccessLayer.Column> dalColumns = new List<DataAccessLayer.Column>();
            foreach(Column c in Columns)
            {
                dalColumns.Add(c.ToDalObject());
            }
            return new DataAccessLayer.Board(UserEmail, TaskCounter, dalColumns);
        }

        public Column AddColumn(string email, int columnOrdinal, string Name) //checked
        {
            if (columnOrdinal < 0 | columnOrdinal > this.Columns.Count)
            {
                log.Warn("New column ordinal was out of range");
                throw new InvalidOperationException("New column ordinal is invalid.");
            }
            if (Name == null || Name.Length > 15)
            {
                log.Warn("New column name was invalid (null or longer than 15 chars.)");
                throw new InvalidOperationException("New column name is invalid.");
            }
            if (!this.Columns.Exists(c => c.Name.Equals(Name)))
            {
                Column newColumn = new Column(Name, email, columnOrdinal);
                this.Columns.Insert(columnOrdinal, newColumn);
                this.DalCopyBoard.Columns.Insert(columnOrdinal, newColumn.DalCopyColumn);
                for (int i = columnOrdinal + 1; i < this.Columns.Count; i++) //increasing the ordinals of following DALColumns.
                    this.Columns[i].DalCopyColumn.Ordinal = this.Columns[i].DalCopyColumn.Ordinal + 1;
                log.Debug("A new column '" + Name + "' was added at the index '" + columnOrdinal + "'.");
                return newColumn;
            }
            else throw new InvalidOperationException("Column with this name ia already exists.");
        }

        public void RemoveColumn(string email, int columnOrdinal) //checked
        {
            if (columnOrdinal >= 0 & columnOrdinal < this.Columns.Count)
            {
                Column toRemove = this.Columns[columnOrdinal];

                //in case toRemove is the first column
                if (columnOrdinal == 0) 
                {
                    if (!this.Columns[columnOrdinal + 1].CheckLimit()) //right column limi check
                    {
                        log.Warn("Right column at index '" + (columnOrdinal + 1) + "' was full.");
                        throw new InvalidOperationException("Right column is full. Update '" + this.Columns[columnOrdinal + 1].Name + "' column limit and try again.");
                    }
                    else if ((this.Columns[columnOrdinal + 1].Tasks.Count + toRemove.Tasks.Count) > this.Columns[columnOrdinal + 1].Limit) //right column capacity check
                    {
                        log.Warn("Right column at index '" + (columnOrdinal + 1) + "' didn't have enough space.");
                        throw new InvalidOperationException("Right column doesn't have enough space. Update '" + this.Columns[columnOrdinal + 1].Name + "' column limit and try again.");
                    }
                    else //valid case
                    {
                        this.Columns.RemoveAt(columnOrdinal);
                        this.DalCopyBoard.Columns.RemoveAt(columnOrdinal);
                        foreach (Task t in toRemove.Tasks)
                        {
                            this.Columns[columnOrdinal + 1].Tasks.Add(t);
                            this.Columns[columnOrdinal + 1].DalCopyColumn.Tasks.Add(t.DalCopyTask);
                            t.DalCopyTask.ColumnName = this.Columns[columnOrdinal + 1].Name;
                        }
                        log.Debug("First column '" + toRemove.Name + "' was removed.");
                    }
                }

                //in case the column on the left reached its limit:
                else if (!this.Columns[columnOrdinal - 1].CheckLimit()) { 
                    log.Warn("Left column at index '" + (columnOrdinal - 1) + "' was full.");
                    throw new InvalidOperationException("Left column is full. Update '" + this.Columns[columnOrdinal-1].Name +"' column limit and try again.");
                }

                //in case the column on the left doesn't have enough space:
                else if ((this.Columns[columnOrdinal-1].Tasks.Count+toRemove.Tasks.Count)> this.Columns[columnOrdinal - 1].Limit) 
                {
                    log.Warn("Left column at index '" + (columnOrdinal - 1) + "' didn't have enough space.");
                    throw new InvalidOperationException("Left column doesn't have enough space. Update '" + this.Columns[columnOrdinal - 1].Name + "' column limit and try again.");
                }

                else //valid case
                {
                    this.Columns.RemoveAt(columnOrdinal);
                    this.DalCopyBoard.Columns.RemoveAt(columnOrdinal);
                    foreach (Task t in toRemove.Tasks)
                    {
                        this.Columns[columnOrdinal - 1].Tasks.Add(t);
                        this.Columns[columnOrdinal - 1].DalCopyColumn.Tasks.Add(t.DalCopyTask);
                        t.DalCopyTask.ColumnName = this.Columns[columnOrdinal - 1].Name;
                    }
                    log.Debug("Column '" + toRemove.Name + "' at index '" + columnOrdinal + "' was removed.");
                }
            }
            else throw new InvalidOperationException("Index of the removed column is invalid.");
        }

        public Column MoveColumnRight(string email, int columnOrdinal) //checked
        {
            if (columnOrdinal == (this.Columns.Count - 1)) //in case of the last column
            {
                log.Warn("Attempt to move right the last column.");
                throw new InvalidOperationException("The last column cannot be moved to its right.");
            }
            if (columnOrdinal < 0 | columnOrdinal > this.Columns.Count) //invalid index
            {
                log.Warn("Given column ordinal is invalid.");
                throw new IndexOutOfRangeException("Given column ordinal is invalid.");
            }

            Column toMove = this.Columns[columnOrdinal];
            this.Columns.RemoveAt(columnOrdinal); 
            this.Columns.Insert(columnOrdinal + 1, toMove);
            this.DalCopyBoard.Columns.RemoveAt(columnOrdinal);
            this.DalCopyBoard.Columns.Insert(columnOrdinal + 1, toMove.DalCopyColumn);
            log.Debug("Column '"+toMove.Name+"' has been moved to its right ("+(columnOrdinal+1)+") successfully.");
            return toMove;
        }

        public Column MoveColumnLeft(string email, int columnOrdinal) //checked
        {
            if (columnOrdinal == 0) //in case of the last column
            {
                log.Warn("Attempt to move left the first column.");
                throw new InvalidOperationException("The first column cannot be moved to its left.");
            }
            if (columnOrdinal < 0 | columnOrdinal > this.Columns.Count) //invalid index
            {
                log.Warn("Given column ordinal is invalid.");
                throw new IndexOutOfRangeException("Given column ordinal is invalid.");
            }

            Column toMove = this.Columns[columnOrdinal];
            this.Columns.RemoveAt(columnOrdinal);
            this.Columns.Insert(columnOrdinal - 1, toMove);
            this.DalCopyBoard.Columns.RemoveAt(columnOrdinal);
            this.DalCopyBoard.Columns.Insert(columnOrdinal - 1, toMove.DalCopyColumn);
            log.Debug("Column '" + toMove.Name + "' has been moved to its left (" + (columnOrdinal - 1) + ") successfully.");
            return toMove;
        }


    }
}
