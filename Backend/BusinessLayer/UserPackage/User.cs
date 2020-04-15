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

        public string Nickname { get; private set; }
        public string Email { get; }
        public string Password { get; private set; }
        public bool Logged_in { get; private set; }
        
        public User (string email, string password, string nickname) {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
            Logged_in = false;
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
            log.Info("User.save was called");
            ToDalObject().Save(path);
        }
    }
}
