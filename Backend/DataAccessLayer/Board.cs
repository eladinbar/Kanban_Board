using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Board : DalObject<Board>
    {
        private readonly string _userEmail;
        private readonly int _taskCounter;
        private readonly List<Column> _columns;

        public Board(string email, int taskCounter, List<Column> columns)
        {
            _userEmail = email;
            _taskCounter = taskCounter;
            _columns = columns;
        }

        public Board() { }

        public override void Save(string path)
        {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + "Boards\\");
            if (!dir.Exists)
                dir.Create();
            dc.WriteToFile(_userEmail, ToJson(), "Boards\\");
            dir = new DirectoryInfo(dir + this.UserEmail);
            dir.Create();
        }

        //getters
        public string UserEmail { get; }
        public int TaskCounter { get; }
        public List<Column> Columns { get; }
    }
}
