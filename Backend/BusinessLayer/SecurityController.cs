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

        //public void Login () - receives CurrentUser from UserController whilst Login() method is callled in ServiceLayer
        public UserPackage.User Login(string email, string password)
        {
            if (CurrentUse != null) throw new AccessViolationException("There is already LoggedIn User. LogOut to switch for another User.");
            this.CurrentUse = UserControl.Login(email, password); //should receive User from UserController.Login();
            return CurrentUse;
        }

        public bool UserValidation(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUse == null) return false;
            return CurrentUse.email.Equals(email);
        }
    }
}
