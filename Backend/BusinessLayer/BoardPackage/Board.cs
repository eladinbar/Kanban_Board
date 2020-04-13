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

        private List<Column> _columns;
        private string _userEmail;
        private int _taskCounter;

        public Board(string email)
        {
            _userEmail = email;
            _taskCounter = 1;
            _columns = new List<Column>();
            _columns.Add(new Column("Backlog"));
            _columns.Add(new Column("In Prograss"));
            _columns.Add(new Column("Done"));
        }

        public Board(string email, int taskCounter, List<Column> columns)
        {
            _userEmail = email;
            _taskCounter = taskCounter;
            _columns = columns;
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
            foreach(Column c in _columns)
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
            return new DataAccessLayer.Board(_userEmail, TaskCounter, dalColumns);
        }

        public List<Column> Columns { get; }
        public int TaskCounter { get; set; }
    }

}
