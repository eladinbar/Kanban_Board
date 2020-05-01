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
        public const string TaskIDColumnName = "ID";
        public const string TaskOrdinalColumnName = "Ordinal";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskCreationDateColumnName = "CreationDate";
        public const string TaskLastChangedDateColumnName = "LastChangedDate";

        private int _taskId;
        public int TaskId { get => _taskId; set { _taskId = value; } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; _controller.Update(Email, Ordinal, TaskId,TaskOrdinalColumnName, value); } }
        private string _title;
        public string Title { get => _title; set { _title = value; _controller.Update(Email, Ordinal, TaskId, TaskTitleColumnName, value); } }
        private string _description;
        public string Description { get => _description; set { _description = value; _controller.Update(Email, Ordinal, TaskId, TaskDescriptionColumnName, value); } }
        private string _dueDate;
        public string DueDate { get => _dueDate; set { _dueDate = value; _controller.Update(Email, Ordinal, TaskId, TaskDueDateColumnName, value); } }
        private string _creationDate;
        public string CreationDate { get => _creationDate; set { _creationDate = value; _controller.Update(Email, Ordinal, TaskId, TaskCreationDateColumnName, value); } }
        private string _lastChangedDate;
        public string LastChangedDate { get => _lastChangedDate; set { _lastChangedDate = value; _controller.Update(Email, Ordinal, TaskId, TaskLastChangedDateColumnName, value); } }

        public DalTask(string email, int ordinal, int id, string title, string description, string dueDate, string creationDate, string lastChangedDate) : base(new TaskDalController())
        {
            Email = email;
            _ordinal = ordinal;
            _taskId = id;
            _title = title;
            _description = description;
            _dueDate = dueDate;
            _creationDate = creationDate;
            _lastChangedDate = lastChangedDate;

        }

    }
}
