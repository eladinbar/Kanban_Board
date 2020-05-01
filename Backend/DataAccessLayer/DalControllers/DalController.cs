using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    public abstract class DalController
    {
        protected readonly string _connectionString;
        private readonly string _tableName;

        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
        }

    }
}
