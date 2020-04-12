using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class BoardService
    {
        private BusinessLayer.SecurityController _securityController;

        public BoardService(BusinessLayer.SecurityController sc)
        {
            _securityController = sc;
        }


        public Response<Board> GetBoard(string email) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response<Board>("Invailid current user.");
            List<string> tempColumnNames = _securityController.BoardController.GetBoard(email).getColumnNames();
            Board tempStructBoard = new Board(tempColumnNames);
            return new Response<Board>(tempStructBoard);
        }

        public Response LimitColumnTasks(string email, int columnOrdinal, int limit) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                _securityController.BoardController.LimitColumnTask(email, columnOrdinal, limit);
                return new Response("Column limit has been updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }

        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate) //in progress: verify BL.Task has getter of its CreationDate
        {
            if (!_securityController.UserValidation(email)) return new Response<Task>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Task tempTask = _securityController.BoardController.AddTask(email, title, description, dueDate);
                Task tempStructTask = new Task(tempTask.Id, tempTask.CreationTime, title, description, dueDate);
                return new Response<Task>(tempStructTask, "Task has been added successfully.");
            }
            catch (Exception ex)
            {
                return new Response<Task>(ex.Message);
            }
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                _securityController.BoardController.UpdateTaskDueDate(email, columnOrdinal, taskId, newDueDate);
                return new Response("Task due date has benn updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                _securityController.BoardController.UpdateTaskTitle(email, columnOrdinal, taskId, newTitle);
                return new Response("Task title has been updated successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) //done+++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                _securityController.BoardController.UpdateTaskDescription(email, columnOrdinal, taskId, newDescription);
                return new Response("Task description has been updated successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response AdvanceTask(string email, int columnOrdinal, int taskId) //done+++++++++++++++++++++++++++++
        {
            if (!_securityController.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                _securityController.BoardController.AdvanceTask(email, columnOrdinal, taskId);
                return new Response("Task has been advanced successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response<Column> GetColumn(string email, string columnName) //in progress--------------------------------------
        {
            if (!_securityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                //declaring BL column by receiving existing column from BL.BoardPackage
                BusinessLayer.BoardPackage.Column tempColumn = _securityController.BoardController.GetColumn(email, columnName);
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

                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                return new Response<Column>(ex.Message);
            }
        }

        public Response<Column> GetColumn(string email, int columnOrdinal) ////in progress--------------------------------------
        {
            //this method replicates GetColumn(string email, string columnName), with only difference of calling BL.BC.GetColumn() with columnOrdinal.
            if (!_securityController.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Column tempColumn = _securityController.BoardController.GetColumn(email, columnOrdinal);
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.Tasks;

                List<Task> structTaskList = new List<Task>();

                foreach (BusinessLayer.BoardPackage.Task tempTask in tempColumnTaskCollection)
                    structTaskList.Add(new Task(tempTask.Id, tempTask.CreationTime, tempTask.Title, tempTask.Description, tempTask.DueDate));

                IReadOnlyCollection<Task> tempReadOnlyStructTaskList = new ReadOnlyCollection<Task>(structTaskList);

                Column tempStructColumn = new Column(tempReadOnlyStructTaskList, tempColumn.Name, tempColumn.Limit);
                return new Response<Column>(tempStructColumn);
            }
            catch (Exception ex)
            {
                return new Response<Column>(ex.Message);
            }
        }
    }
}
