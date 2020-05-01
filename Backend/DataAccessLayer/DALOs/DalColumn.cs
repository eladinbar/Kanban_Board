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
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string ColumnNameColumnName = "Name";
        public const string ColumnOrdinalColumnName = "Ordinal";
        public const string ColumnLimitColumnName = "Limit";

        private string _name;
        public string Name { get => _name; set { _name = value; _controller.Update(Email, Ordinal, ColumnNameColumnName, value); } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; _controller.Update(Email, Ordinal, ColumnNameColumnName, value); } }
        private int _limit;
        public int Limit { get => _limit; set { _limit = value; _controller.Update(Email, Ordinal, ColumnNameColumnName, value); } }
        public List<DalTask> Tasks { get; set; }

        public DalColumn(string email, int ordinal, string name, int limit) : base ( new ColumnDalController())
        {
            Email = email;
            _name = name;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
