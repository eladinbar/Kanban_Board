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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        RegistrationViewModal viewModal;

        public RegistrationWindow(BackendController controller)
        {
            InitializeComponent();
            viewModal = new RegistrationViewModal(controller);
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
    }
}
