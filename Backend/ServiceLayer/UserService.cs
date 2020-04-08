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
    }
}
