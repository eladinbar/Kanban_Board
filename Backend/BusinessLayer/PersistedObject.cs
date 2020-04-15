using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface PersistedObject<T> where T : DalObject<T>
    {
        /// <summary>
        /// Transforms the object to his coresponding DalObject.
        /// </summary>
        /// <returns>
        /// return a DalObject
        /// </returns>
        T ToDalObject();

        /// <summary>
        /// Saves to disk method
        /// </summary>
        /// <param name="path">location to save</param>
        void Save(string path);
    }
}