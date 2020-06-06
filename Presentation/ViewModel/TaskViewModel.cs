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
        public string Message { get => Message; set { Message = value; RaisePropertyChanged("Message"); } }

        //get {return new SolidColorBrush(Task.DueDate.CompareTo(DateTime.Now) ? Colors.Blue : Colors.Red);} }


        //Constructor
        public TaskViewModel(TaskModel Task, bool isAssignee) {
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
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain an error message in case of an error</returns>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string title)
        {
            Message = "";
            try
            {
                Controller.UpdateTaskTitle(email, columnOrdinal, taskId, title);
                this.Title = title;
                RaisePropertyChanged("Title");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain an error message in case of an error</returns>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string description)
        {
            Message = "";
            try
            {
                Controller.UpdateTaskDescription(email, columnOrdinal, taskId, description);
                this.Description = description;
                RaisePropertyChanged("Description");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        


        /// <summary>
        /// Update the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <param name="taskId">The task to be updated identified task ID.</param>
        /// <param name="dueDate">The new due date of the column.</param>
        /// <returns>A response object. The response should contain an error message in case of an error.</returns>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Message = "";
            try
            {
                Controller.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
                this.DueDate = dueDate;
                RaisePropertyChanged("DueDate");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void AssignTask(string email, int columnOrdinal, int taskId, string emailAssignee)
        {
            Message = "";
            try
            {
                Controller.AssignTask(email, columnOrdinal, taskId, emailAssignee);
                this.TaskAssigneeUsername = emailAssignee;
                RaisePropertyChanged("DueDate");
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        public void UpdateTask(Border[] validFields) {
            if (validFields[(int)Update.Title] == Border.Green)
                UpdateTaskTitle(email, columnOrdinal, ID, Title);
            if (validFields[(int)Update.Description] == Border.Green)
                UpdateTaskDescription(email, columnOrdinal, ID, Description);
            if (validFields[(int)Update.DueDate] == Border.Green)
                UpdateTaskDueDate(email, columnOrdinal, ID, DueDate);
            if (validFields[(int)Update.TaskAssignee] == Border.Green)
                AssignTask(email, columnOrdinal, ID, TaskAssigneeUsername);
        }
        
        private enum Update { Title=0, Description=1, DueDate=2, TaskAssignee=3 }
        
        public enum Border { Blue=0, Green=1, Red=2 }

        public Border[] ConfirmChangesValidity(params Brush[] fields) {
            Border[] validFields = ValidFields(fields.Take(fields.Length - 1));
            if (validFields.Contains(Border.Red)) //Checks all fields except TaskAssignee
            {
                MessageBox.Show("Some fields were assigned invalid values. \n" +
                "Please review your changes and try again.", "Invalid fields", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else {
                if (fields[fields.Length-1].Equals(Brushes.Green)) //If the task assignee field was modified
                {
                    MessageBoxResult Result = MessageBox.Show("You are trying to change the task assignee. \n  " +
                    "Confirming your changes will prevent you from making any further adjustments to this task. \n" +
                    "Would you like to proceed?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                    if (Result == MessageBoxResult.OK)
                    {
                        MessageBox.Show("Task Assignee was changed successfully.", "", MessageBoxButton.OK);
                        MessageBox.Show("Task data was updated successfully!", "", MessageBoxButton.OK);
                    }
                }
            }
            return validFields;
        }

        private Border[] ValidFields(IEnumerable<Brush> fields) {
            Border[] validFields = new Border[fields.Count()];
            foreach (Brush brush in fields) {
                if (brush.Equals(Brushes.Red))
                    validFields.Concat(new Border[] { Border.Red });
                else if (brush.Equals(Brushes.Green))
                    validFields.Concat(new Border[] { Border.Green });
                else
                    validFields.Concat(new Border[] { Border.Blue });
            }     
            return validFields;
        }

        internal void ChangeTitle(TextBox tTitle)
        {
            if (tTitle.Text.Length > MAXIMUM_TITLE_LENGTH | tTitle.Text.Length == MINIMUM_TITLE_LENGTH)
                tTitle.BorderBrush = INVALID_BORDER_COLOR;
            else if (!tTitle.Text.Equals(Title))
                tTitle.BorderBrush = VALID_BORDER_COLOR;
            else
                tTitle.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        internal void ChangeDescription(TextBox tDescription)
        {
            if (tDescription.Text.Length > MAXIMUM_DESCRIPTION_LENGTH)
                tDescription.BorderBrush = INVALID_BORDER_COLOR;
            else if (!tDescription.Text.Equals(Description))
                tDescription.BorderBrush = VALID_BORDER_COLOR;
            else
                tDescription.BorderBrush = ORIGINAL_BORDER_COLOR;
        }

        internal void ChangeDueDate(TextBox tDueDate)
        {
            if (!tDueDate.Equals(DueDate))
                tDueDate.BorderBrush = VALID_BORDER_COLOR;
            else
                tDueDate.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    

        internal void ChangeTaskAssignee(TextBox tTaskAssignee)
        {
            if (!tTaskAssignee.Equals(TaskAssigneeUsername))
                tTaskAssignee.BorderBrush = VALID_BORDER_COLOR;
            else
                tTaskAssignee.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    }
}
