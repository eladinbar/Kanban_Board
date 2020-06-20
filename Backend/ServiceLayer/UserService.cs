using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    /// The service for perfoming User related actions.
    /// </summary>
    internal class UserService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        internal BusinessLayer.SecurityController SecurityController;

        /// <summary>
        /// Public constructor. Initializes that User Service with the current SecurityController object.
        /// <param name="securityController">Current SecurityController object.</param>
        /// </summary>
        public UserService(BusinessLayer.SecurityController securityController)
        {
            SecurityController = securityController;
            log.Debug("UserService has been created.");
        }
      
        /// <summary>
        /// Registers a new user to the system. A new kanban board is created for it.
        /// </summary>
        /// <param name="email">New user's email for registration.</param>
        /// <param name="password">New user's proper password for registration.</param>
        /// <param name="nickname">New user's nickname for registration.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response Register(string email, string password, string nickname) 
        {
            try
            {
                SecurityController.UserController.Register(email, password, nickname);
                SecurityController.BoardController.AddNewBoard(email, nickname);
                
                Response r = new Response();
                log.Info("Registered with " + email);
                return r;
            }
            catch (Exception ex)
            {
                Response resp = new Response(ex.Message);
                log.Warn(ex.Message, ex);
                return resp;
            }
        }

        /// <summary>
        /// Registers a new user and joins the user to an existing board.
        /// </summary>
        /// <param name="email">The email address of the user to register</param>
        /// <param name="password">The password of the user to register</param>
        /// <param name="nickname">The nickname of the user to register</param>
        /// <param name="hostEmail">The email address of the host user which owns the board.</param>
        /// <returns>A response object. The response should contain a error message in case of an error<returns>
        public Response Register(string email, string password, string nickname, string hostEmail)
        {
            try
            {
                SecurityController.BoardExistence(hostEmail);
                SecurityController.UserController.Register(email, password, nickname, hostEmail);
                SecurityController.BoardController.JoinBoard(email, nickname, hostEmail);
                log.Info($"{email} register successfully and joined {hostEmail} Board");
                return new Response();
            }
            catch (Exception e)
            {
                log.Warn(e.Message, e);
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Performs a validated Login action.
        /// </summary>
        /// <param name="email">User's email to login with.</param>
        /// <param name="password">User's password to login with.</param>
        /// <returns>A Response<ServiceLayer.User> object. The response should contain an error message in case of an error.</returns>
        public Response<User> Login (string email, string password) 
        {
            try
            {
                BusinessLayer.UserPackage.User tempUser = SecurityController.Login(email, password);
                User tempStructUser = new User(tempUser.Email,tempUser.Nickname,tempUser.AssociatedBoard);
                Response<User> r = new Response<User>(tempStructUser);
                log.Info("Successful login action.");
                return r;
            }
            catch (Exception ex)
            {
                Response<User> resp = new Response<User>(ex.Message);
                log.Error(ex.Message, ex);
                return resp;
            }
        }

        /// <summary>
        /// Perfroms a Logout action.
        /// </summary>
        /// <param name="email">Currently logged in user's email.</param>
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response Logout(string email) 
        {
            try
            {
                SecurityController.Logout(email);
                
                Response r = new Response();
                log.Info(r.ErrorMessage);
                return r;               
            }
            catch(Exception ex)
            {
                Response r = new Response(ex.Message);
                log.Error(r.ErrorMessage, ex);
                return r;
            }
        }

        /// <summary>
        /// Allows to change the password of a user.
        /// </summary>
        /// <param name="email">An existing user's email.</param>
        /// <param name="oldPassword">An existing user's old password.</param>
        /// <param name="newPassword">An existing user's new password.</param>\
        /// <returns>A Response object. The response should contain an error message in case of an error.</returns>
        public Response ChangePassword (string email, string oldPassword, string newPassword) 
        {
            //This method doesn't perform user validation in the case of administrative purposes.
            try
            {
                SecurityController.UserController.ChangePassword(email, oldPassword, newPassword);
                Response resp = new Response();
                log.Info(resp.ErrorMessage);
                return resp;
            }
            catch (Exception ex)
            {
                Response resp = new Response(ex.Message);
                log.Error(resp.ErrorMessage,ex);
                return resp;
            }
        }
    }
}
