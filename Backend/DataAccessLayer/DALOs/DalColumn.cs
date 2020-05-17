using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// The data access layer representation of a Column.
    /// </summary>
    internal class DalColumn : DalObject<DalColumn>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string ColumnNameColumnName = "Name";
        public const string ColumnOrdinalColumnName = "Ordinal";
        public const string ColumnLimitColumnName = "TaskLimit";

        private string _name;
        public string Name { get => _name; set { _name = value; _controller.Update(Email, Name, ColumnNameColumnName, value); } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; _controller.Update(Email, Name, ColumnOrdinalColumnName, value); } }
        private int _limit;
        public int Limit { get => _limit; set { _limit = value; _controller.Update(Email, Name, ColumnLimitColumnName, value); } }
        public List<DalTask> Tasks { get; set; }

        /// <summary>
        /// A public constructor that initializes all necessary fields to be persisted.
        /// </summary>
        /// <param name="email">The email of the user that is to be associated with the new DalColumn.</param>
        /// <param name="name">The name of the column to be persisted.</param>
        /// <param name="ordinal">The ordinal of the column to be persisted.</param>
        /// <param name="limit">The task limit of the column to be persisted.</param>
        public DalColumn(string email, string name, int ordinal, int limit) : base ( new ColumnDalController())
        {
            Email = email;
            _name = name;
            _ordinal = ordinal;
            _limit = limit;
        }
    }
}
