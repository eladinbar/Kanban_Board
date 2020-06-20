using Presentation.Model;
using Presentation.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml. Used to login existing users open the registration window for new ones.
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainViewModel ViewModel;

        /// <summary>
        /// A main constructor for the program start up.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
        }
        /// <summary>
        /// A constructor to re-open the main window form registration or board window.
        /// </summary>
        /// <param name="controller">controller for the backend service</param>
        public MainWindow(BackendController controller)
        {
            InitializeComponent();
            ViewModel = new MainViewModel(controller);
            DataContext = ViewModel;
        }
        /// <summary>
        /// Method binding for on click event for login.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = ViewModel.Login();
            if(user != null)
            {
                BoardWindow bw = new BoardWindow(user.Controller, user, user.AssociatedBoard);
                bw.Show();
                Close();
            }
        }

        /// <summary>
        /// Method binding for on click event for opening registration window.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow(ViewModel.Controller);
            registration.Show();
            this.Close();
        }
        /// <summary>
        /// Method binding to receive the typed password.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = PasswordBox.Password;
        }
    }
}
