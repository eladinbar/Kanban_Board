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

        private MainViewModel viewModal;

        /// <summary>
        /// A main constructer for the program start up.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            viewModal = new MainViewModel();
            DataContext = viewModal;
        }
        /// <summary>
        /// A constructor to re-open the main window form registration or board Window.
        /// </summary>
        /// <param name="controller">controller for the backend service</param>
        public MainWindow(BackendController controller)
        {
            InitializeComponent();
            viewModal = new MainViewModel(controller);
            DataContext = viewModal;
        }
        /// <summary>
        /// Method binding for on click event for login.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = viewModal.Login();
            if(user != null)
            {
                BoardWindow bw = new BoardWindow(user.Controller, user, user.AssociatedBoard);
                bw.Show();
                Close();
            }
        }

        /// <summary>
        /// Method binding for on click event for opening regestration window.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow(viewModal.Controller);
            registration.Show();
            this.Close();
        }
        /// <summary>
        /// Mothod binding to resive the typed password.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModal.Password = PasswordBox.Password;
        }
    }
}
