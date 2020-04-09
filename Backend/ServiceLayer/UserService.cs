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

        public Response Register(string email, string password, string nickname)
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

        public Response<User> Login (string email, string password)
        {
            try
            {
                SecurityControl.UserController.Login(email, password);
            }
            catch(Exception ex)
            {
                User tempUser = new User();
                Response<User> resp = new Response<User>(tempUser);
                return resp;
            }
            SecurityControl.CurrentUser = 
            User legalUser = new User(email, )

        }

        public Response Logout(string email)
        {
            throw new NotImplementedException();
        }

        public Response ChangePassword (string email, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
