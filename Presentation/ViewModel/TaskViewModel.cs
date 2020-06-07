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
        private string _dpMessage;
        public string dpMessage { get => _dpMessage; set { _dpMessage = value; RaisePropertyChanged("dpMessage"); } }

        //get {return new SolidColorBrush(Task.DueDate.CompareTo(DateTime.Now) ? Colors.Blue : Colors.Red);} }


        //Constructor
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
        
        private enum Update { Title=0, Description=1, DueDate=2, TaskAssignee=3 }
        
        public enum Border { Blue=0, Green=1, Red=2 }

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
                else if (fields[fields.Length - 1].Equals(Brushes.Blue))
                    validFields.Add(Border.Blue);
            }
            return validFields;
        }

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

        internal void ChangedTitle(TextBox tTitle)
        {
            if (tTitle.Text.Length > MAXIMUM_TITLE_LENGTH | tTitle.Text.Length == MINIMUM_TITLE_LENGTH)
                tTitle.BorderBrush = INVALID_BORDER_COLOR;
            else if (!tTitle.Text.Equals(Title))
                tTitle.BorderBrush = VALID_BORDER_COLOR;
            else
                tTitle.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        internal void ChangedDescription(TextBox tDescription)
        {
            if (tDescription.Text.Length > MAXIMUM_DESCRIPTION_LENGTH)
                tDescription.BorderBrush = INVALID_BORDER_COLOR;
            else if (!tDescription.Text.Equals(Description))
                tDescription.BorderBrush = VALID_BORDER_COLOR;
            else
                tDescription.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        internal void ChangedDueDate(DatePicker dpDueDate)
        {
            dpMessage = (dpDueDate.SelectedDate <= DueDate.Date).ToString();
            if (dpDueDate.DisplayDate < DueDate.Date)
                dpDueDate.BorderBrush = INVALID_BORDER_COLOR;
            if (!dpDueDate.SelectedDate.Equals(this.DueDate))
            {
                dpDueDate.BorderBrush = VALID_BORDER_COLOR;

            }
            else
                dpDueDate.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    

        internal void ChangedTaskAssignee(TextBox tTaskAssignee)
        {
            if (!tTaskAssignee.Text.Equals(TaskAssigneeUsername))
                tTaskAssignee.BorderBrush = VALID_BORDER_COLOR;
            else
                tTaskAssignee.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    }
}
