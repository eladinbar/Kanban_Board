using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// Dal access layer representation  of a Column
    /// </summary>
    internal class DalColumn : DalObject<DalColumn>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string ColumnNameColumnName = "ColumnName";
        public const string ColumnOrdinalColumnName = "Ordinal";
        public const string ColumnLimitColumnName = "TaskLimit";

        private string _name;
        public string Name { get => _name; set { _name = value; _controller.Update(Email, Name, ColumnNameColumnName, value); } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; _controller.Update(Email, Name, ColumnOrdinalColumnName, value); } }
        private int _limit;
        public int Limit { get => _limit; set { _limit = value; _controller.Update(Email, Name, ColumnLimitColumnName, value); } }
        public List<DalTask> Tasks { get; set; }

        public DalColumn(string email, string name, int ordinal, int limit) : base ( new ColumnDalController())
        {
            Email = email;
            _name = name;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
