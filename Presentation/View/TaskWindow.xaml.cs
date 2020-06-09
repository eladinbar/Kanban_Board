using Presentation.Model;
using Presentation.ViewModel;
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
        private TaskViewModel ViewModel;

        /// <summary>
        /// The task window constructor. Initializes the window and creates its respective data context with the required information given from the board window.
        /// </summary>
        /// <param name="taskModel">The task this window represents.</param>
        /// <param name="columnOrdinal">The column ordinal the task this window represents belongs to.</param>
        /// <param name="isAssignee">The token used to decide whether the current user can make any task modifications.</param>
        public TaskWindow(TaskModel taskModel, int columnOrdinal, bool isAssignee)
        {
            InitializeComponent();
            ViewModel = new TaskViewModel(taskModel, columnOrdinal, isAssignee);
            DataContext = ViewModel;
        }

        /// <summary>
        /// Ensures all fields are valid, then updates them in the Backend then closes. Otherwise allows re-evaluation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<TaskViewModel.BorderColor> validFields = ViewModel.ConfirmChangesValidity(txtTitle.BorderBrush, txtDescription.BorderBrush,
                                                                                  dpDueDate.BorderBrush, txtTaskAssignee.BorderBrush);
            if (!validFields.Contains(TaskViewModel.BorderColor.Red))
            {
                ViewModel.UpdateTask(validFields, txtTitle.Text, txtDescription.Text, dpDueDate.DisplayDate, txtTaskAssignee.Text);
                this.Close();
            }
            
        }

        /// <summary>
        /// Discards all changes made to the task and closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Update the "txtTitle" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Title_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedTitle(txtTitle, titleMessage);
        }

        /// <summary>
        /// Update the "txtDescription" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedDescription(txtDescription, descMessage);
        }

        /// <summary>
        /// Update the "dpDueDate" SelectedDate state according to the changes made to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DueDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.ChangedDueDate(dpDueDate, dueMessage);
        }

        /// <summary>
        /// Update the "txtTaskAssignee" TextBox state according to the changes made to the field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskAssignee_Changed(object sender, TextChangedEventArgs e) {
            ViewModel.ChangedTaskAssignee(txtTaskAssignee, assigneeMessage);
        }
    }
}
