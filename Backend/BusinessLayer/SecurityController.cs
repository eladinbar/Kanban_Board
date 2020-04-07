using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class SecurityController
    {
        private UserPackage.UserController UserControl;
        private BoardPackage.BoardController BoardControl;
        private UserPackage.User CurrentUse;

        public SecurityController()
        {
            UserControl = new UserPackage.UserController();
            BoardControl = new BoardPackage.BoardController();
            CurrentUser = null;
        }

        public UserPackage.UserController UserController 
        {
            get { return UserControl; }
        }

        public BoardPackage.BoardController BoardController
        {
            get { return BoardControl; }
        }

        public UserPackage.User CurrentUser
        {
            get { return CurrentUse; }
            set { CurrentUse = value; }

        }

        public bool UserValidation (string email)
        {
            return false; // change to: return CurrentUse.Email().Equals(email); after class User is created in BL.
        }
    }
}
