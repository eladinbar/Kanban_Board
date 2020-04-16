using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// Represents all system objects in the simplest manner that will allow all relevant information to be persisted.
    /// </summary>
    public abstract class DalObject<T> where T:DalObject<T>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        /// <summary>
        /// The method to convert an object into Json format, ready to be saved to the disk.
        /// </summary>
        /// <returns>Returns the T instance in Json (string) form.</returns>
        public string ToJson() {
            return JsonSerializer.Serialize(((T)this));
        }

        /// <summary>
        /// The method to revert an object from Json back into its original T instance.
        /// </summary>
        /// <returns>Returns the Json string given back in T form.</returns>
        public T FromJson(string json) {
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public abstract void Save(string path);
    }
}
