using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Presentation.ViewModal
{
    public class RegistrationViewModal : NotifiableObject
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

        private const string HostEmailDefualt = "";
        private string _hostEmail = HostEmailDefualt;
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

        private string _responseMassage = "";
        public string ResponseMassage
        {
            get => _responseMassage;
            set
            {
                _responseMassage = value;
                RaisePropertyChanged("ResponseMassage");
            }
        }

        public RegistrationViewModal(BackendController controller)
        {
            this.Controller = controller;
        }

        public bool Register()
        {
            try
            {
                if (Password.Equals(PasswordConfirm) && !HostEmail.Equals(String.Empty))
                {
                    Controller.Register(Email, Password, Nickname, HostEmail);
                    return true;
                }
                else if (Password.Equals(PasswordConfirm) && HostEmail.Equals(String.Empty))
                {
                    Controller.Register(Email, Password, Nickname, null);
                    return true;
                }
                else
                {
                    ResponseMassage = "Password Entrys does not match";
                    return false;
                }
            }
            catch(Exception ex)
            {
                ResponseMassage = ex.Message;
                return false;
            }
        }
    }
}
