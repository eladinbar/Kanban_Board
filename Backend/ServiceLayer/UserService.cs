﻿using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    /// <summary>
    ///The servicve for perfoming User-account actions.
    /// </summary>
    internal class UserService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private BusinessLayer.SecurityController _securityController;

        /// <summary>
        /// Public constructor. 
        /// <param name="securityController">Current SecurityController object.</param>
        /// </summary>
        public UserService(BusinessLayer.SecurityController securityController)
        {
            _securityController = securityController;
            log.Debug("UserService has been created.");
        }

        /// <summary>
        /// Security Controller getter. 
        /// </summary>
        /// <returns>Current SecurityController initialization.</returns>
        private BusinessLayer.SecurityController SecurityController
        {
            get
            {
                log.Debug("SecurityController getter was called.");
                return _securityController;
            }
        }

        /// <summary>
        /// Registers a new user to the system. A new kanban board is created for it.
        /// </summary>
        /// <param name="email">New user's email for registration.</param>
        /// <param name="password">New user's proper password for registration.</param>
        /// <param name="nickname">New user's nickname for registration.</param>
        /// <returns>A Response object. The response should contain a error message in case of an error.</returns>
        public Response Register(string email, string password, string nickname) 
        {
            try
            {
                _securityController.UserController.Register(email, password, nickname);
                _securityController.BoardController.AddNewBoard(email);
                
                Response r = new Response("User "+email+" has been registered successfully.");
                log.Info(r.ErrorMessage);
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
        /// Performs a validated Login action.
        /// </summary>
        /// <param name="email">User's email to login with.</param>
        /// <param name="password">User's password to login with.</param>
        /// <returns>A Response<ServiceLayer.User> object. The response should contain a error message in case of an error.</returns>
        public Response<User> Login (string email, string password) 
        {
            try
            {
                BusinessLayer.UserPackage.User tempUser = _securityController.Login(email, password);
                User tempStructUser = new User(tempUser.Email,tempUser.Nickname);
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
        /// <returns>A Response object. The response should contain a error message in case of an error.</returns>
        public Response Logout(string email) 
        {
            try
            {
                _securityController.Logout(email);
                
                Response r = new Response("User " + email + " logged out.");
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
        /// <returns>A Response object. The response should contain a error message in case of an error.</returns>
        public Response ChangePassword (string email, string oldPassword, string newPassword) 
        {
            //This method doesn't perform user validation in the case of administrative purposes.
            try
            {
                _securityController.UserController.ChangePassword(email, oldPassword, newPassword);
                Response resp = new Response("The password has been changed successfully.");
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
