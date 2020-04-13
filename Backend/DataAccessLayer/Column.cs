using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Column : DalObject<Column>
    {
        public string Name { get; }
        public int Limit { get; }

        [JsonIgnore] //List is retrieved using the individual <Task>.json files
        public List<Task> Tasks { get; }

        public Column(string name, int limit, List<Task> tasks)
        {
            Name = name;
            Limit = limit;
            Tasks = tasks;
        }

        public Column() { }

        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + path);
            if (!dir.Exists)
                dir.Create();
            dc.WriteToFile(this.Name, ToJson(), path);
        }
    }
}
