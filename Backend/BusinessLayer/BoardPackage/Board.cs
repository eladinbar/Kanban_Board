using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Board : PersistedObject<DataAccessLayer.Board>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public List<Column> Columns { get; }
        public string UserEmail { get; }
        public int TaskCounter { get; set; }

        public Board(string email)
        {
            UserEmail = email;
            TaskCounter = 1;
            Columns = new List<Column>();
            Columns.Add(new Column("Backlog"));
            Columns.Add(new Column("In Prograss"));
            Columns.Add(new Column("Done"));
        }

        public Board(string email, int taskCounter, List<Column> columns)
        {
            UserEmail = email;
            TaskCounter = taskCounter;
            Columns = columns;
        }

        public Column GetColumn(string columnName)
        {
            return Columns.Find(x => x.Name.Equals(columnName));
        }

        public Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal > Columns.Count)
                throw new ArgumentOutOfRangeException("Column index out of range");
            
               return Columns[columnOrdinal];
        }

        public List<string> getColumnNames()
        {
            List<string> columnNames = new List<string>();
            foreach(Column c in Columns)
            {
                columnNames.Add(c.Name);
            }
            return columnNames;
        }

        public void Save(string path)
        {
            ToDalObject().Save(path);         
        }

        public DataAccessLayer.Board ToDalObject()
        {
            List<DataAccessLayer.Column> dalColumns = new List<DataAccessLayer.Column>();
            foreach(Column c in Columns)
            {
                dalColumns.Add(c.ToDalObject());
            }
            return new DataAccessLayer.Board(UserEmail, TaskCounter, dalColumns);
        }


    }

}
