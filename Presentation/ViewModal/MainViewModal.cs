using Presentation.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModal
{
    public class MainViewModal : NotifiableObject
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

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged("Massage");
            }
        }

        internal UserModal Login()
        {
            Message = "";
            try
            {
                return Controller.Login(Email, Password);
            }
            catch(Exception ex)
            {
                Message = ex.Message;
                return null;
            }
        }

        public MainViewModal()
        {
            Controller = new BackendController();
            Email = "Email";
            Password = "";
            Message = "";
        }
    }
}
