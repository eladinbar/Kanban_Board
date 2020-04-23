using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Column : DalObject<Column>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string Name { get; set; }
        public int Limit { get; set; }

        [JsonIgnore] //List is retrieved using the individual <Task>.json files
        public List<Task> Tasks { get; }

        /// <summary>
        /// Data Access Layer Column constructor that receives all necessary Business Layer Column parameters to ensure they are all saved properly.
        /// </summary>
        /// <param name="name">This column's name.</param>
        /// <param name="limit">The maximum amount of tasks this column can contain.</param>
        /// <param name="tasks">The list of this column's current tasks.</param>
        public Column(string name, int limit, List<Task> tasks)
        {
            Name = name;
            Limit = limit;
            Tasks = tasks;
        }

        /// <summary>
        /// Empty public constructor for use when loading .json files from memory.
        /// </summary>
        public Column() { }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo FolderDir = new DirectoryInfo(dc.BASE_PATH + path.Substring(0,path.Length-2));
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + path);
            if (!FolderDir.Exists)
                FolderDir.Create();
            if (!File.Exists(dir + this.Name))
                dc.WriteToFile(this.Name, ToJson(), path);
            dir = new DirectoryInfo(dir + this.Name);
            dir.Create();
        }
    }
}
