using Presentation.Modal;
using Presentation.ViewModal;
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

        private MainViewModal viewModal;

        public MainWindow()
        {
            InitializeComponent();
            viewModal = new MainViewModal();
            DataContext = viewModal;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModal user = viewModal.Login();
            if(user != null)
            {
                BoardWindow bw = new BoardWindow(user);
                bw.Show();
                Close();
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow(viewModal.Controller);
            registration.Show();
            this.Close();
        }
    }
}
