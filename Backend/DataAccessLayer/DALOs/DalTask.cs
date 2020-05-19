using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// The data access layer representation of a Task.
    /// </summary>
    internal class DalTask : DalObject<DalTask>
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
        public string ColumnName { get => _columnName; set { _controller.Update(Email, ColumnName, TaskId, ContainingTaskColumnNameColumnName, value); _columnName = value; } }
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

        /// <summary>
        /// A public constructor that initializes all necessary fields to be persisted.
        /// </summary>
        /// <param name="email">The email of the user that is to be associated with the new DalTask.</param>
        /// <param name="columnName">The name of the column this new DalTask is associated with.</param>
        /// <param name="id">The ID of the task to be persisted.</param>
        /// <param name="title">The title of the task to ber persisted.</param>
        /// <param name="description">The description of the task to be persisted.</param>
        /// <param name="dueDate">The due date of the task to be persisted.</param>
        /// <param name="creationDate">The creation date of the task to be persisted.</param>
        /// <param name="lastChangedDate">The last changed date of the task to be persisted.</param>
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
