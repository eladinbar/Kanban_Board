using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class SecurityController
    {
        public UserPackage.UserController UserController { get; }
        public BoardPackage.BoardController BoardController { get; }
        private UserPackage.User CurrentUser;


        public SecurityController()
        {
            UserController = new UserPackage.UserController();
            BoardController = new BoardPackage.BoardController();
            CurrentUser = null;
        }


        public UserPackage.User Login(string email, string password) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUser != null) throw new AccessViolationException("There is already LoggedIn User. LogOut to switch for another User.");
            CurrentUser = UserController.Login(email, password);
            return CurrentUser;
        }


        public void Logout(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUser == null) throw new AccessViolationException("There is no logged in users.");
            if (!CurrentUser.Email.Equals(email)) throw new AccessViolationException("Logout failed: User "+email+" is not logged in.");
            UserController.Logout(email);
            CurrentUser = null;
        }

               

        public bool UserValidation(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (CurrentUser == null) return false;
            return CurrentUser.Email.Equals(email);
        }
    }
}
