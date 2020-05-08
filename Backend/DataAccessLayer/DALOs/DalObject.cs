using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// Base abstract class for Dal Entities
    /// </summary>
    public abstract class DalObject<T> where T:DalObject<T>
    {
        public const string EmailColumnName = "email";
        protected DalController<T> _controller;
        public string Email { get; set; } = "";

        protected DalObject(DalController<T> controller)
        {
            _controller = controller;
        }

        

    }
}
