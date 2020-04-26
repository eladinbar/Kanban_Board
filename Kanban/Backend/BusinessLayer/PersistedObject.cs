using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal interface PersistedObject<T> where T : DalObject<T>
    {
        /// <summary>
        /// Transforms the object to its corresponding DalObject.
        /// </summary>
        /// <returns>return a DalObject</returns>
        T ToDalObject();

        /// <summary>
        /// The method in the BusinessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        void Save(string path);
    }
}