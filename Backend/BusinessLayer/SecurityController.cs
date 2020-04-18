using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    ///Security class that performs major user validation tasks.
    /// </summary>
    class SecurityController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public UserPackage.UserController UserController { get; }
        public BoardPackage.BoardController BoardController { get; }
        private UserPackage.User CurrentUser;

        /// <summary>
        /// Public constructor. Initializes UserController and BoardController classes.
        /// </summary>
        public SecurityController()
        {
            UserController = new UserPackage.UserController();
            BoardController = new BoardPackage.BoardController();
            CurrentUser = null;
        }

        /// <summary>
        /// Calls UserController Login() method in case there is no other logged in user.
        /// </summary>
        /// <param name="email">User's email to Login with.</param>
        /// <param name="password">User's password to Login with.</param>
        /// <exception cref="AccessViolationException">Thrown if there is already logged in user.</exception>
        /// <returns>A BussinesLayer.UserPackage.User object.</returns>
        public UserPackage.User Login(string email, string password) 
        {
            if (CurrentUser != null) throw new AccessViolationException("There is already logged in User. LogOut to switch for another User.");
            CurrentUser = UserController.Login(email, password);
            return CurrentUser;
        }

        /// <summary>
        /// Calls UserController Logout() method.
        /// </summary>
        /// <param name="email">Currently logged in user's email.</param>
        /// <exception cref="AccessViolationException">Thrown if there is no logged in user.
        /// Alternatively thrown if "email" parameter doesn't match current logged in user's email.</exception>
        public void Logout(string email) 
        {
            if (CurrentUser == null) throw new AccessViolationException("There is no logged in Users.");
            if (!CurrentUser.Email.Equals(email)) throw new AccessViolationException("Logout failed: User "+email+" is not logged in.");
            UserController.Logout(email);
            CurrentUser = null;
        }


        /// <summary>
        /// Performs current user validation.
        /// </summary>
        /// <param name="email">User email to validate.</param>
        /// <returns>Boolean: true - validation succeeded, false - validation unsucceeded.</returns>
        public bool UserValidation(string email) 
        {
            if (CurrentUser == null) return false;
            return CurrentUser.Email.Equals(email);
        }
    }
}
