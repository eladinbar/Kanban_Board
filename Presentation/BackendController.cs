using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;

namespace Presentation
{
    public class BackendController
    {
        private IService Service { get; set; }
        public BackendController()
        {
            //Service = new Service();
        }

        internal UserModal Login(string email, string password)
        {
            Response<User> user = Service.Login(email, password);
            if (user.ErrorOccured)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModal(this, email, user.Value.Nickname);
        }
    }
}
