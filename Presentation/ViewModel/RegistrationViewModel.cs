using System;

namespace Presentation.ViewModel
{
    /// <summary>
    /// The data context of the registration window.
    /// </summary>
    public class RegistrationViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                Console.WriteLine(_email);
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
                Console.WriteLine(_password);
                RaisePropertyChanged("Password");
            }
        }

        private string _passwordConfirm;
        public string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                Console.WriteLine(_passwordConfirm);
                RaisePropertyChanged("PasswordConfirm");
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                Console.WriteLine(_nickname);
                RaisePropertyChanged("Nickname");
            }
        }
        
        private string _hostEmail = "";
        public string HostEmail
        {
            get => _hostEmail;
            set
            {
                _hostEmail = value;
                Console.WriteLine(_hostEmail);
                RaisePropertyChanged("HostEmail");
                
            }
        }

        private string _responseMessage = "";
        public string ResponseMessage
        {
            get => _responseMessage;
            set
            {
                _responseMessage = value;
                RaisePropertyChanged("ResponseMessage");
            }
        }

        public RegistrationViewModel(BackendController controller)
        {
            this.Controller = controller;
        }


        /// <summary>
        /// Invokes the register function in the backend.
        /// </summary>
        /// <returns>Returns true if the registraion process was successful, otherwise returns false.</returns>
        public bool Register()
        {
            try
            {
                if (Email.Equals(string.Empty) || Password.Equals(string.Empty) || PasswordConfirm.Equals(string.Empty) || Nickname.Equals(string.Empty))
                {
                    ResponseMessage = "One or more of the fields is empty. Please re-evaluate entered info.";
                    return false;
                }
                else if (Password.Equals(PasswordConfirm) && !HostEmail.Equals(string.Empty))
                {
                    Controller.Register(Email, Password, Nickname, HostEmail);
                    return true;
                }
                else if (Password.Equals(PasswordConfirm) && HostEmail.Equals(string.Empty))
                {
                    Controller.Register(Email, Password, Nickname, "");
                    return true;
                }
                else
                {
                    ResponseMessage = "Password entries do not match";
                    return false;
                }
            }
            catch(Exception ex)
            {
                ResponseMessage = ex.Message;
                return false;
            }
        }
    }
}
