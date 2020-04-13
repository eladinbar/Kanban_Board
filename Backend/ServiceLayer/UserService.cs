using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private BusinessLayer.SecurityController _securityController;

        public UserService(BusinessLayer.SecurityController sc)
        {
            _securityController = sc;
            log.Debug("UserService Created");
        }



        public BusinessLayer.SecurityController SecurityController
        {
            get
            {
                log.Debug("SecurityController getter was called");
                return _securityController;
            }
        }



        public Response Register(string email, string password, string nickname) //add a method of creating a new Board of a new User
        {
            try
            {
                _securityController.UserController.Register(email, password, nickname);
                _securityController.BoardController.AddNewBoard(email);
                
                Response r = new Response("User "+nickname+" has been registered successfully.");
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



        public Response<User> Login (string email, string password) //done+++++++++++++++++++++++++++++++++++++++
        {
            try
            {
                BusinessLayer.UserPackage.User tempUser = _securityController.Login(email, password);
                User tempStructUser = new User(tempUser.Email,tempUser.Nickname);
                Response<User> r = new Response<User>(tempStructUser);
                log.Info("Seccesfull login");
                return r;
            }
            catch (Exception ex)
            {
                Response<User> resp = new Response<User>(ex.Message);
                log.Error(ex.Message, ex);
                return resp;
            }
        }



        public Response Logout(string email) //done+++++++++++++++++++++
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



        public Response ChangePassword (string email, string oldPassword, string newPassword) //done++++++++++++++++++++++++++
        {
            try
            {
                _securityController.UserController.ChangePassword(email, oldPassword, newPassword);
                Response resp = new Response("Password successfully changed.");
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
