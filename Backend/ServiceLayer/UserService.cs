using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private BusinessLayer.SecurityController _securityController;

        public UserService(BusinessLayer.SecurityController sc)
        {
            _securityController = sc;
        }



        public BusinessLayer.SecurityController SecurityController
        {
            get { return _securityController; }
        }



        public Response Register(string email, string password, string nickname) //add a method of creating a new Board of a new User
        {
            try
            {
                _securityController.UserController.Register(email, password, nickname);
                _securityController.BoardController.AddNewBoard(email);
                return new Response("User "+nickname+" has been registered successfully.");
            }
            catch (Exception ex)
            {
                Response resp = new Response(ex.Message);
                return resp;
            }
        }



        public Response<User> Login (string email, string password) //done+++++++++++++++++++++++++++++++++++++++
        {
            try
            {
                BusinessLayer.UserPackage.User tempUser = _securityController.Login(email, password);
                User tempStructUser = new User(tempUser.Email,tempUser.Nickname);
                return new Response<User>(tempStructUser);        
            }
            catch (Exception ex)
            {
                Response<User> resp = new Response<User>(ex.Message);
                return resp;
            }
        }



        public Response Logout(string email) //done+++++++++++++++++++++
        {
            try
            {
                _securityController.Logout(email);
                return new Response("User " + email + " logged out.");
            }
            catch(Exception ex)
            {
                return new Response(ex.Message);
            }
        }



        public Response ChangePassword (string email, string oldPassword, string newPassword) //done++++++++++++++++++++++++++
        {
            try
            {
                _securityController.UserController.ChangePassword(email, oldPassword, newPassword);
                return new Response("Password successfully changed.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }
    }
}
