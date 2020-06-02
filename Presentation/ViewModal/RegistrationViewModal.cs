using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModal
{
    public class RegistrationViewModal : NotifiableObject
    {
        private BackendController controller;

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanging("Email");
            }

        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanging("Password");
            }
        }

        private string _passwordConfirm;
        private string PasswordConfirm
        {
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                RaisePropertyChanging("PasswordConfirm");
            }
        }

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                RaisePropertyChanging("Nickname");
            }
        }

        private string _hostEmail;
        public string HostEmail
        {
            get => _hostEmail;
            set
            {
                _hostEmail = value;
                RaisePropertyChanging("HostEmail");
            }
        }

        public RegistrationViewModal(BackendController controller)
        {
            this.controller = controller;
        }
    }
}
