using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    /// <summary>
    /// The BoardController is the class that controls the functionality of the BoardPackage.
    /// Contains the methods for adding and modifying the content of a board.
    /// </summary>
    internal class BoardController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private Dictionary<String, Board> Boards;

        /// <summary>
        /// The board controller constructor. Initializes the 'Boards' field by loading all existing data from memory, if no data exists, creates an empty dictionary.
        /// </summary>
        public BoardController() //checked
        {
            BoardDalController boardDalC = new BoardDalController();
            Boards = new Dictionary<string, Board>();
            List<DalBoard> dalBoards = boardDalC.SelectAllBoards();
            foreach (DalBoard b in dalBoards)
            {
                List<Column> tempColumns = new List<Column>();
                foreach (DalColumn c in b.Columns)
                {
                    List<Task> tempTasks = new List<Task>();
                    foreach (DalTask t in c.Tasks)
                    {
                        tempTasks.Add(new Task(t.Title, t.Description, t.DueDate, t.TaskId, t.CreationDate, t.LastChangedDate, t));
                    }
                    tempColumns.Add(new Column(c.Name, c.Limit, tempTasks, c));
                }
                Boards.Add(b.Email, new Board(b.Email, b.TaskCounter, tempColumns, b));
            }

        }

            /// <summary>
            /// Gets the board associated with the given email.
            /// </summary>
            /// <param name="email">The user's email that the board is associated with.</param>
            /// <returns>The Board of the user with that email.</returns>
            /// <exception cref="ArgumentException.ArgumentException(string)">Thrown when the email given is not associated with any board.</exception>
            public Board GetBoard(string email) //checked
        {
            Board tempBoard;
            if (Boards.TryGetValue(email, out tempBoard))
                return tempBoard;
            else
                throw new ArgumentException("There are no boards associated with this email address");
        }

        /// <summary>
        /// Get the column in the board associated with the given email and its specified column name.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnName">The name of the column in the board.</param>
        /// <returns>Returns the column with the specified name in the board associated with the given email.</returns>
        public Column GetColumn(string email, string columnName) //checked
        {
            Board newBoard = GetBoard(email);
            return newBoard.GetColumn(columnName);
        }

        /// <summary>
        /// Get the column in the board associated with key email and its specified column ordinal.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <returns>Returns the column with the specified column ordinal in the board associated with the given email.</returns>
        public Column GetColumn(string email, int columnOrdinal) //checked
        {
            Board b = GetBoard(email);
            return b.GetColumn(columnOrdinal);
        }


        /// <summary>
        /// Limits the number of tasks in a given board's column.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="limit">>The maximum amount of tasks to be allowed in the given column.</param>
        public void LimitColumnTask(string email, int columnOrdinal, int limit) //checked
        {
            Board b = GetBoard(email);
            Column c = b.GetColumn(columnOrdinal);
            c.LimitColumnTasks(limit);

            //the "Save' method is executed in 'Column' class itself
        }

        /// <summary>
        /// Advances a task from the specified column to the next one.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if trying to advance from the 'done' column or if the next column is full.</exception>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        public void AdvanceTask(string email, int columnOrdinal, int taskId) //checked
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - " + taskId + " .");

            if (b.Columns.Count == (columnOrdinal+1))
            {
                log.Error("Attempt to advance a task from the current last column");
                throw new ArgumentOutOfRangeException("Cannot advance tasks from the last column.");
            }
            else if (!b.GetColumn(columnOrdinal + 1).CheckLimit())
            {
                log.Error("Attempt to advance a task to a full column");
                throw new ArgumentOutOfRangeException("The next column: '" + b.GetColumn(columnOrdinal+1).Name + "' is full. Please change the column limit and try again.");
            }
            else
            {
                //Bussiness layer runtime list update 
                Column c = b.GetColumn(columnOrdinal);
                Task toAdvance = c.RemoveTask(taskId);
                Column targetColumn = b.GetColumn(columnOrdinal + 1);
                targetColumn.InsertTask(toAdvance);

                //DataAccess layer runtime list update 
                toAdvance.DalCopyTask.ColumnName = targetColumn.Name;

                log.Debug("Task #" + taskId + "-" + toAdvance.Title + " was advanced");
            }
        }

        /// <summary>
        /// Adds a new task to 'backlog' column.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="title">The title the task will be added with.</param>
        /// <param name="description">The description the task will be added with.</param>
        /// <param name="dueDate">The due date the task will be added with.</param>
        /// <returns>Returns the new task that was created.</returns>
        /// <exception cref="Exception">Thrown if the 'backlog' column is full.</exception>
        public Task AddTask(string email, string title, string description, DateTime dueDate) //checked
        {
            Board b = GetBoard(email);
            Column c = GetColumn(email, 0);
            if (!c.CheckLimit())
            {
                log.Error("Attempt to add a task when first column is full");
                throw new Exception("The first column is full, please adjust the column limit accordingly and try again.");
            }

            Task newTask = new Task(title, description, dueDate, b.TaskCounter + 1, email, c.Name);
            newTask.Save(email, c.Name);
            b.TaskCounter = (b.TaskCounter + 1); 
            b.DalCopyBoard.TaskCounter = b.DalCopyBoard.TaskCounter + 1;
            c.InsertTask(newTask); //DAL.Column is updated via inner BP.Column method

            log.Debug("A new task was added to the first column.");
            return newTask;            
        }

        /// <summary>
        /// Updates the title of the task with the given ID.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <param name="newTitle">The new title to update the task with.</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle) //checked
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                toUpdate.UpdateTaskTitle(newTitle);
                //save method is a part of inner 'Task' update method

                log.Debug("Task #" + taskId + " title was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in the last column.");
                throw new InvalidOperationException("Tasks cannot be edited in the last column.");
            }
        }

        /// <summary>
        /// Updates the description of the task with the given ID.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <param name="newTitle">The new title to update the task with.</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) //checked
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                toUpdate.UpdateTaskDescription(newDescription);
                //save method is a part of inner 'Task' update method

                log.Debug("Task #" + taskId + " description was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in the last column.");
                throw new InvalidOperationException("Tasks cannot be edited in the last column.");
            }
        }

        /// <summary>
        /// Update the duedate of the task with taskId.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <param name="newTitle">The new title to update the task with.</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate) //checked
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                toUpdate.UpdateTaskDuedate(newDueDate);
                //save method is a part of inner 'Task' update method

                log.Debug("Task #" + taskId + " dueDate was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in the last column.");
                throw new InvalidOperationException("Tasks cannot be edited in the last column.");
            }
        }

        /// <summary>
        /// Adds a new board.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        public void AddNewBoard(string email) //checked
        {
            Board newBoard = new Board(email);
            Boards.Add(email, newBoard);
            //save method is a part of inner 'Board' update method
            newBoard.Save();
            AddColumn(email, 0, "backlog");
            AddColumn(email, 1, "in progress");
            AddColumn(email, 2, "done");

            log.Info("New board was added with key " + email);
        }


        /// <summary>
        /// Adds a new column at the demanded index (ordinal).
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal the column should be added at.</param>
        /// <param name="Name">New column name.</param>
        public Column AddColumn(string email, int columnOrdinal, string Name) //checked
        {
            Board b = GetBoard(email);
            Column newColumn = b.AddColumn(email, columnOrdinal, Name);
            newColumn.Save(email, columnOrdinal);
            return newColumn;
        }

        /// <summary>
        /// Remove a column at the demanded index (ordinal).
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal of the column to delete.</param>
        public void RemoveColumn(string email, int columnOrdinal) //checked
        {
            Board b = GetBoard(email);
            Column c = GetColumn(email, columnOrdinal);
            if (b.Columns.Count == 2)
            {
                log.Warn("Attempt to remopve a column from board (" + b.UserEmail + ") with 2 columns");
                throw new InvalidOperationException("The board has 2 columns. Can't remove another column.");
            }
            b.RemoveColumn(email, columnOrdinal);
            c.Delete();
        }

        /// <summary>
        /// Move a column at the demanded index (ordinal) to its right.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal of the column to move right.</param>
        public Column MoveColumnRight(string email, int columnOrdinal) //checked
        {
            Board b = GetBoard(email);
            return b.MoveColumnRight(email, columnOrdinal);

        }

        /// <summary>
        /// Move a column at the demanded index (ordinal) to its left.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal of the column to move left.</param>
        public Column MoveColumnLeft(string email, int columnOrdinal) //checked
        {
            Board b = GetBoard(email);
            return b.MoveColumnLeft(email, columnOrdinal);
        }


    }
}
