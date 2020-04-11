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
        private BusinessLayer.SecurityController SecurityControl;

        public BoardService(BusinessLayer.SecurityController sc)
        {
            this.SecurityControl = sc;
        }


        public Response<Board> GetBoard(string email) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response<Board>("Invailid current user.");
            List<string> tempColumnNames = this.SecurityControl.BoardController.GetBoard(email).getColumnNames();
            Board tempStructBoard = new Board(tempColumnNames);
            return new Response<Board>(tempStructBoard);
        }

        public Response LimitColumnTasks(string email, int columnOrdinal, int limit) //done+++++++++++++++++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                this.SecurityControl.BoardController.LimitColumnTask(email, columnOrdinal, limit);
                return new Response("Column limit has been updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }

        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate) //in progress: verify BL.Task has getter of its CreationDate
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response<Task>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Task tempTask = this.SecurityControl.BoardController.AddTask(email, title, description, dueDate);
                Task tempStructTask = new Task(tempTask.Id, tempTask.CreationDate(), title, description, dueDate);
                return new Response<Task>(tempStructTask, "Task has been added successfully.");
            }
            catch (Exception ex)
            {
                return new Response<Task>(ex.Message);
            }
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                this.SecurityControl.BoardController.UpdateTaskDueDate(email, columnOrdinal, taskId, newDueDate);
                return new Response("Task due date has benn updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle) //done++++++++++++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                this.SecurityControl.BoardController.UpdateTaskTitle(email, columnOrdinal, taskId, newTitle);
                return new Response("Task title has been updated successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription) //done+++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                this.SecurityControl.BoardController.UpdateTaskDescription(email, columnOrdinal, taskId, newDescription);
                return new Response("Task description has been updated successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response AdvanceTask(string email, int columnOrdinal, int taskId) //done+++++++++++++++++++++++++++++
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response("Invalid current user.");
            try
            {
                this.SecurityControl.BoardController.AdvanceTask(email, columnOrdinal, taskId);
                return new Response("Task has been advanced successfully");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }

        public Response<Column> GetColumn(string email, string columnName) //in progress--------------------------------------
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
               BusinessLayer.BoardPackage.Column tempColumn = this.SecurityControl.BoardController.GetColumn(email, columnName);                
               List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.GetTasks;

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

        public Response<Column> GetColumn(string email, int columnOrdinal) ////in progress--------------------------------------
        {
            if (!this.SecurityControl.UserValidation(email)) return new Response<Column>("Invalid current user.");
            try
            {
                BusinessLayer.BoardPackage.Column tempColumn = this.SecurityControl.BoardController.GetColumn(email, columnOrdinal);
                List<BusinessLayer.BoardPackage.Task> tempColumnTaskCollection = tempColumn.GetTasks;

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
