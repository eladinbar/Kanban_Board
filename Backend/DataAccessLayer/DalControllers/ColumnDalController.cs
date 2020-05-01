using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Columns";

        public ColumnDalController() : base(ColumnTableName) { }

        public List<DalColumn> SelectAllColumns(string email)
        {
            List<DalColumn> columnList = Select(email).Cast<DalColumn>().ToList();
            return columnList;
        }

        internal override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
