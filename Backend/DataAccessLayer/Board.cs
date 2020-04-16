using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Board : DalObject<Board>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string UserEmail { get; }
        public int TaskCounter { get; }

        [JsonIgnore] //List is retrieved using the individual <Column>.json files
        public List<Column> Columns { get; private set; }

        public Board(string email, int taskCounter, List<Column> columns)
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
        }

        public Board() { }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path)
        {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + "Boards\\");
            if (!dir.Exists)
                dir.Create();
            dc.WriteToFile(this.UserEmail, ToJson(), "Boards\\");
            dir = new DirectoryInfo(dir + this.UserEmail);
            dir.Create();
        }
    }
}
