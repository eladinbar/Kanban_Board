using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    ///The servicve for perfoming Board-involved actions.
    /// </summary>
    internal class BoardService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private BusinessLayer.SecurityController SecurityController;

        /// <summary>
        /// Public constructor. 
        /// <param name="sc">Current SecurityController object.</param>
        /// </summary>
        public BoardService(BusinessLayer.SecurityController sc)
        {
            log.Debug("BoardService Created");
            SecurityController = sc;
        }


        /// <summary>
        /// Allows to reach current logged in user's kanban board, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board.</param>
        /// <returns>A Response<ServiceLayer.Board> object. The response should contain an error message in case of an error.</returns>
        public Response<Board> GetBoard(string email) 
        {
            if (!SecurityController.UserValidation(email))
            {
                Response<Board> resp = new Response<Board>("Invailid current user.");
                log.Error(resp.ErrorMessage);
                return resp;
            }
            try
            {
                List<string> tempColumnNames = SecurityController.BoardController.GetBoard(email).getColumnNames();
                Board tempStructBoard = new Board(tempColumnNames);
                log.Debug("Board reached service layer successfully");
                return new Response<Board>(tempStructBoard,"'" + email + "' board was loaded succeessfully.");
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                return new Response<Board>(ex.Message);
            }
        }



        /// <summary>
        /// Limits the maximum number of tasks in the requested column.
        /// </summary>
        /// <param name="email">The email of the user that the board belongs to.</param>
        /// <param name="columnOrdinal">The column number in the board.</param>
        /// <param name="limit">The number of maximum tasks that the column should hold.</param>
        /// <returns>A Response object. The response should contain a error message in case of an error.</returns>
        public Response LimitColumnTasks(string email, int columnOrdinal, int limit) 
        {
            if (!SecurityController.UserValidation(email))
            {
                Response resp = new Response("Invalid current user.");
                log.Error(resp.ErrorMessage);
                return resp;
            }
            try
            {
                SecurityController.BoardController.LimitColumnTask(email, columnOrdinal, limit);
                log.Info("Column limit has been updated successfully.");
                return new Response("Column limit has been updated successfully.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }

        }



        /// <summary>
        /// Adds a new task to the "Backlog" column.
        /// </summary>
        /// <param name="email">The email of the user that the task belongs to.</param>
        /// <param name="title">New task title.</param>
        /// <param name="description">New task description - body of the task.</param>
        /// <param name="dueDate">New task due date.</param>
        /// <returns>A Response<ServiceLayer.Task> object. The response should contain an error message in case of an error.</returns>
        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate) 
        {
            if (!SecurityController.UserValidation(email)) return new Response<Task>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Task tempTask = SecurityController.BoardController.AddTask(email, title, description, dueDate);
                Task tempStructTask = new Task(tempTask.Id, tempTask.CreationTime, title, description, dueDate);
                log.Info("Task added seccessfully.");
                return new Response<Task>(tempStructTask, "Task has been added successfully to '" + email + "' board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Task>(ex.Message);
            }
        }



        /// <summary>
        /// Updates a requested task due date.
        /// </summary>
        /// <param name="email">The email of the user that the task belongs to.</param>
        /// <param name="columnOrdinal">A number of the column the task belongs to.</param>
        /// <param name="taskId">A requested task ID.</param>
        /// <param name="newDueDate">New due date of the requested task.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate) 
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskDueDate(email, columnOrdinal, taskId, newDueDate);
                log.Info("Task due date was updated seccessfully.");
                return new Response("Task #" + taskId + " due date has benn updated successfullyin " + GetColumn(email, columnOrdinal).Value.Name + "of '" + email + "' board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Updates a requested task title.
        /// </summary>
        /// <param name="email">The email of the user that the task belongs to.</param>
        /// <param name="columnOrdinal">A number of the column the task belongs to.</param>
        /// <param name="taskId">A requested task ID.</param>
        /// <param name="newTitle">New title for the requested task.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle)
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskTitle(email, columnOrdinal, taskId, newTitle);
                log.Info("Task title updated seccessfully.");
                return new Response("Task #" + taskId + " title has been updated successfullyin " + GetColumn(email, columnOrdinal).Value.Name + "of '" + email + "' board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message,ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Updates a requested task description.
        /// </summary>
        /// <param name="email">The email of the user that the task belongs to.</param>
        /// <param name="columnOrdinal">A number of the column the task belongs to.</param>
        /// <param name="taskId">A requested task ID.</param>
        /// <param name="newDescription">New description(body) for the requested task.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) 
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskDescription(email, columnOrdinal, taskId, newDescription);
                log.Info("Task description has been updated seccessfully.");
                return new Response("Task #" + taskId + " description has been updated successfully in " + GetColumn(email, columnOrdinal).Value.Name + "of '" + email + "' board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Advances the requested task to the next column.
        /// Task can't be advanced further than the "Done" column.
        /// </summary>
        /// <param name="email">The email of the user that the task belongs to.</param>
        /// <param name="columnOrdinal">A number of the column the task belongs to.</param>
        /// <param name="taskId">A requested task ID.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response AdvanceTask(string email, int columnOrdinal, int taskId) 
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.AdvanceTask(email, columnOrdinal, taskId);
                log.Info("Task has been advanced to the column #" + columnOrdinal+1);
                return new Response("Task #" + taskId + " has been advanced successfully to " + GetColumn(email,columnOrdinal).Value.Name + ".");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Allows to reach current logged in user's requested column, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board with the requested column.</param>
        /// <param name="columnName">Requested column name.</param>
        /// <returns>A Response<ServiceLayer.Column> object. The response should contain an error message in case of an error.</returns>
        public Response<Column> GetColumn(string email, string columnName) 
        {
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                //declaring BL column by receiving existing column from BL.BoardPackage
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnName);
                //declaring List of BL.Tasks by receiving it from 'tempColumn'
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                //declaring List of struct Tasks
                List<Task> structTaskList = new List<Task>();

                //converting BL.Tasks of 'tempColumnTaskCollection' into struct Users and adding them to 'structTaskList'
                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.Title, tempTask.Description, tempTask.DueDate));

                //declaring ReadOnlyCollection by using its copying constructor with List of struct Users
                IReadOnlyCollection<Task> tempReadOnlyStructTaskList = new ReadOnlyCollection<Task>(structTaskList);

                //declaring struct Column with ReadOnlyCollection of struct Tasks
                Column tempStructColumn = new Column(tempReadOnlyStructTaskList, tempColumn.Name, tempColumn.Limit);

                log.Debug("Required column has reached the Service Layer");
                return new Response<Column>(tempStructColumn, tempColumn.Name + " of " + email + "'s board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }



        /// <summary>
        /// Allows to reach current logged in user's requested column, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board with the requested column.</param>
        /// <param name="columnOrdinal">Requested column ordinal.</param>
        /// <returns>A Response<ServiceLayer.Column> object. The response should contain an error message in case of an error.</returns>
        public Response<Column> GetColumn(string email, int columnOrdinal) 
        {
            //this method replicates GetColumn(string email, string columnName), with only difference of calling BL.BC.GetColumn() with columnOrdinal.
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnOrdinal);
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                List<Task> structTaskList = new List<Task>();

                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.Title, tempTask.Description, tempTask.DueDate));

                IReadOnlyCollection<Task> tempReadOnlyStructTaskList = new ReadOnlyCollection<Task>(structTaskList);

                Column tempStructColumn = new Column(tempReadOnlyStructTaskList, tempColumn.Name, tempColumn.Limit);
                log.Debug("Required column has reached the Service Layer");
                return new Response<Column>(tempStructColumn, tempColumn.Name + "with ordinal " + columnOrdinal + " of " + email + "'s board.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }
    }
}
