using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{

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
        /// Invokes a login quary to the backend.
        /// </summary>
        /// <returns>A UserModel if login is successfull.</returns>
        internal UserModel Login()
        {
            Message = "";
            try
            {
                if(Email.Equals(String.Empty) || Password.Equals(String.Empty))
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
        /// A constructor when reopening the mainWindow.
        /// </summary>
        /// <param name="controller"></param>
        public MainViewModel(BackendController controller)
        {
            Controller = controller;
            Email = "";
            Password = "";
            Message = "";
        }
    }
}
