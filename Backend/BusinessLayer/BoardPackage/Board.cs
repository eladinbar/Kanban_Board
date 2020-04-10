using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class Board : PersistedObject<DataAccessLayer.Board>
    {
        private List<Column> _columns;
        private string _userEmail;

        public Board(string email)
        {

        }

        public Column GetColumn(string columnName)
        {
            throw new NotImplementedException();
        }

        public Column GetColumn(int columnOrdinal)
        {
            throw new NotImplementedException();
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
    }

}
