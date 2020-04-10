using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    class UserService
    {
        private BusinessLayer.SecurityController SecurityControl;

        public UserService(BusinessLayer.SecurityController sc)
        {
            this.SecurityControl = sc;
        }

        public BusinessLayer.SecurityController SecurityController
        {
            get { return this.SecurityControl; }
        }

        public Response Register(string email, string password, string nickname) //done++++++++++++++++++++++
        {
            try
            {
                SecurityControl.UserController.Register(email, password, nickname);
            }
            catch (Exception ex)
            {
                Response resp = new Response(ex.Message);
                return resp;
            }
            return new Response();                    
        }

        public Response<User> Login (string email, string password) //done+++++++++++++++++++++++++++++++++++++++
        {
            try
            {
                BusinessLayer.UserPackage.User tempUser = SecurityControl.Login(email, password);
                User tempStructUser = new User(tempUser.email,tempUser.nickname);
                return new Response<User>(tempStructUser);        
            }
            catch (Exception ex)
            {
                User tempStructExceptionUser = new User();
                Response<User> resp = new Response<User>(tempStructExceptionUser, ex.Message);
                return resp;
            }
            //finally release!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public Response Logout(string email) //done+++++++++++++++++++++
        {
            SecurityControl.UserController.Logout(email);
            return new Response("User "+email+" logged out.");
        }

        public Response ChangePassword (string email, string oldPassword, string newPassword) //done++++++++++++++++++++++++++
        {
            try
            {
                this.SecurityControl.UserController.ChangePassword(email, oldPassword, newPassword);
                return new Response("Password successfully changed.");
            }
            catch (Exception ex)
            {
                return new Response(ex.Message);
            }
        }
    }
}
