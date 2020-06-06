using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private TaskViewModel ViewModel;

        public TaskWindow(TaskModel taskModel, bool isAssignee)
        {
            InitializeComponent();
            ViewModel = new TaskViewModel(taskModel, isAssignee);
            DataContext = ViewModel;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            TaskViewModel.Border[] validFields = ViewModel.ConfirmChangesValidity(tTitle.BorderBrush, tDescription.BorderBrush,
                                                                                  tDueDate.BorderBrush, tTaskAssignee.BorderBrush);
            if (!validFields.Contains(TaskViewModel.Border.Red)) {
                ViewModel.UpdateTask(validFields);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Title_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangeTitle(tTitle);
        }

        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangeDescription(tDescription);
        }

        private void DueDate_Changed(object sender, TextChangedEventArgs e) {
            ViewModel.ChangeDueDate(tDueDate);
        }

        private void TaskAssignee_Changed(object sender, TextChangedEventArgs e) {
            ViewModel.ChangeTaskAssignee(tTaskAssignee);
        }
    }
}
