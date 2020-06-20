using System;
using System.Windows.Media;

namespace Presentation.Model
{
    /// <summary>
    /// The model the task window is designed after.
    /// </summary>
    public class TaskModel : NotifiableModelObject
    {
        public readonly SolidColorBrush ALMOST_DUE_DATE_BACKGROUND_COLOR = Brushes.Orange;
        public readonly SolidColorBrush PAST_DUE_DATE_BACKGROUND_COLOR = Brushes.Red;
        public readonly SolidColorBrush ORIGINAL_BACKGROUND_COLOR = Brushes.Khaki;
        public readonly SolidColorBrush CURRENT_USER_BORDER_COLOR = Brushes.Blue;
        public readonly SolidColorBrush ORIGINAL_BORDER_COLOR = Brushes.Black;

        public SolidColorBrush TaskBorderColor
        {
            get => CalculateTaskBorderColor();
            private set { }
        }

        public SolidColorBrush TaskBackgroundColor
        {
            get => CalculateTaskBackgroundColor();
            private set { }
        }

        public int ID { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; set; }
        public DateTime LastChangedDate{ get; }
        public string AssigneeEmail { get; set; }
        public UserModel CurrentUser { get; private set; }
        private int _columnOrdinal;
        public int ColumnOrdinal { get => _columnOrdinal; set { _columnOrdinal = value; RaisePropertyChanged("ColumnOrdinal"); } }

        /// <summary>
        /// The task model constructor. Initializes all task relevant fields in addition to
        /// the assignee of this task and the ordinal of the column this task belongs to.
        /// </summary>
        /// <param name="Controller">The controller this task uses to communicate with the backend.</param>
        /// <param name="ID">The task's ID.</param>
        /// <param name="Title">The task's title.</param>
        /// <param name="Description">The task's description.</param>
        /// <param name="CreationTime">The task's creation time.</param>
        /// <param name="DueDate">The task's due date.</param>
        /// <param name="LastChangedDate">The task's last changed date.</param>
        /// <param name="AssigneeEmail">The task assignee's email address.</param>
        /// <param name="columnOrdinal">The column ordinal this task is associated with.</param>
        /// <param name="currentUser">The current user viewing this task.</param>
        public TaskModel(BackendController Controller, int ID, string Title, string Description, DateTime CreationTime, DateTime DueDate, 
        DateTime LastChangedDate, string AssigneeEmail, int columnOrdinal, UserModel currentUser) : base(Controller) {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.CreationTime = CreationTime;
            this.DueDate = DueDate;
            this.LastChangedDate = LastChangedDate;
            this.AssigneeEmail = AssigneeEmail;
            this.ColumnOrdinal = columnOrdinal;
            this.CurrentUser = currentUser;
        }

        /// <summary>
        /// Update task title.
        /// </summary>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string title)
        {
            Controller.UpdateTaskTitle(CurrentUser.AssociatedBoard, ColumnOrdinal, ID, title);
            this.Title = title;
            RaisePropertyChanged("Title");
        }

        /// <summary>
        /// Update the description of a task.
        /// </summary>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string description)
        {
            Controller.UpdateTaskDescription(CurrentUser.AssociatedBoard, ColumnOrdinal, ID, description);
            this.Description = description;
            RaisePropertyChanged("Description");
        }

        /// <summary>
        /// Update the due date of a task.
        /// </summary>
        /// <param name="dueDate">The new due date of the column.</param>
        public void UpdateTaskDueDate(DateTime dueDate)
        {
            Controller.UpdateTaskDueDate(this.CurrentUser.AssociatedBoard, ColumnOrdinal, ID, dueDate);
            this.DueDate = dueDate;
            RaisePropertyChanged("DueDate");
        }

        /// <summary>
        /// Assigns a task to a user.
        /// </summary> 
        /// <param name="emailAssignee">The email of the user to assign the task to.</param>
        public void AssignTask(string emailAssignee)
        {
            Controller.AssignTask(CurrentUser.AssociatedBoard, ColumnOrdinal, ID, emailAssignee);
            this.AssigneeEmail = emailAssignee;
            RaisePropertyChanged("TaskAssigneeUsername");
        }

        /// <summary>
        /// Defines current task border color accordingly to current user.
        /// </summary>
        /// <returns>SolidBrushColor object with current task appropriate border color.</returns>
        private SolidColorBrush CalculateTaskBorderColor()
        {
            if (this.AssigneeEmail.Equals(this.CurrentUser.Email)) return CURRENT_USER_BORDER_COLOR;
            else return ORIGINAL_BORDER_COLOR;
        }

        /// <summary>
        /// Defines current task background color accordingly to its creation date and due date.
        /// </summary>
        /// <returns>SolidBrushColor object with current task appropriate background color.</returns>
        private SolidColorBrush CalculateTaskBackgroundColor()
        {
            long totalTime = this.DueDate.Ticks - this.CreationTime.Ticks;
            long remainingTime = this.DueDate.Ticks - DateTime.Now.Ticks;
            if (remainingTime <= 0) return PAST_DUE_DATE_BACKGROUND_COLOR;
            else
            {
                if (remainingTime > (totalTime / 4)) return ORIGINAL_BACKGROUND_COLOR;
                else return ALMOST_DUE_DATE_BACKGROUND_COLOR;
            }
        }

        /// <summary>
        /// Allows to raise a property in this class from another class.
        /// </summary>
        /// <param name="propertyName">Name of the property to raise.</param>
        public void RaiseProperty(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
    }
}
