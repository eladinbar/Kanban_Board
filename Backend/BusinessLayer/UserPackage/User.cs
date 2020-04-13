using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    internal class User : PersistedObject<DataAccessLayer.User>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private string Nickname;
        private string Email;
        private string Password;
        private bool Logged_in;
        
        public User (string email, string password, string nickname) {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
            Logged_in = false;
        }

        public string email {
            get { return Email; }
        }

        public string password {
            get { return Password; }
        }

        public string nickname {
            get { return Nickname; }
        }

        public void Login() {
            Logged_in = true;
        }

        public void Logout() {
            Logged_in = false;
        }

        public void ChangePassword (string newPassword) {
            Password = newPassword;
        }

        public DataAccessLayer.User ToDalObject() {
            return new DataAccessLayer.User(this.Email, this.Password, this.Nickname);
        }

        public void Save(string path) {
            ToDalObject().Save(path);
        }
    }
}
