using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class Board : DalObject<Board>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string UserEmail { get; set; }
        public int TaskCounter { get; set; }

        [JsonIgnore] //List is retrieved using the individual <Column>.json files
        public List<Column> Columns { get; private set; }

        /// <summary>
        /// Data Access Layer Board constructor that receives all necessary Buisness Layer Board parameters to ensure they are all saved properly.
        /// </summary>
        /// <param name="email">The email address associated with this board.</param>
        /// <param name="taskCounter">The amount of tasks currently contained by this board.</param>
        /// <param name="columns">The list of this board's columns.</param>
        public Board(string email, int taskCounter, List<Column> columns)
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
        }

        /// <summary>
        /// Empty public constructor for use when loading .json files from memory.
        /// </summary>
        public Board() { }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path)
        {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + path);
            if (!dir.Exists)
                dir.Create();
            dc.WriteToFile(this.UserEmail, ToJson(), path);
        }
    }
}
