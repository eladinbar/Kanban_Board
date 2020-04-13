using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class SecurityController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private UserPackage.UserController UserControl;
        private BoardPackage.BoardController BoardControl;
        private UserPackage.User CurrentUse;


        public SecurityController()
        {
            UserControl = new UserPackage.UserController();
            BoardControl = new BoardPackage.BoardController();
            this.CurrentUse = null;
        }



        public UserPackage.UserController UserController 
        {
            get { return this.UserControl; }
        }



        public BoardPackage.BoardController BoardController
        {
            get { return this.BoardControl; }
        }



        public UserPackage.User Login(string email, string password) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUse != null) throw new AccessViolationException("There is already LoggedIn User. LogOut to switch for another User.");
            this.CurrentUse = UserControl.Login(email, password);
            return CurrentUse;
        }



        public bool UserValidation(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUse == null) return false;
            return CurrentUse.email.Equals(email);
        }
    }
}
