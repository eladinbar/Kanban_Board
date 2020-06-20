using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    /// <summary>
    /// Controller that Presentation layer uses to communicate with the Backend.
    /// </summary>
    public class BackendController
    {
        public IService Service { get; set; }

        /// <summary>
        /// Controller construcor. Initializes Service.
        /// </summary>
        public BackendController()
        {
            Service = new Service();
        }


        /// <summary>
        /// Allows to login into the system.
        /// </summary>
        /// <param name="email">Current user email.</param>
        /// <param name="password">Current user password.</param>
        /// <returns>UserModel object with loge in user parameters.</returns>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal UserModel Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email, user.Value.Nickname, user.Value.AssociatedBoard);
        }

        /// <summary>
        /// Allows to receive an Service.Response with demanded Column by its name.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <param name="columnName">Demanded column name.</param>
        /// <returns>Service.Column object with demanded column parameters.</returns>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal Column GetColumn(string creatorEmail, string columnName)
        {
            Response<Column> column = Service.GetColumn(creatorEmail, columnName);
            if (column.ErrorOccured)
            {
                throw new Exception(column.ErrorMessage);
            }
            return column.Value;
        }

        /// <summary>
        /// Allows to receive an Service.Response with demanded Column by its ordinal.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <param name="columnOrdinal">Demanded column ordinal.</param>
        /// <returns>Service.Column object with all the demanded column parameters.</returns>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal Column GetColumn(string creatorEmail, int columnOrdinal)
        {
            Response<Column> column = Service.GetColumn(creatorEmail, 0);
            if (column.ErrorOccured)
            {
                throw new Exception(column.ErrorMessage);
            }
            return column.Value;
        }

        /// <summary>
        /// Allows to change the selected column name.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <param name="columnOrdinal">Demanded column ordinal.</param>
        /// <param name="newName">The new column name.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void ChangeColumnName(string creatorEmail, int columnOrdinal, string newName)
        {
            Response rsp = Service.ChangeColumnName(creatorEmail, columnOrdinal, newName);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to add new column at the demanded ordinal.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="newColumnOrdinal">The ordinal of the new column.</param>
        /// <param name="newColumnName">The name of the new column.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void AddColumn(string email, int newColumnOrdinal, string newColumnName)
        {
            Response rsp = Service.AddColumn(email, newColumnOrdinal, newColumnName);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to perform a logout action of the current user.
        /// </summary>
        /// <param name="emailToLogout">Email of the current user to logout.</param>
        /// <returns>Appropriate message of the logout action.</returns>
        internal string Logout(string emailToLogout)
        {
            Response rsp = Service.Logout(emailToLogout);
            if (rsp.ErrorOccured)
            {
                return rsp.ErrorMessage;
            }
            return "User was logged out successfully.";
        }

        /// <summary>
        /// Allows to change the selected column limit.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Ordinal of the column to change its limit.</param>
        /// <param name="limit">The new limit.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response rsp = Service.LimitColumnTasks(email, columnOrdinal, limit);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to move the selected column to its left.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Ordinal of the column to move.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void MoveColumnLeft(string email, int columnOrdinal)
        {
            Response rsp = this.Service.MoveColumnLeft(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to move the selected column to its right.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Ordinal of the column to move.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void MoveColumnRight(string email, int columnOrdinal)
        {
            Response rsp = this.Service.MoveColumnRight(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to remove the selected column from the Data Base.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Ordinal of the column to remove.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void RemoveColumn(string email, int columnOrdinal)
        {
            Response rsp = Service.RemoveColumn(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to remove the selected task from the Data Base.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <param name="columnOrdinal">Ordinal of the column that contains the task.</param>
        /// <param name="taskId">The id of the task to remove.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void RemoveTask(string creatorEmail, int columnOrdinal, int taskId)
        {
            Response rsp =  this.Service.DeleteTask(creatorEmail, columnOrdinal, taskId);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        /// <summary>
        /// Allows to receive the demanded board from the Data Base.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <returns>Service.Board object with all the demanded board parameters.</returns>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal Board GetBoard(string creatorEmail)
        {
            Response<Board> board = Service.GetBoard(creatorEmail);
            if (board.ErrorOccured)
            {
                throw new Exception(board.ErrorMessage);
            }
            return board.Value;
        }

        /// <summary>
        /// Allows to register a new user to the system.
        /// </summary>
        /// <param name="email">New user email.</param>
        /// <param name="password">New user password.</param>
        /// <param name="nickname">New user nickname.</param>
        /// <param name="hostEmail">Email of the board the new user is associated with.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void Register(string email, string password, string nickname, string hostEmail)
        {
            Response r;
            if (hostEmail.Equals(String.Empty))
                r = Service.Register(email, password, nickname);
            else
                r = Service.Register(email, password, nickname, hostEmail);

            if (r.ErrorOccured)
            {
                throw new Exception(r.ErrorMessage);
            }
        }

        /// <summary>
        /// Allows to advance the selected task.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">The ordinal of the column that contains the task.</param>
        /// <param name="taskId">Id of the task to advance.</param>
        /// <exception cref="Exception">Thrown when an error in Backend logic appears.</exception>
        internal void AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            Response r = this.Service.AdvanceTask(email, columnOrdinal, taskId);
            if (r.ErrorOccured) throw new Exception(r.ErrorMessage);
        }


        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public TaskModel AddTask(string email, string title, string description, DateTime dueDate, UserModel currentUser)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> res = Service.AddTask(email, title, description, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            TaskModel task = new TaskModel(this, res.Value.Id, res.Value.Title, res.Value.Description, res.Value.CreationTime, res.Value.DueDate,
                                            res.Value.CreationTime, res.Value.emailAssignee, 0, currentUser);
            return task;
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Response res = Service.UpdateTaskTitle(email, columnOrdinal, taskId, title);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            Response res = Service.UpdateTaskDescription(email, columnOrdinal, taskId, description);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res = Service.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = Service.AssignTask(email, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
        }
    }

}
