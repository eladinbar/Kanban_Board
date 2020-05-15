using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal abstract class PersistedObject<T> where T : DataAccessLayer.DALOs.DalObject<T>
    {
        internal DataAccessLayer.DALOs.DalObject<T> DalCopyObject;

        /// <summary>
        /// Transforms the object to its corresponding DalObject.
        /// </summary>
        /// <returns>return a DalObject</returns>
        internal abstract T ToDalObject();

        /// <summary>
        /// The method in the BusinessLayer to save an object to the persistent layer.
        /// </summary>
        internal virtual void Save() {
            ToDalObject();
            DalCopyObject.Save();
        }
    }
}
