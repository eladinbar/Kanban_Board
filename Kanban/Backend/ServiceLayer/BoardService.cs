﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    ///The servicve for perfoming Board involved actions.
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
        /// Retrieves the current logged in user's kanban board, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board.</param>
        /// <returns>A Response<ServiceLayer.Board> object. The response should contain an error message in case of an error.</returns>
        public Response<Board> GetBoard(string email) 
        {
            if (!SecurityController.UserValidation(email))
            {
                Response<Board> resp = new Response<Board>("Invalid current user.");
                log.Error(resp.ErrorMessage);
                return resp;
            }
            try
            {
                List<string> tempColumnNames = SecurityController.BoardController.GetBoard(email).getColumnNames();
                Board tempStructBoard = new Board(tempColumnNames);
                log.Debug("Board reached the Service Layer successfully");
                return new Response<Board>(tempStructBoard);
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
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
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
                return new Response();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Adds a new task to the 'backlog' column.
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
                Task tempStructTask = new Task(tempTask.Id, tempTask.CreationTime,dueDate, title, tempTask.Description);
                log.Info("Task added successfully.");
                return new Response<Task>(tempStructTask);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Task>(ex.Message);
            }
        }



        /// <summary>
        /// Updates a requested task's due date.
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
                log.Info("Task due date was updated successfully.");
                return new Response();
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
                log.Info("Task title updated successfully.");
                return new Response();
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
        /// <param name="newDescription">New description (body) for the requested task.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) 
        {
            if (!SecurityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                SecurityController.BoardController.UpdateTaskDescription(email, columnOrdinal, taskId, newDescription);
                log.Info("Task description has been updated successfully.");
                return new Response();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Advances the requested task to the next column.
        /// Task can't be advanced further than the 'done' column.
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
                log.Info("Task has been advanced to column #" + (columnOrdinal+1));
                return new Response();
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response(ex.Message);
            }
        }



        /// <summary>
        /// Retrieves the current logged in user's requested column, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board with the requested column.</param>
        /// <param name="columnName">Requested column name.</param>
        /// <returns>A Response<ServiceLayer.Column> object. The response should contain an error message in case of an error.</returns>
        public Response<Column> GetColumn(string email, string columnName) 
        {
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                //Declaring BL column by receiving existing column from BL.BoardPackage
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnName);
                //Declaring List of BL.Tasks by receiving it from 'tempColumn'
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                //Declaring List of struct Tasks
                List<Task> structTaskList = new List<Task>();

                //Converting BL.Tasks of 'tempColumnTaskCollection' into struct Users and adding them to 'structTaskList'
                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.DueDate, tempTask.Title, tempTask.Description));

                //Declaring struct Column with ReadOnlyCollection of struct Tasks
                Column tempStructColumn = new Column(structTaskList, tempColumn.Name, tempColumn.Limit);

                log.Debug("Required column has reached the Service Layer");
                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }



        /// <summary>
        /// Retrieves the current logged in user's requested column, for performing required actions on it.
        /// </summary>
        /// <param name="email">User's email to receive its board with the requested column.</param>
        /// <param name="columnOrdinal">Requested column ordinal.</param>
        /// <returns>A Response<ServiceLayer.Column> object. The response should contain an error message in case of an error.</returns>
        public Response<Column> GetColumn(string email, int columnOrdinal) 
        {
            //This method replicates GetColumn(string email, string columnName), with the only difference being calling BL.BC.GetColumn() with columnOrdinal.
            if (!SecurityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Column tempColumn = SecurityController.BoardController.GetColumn(email, columnOrdinal);
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                List<Task> structTaskList = new List<Task>();

                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.DueDate, tempTask.Title, tempTask.Description));

                Column tempStructColumn = new Column(structTaskList, tempColumn.Name, tempColumn.Limit);

                log.Debug("Required column has reached the Service Layer");
                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                return new Response<Column>(ex.Message);
            }
        }
    }
}