using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    internal abstract class DalObject
    {
        public const string EmailColumnName = "email";
        protected DalController _controller;
        public string Email { get; set; } = "";

        protected DalObject(DalController controller)
        {
            _controller = controller;
        }

    }
}
