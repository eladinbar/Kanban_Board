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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel viewModal;

        public RegistrationWindow(BackendController controller)
        {
            InitializeComponent();
            viewModal = new RegistrationViewModel(controller);
            DataContext = viewModal;
        }

        private void ConfirmRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (viewModal.Register())
            {
                MainWindow main = new MainWindow(viewModal.Controller);
                main.Show();
                this.Close();
            }
        }

        private void JoinBoardCheck_Checked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Visible;
        }

        private void JoinBoardCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Hidden;
            HostEmailBox.Clear();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModal.Password = PasswordTextBox.Password;
            PasswordTextBox.PasswordChar = RandomizeCher();
        }

        private void PasswordConfirmTexBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModal.PasswordConfirm = PasswordConfirmTexBox.Password;
            PasswordConfirmTexBox.PasswordChar = RandomizeCher();
        }

        private char RandomizeCher()
        {
            Random rnd = new Random();
            return (char) rnd.Next(0, 65536);
        }

        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(viewModal.Controller);
            main.Show();
            this.Close();
        }
    }
}
