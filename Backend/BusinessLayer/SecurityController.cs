using IntroSE.Kanban.Backend.BusinessLayer.BoardPackage;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    /// <summary>
    /// A security class that performs major user validation tasks.
    /// </summary>
    internal class SecurityController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public UserPackage.UserController UserController { get; }
        public BoardPackage.BoardController BoardController { get; }
        public UserPackage.User CurrentUser { get; internal set; }

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
        /// Removes all persistent data.
        /// </summary>
        public void DeleteData() {
            UserDalController UserDal = new UserDalController();
            UserDal.DeleteDatabase();
        }

        /// <summary>
        /// Calls UserController Login() method in case there is no other logged in user.
        /// </summary>
        /// <param name="email">User's email to Login with.</param>
        /// <param name="password">User's password to Login with.</param>
        /// <exception cref="AccessViolationException">Thrown if there is a user logged in already.</exception>
        /// <returns>A BussinesLayer.UserPackage.User object.</returns>
        public UserPackage.User Login(string email, string password) 
        {
            if (CurrentUser != null) throw new AccessViolationException("There is a user logged into the system already. Please Logout to switch to another user.");
            CurrentUser = UserController.Login(email, password);
            return CurrentUser;
        }

        /// <summary>
        /// Calls UserController Logout() method.
        /// </summary>
        /// <param name="email">Currently logged in user's email.</param>
        /// <exception cref="AccessViolationException">Thrown if there is no logged in user.
        /// Alternatively thrown if the 'email' parameter doesn't match the current logged in user's email.</exception>
        public void Logout(string email) 
        {
            if (CurrentUser == null) throw new AccessViolationException("There is no logged in user.");
            if (!CurrentUser.Email.Equals(email)) throw new AccessViolationException("Logout failed: user "+email+" is not logged in.");
            UserController.Logout(email);
            CurrentUser = null;
        }

        /// <summary>
        /// Performs current user validation.
        /// </summary>
        /// <param name="email">User email to validate.</param>
        /// <returns>Returns true if the user validation succeeded, otherwise returns false.</returns>
        public bool UserValidation(string email) 
        {
            if (CurrentUser == null) return false;
            return CurrentUser.AssociatedBoard.Equals(email);
        }

        public bool ValidateHost()
        {
            if (CurrentUser == null) return false;
            return CurrentUser.Email.Equals(CurrentUser.AssociatedBoard);
        }

        public void BoardExistence(string boardId)
        {
            if (!BoardController.BoardExistence(boardId))
                throw new ArgumentException($"{boardId} does not exist in the system, check validity");
        }
    }
}
