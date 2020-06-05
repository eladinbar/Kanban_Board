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
        private BoardModel boardModel;

        public TaskWindow(TaskModel taskWindow, UserModel userModel)
        {
            InitializeComponent();
            DataContext = new TaskViewModel(taskWindow);
            taskViewModel = (TaskViewModel)DataContext;
            this.userModel = userModel;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            taskViewModel.ConfirmChangesValidity();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //BoardWindow Board = new BoardWindow();
            this.Hide();
        }

        private void Title_TextChanged(object sender, TextChangedEventArgs e)
        {
            taskViewModel.UpdateTaskTitle(userModel.Username, boardModel.Columns)
        }
    }
}
