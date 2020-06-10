using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    public class BackendController
    {
        private IService Service { get; set; }
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
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public void AddTask(string email, string title, string description, DateTime dueDate)
        {
            string ErrorMessage = Service.AddTask(email, title, description, dueDate).ErrorMessage;
            if (ErrorMessage.Length>0)
                throw new Exception(ErrorMessage);
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
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate) {
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
