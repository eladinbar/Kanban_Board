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

        private const int MAXIMUM_COLUMN_NAME_LENGTH = 15;
        private const int MINIMUM_COLUMN_NAME_LENGTH = 0;

        public List<Column> Columns { get; }
        public string UserEmail { get; }
        public int TaskCounter { get; set; }
        public DalBoard DalCopyBoard { get; private set; }


        /// <summary>
        /// A public constructor that creates a new board and initializes all of its fields.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        public Board(string email) 
        {
            UserEmail = email;
            TaskCounter = 0;
            Columns = new List<Column>();
            log.Info("New board created");
        }

        /// <summary>
        /// An internal constructor that initializes all of the required fields upon loading an existing board from memory.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="taskCounter">The amount of tasks that the board contains.</param>
        /// <param name="columns">The list of columns the board contains.</param>
        /// <param name="dalBoard">The DAL appearance of the current board.</param>
        internal Board(string email, int taskCounter, List<Column> columns, DalBoard dalBoard) 
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
            DalCopyBoard = dalBoard;
            log.Info("load - Board " + email + " was loaded from memory");
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
        /// Adds a column at the demanded index (ordinal).
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="columnOrdinal">The index of the column to add at.</param>
        /// <param name="Name">The name of the new column.</param>
        /// <returns>Returns the added column.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the ordinal given is not in the valid range.</exception>
        public Column AddColumn(string email, int columnOrdinal, string Name) 
        {
            if (columnOrdinal < 0 | columnOrdinal > this.Columns.Count)
            {
                log.Warn("New column ordinal was out of range.");
                throw new InvalidOperationException("New column ordinal is invalid.");
            }
            if (Name.Length > 15 | Name.Length == MINIMUM_COLUMN_NAME_LENGTH)
            {
                log.Warn("New column name was invalid (null or longer than 15 characters).");
                throw new InvalidOperationException("New column name is invalid.");
            }
            if (!this.Columns.Exists(c => c.Name.Equals(Name)))
            {
                Column newColumn = new Column(Name, email, columnOrdinal);
                if (columnOrdinal == this.Columns.Count) //in case of adding to the end 
                {
                    this.Columns.Add(newColumn);
                }
                else
                {
                    this.Columns.Insert(columnOrdinal, newColumn);
                    for (int i = columnOrdinal + 1; i < this.Columns.Count; i++) //increasing the ordinals of following DALColumns
                        this.Columns[i].DalCopyColumn.Ordinal = this.Columns[i].DalCopyColumn.Ordinal + 1;
                }
                
                log.Debug("A new column '" + Name + "' was added at the index '" + columnOrdinal + "'.");
                return newColumn;
            }
            else throw new InvalidOperationException("A column with this name already exists.");
        }


        /// <summary>
        /// Adds a column at the demanded index (ordinal).
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="columnOrdinal">The index of the column to remove.</param>
        /// <exception cref="InvalidOperationException">Thrown when the ordinal given is not in the valid range.</exception>
        public void RemoveColumn(string email, int columnOrdinal) 
        {
            if (columnOrdinal >= 0 & columnOrdinal < this.Columns.Count)
            {
                Column toRemove = this.Columns[columnOrdinal];

                //in case toRemove is the first column
                if (columnOrdinal == 0) 
                {
                    if (!this.Columns[columnOrdinal + 1].CheckLimit()) //right column limit check
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
                        foreach (Task t in toRemove.Tasks)
                        {
                            this.Columns[columnOrdinal].Tasks.Add(t);
                            t.DalCopyTask.ColumnName = this.Columns[columnOrdinal].Name;
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
                    foreach (Task t in toRemove.Tasks)
                    {
                        this.Columns[columnOrdinal - 1].Tasks.Add(t);
                        t.DalCopyTask.ColumnName = this.Columns[columnOrdinal - 1].Name;
                    }
                    log.Debug("Column '" + toRemove.Name + "' at index '" + columnOrdinal + "' was removed.");
                }
                for (int i = columnOrdinal; i < this.Columns.Count; i++) //updating the DAL.Columns ordinals 
                    this.Columns[i].DalCopyColumn.Ordinal = this.Columns[i].DalCopyColumn.Ordinal - 1;
            }
            else throw new InvalidOperationException("Index to remove column from is invalid.");
        }


        /// <summary>
        /// Move a column at the demanded index (ordinal) to its right.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="columnOrdinal">The index of the column to move.</param>
        /// <exception cref="InvalidOperationException">Thrown when the ordinal given is not in the valid range.</exception>
        public Column MoveColumnRight(string email, int columnOrdinal) 
        {
            if (columnOrdinal == (this.Columns.Count - 1)) //in case of the last column
            {
                log.Warn("Attempt to move the last column right.");
                throw new InvalidOperationException("The last column cannot be moved to its right.");
            }
            if (columnOrdinal < 0 | columnOrdinal >= this.Columns.Count) //invalid index
            {
                log.Warn("Given column ordinal is invalid.");
                throw new IndexOutOfRangeException("Given column ordinal is invalid.");
            }

            Column toMove = this.Columns[columnOrdinal];
            this.Columns.RemoveAt(columnOrdinal); 
            this.Columns.Insert(columnOrdinal + 1, toMove);

            //updating DAL.Columns ordinals
            toMove.DalCopyColumn.Ordinal = toMove.DalCopyColumn.Ordinal + 1;
            this.Columns[columnOrdinal].DalCopyColumn.Ordinal = this.Columns[columnOrdinal].DalCopyColumn.Ordinal - 1;

            log.Debug("Column '" + toMove.Name + "' has been moved to its right (" +(columnOrdinal + 1)+ ") successfully.");
            return toMove;
        }

        /// <summary>
        /// Move a column at the demanded index (ordinal) to its left.
        /// </summary>
        /// <param name="email">The email of the board's user.</param>
        /// <param name="columnOrdinal">The index of the column to move.</param>
        /// <exception cref="InvalidOperationException">Thrown when the ordinal given is not in the valid range.</exception>
        public Column MoveColumnLeft(string email, int columnOrdinal) 
        {
            if (columnOrdinal == 0) //in case of the first column
            {
                log.Warn("Attempt to move the first column left.");
                throw new InvalidOperationException("The first column cannot be moved to its left.");
            }
            if (columnOrdinal < 0 | columnOrdinal >= this.Columns.Count) //invalid index
            {
                log.Warn("Given column ordinal is invalid.");
                throw new IndexOutOfRangeException("Given column ordinal is invalid.");
            }

            Column toMove = this.Columns[columnOrdinal];
            this.Columns.RemoveAt(columnOrdinal);
            this.Columns.Insert(columnOrdinal - 1, toMove);

            //updating DAL.Columns ordinals
            toMove.DalCopyColumn.Ordinal = toMove.DalCopyColumn.Ordinal - 1;
            this.Columns[columnOrdinal].DalCopyColumn.Ordinal = this.Columns[columnOrdinal].DalCopyColumn.Ordinal + 1;

            log.Debug("Column '" + toMove.Name + "' has been moved to its left (" + (columnOrdinal - 1) + ") successfully.");
            return toMove;
        }

        /// <summary>
        /// Transforms the board to its data access layer variant.
        /// </summary>
        /// <returns>return a DalBoard with all necessary elements to be persisted.</returns>
        internal DalBoard ToDalObject()
        {
            DalCopyBoard = new DalBoard(UserEmail, TaskCounter);
            return DalCopyBoard;
        }

        /// <summary>
        /// The method in the BusinessLayer to save a board to the database.
        /// </summary>
        internal void Save()
        {
            ToDalObject();
            DalCopyBoard.Save();
        }
    }
}
