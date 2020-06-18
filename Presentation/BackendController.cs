using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Modal;

namespace Presentation
{
    public class BackendController
    {
        public IService Service { get; set; }
        public BackendController()
        {
            Service = new Service();
        }


        internal UserModel Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, email, user.Value.Nickname);
        }


        internal Column GetColumn(string creatorEmail, string columnName)
        {
            Response<Column> column = Service.GetColumn(creatorEmail, columnName);
            if (column.ErrorOccured)
            {
                throw new Exception(column.ErrorMessage);
            }
            return column.Value;
        }


        internal Column GetColumn(string creatorEmail, int columnOrdinal)
        {
            Response<Column> column = Service.GetColumn(creatorEmail, 0);
            if (column.ErrorOccured)
            {
                throw new Exception(column.ErrorMessage);
            }
            return column.Value;
        }


        internal void ChangeColumnName(string creatorEmail, int columnOrdinal, string newName)
        {
            Response rsp = Service.ChangeColumnName(creatorEmail, columnOrdinal, newName);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }


        internal void AddColumn(string email, int newColumnOrdinal, string newColumnName)
        {
            Response rsp = Service.AddColumn(email, newColumnOrdinal, newColumnName);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }


        internal string Logout(string emailToLogout)
        {
            Response rsp = Service.Logout(emailToLogout);
            if (rsp.ErrorOccured)
            {
                return rsp.ErrorMessage;
            }
            return "User was logged out successfully.";
        }

        internal void LimitColumnTasks(string email, int columnOrdinal, int limit)
        {
            Response rsp = Service.LimitColumnTasks(email, columnOrdinal, limit);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        internal void MoveColumnLeft(string email, int columnOrdinal)
        {
            Response rsp = this.Service.MoveColumnLeft(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }


        internal void MoveColumnRight(string email, int columnOrdinal)
        {
            Response rsp = this.Service.MoveColumnRight(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }

        internal void RemoveColumn(string email, int columnOrdinal)
        {
            Response rsp = Service.RemoveColumn(email, columnOrdinal);
            if (rsp.ErrorOccured) throw new Exception(rsp.ErrorMessage);
        }


        internal Board GetBoard(string creatorEmail)
        {
            Response<Board> board = Service.GetBoard(creatorEmail);
            if (board.ErrorOccured)
            {
                throw new Exception(board.ErrorMessage);
            }
            return board.Value;
        }


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
        public TaskModel AddTask(string email, string title, string description, DateTime dueDate)
        {
            Response<IntroSE.Kanban.Backend.ServiceLayer.Task> res = Service.AddTask(email, title, description, dueDate);
            if (res.ErrorOccured)
                throw new Exception(res.ErrorMessage);
            TaskModel task = new TaskModel(this, res.Value.Id, res.Value.Title, res.Value.Description, res.Value.CreationTime, res.Value.DueDate,
                                            res.Value.CreationTime, res.Value.emailAssignee, 0);
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
