

namespace Presentation.Model
{
    public class NotifiableModelObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        /// <summary>
        /// NotifiableModelObject constructor. Initializes the controller the model object uses to interact with the backend.
        /// </summary>
        /// <param name="controller"></param>
        protected NotifiableModelObject(BackendController controller)
        {
            Controller = controller;
        }
    }
}
