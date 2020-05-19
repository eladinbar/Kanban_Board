using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;

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

        /// <summary>
        /// A protected constructor that initializes the given controller.
        /// </summary>
        /// <param name="controller">The respective controller of the DalObject.</param>
        protected DalObject(DalController<T> controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Inserts 'this' into the database.
        /// </summary>
        /// <returns>Returns true if 'Insert' was successful.</returns>
        public bool Save()
        {
            return _controller.Insert((T) this);
        }

        /// <summary>
        /// Deletes the equivalent row of 'this' in the database.
        /// </summary>
        /// <returns>Returns true if the row was removed successfully.</returns>
        public bool Delete()
        {
            return _controller.Delete((T) this);
        }
        

    }
}
