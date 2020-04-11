﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class Column : DalObject<Column>
    {
        private readonly string _name;
        private readonly int _limit;
        private List<Task> _tasks;

        public Column(string name, int limit, List<Task> tasks)
        {
            _name = name;
            _limit = limit;
            _tasks = tasks;
        }

        public Column() { }

        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + path);
            if (!dir.Exists)
                dir.Create();
            dc.WriteToFile(_name, ToJson(), path);
        }

        public void Delete (string path) {
            DalController dc = new DalController();
            dc.RemoveFromFile(path);
        }

        //getters
        public string Name { get; }
        public int Limit { get; }
        public List<Task> Tasks { get; }
    }
}
