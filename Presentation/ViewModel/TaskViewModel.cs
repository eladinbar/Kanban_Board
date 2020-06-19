﻿using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime LastChangedDate { get; set; }
        public string AssigneeEmail { get; set; }
        public bool IsAssignee { get; set; }

        /// <summary>
        /// A TaskViewModel constructor for an existing task. Initializes all bound fields as well as its respective TaskModel and BackendController.
        /// </summary>
        /// <param name="backendController">The controller this task uses to communicate with the backend.</param>
        /// <param name="task">The TaskModel representing this TaskViewModel.</param>
        /// <param name="isAssignee">A verification variable to ensure task editing can only be performed by its assignee.</param>
        public TaskViewModel(BackendController backendController, TaskModel task, bool isAssignee) { //columnOrdinal is unnecessary?
             this.Controller = backendController;
             this.Task = task;
             this.ID = task.ID;
             this.Title = task.Title;
             this.Description = task.Description;
             this.CreationTime = task.CreationTime;
             this.DueDate = task.DueDate;
             this.LastChangedDate = task.LastChangedDate;
             this.AssigneeEmail = task.AssigneeEmail;
             this.IsAssignee = isAssignee;
        }

        /// <summary>
        /// A TaskViewModel constructor for a new task. Assigns default values to all bound fields and initializes the BackendController field.
        /// </summary>
        /// <param name="backendController">The controller this task uses to communicate with the backend.</param>
        /// <param name="assigneeEmail">The email of the creator of the task.</param>
        public TaskViewModel(BackendController backendController, string assigneeEmail)
        {
            this.Controller = backendController;
            this.AssigneeEmail = assigneeEmail;
            this.DueDate = DateTime.Now.AddDays(1);
            this.CreationTime = DateTime.Now;
            this.LastChangedDate = DateTime.Now;
            this.IsAssignee = true;
        }

        public void ControlButtonsVisibility(bool newTask, Button confirm, Button cancel, Button ok, Button addTask) {
            if (newTask) {
                confirm.Visibility = Visibility.Collapsed;
                addTask.Visibility = Visibility.Visible;
            }
            if (!IsAssignee) {
                confirm.Visibility = Visibility.Collapsed;
                cancel.Visibility = Visibility.Collapsed;
                ok.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Updates all task fields that were modified by the user in the GUI.
        /// </summary>
        /// <param name="validFields">Represents the state of all fields in the task window.</param>
        /// <param name="title">The title of the task to update.</param>
        /// <param name="description">The description of the task to update.</param>
        /// <param name="dueDate">The due date of the task to update.</param>
        /// <param name="taskAssignee">The assignee of the task to update.</param>
        public void UpdateTask(List<BorderColor> validFields, string title, string description, DateTime dueDate, string taskAssignee) {
            if (validFields[Convert.ToInt32(Update.Title)] == BorderColor.Green)
            {
                Task.UpdateTaskTitle(title);
                this.Title = title;
            }
            if (validFields[Convert.ToInt32(Update.Description)] == BorderColor.Green)
            {
                Task.UpdateTaskDescription(description);
                this.Description = description;

            }
            if (validFields[(int)Update.DueDate] == BorderColor.Green)
            {
                Task.UpdateTaskDueDate(dueDate);
                this.DueDate = dueDate;
            }
            if (validFields[(int)Update.TaskAssignee] == BorderColor.Green)
            {
                Task.AssignTask(taskAssignee);
                this.AssigneeEmail = taskAssignee;
            }
        }

        /// <summary>
        /// Adds a new task to the Kanban board.
        /// </summary>
        /// <param name="assigneeEmail">The email associated with the user to assign this task to.</param>
        /// <param name="title">The title to add this task with.</param>
        /// <param name="description">The description to add this task with.</param>
        /// <param name="dueDate">The due date this task will be due by.</param>
        public void NewTask(string assigneeEmail, string title, string description, DateTime dueDate)
        {
            Task = Controller.AddTask(AssigneeEmail, title, description, dueDate);
            if (AssigneeEmail != assigneeEmail)
                Controller.AssignTask(AssigneeEmail, 0, Task.ID, assigneeEmail);
            this.Task.CurrentUserEmail = this.AssigneeEmail;
            this.ID = Task.ID;                               RaisePropertyChanged("ID");
            this.AssigneeEmail = Task.AssigneeEmail;         RaisePropertyChanged("TaskAssignee");
            this.Title = Task.Title;                         RaisePropertyChanged("Title");
            this.Description = Task.Description;             RaisePropertyChanged("Description");
            this.DueDate = Task.DueDate;                     RaisePropertyChanged("DueDate");
            this.CreationTime = Task.CreationTime;           RaisePropertyChanged("CreationTime");
            this.LastChangedDate = Task.LastChangedDate;     RaisePropertyChanged("LastChangedDate");            
        }

        /// <summary>
        /// An enum used to represent which update function should be called.
        /// </summary>
        private enum Update { Title=0, Description=1, DueDate=2, TaskAssignee=3 }
        
        /// <summary>
        /// An enum used to represent the state of a field in the task window.
        /// </summary>
        public enum BorderColor { Blue=0, Green=1, Red=2 }

        /// <summary>
        /// Checks if all user changes are valid. Presents relevant message boxes for every different case.
        /// </summary>
        /// <param name="fields">An array representing the state of each of the Task Window's fields.</param>
        /// <returns>Returns a list of borders representing the state of each field in the task window.</returns>
        public List<BorderColor> ConfirmChangesValidity(params Brush[] fields) {
            List<BorderColor> validFields = ValidFields(fields.Take(fields.Length - 1));
            if (validFields.Contains(BorderColor.Red)) //Checks all fields except TaskAssignee
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
                        validFields.Add(BorderColor.Green);
                    else
                        validFields.Add(BorderColor.Red);
                }
                else
                    validFields.Add(BorderColor.Blue);
            }
            return validFields;
        }

        /// <summary>
        /// Creates a list representing the state of all of the window's fields.
        /// </summary>
        /// <param name="fields">The fields' list to check.</param>
        /// <returns>Returns a list of borders representing the state of each field in the task window.</returns>
        private List<BorderColor> ValidFields(IEnumerable<Brush> fields) {
            List<BorderColor> validFields = new List<BorderColor>();
            foreach (Brush brush in fields) {
                if (brush.Equals(Brushes.Red))
                    validFields.Add(BorderColor.Red);
                else if (brush.Equals(Brushes.Green))
                    validFields.Add(BorderColor.Green);
                else
                    validFields.Add(BorderColor.Blue);
            }     
            return validFields;
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtTitle" text box according to its state.
        /// </summary>
        /// <param name="txtTitle">The text box to assign the state to.</param>
        /// <param name="titleMessage">The message to present to the user in case the 'Title' field is invalid.</param>
        /// <param name="txtHintTitle">The text block displayed to hint at the desired field content.</param>
        internal void ChangedTitle(TextBox txtTitle, Label titleMessage, TextBlock txtHintTitle, TextBlock txtBlockTitle)
        {
            txtHintTitle.Visibility = Visibility.Visible;
            if (txtTitle.Text.Length > 0)
                txtHintTitle.Visibility = Visibility.Hidden;
            if (!IsAssignee) {
                txtBlockTitle.Visibility = Visibility.Visible;
                txtTitle.Visibility = Visibility.Collapsed;
            }
            if (txtTitle.Text.Length > MAXIMUM_TITLE_LENGTH | txtTitle.Text.Length == MINIMUM_TITLE_LENGTH)
            {
                txtTitle.BorderBrush = INVALID_BORDER_COLOR;
                titleMessage.Content = "The title cannot be empty or exceed 50 characters.";
            }
            else if (!txtTitle.Text.Equals(Title))
            {
                txtTitle.BorderBrush = VALID_BORDER_COLOR;
                titleMessage.Content = "";
            }
            else
            {
                txtTitle.BorderBrush = ORIGINAL_BORDER_COLOR;
                titleMessage.Content = "";
            }
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtDescription" text box according to its state.
        /// </summary>
        /// <param name="txtDescription">The text box to assign the state to.</param>
        /// <param name="descMessage">The message to present to the user in case the 'Description' field is invalid.</param>
        /// <param name="txtHintDescription">The text block displayed to hint at the desired field content.</param>
        internal void ChangedDescription(TextBox txtDescription, Label descMessage, TextBlock txtHintDescription, TextBlock txtBlockDescription)
        {
            txtHintDescription.Visibility = Visibility.Visible;
            if (txtDescription.Text.Length > 0)
                txtHintDescription.Visibility = Visibility.Hidden;
            if (!IsAssignee)
            {
                txtBlockDescription.Visibility = Visibility.Visible;
                txtDescription.Visibility = Visibility.Collapsed;
            }
            if (txtDescription.Text.Length > MAXIMUM_DESCRIPTION_LENGTH)
            {
                txtDescription.BorderBrush = INVALID_BORDER_COLOR;
                descMessage.Content = "The description cannot exceed 300 characters.";
            }
            else if (!txtDescription.Text.Equals(Description))
            {
                txtDescription.BorderBrush = VALID_BORDER_COLOR;
                descMessage.Content = "";
            }
            else
            {
                txtDescription.BorderBrush = ORIGINAL_BORDER_COLOR;
                descMessage.Content = "";
            }
        }

        /// <summary>
        /// Assigns the appropriate border to the "dpDueDate" date picker according to its state.
        /// </summary>
        /// <param name="dpDueDate">The date picker to assign the state to.</param>
        /// <param name="dueMessage">The message to present to the user in case the 'Due Date' field is invalid.</param>
        internal void ChangedDueDate(DatePicker dpDueDate, Label dueMessage, TextBlock txtBlockDueDate)
        {
            if (!IsAssignee)
            {
                txtBlockDueDate.Visibility = Visibility.Visible;
                dpDueDate.Visibility = Visibility.Collapsed;
            }
            if (dpDueDate.SelectedDate?.CompareTo(DateTime.Now) < 0)
            {
                dpDueDate.BorderBrush = INVALID_BORDER_COLOR;
                dueMessage.Content = "Due date cannot be set to past time.";
            }
            else if (dpDueDate.SelectedDate?.CompareTo(DueDate.Date) != 0)
            {
                dpDueDate.BorderBrush = VALID_BORDER_COLOR;
                dueMessage.Content = "";
            }
            else
            {
                dpDueDate.BorderBrush = ORIGINAL_BORDER_COLOR;
                dueMessage.Content = "";
            }
        }

        /// <summary>
        /// Assigns the appropriate border to the "txtTaskAssignee" text box according to its state.
        /// </summary>
        /// <param name="txtTaskAssignee">The text box to assign the state to.</param>
        internal void ChangedTaskAssignee(TextBox txtTaskAssignee, TextBlock txtBlockTaskAssignee)
        {
            if (!IsAssignee) {
                txtBlockTaskAssignee.Visibility = Visibility.Visible;
                txtTaskAssignee.Visibility = Visibility.Collapsed;
            }
            if (!txtTaskAssignee.Text.Equals(AssigneeEmail))
                txtTaskAssignee.BorderBrush = VALID_BORDER_COLOR;
            else
                txtTaskAssignee.BorderBrush = ORIGINAL_BORDER_COLOR;
        }
    }
}
