using System;
using System.Collections.Generic;
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

        public BusinessLayer.SecurityController SecurityController
        {
            get { return this.SecurityControl; }
        }

        public Response<Board> GetBoard(string email)
        {
            if (this.SecurityControl.UserValidation(email))
            {
                BusinessLayer.BoardPackage.BoardController boardController = this.SecurityControl.BoardController;
                
            }
            throw new NotImplementedException(); //waiting for BoardController columns dictionary
            
        }

        public Response LimitColumnTasks (string email, int columnOrdinal, int limit)
        {
            throw new NotImplementedException(); 
        }

        public Response<Task> AddTask(string email, string title, string description, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            throw new NotImplementedException();
        }

        public Response UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            throw new NotImplementedException();
        }

        public Response AdvanceTask(string email, int columnOrdinal, int taskId)
        {
            throw new NotImplementedException();
        }

        public Response<Column> GetColumn(string email, string columnName)
        {
            throw new NotImplementedException();
        }

        public Response<Column> GetColumn(string email, int columnOrdinal)
        {
            throw new NotImplementedException();
        }
    }
}
