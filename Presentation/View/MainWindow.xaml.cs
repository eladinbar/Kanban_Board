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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModal();
            DataContext = viewModel;
        }
        public MainWindow(BackendController controller)
        {
            InitializeComponent();
            viewModel = new MainViewModal(controller);
            DataContext = viewModel;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModel.Login();
            if(user != null)
            {
                //BoardWindow bw = new BoardWindow(user);
                //bw.Show();
                Close();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow(viewModel.Controller);
            registration.Show();
            this.Close();
        }
    }
}
