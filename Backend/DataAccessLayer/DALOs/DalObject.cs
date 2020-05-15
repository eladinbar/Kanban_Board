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
        /// <summary>
        /// Insert this to the Database.
        /// </summary>
        /// <returns>True if insert was successful</returns>
        public bool Save()
        {
            return _controller.Insert((T) this);
        }
        /// <summary>
        /// Delete the equivelnt row of this in the Database
        /// </summary>
        /// <returns></returns>
        public bool Delate()
        {
            return _controller.Delete((T) this);
        }
        

    }
}
