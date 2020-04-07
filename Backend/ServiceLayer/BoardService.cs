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

        public BoardService()
        {
            this.SecurityControl = new BusinessLayer.SecurityController();
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
    }
}
