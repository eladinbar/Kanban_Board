using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    internal class DalColumn : DalObject
    {
        public const string ColumnNameColumnName = "Name";
        public const string ColumnOrdinalColumnName = "Ordinal";
        public const string ColumnLimitColumnName = "Limit";

        private string _name;
        public string Name { get => _name; set { _name = value; } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; } }
        private int _limit;
        public int Limit { get => _limit; set { _limit = value; } }

        public DalColumn(string email, string name, int ordinal, int limit) : base ( new ColumnDalController())
        {
            Email = email;
            _name = name;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
