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
        private TaskViewModel taskViewModel;

        public TaskWindow(TaskModel taskModel)
        {
            InitializeComponent();
            DataContext = new TaskViewModel(taskModel);
            taskViewModel = (TaskViewModel)DataContext;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            bool valid = taskViewModel.ConfirmChangesValidity();
            if (valid) {
                taskViewModel.UpdateTask(); //
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide(); //
        }

        private void Title_Changed(object sender, TextChangedEventArgs e)
        {
            taskViewModel.ChangeTitle(tTitle);
        }

        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            taskViewModel.ChangeDescription(tDescription);
        }

        private void DueDate_Changed(object sender, TextChangedEventArgs e) {
            taskViewModel.ChangeDueDate(tDueDate);
        }

        private void TaskAssignee_Changed(object sender, TextChangedEventArgs e) {
            taskViewModel.ChangeTaskAssignee(tTaskAssignee);
        }
    }
}
