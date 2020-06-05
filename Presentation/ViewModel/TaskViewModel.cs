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
        public int ID { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Message { get => Message; set { Message = value; RaisePropertyChanged("Message"); } }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; set; }
        public DateTime LastChangedDate { get; }
        public string TaskAssigneeUsername { get; set; }
        public string TaskAsigneeNickname { get; set; }
        private BackendController Controller;
        private TaskModel Task;
        private UserModel TaskAssignee;
        public readonly SolidColorBrush INVALID_BACKGROUND_COLOR = new SolidColorBrush(Colors.Red);
        public readonly SolidColorBrush VALID_BACKGROUND_COLOR = new SolidColorBrush(Colors.CornflowerBlue);
        public readonly SolidColorBrush EDITED_BACKGROUND_COLOR = new SolidColorBrush(Colors.Green);

        //get {return new SolidColorBrush(Task.DueDate.CompareTo(DateTime.Now) ? Colors.Blue : Colors.Red);} }


        //Constructor
        public TaskViewModel(TaskModel Task) {
             this.Controller = Task.Controller;
             this.Task = Task;
             this.ID = Task.ID;
             this.Title = Task.Title;
             this.Description = Task.Description;
             this.CreationTime = Task.CreationTime;
             this.DueDate = Task.DueDate;
             this.LastChangedDate = Task.LastChangedDate;
             this.TaskAssignee = Task.TaskAssignee;
             this.TaskAssigneeUsername = TaskAssignee.Username;
             this.TaskAsigneeNickname = TaskAssignee.Nickname;
        }

        internal void ChangeTaskAssignee(TextBox tTaskAssignee, Brush borderBrush)
        {
            throw new NotImplementedException();
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
            this.DueDate = dueDate;
            RaisePropertyChanging("DueDate");
            try {
                Controller.UpdateTaskDueDate(email, columnOrdinal, taskId, dueDate);
                this.DueDate = dueDate;
                RaisePropertyChanged("DueDate");
            }
            catch(Exception ex) {
                Message = ex.Message;
            }
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

        public void UpdateTask() {
            
        }

        public bool ConfirmChangesValidity() { //All changeable parameters?
            if (true)
            { //if(fields.contains(invalid))
                MessageBox.Show("Some fields were assigned invalid values. \n" +
                "Please review your changes and try again.", "Invalid fields", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (true)
            { //if (assignee.changed)
                MessageBox.Show("You are trying to change the task assignee. \n  " +
                "Confirming your changes will prevent you from making any further adjustments to this task. \n" +
                "Would you like to proceed?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            }
            else
            {
                MessageBox.Show("Task data updated successfully!");
                return true;
            }
            return false;
        }

        internal void ChangeTitle(TextBox tTitle)
        {
            if (tTitle.Text.Length > 50 | tTitle.Text.Length < 1)
                tTitle.BorderBrush = INVALID_BACKGROUND_COLOR;
            else if (!tTitle.Text.Equals(Title))
                tTitle.BorderBrush = EDITED_BACKGROUND_COLOR;
            else
                tTitle.BorderBrush = VALID_BACKGROUND_COLOR;
        }

        internal void ChangeDescription(TextBox tDescription)
        {
            if (tDescription.Text.Length > 300)
                tDescription.BorderBrush = INVALID_BACKGROUND_COLOR;
            else if (!tDescription.Text.Equals(Description))
                tDescription.BorderBrush = EDITED_BACKGROUND_COLOR;
            else
                tDescription.BorderBrush = VALID_BACKGROUND_COLOR;
        }

        internal void ChangeDueDate(TextBox tDueDate)
        {
            //if (tDueDate.Text.) //DateTime.Now something something
            //    tDescription.BorderBrush = INVALID_BACKGROUND_COLOR;
            //else if (!tDueDate.Text.Equals(DueDate))
            //    tDescription.BorderBrush = EDITED_BACKGROUND_COLOR;
            //else
            //    tDescription.BorderBrush = VALID_BACKGROUND_COLOR;
        }
    }
}
