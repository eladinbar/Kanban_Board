using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using System;
using System.Linq;
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

        private const int MINIMAL_NUMBER_OF_COLUMNS = 2;

        private Dictionary<String, Board> Boards;

        /// <summary>
        /// The board controller constructor. Initializes the 'Boards' field by loading all existing data from memory, if no data exists, creates an empty dictionary.
        /// </summary>
        public BoardController() 
        {
            BoardDalController boardDalC = new BoardDalController();
            Boards = new Dictionary<string, Board>();
            List<DalBoard> dalBoards = boardDalC.SelectAllBoards();
            foreach (DalBoard b in dalBoards)
            {
                Dictionary<string, string> tampMembers = new UserDalController().SelectAllUsersOfBoard(b.Email)
                                                         .ToDictionary<DalUser, string, string>(x => x.Email, y => y.Nickname);
                List<Column> tempColumns = new List<Column>();
                foreach (DalColumn c in b.Columns)
                {
                    List<Task> tempTasks = new List<Task>();
                    foreach (DalTask t in c.Tasks)
                    {
                        tempTasks.Add(new Task(t.Title, t.Description, t.DueDate, t.TaskId, t.CreationDate, t.LastChangedDate, t.EmailAssignee, t));
                    }
                    tempColumns.Add(new Column(c.Name, c.Limit, tempTasks, c));
                }
                Boards.Add(b.Email, new Board(b.Email, b.TaskCounter, tempColumns, tampMembers, b));

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

            //field database updates are executed in the 'Column' class itself
        }

        /// <summary>
        /// Advances a task from the specified column to the next one.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">The column's number of the board's column list.</param>
        /// <param name="taskId">The ID of the task to advance.</param>
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if trying to advance from the 'done' column or if the next column is full.</exception>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        public void AdvanceTask(string email, int columnOrdinal, int taskId, string currentUserEmail) 
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
                //Business layer runtime list update 
                Column c = b.GetColumn(columnOrdinal);
                if (!c.GetTask(taskId).AssigneeCheck(currentUserEmail))
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");

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
        /// <param name="emailAssignee">The email to assign the task to.</param>
        /// <returns>Returns the new task that was created.</returns>
        /// <exception cref="Exception">Thrown if the 'backlog' column is full.</exception>
        public Task AddTask(string email, string title, string description, DateTime dueDate, string emailAssignee) 
        {
            Board b = GetBoard(email);
            Column c = GetColumn(email, 0);
            if (!c.CheckLimit())
            {
                log.Error("Attempt to add a task when first column is full");
                throw new Exception("The first column is full, please adjust the column limit accordingly and try again.");
            }
            if (!b.Members.ContainsKey(emailAssignee))
                throw new ArgumentException("email Assignee is not a member of the board");

            Task newTask = new Task(title, description, dueDate, b.TaskCounter + 1, email, c.Name, emailAssignee);
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
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle, string currentUserEmail) 
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                if (toUpdate.AssigneeCheck(currentUserEmail))
                    toUpdate.UpdateTaskTitle(newTitle);
                else
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");
               
                //field database updates are a part of the inner 'Task' functionality
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
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription, string currentUserEmail) 
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                if (toUpdate.AssigneeCheck(currentUserEmail))
                    toUpdate.UpdateTaskDescription(newDescription);
                else
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");
                
                //field database updates are a part of the inner 'Task' functionality
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
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        /// <exception cref="ArgumentException">Thrown in case task ID given is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if attempting to edit tasks in the 'done' column.</exception>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate, string currentUserEmail) 
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                if (toUpdate.AssigneeCheck(currentUserEmail))
                    toUpdate.UpdateTaskDuedate(newDueDate);
                else
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");
               
                //field database updates are a part of the inner 'Task' functionality
                log.Debug("Task #" + taskId + " dueDate was updated.");
            }
            else
            {
                log.Warn("Tasks cannot be edited in the last column.");
                throw new InvalidOperationException("Tasks cannot be edited in the last column.");
            }
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee, string currentUserEmail)
        {
            Board b = GetBoard(email);
            if (!b.Members.ContainsKey(emailAssignee))
                throw new ArgumentException($"{emailAssignee} is not a member of {email} board");
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toUpdate = c.GetTask(taskId);
                if (toUpdate.AssigneeCheck(currentUserEmail))
                    toUpdate.UpdateTaskAssignee(emailAssignee);
                else
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");

                //field database updates are a part of the inner 'Task' functionality
                log.Debug("Task #" + taskId + " Assignee was updated to " + emailAssignee + " .");
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
        public void AddNewBoard(string email, string nickName) 
        {
            Board newBoard = new Board(email);
            Boards.Add(email, newBoard);

            newBoard.Save();
            AddColumn(email, 0, "backlog");
            AddColumn(email, 1, "in progress");
            AddColumn(email, 2, "done");
            newBoard.AddMember(email, nickName);
            log.Info("New board was added with key " + email);
        }

        public void JoinBoard(string newMemberEmail, string newMemberNickname, string boardToJoinEmail)
        {
            
            if(Boards.TryGetValue(boardToJoinEmail, out Board tmpBoard))
            {
                log.Info("adding new member to board " + boardToJoinEmail);
                tmpBoard.AddMember(newMemberEmail, newMemberNickname);
            }
            else
            {
                throw new ArgumentException($"{boardToJoinEmail} does not exist, check if email entered correctly");
            }
        }


        /// <summary>
        /// Adds a new column at the demanded index (ordinal).
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal the column should be added at.</param>
        /// <param name="Name">New column name.</param>
        public Column AddColumn(string email, int columnOrdinal, string Name) 
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
        public void RemoveColumn(string email, int columnOrdinal) 
        {
            Board b = GetBoard(email);
            Column c = GetColumn(email, columnOrdinal);
            if (b.Columns.Count == MINIMAL_NUMBER_OF_COLUMNS)
            {
                log.Warn("Attempt to remopve a column from board (" + b.UserEmail + ") with 2 columns.");
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
        public Column MoveColumnRight(string email, int columnOrdinal) 
        {
            Board b = GetBoard(email);
            return b.MoveColumnRight(email, columnOrdinal);

        }

        /// <summary>
        /// Move a column at the demanded index (ordinal) to its left.
        /// </summary>
        /// <param name="email">The user's email that the board is associated with.</param>
        /// <param name="columnOrdinal">Ordinal of the column to move left.</param>
        public Column MoveColumnLeft(string email, int columnOrdinal) 
        {
            Board b = GetBoard(email);
            return b.MoveColumnLeft(email, columnOrdinal);
        }

        /// <summary>
        /// Change the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newName">The new name.</param>
        public void ChangeColumnName(string email, int columnOrdinal, string newName)
        {
            GetBoard(email).ChangeColumnName(columnOrdinal, newName);  
        }

        /// <summary>
        /// Chacks if exist a board associated with email.
        /// </summary>
        /// <param name="email">email associated with the baord.</param>
        /// <returns>Returns true if the board exists, otherwise returns false.</returns>
        public bool BoardExistence(string email)
        {
            return Boards.ContainsKey(email);
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>  
        /// <param name="currentUserEmail">The current login user for assignee valedation</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        internal void DeleteTask(string email, int columnOrdinal, int taskId, string currentUserEmail)
        {
            Board b = GetBoard(email);
            if (!b.TaskIdExistenceCheck(taskId))
                throw new ArgumentException("A task does not exist with the given task ID - + " + taskId + " .");

            if (b.Columns.Count != columnOrdinal + 1)
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toDelete = c.GetTask(taskId);
                if (toDelete.AssigneeCheck(currentUserEmail))
                    c.RemoveTask(taskId).Delete();
                else
                    throw new InvalidOperationException("Tasks can only be modified by its assignee. Current user is not the assigned to this task");

                //field database updates are a part of the inner 'Task' functionality
                log.Debug("Task #" + taskId + " was deleted");
            }
            else
            {
                log.Warn("Tasks cannot be edited in the last column.");
                throw new InvalidOperationException("Tasks cannot be edited in the last column.");
            }
        }
    }
}
