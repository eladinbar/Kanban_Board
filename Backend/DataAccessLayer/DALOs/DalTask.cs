using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    internal class DalTask : DalObject
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string TaskIDColumnName = "ID";
        public const string ContainingTaskColumnNameColumnName = "ColumnName";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskCreationDateColumnName = "CreationDate";
        public const string TaskLastChangedDateColumnName = "LastChangedDate";

        private int _taskId;
        public int TaskId { get => _taskId; set { _taskId = value; } }
        private string _columnName;
        public string ColumnName { get => _columnName; set { _columnName = value; _controller.Update(Email, ColumnName, TaskId, ContainingTaskColumnNameColumnName, value); } }
        private string _title;
        public string Title { get => _title; set { _title = value; _controller.Update(Email, ColumnName, TaskId, TaskTitleColumnName, value); } }
        private string _description;
        public string Description { get => _description; set { _description = value; _controller.Update(Email, ColumnName, TaskId, TaskDescriptionColumnName, value); } }
        private DateTime _dueDate;
        public DateTime DueDate { get => _dueDate; set { _dueDate = value; _controller.Update(Email, ColumnName, TaskId, TaskDueDateColumnName, value.ToString()); } }
        private DateTime _creationDate;
        public DateTime CreationDate { get => _creationDate; set { _creationDate = value; _controller.Update(Email, ColumnName, TaskId, TaskCreationDateColumnName, value.ToString()); } }
        private DateTime _lastChangedDate;
        public DateTime LastChangedDate { get => _lastChangedDate; set { _lastChangedDate = value; _controller.Update(Email, ColumnName, TaskId, TaskLastChangedDateColumnName, value.ToString()); } }

        public DalTask(string email, string columnName, int id, string title, string description, DateTime dueDate, DateTime creationDate, DateTime lastChangedDate) : base(new TaskDalController())
        {
            Email = email;
            _columnName = columnName;
            _taskId = id;
            _title = title;
            _description = description;
            _dueDate = dueDate;
            _creationDate = creationDate;
            _lastChangedDate = lastChangedDate;

        }

    }
}
