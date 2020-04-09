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

        }

        //public void Login ()

        public bool UserValidation(string email)
        {
            if (CurrentUse == null) return false;
            return CurrentUse.email.Equals(email);
        }
    }
}
