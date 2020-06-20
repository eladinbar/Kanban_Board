using Presentation.ViewModel;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel viewModal;

        /// <summary>
        /// Constructor for iniselizing the registration window.
        /// </summary>
        /// <param name="controller">controller for the backend service</param>
        public RegistrationWindow(BackendController controller)
        {
            InitializeComponent();
            viewModal = new RegistrationViewModel(controller);
            DataContext = viewModal;
        }
        /// <summary>
        /// a Method for binding on click event on confirm button for registering.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ConfirmRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (viewModal.Register())
            {
                MainWindow main = new MainWindow(viewModal.Controller);
                main.Show();
                this.Close();
            }
        }

        /// <summary>
        /// a Method for binding to change the Host Email box to visable
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void JoinBoardCheck_Checked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// a Method for binding to change the Host Email box to hidden.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void JoinBoardCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            HostEmailPanal.Visibility = Visibility.Hidden;
            HostEmailBox.Clear();
        }

        /// <summary>
        /// a Method for binding to retrive the typed password
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModal.Password = PasswordTextBox.Password;
        }

        /// <summary>
        /// a Method for binding to retrive the typed password
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordConfirmTexBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModal.PasswordConfirm = PasswordConfirmTexBox.Password;
        }

        /// <summary>
        /// a Method for binding to cancal registarion and open the MainWindow
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void CancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow(viewModal.Controller);
            main.Show();
            this.Close();
        }
    }
}
