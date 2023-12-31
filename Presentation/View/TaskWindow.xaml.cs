﻿using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private const bool NEW_TASK = true;

        private TaskViewModel ViewModel;
        public string LastClickedButton { get; private set; }

        /// <summary>
        /// The task window constructor. Initializes the window and creates its respective data context with the required information given from the board window.
        /// </summary>
        /// <param name="backendController">The controller this task uses to communicate with the backend.</param>
        /// <param name="taskModel">The task this window represents.</param>
        /// <param name="columnOrdinal">The column ordinal the task this window represents belongs to.</param>
        /// <param name="isAssignee">The token used to decide whether the current user can make any task modifications.</param>
        public TaskWindow(BackendController backendController, TaskModel taskModel, bool isAssignee, UserModel currentUser)
        {
            InitializeComponent();
            ViewModel = new TaskViewModel(backendController, taskModel, isAssignee, currentUser);
            DataContext = ViewModel;
            ControlDataVisiblity(!NEW_TASK);
        }

        /// <summary>
        /// The task window constructor. Initializes the window and creates its respective data context with the required information given from the board window.
        /// </summary>
        /// <param name="backendController">The controller this task uses to communicate with the backend.</param>
        /// <param name="assigneeEmail">The email of the creator of the task.</param>
        public TaskWindow(BackendController backendController, UserModel currentUser)
        {
            InitializeComponent();
            ViewModel = new TaskViewModel(backendController, currentUser);
            DataContext = ViewModel;
            ControlDataVisiblity(NEW_TASK);
        }

        /// <summary>
        /// Adjusts the task window's relevant fields' and buttons' visibility on creation according to task status and ownership.
        /// </summary>
        /// <param name="newTask"></param>
        private void ControlDataVisiblity(bool newTask) {
            ViewModel.ControlFieldVisibility(newTask, txtOwnership, lTaskID, txtHintDescription, txtBlockDescription, dueMessage);
            ViewModel.ControlButtonsVisibility(newTask, Confirm, Cancel, OK, AddTask);
        }

        /// <summary>
        /// Ensures all fields are valid, then updates them in the Backend then closes. Otherwise allows re-evaluation.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            LastClickedButton = Confirm.Content.ToString();
            List<TaskViewModel.BorderColor> validFields = ViewModel.ConfirmChangesValidity(txtTitle.BorderBrush, txtDescription.BorderBrush,
                                                                                  dpDueDate.BorderBrush, txtTaskAssignee.BorderBrush);
            if (!validFields.Contains(TaskViewModel.BorderColor.Red))
            {
                try
                {
                    ViewModel.UpdateTask(validFields, txtTitle.Text, txtDescription.Text, (DateTime)dpDueDate.SelectedDate, txtTaskAssignee.Text);
                    if (validFields.Contains(TaskViewModel.BorderColor.Green))
                        MessageBox.Show("Task data was updated successfully!");
                    this.Close();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }     
        }

        /// <summary>
        /// Discards all changes made to the task and closes.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            LastClickedButton = Cancel.Content.ToString();
            this.Close();
        }

        /// <summary>
        /// Discards all changes made to the task and closes.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a cancel event.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (LastClickedButton==null) LastClickedButton = Cancel.Content.ToString();
        }

        /// <summary>
        /// Adds a new task to the Kanban Board.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            LastClickedButton = AddTask.Content.ToString();
            List<TaskViewModel.BorderColor> validFields = ViewModel.ConfirmChangesValidity(txtTitle.BorderBrush, txtDescription.BorderBrush,
                                                                                      dpDueDate.BorderBrush, txtTaskAssignee.BorderBrush);
            if (!validFields.Contains(TaskViewModel.BorderColor.Red))
            {
                try {
                    ViewModel.NewTask(txtTaskAssignee.Text, txtTitle.Text, txtDescription.Text, (DateTime)dpDueDate.SelectedDate);
                    MessageBox.Show("Task was added successfully to board " + ViewModel.CurrentUser.AssociatedBoard);
                    this.Close();
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Update the "txtTitle" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Provides data for the TextBoxBase.TextChanged event.</param>
        private void Title_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedTitle(txtTitle, titleMessage, txtHintTitle, txtBlockTitle);
        }

        /// <summary>
        /// Update the "txtDescription" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Provides data for the TextBoxBase.TextChanged event.</param>
        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedDescription(txtDescription, descMessage, txtHintDescription, txtBlockDescription);
        }

        /// <summary>
        /// Update the "dpDueDate" SelectedDate state according to the changes made to the field.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Provides data for the Selector.SelctionChanged event.</param>
        private void DueDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.ChangedDueDate(dpDueDate, dueMessage, txtBlockDueDate);
        }

        /// <summary>
        /// Update the "txtTaskAssignee" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Provides data for the TextBoxBase.TextChanged event.</param>
        private void TaskAssignee_Changed(object sender, TextChangedEventArgs e) {
            ViewModel.ChangedTaskAssignee(txtTaskAssignee, txtBlockTaskAssignee);
        }
    }
}
