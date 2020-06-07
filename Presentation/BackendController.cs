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

        internal Board GetBoard(string creatorEmail)
        {
            Response<Board> board = Service.GetBoard(creatorEmail);
            if (board.ErrorOccured)
            {
                throw new Exception(board.ErrorMessage);
            }
            return board.Value;
        }
    }
}
