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

        public TaskWindow(TaskModel taskModel, int columnOrdinal, bool isAssignee)
        {
            InitializeComponent();
            ViewModel = new TaskViewModel(taskModel, columnOrdinal, isAssignee);
            DataContext = ViewModel;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            List<TaskViewModel.Border> validFields = ViewModel.ConfirmChangesValidity(tTitle.BorderBrush, tDescription.BorderBrush,
                                                                                  dpDueDate.BorderBrush, tTaskAssignee.BorderBrush);
            if (!validFields.Contains(TaskViewModel.Border.Red))
            {
                ViewModel.UpdateTask(validFields, tTitle.Text, tDescription.Text, dpDueDate.DisplayDate, tTaskAssignee.Text);
                this.Close();
            }
            
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Title_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedTitle(tTitle);
        }

        private void Description_Changed(object sender, TextChangedEventArgs e)
        {
            ViewModel.ChangedDescription(tDescription);
        }

        private void DueDate_Changed(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.ChangedDueDate(dpDueDate);
        }

        private void TaskAssignee_Changed(object sender, TextChangedEventArgs e) {
            ViewModel.ChangedTaskAssignee(tTaskAssignee);
        }

        
    }
}
