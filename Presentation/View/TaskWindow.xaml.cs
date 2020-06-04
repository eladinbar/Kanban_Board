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
        private UserModel userModel;

        public TaskWindow(TaskModel taskWindow, UserModel userModel)
        {
            InitializeComponent();
            DataContext = new TaskViewModel(taskWindow);
            taskViewModel = (TaskViewModel)DataContext;
            userModel = userModel;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (true) { //if(fields.contains(invalid))
                MessageBox.Show("Some fields were assigned invalid values. \n" +
                "Please review your changes and try again.", "Invalid fields", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (true) { //if (assignee.changed)
                MessageBox.Show("You are trying to change the task assignee. \n  " +
                "Confirming your changes will prevent you from making any further adjustments to this task. \n" +
                "Would you like to proceed?", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
            }
            else {
                MessageBox.Show("Task data updated successfully!");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            BoardWindow Board = new BoardWindow();
        }
    }
}
