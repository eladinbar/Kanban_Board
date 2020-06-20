using Presentation.Model;
using System;

namespace Presentation.ViewModel
{
    /// <summary>
    /// The data context of the main window.
    /// </summary>
    public class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        private string _message = "this is a message";
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// Invokes a login query to the backend.
        /// </summary>
        /// <returns>Returns the requested UserModel if the login was successful.</returns>
        internal UserModel Login()
        {
            Message = "";
            try
            {
                if(Email.Equals(string.Empty) || Password.Equals(string.Empty))
                {
                    Message = "Please enter Email and Password";
                    return null;
                }
                return Controller.Login(Email, Password);
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// A constructor for startup.
        /// </summary>
        public MainViewModel()
        {
            Controller = new BackendController();
            Email = "";
            Password = "";
            Message = "";
        }

        /// <summary>
        /// A constructor when reopening the main window.
        /// </summary>
        /// <param name="controller">The controller this view model uses to interact with the backend.</param>
        public MainViewModel(BackendController controller)
        {
            Controller = controller;
            Email = "";
            Password = "";
            Message = "";
        }
    }
}
