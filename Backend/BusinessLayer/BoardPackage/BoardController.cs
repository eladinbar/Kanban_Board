using IntroSE.Kanban.Backend.DataAccessLayer;
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
        public BoardController()
        {
            DalController dalC = new DalController();
            Boards = new Dictionary<string, Board>();
            List<DataAccessLayer.Board> DALboards = dalC.LoadAllBoards();
            foreach (DataAccessLayer.Board DALboard in DALboards) {
                List<Column> columns = new List<Column>();
                foreach (DataAccessLayer.Column DALcolumn in DALboard.Columns) {
                    List<Task> tasks = new List<Task>();
                    foreach (DataAccessLayer.Task DALtask in DALcolumn.Tasks) {
                        tasks.Add(new Task(DALtask.Title, DALtask.Description, DALtask.DueDate, DALtask.Id, DALtask.CreationTime, DALtask.LastChangedDate));
                    }
                    columns.Add(new Column(DALcolumn.Name, DALcolumn.Limit, tasks));
                }
                Boards.Add(DALboard.UserEmail, new Board(DALboard.UserEmail, DALboard.TaskCounter, columns));
            }
        }
         
        /// <summary>
        /// Gets the board associated with the given email.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <returns>The Board of the user with that email.</returns>
        /// <exception cref="ArgumentException.ArgumentException(string)">Thrown when the email given is not associated with any board.</exception>
        public Board GetBoard(string email)
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
        public Column GetColumn(string email, string columnName)
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
        public Column GetColumn(string email, int columnOrdinal)
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
        public void LimitColumnTask(string email, int columnOrdinal, int limit)
        {
            Board b = GetBoard(email);
            Column c = b.GetColumn(columnOrdinal);
            c.LimitColumnTasks(limit);
            c.Save("Boards\\" + email + "\\" + columnOrdinal + "-");
        }

        /// <summary>
        /// Advances a task from the specified column to the next one.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if trying to advance from the 'done' column or if the next column is full.</exception>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        public void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.GetColumn(columnOrdinal).Name.Equals("done"))
            {
                log.Error("Attempt to advance a task from the 'done' column");
                throw new ArgumentOutOfRangeException("Cannot advance tasks from the 'done' column.");
            }
            else if (!b.GetColumn(columnOrdinal + 1).CheckLimit())
            {
                log.Error("Attempt to advance a task to a full column");
                throw new ArgumentOutOfRangeException("The next column: '" + b.GetColumn(columnOrdinal+1).Name + "' is full. Please change the column limit and try again.");
            }
            else
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toAdvance = c.RemoveTask(taskId);
                Column targetColumn = b.GetColumn(columnOrdinal + 1);
                targetColumn.InsertTask(toAdvance);
                toAdvance.Save("Boards\\" + email + "\\" + (columnOrdinal+1) +"-" + targetColumn.Name + "\\");
                toAdvance.Delete(toAdvance.Id + "-" + toAdvance.Title, "Boards\\" + email + "\\" + columnOrdinal + "-" + c.Name + "\\");
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
        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            Board b = GetBoard(email);
            Column c = GetColumn(email, 0);
            if (!c.CheckLimit())
            {
                log.Error("Attempt to add a task when 'backlog' is full");
                throw new Exception("The 'backlog' column is full, please delete tasks or adjust the column limit accordingly and try again.");
            }
            b.TaskCounter = GetBoard(email).TaskCounter + 1;
            Task newTask = new Task(title, description, dueDate, b.TaskCounter);
            
            c.InsertTask(newTask);
            newTask.Save("Boards\\" + email + "\\" + "0-" + c.Name + "\\");
            log.Debug("A new task was added to the 'backlog' column.");

            b.Save("Boards\\");

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
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle)
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            Column c = GetColumn(email, columnOrdinal);
            if (!c.Name.Equals("done"))
            {
                Task toUpdate = c.GetTask(taskId);
                toUpdate.Delete(toUpdate.Id + "-" + toUpdate.Title, "Boards\\" + email + "\\" + columnOrdinal + "-" + c.Name + "\\");
                toUpdate.UpdateTaskTitle(newTitle);
                toUpdate.Save("Boards\\" + email + "\\" + columnOrdinal + "-" + c.Name + "\\");
                log.Debug("Task #" + taskId + " title was updated.");
            }
            else {
                log.Warn("Tasks cannot be edited in 'done' column.");
                throw new InvalidOperationException("Tasks cannot be edited in the 'done' column.");
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
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription)
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            Column c = GetColumn(email, columnOrdinal);
            if (!c.Name.Equals("done"))
            {
                Task toUpdate = c.GetTask(taskId);
                toUpdate.UpdateTaskDescription(newDescription);
                toUpdate.Save("Boards\\" + email + "\\" + columnOrdinal + "-" + c.Name + "\\");
                log.Debug("Task #" + taskId + " description was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in 'done' column.");
                throw new InvalidOperationException("Tasks cannot be edited in the 'done' column.");
            }
        }

        /// <summary>
        /// update the duedate of the task with taskId 
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <param name="newTitle">The new title to update the task with.</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            Column c = GetColumn(email, columnOrdinal);
            if (!c.Name.Equals("done"))
            {
                Task toUpdate = c.GetTask(taskId);
                toUpdate.UpdateTaskDuedate(newDueDate);
                toUpdate.Save("Boards\\" + email + "\\" + columnOrdinal + "-" + c.Name + "\\");
                log.Debug("Task #" + taskId + " dueDate was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in 'done' column.");
                throw new InvalidOperationException("Tasks cannot be edited in the 'done' column.");
            }
        }

        /// <summary>
        /// Adds a new board to the Boards dictionary.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        public void AddNewBoard(string email)
        {
            Board newBoard = new Board(email);
            Boards.Add(email, newBoard);
            newBoard.Save("Boards\\");
            log.Info("New board was added with kay " + email);
        }
    }
}
