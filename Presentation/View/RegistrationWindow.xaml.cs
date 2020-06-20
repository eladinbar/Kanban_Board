using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel ViewModel;

        /// <summary>
        /// Constructor for initializing the registration window.
        /// </summary>
        /// <param name="controller">The controller for the backend service.</param>
        public RegistrationWindow(BackendController controller)
        {
            InitializeComponent();
            ViewModel = new RegistrationViewModel(controller);
            DataContext = ViewModel;
        }
        /// <summary>
        /// The method for binding on click event on confirm button for registering.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ConfirmRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Register())
            {
                MainWindow main = new MainWindow(ViewModel.Controller);
                main.Show();
                this.Close();
            }
        }

        /// <summary>
        /// The method for binding to change the Host Email box to visible.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void JoinBoardCheck_Checked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The method for binding to change the Host Email box to hidden.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void JoinBoardCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Hidden;
            HostEmailBox.Clear();
        }

        /// <summary>
        /// The method for binding to retrieve the typed password.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordTextBox.Password;
        }

        /// <summary>
        /// The Method for binding to retrieve the typed password.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordConfirmTexBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.PasswordConfirm = PasswordConfirmTexBox.Password;
        }

        /// <summary>
        /// The Method for binding to cancel registration and open the main window.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(ViewModel.Controller);
            main.Show();
            this.Close();
        }
    }
}
