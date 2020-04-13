using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class SecurityController
    {
        private UserPackage.UserController _userController;
        private BoardPackage.BoardController _boardController;
        private UserPackage.User _currentUser;


        public SecurityController()
        {
            _userController = new UserPackage.UserController();
            _boardController = new BoardPackage.BoardController();
            _currentUser = null;
        }



        public UserPackage.UserController UserController 
        {
            get { return _userController; }
        }



        public BoardPackage.BoardController BoardController
        {
            get { return _boardController; }
        }



        public UserPackage.User Login(string email, string password) //done++++++++++++++++++++++++++++++++++++++
        {
            if (_currentUser != null) throw new AccessViolationException("There is already LoggedIn User. LogOut to switch for another User.");
            _currentUser = _userController.Login(email, password);
            return _currentUser;
        }


        public void Logout(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (_currentUser == null) throw new AccessViolationException("There is no logged in users.");
            if (!_currentUser.email.Equals(email)) throw new AccessViolationException("Logout failed: User "+email+" is not logged in.");
            _userController.Logout(email);
            _currentUser = null;
        }





        public bool UserValidation(string email) //done++++++++++++++++++++++++++++++++++++++
        {
            if (_currentUser == null) return false;
            return _currentUser.email.Equals(email);
        }
    }
}
