using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    /// <summary>
    ///Represents a user profile in the Kanban Board system.
    /// </summary>
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

        /// <summary>
        /// Log in an existing user.
        /// </summary>
        public void Login() {
            Logged_in = true;
        }

        /// <summary>
        /// Log out a logged in user.
        /// </summary>
        public void Logout() {
            Logged_in = false;
        }

        /// <summary>
        /// Changes the user's password with a new one.
        /// </summary>
        public void ChangePassword (string newPassword) {
            Password = newPassword;
        }

        /// <summary>
        /// Transforms the user to its corresponding DalObject.
        /// </summary>
        /// <returns>
        /// Returns a DataAccessLayer.User object.
        /// </returns>
        public DataAccessLayer.User ToDalObject() {
            return new DataAccessLayer.User(this.Email, this.Password, this.Nickname);
        }

        /// <summary>
        /// The method in the BusinessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public void Save(string path) {
            log.Info("User.save was called");
            ToDalObject().Save(path);
        }
    }
}
