using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    /// <summary>
    /// The data context of the task window.
    /// </summary>
    internal class TaskViewModel : NotifiableObject
    {
        public readonly SolidColorBrush INVALID_BORDER_COLOR = Brushes.Red;
        public readonly SolidColorBrush VALID_BORDER_COLOR = Brushes.Green;
        public readonly SolidColorBrush ORIGINAL_BORDER_COLOR = Brushes.CornflowerBlue;

        private const int MAXIMUM_TITLE_LENGTH = 50;
        private const int MINIMUM_TITLE_LENGTH = 0;
        private const int MAXIMUM_DESCRIPTION_LENGTH = 300;

        private BackendController Controller;
        private TaskModel Task;
        public int ID { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; set; }
        public DateTime LastChangedDate { get; }
        public string TaskAssigneeUsername { get; set; }
        public string TaskAsigneeNickname { get; set; } //Pending
        public bool IsAssignee { get; set; }
        private string _message;
        public string Message { get => _message; set { _message = value; RaisePropertyChanged("Message"); } }
        private string _dpMessage; //For testing DueDate
        public string dpMessage { get => _dpMessage; set { _dpMessage = value; RaisePropertyChanged("dpMessage"); } }

        /// <summary>
        /// The TaskViewModel constructor. Initializes all bound fields as well as its respective TaskModel and BackendController.
        /// </summary>
        /// <param name="Task">The TaskModel representing this TaskViewModel.</param>
        /// <param name="columnOrdinal">The column ordinal that contains the task this view model represents.</param>
        /// <param name="isAssignee">A verification variable to ensure task editing can only be performed by its assignee.</param>
        public TaskViewModel(TaskModel Task, int columnOrdinal, bool isAssignee) {
             this.Controller = Task.Controller;
             this.Task = Task;
             this.ID = Task.ID;
             this.Title = Task.Title;
             this.Description = Task.Description;
             this.CreationTime = Task.CreationTime;
             this.DueDate = Task.DueDate;
             this.LastChangedDate = Task.LastChangedDate;
             this.TaskAssigneeUsername = Task.AssigneeEmail;
             this.IsAssignee = isAssignee;
             this.Message = "";
        }

        /// <summary>
        /// Updates all task fields that were modified by the user in the GUI.
        /// </summary>
        /// <param name="validFields">Represents the state of all fields in the task window.</param>
        /// <param name="title">The title of the task to update.</param>
        /// <param name="description">The description of the task to update.</param>
        /// <param name="dueDate">The due date of the task to update.</param>
        /// <param name="taskAssignee">The assignee of the task to update.</param>
        public void UpdateTask(List<Border> validFields, string title, string description, DateTime dueDate, string taskAssignee) {
            Message = "";
            if (validFields[(int)Update.Title] == Border.Green)
            {
                try
                {
                    Task.UpdateTaskTitle(title);
                    this.Title = title;
                    RaisePropertyChanged("Title");
                }
                catch(Exception ex) {
                    Message += ex.Message;
                }
            }
            if (validFields[(int)Update.Description] == Border.Green)
            {
                try {
                    Task.UpdateTaskDescription(description);
                    this.Description = description;
                    RaisePropertyChanged("Description");
                }
                catch(Exception ex) {
                    Message += " " + ex.Message;
                }

            }
            if (validFields[(int)Update.DueDate] == Border.Green)
            {
                try
                {
                    Task.UpdateTaskDueDate(dueDate);
                    this.DueDate = dueDate;
                    RaisePropertyChanged("DueDate");
                }
                catch(Exception ex) {
                    Message += " " + ex.Message;
                }
            }
            if (validFields[(int)Update.TaskAssignee] == Border.Green)
            {
                try
                {
                    Task.AssignTask(taskAssignee);
                    this.TaskAssigneeUsername = taskAssignee;
                    RaisePropertyChanged("TaskAssigneeUsername");
                }
                catch(Exception ex) {
                    Message += " " + ex.Message;
                }
            }
        }
        
        /// <summary>
        /// An enum used to represent 
        /// </summary>
        private enum Update { Title=0, Description=1, DueDate=2, TaskAssignee=3 }
        
        /// <summary>
        /// An enum used to represent the state of a field in the task window.
        /// </summary>
        public enum Border { Blue=0, Green=1, Red=2 }

        /// <summary>
        /// Checks if all user changes are valid. Presents relevant message boxes for every different case.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>Returns a list of borders representing the state of each field in the task window.</returns>
        public List<Border> ConfirmChangesValidity(params Brush[] fields) {
            List<Border> validFields = ValidFields(fields.Take(fields.Length - 1));
            if (validFields.Contains(Border.Red)) //Checks all fields except TaskAssignee
            {
                MessageBox.Show("Some fields were assigned invalid values. \n" +
                "Please review your changes and try again.", "Invalid fields", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else {
                if (fields[fields.Length - 1].Equals(Brushes.Green)) //If the task assignee field was modified
                {
                    MessageBoxResult Result = MessageBox.Show("You are trying to change the task assignee. \n" +
                    "Confirming your changes will prevent you from making any further adjustments to this task. \n" +
                    "Would you like to proceed?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                    if (Result == MessageBoxResult.OK)
                    {
                        validFields.Add(Border.Green);
                        MessageBox.Show("Task Assignee was changed successfully.", "", MessageBoxButton.OK);
                        MessageBox.Show("Task data was updated successfully!", "", MessageBoxButton.OK);
                    }
                    else
                        validFields.Add(Border.Red);
                }
                else
                    validFields.Add(Border.Blue);
            }
            return validFields;
        }

        /// <summary>
        /// Creates a list representing the state of all of the window's fields.
        /// </summary>
        /// <param name="fields">The fields' list to check.</param>
        /// <returns>Returns a list of borders representing the state of each field in the task window.</returns>
        private List<Border> ValidFields(IEnumerable<Brush> fields) {
            List<Border> validFields = new List<Border>();
            foreach (Brush brush in fields) {
                if (brush.Equals(Brushes.Red))
                    validFields.Add(Border.Red);
                else if (brush.Equals(Brushes.Green))
                    validFields.Add(Border.Green);
                else
                    validFields.Add(Border.Blue);
            }     
            return validFields;
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtTitle" text box according to its state.
        /// </summary>
        /// <param name="txtTitle">The text box to assign the state to.</param>
        internal void ChangedTitle(TextBox txtTitle)
        {
            if (txtTitle.Text.Length > MAXIMUM_TITLE_LENGTH | txtTitle.Text.Length == MINIMUM_TITLE_LENGTH)
                txtTitle.BorderBrush = INVALID_BORDER_COLOR;
            else if (!txtTitle.Text.Equals(Title))
                txtTitle.BorderBrush = VALID_BORDER_COLOR;
            else
                txtTitle.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtDescription" text box according to its state.
        /// </summary>
        /// <param name="txtDescription">The text box to assign the state to.</param>
        internal void ChangedDescription(TextBox txtDescription)
        {
            if (txtDescription.Text.Length > MAXIMUM_DESCRIPTION_LENGTH)
                txtDescription.BorderBrush = INVALID_BORDER_COLOR;
            else if (!txtDescription.Text.Equals(Description))
                txtDescription.BorderBrush = VALID_BORDER_COLOR;
            else
                txtDescription.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        /// <summary>
        /// Assigns the appropriate border to the "dpDueDate" date picker according to its state.
        /// </summary>
        /// <param name="dpDueDate">The date picker to assign the state to.</param>
        internal void ChangedDueDate(DatePicker dpDueDate)
        {
            dpMessage = (dpDueDate.SelectedDate <= DueDate.Date).ToString(); //Experimental DatePicker CompareTo checking
            if (dpDueDate.DisplayDate < DueDate.Date)
                dpDueDate.BorderBrush = INVALID_BORDER_COLOR;
            if (!dpDueDate.SelectedDate.Equals(this.DueDate))
            {
                dpDueDate.BorderBrush = VALID_BORDER_COLOR;

            }
            else
                dpDueDate.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtTaskAssignee" text box according to its state.
        /// </summary>
        /// <param name="txtTaskAssignee">The text box to assign the state to.</param>
        internal void ChangedTaskAssignee(TextBox txtTaskAssignee)
        {
            if (!txtTaskAssignee.Text.Equals(TaskAssigneeUsername))
                txtTaskAssignee.BorderBrush = VALID_BORDER_COLOR;
            else
                txtTaskAssignee.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    }
}
