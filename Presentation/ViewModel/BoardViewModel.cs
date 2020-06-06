using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using System.Collections.ObjectModel;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        private BackendController Controller;
        private UserModel CurrentUser;
        private BoardModel Board;

        public string CurrentUserEmail() { return CurrentUser.Email; }

        public string BoardCreatorEmail() { return Board.CreatorEmail; }

        public string CurrentUserNickname() { return CurrentUser.Nickname; }



        public BoardViewModel(BackendController controller, UserModel currentUser, string creatorEmail)
        {
            this.Controller = controller;
            this.CurrentUser = currentUser;
            this.Board = new BoardModel(controller, creatorEmail);
        }



    }
}
