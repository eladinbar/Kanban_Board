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
            get { return this.UserControl; }
        }

        public BoardPackage.BoardController BoardController
        {
            get { return this.BoardControl; }
        }

        public UserPackage.User CurrentUser
        {
            get { return this.CurrentUse; }
            set { this.CurrentUse = value; }

        }

        public bool UserValidation (string email)
        {
            throw new NotImplementedException("this method still isn't implemented"); //return CurrentUse.Email().Equals(email); (after class User is created in BL).
        }
    }
}
