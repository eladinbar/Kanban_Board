using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    /// <summary>
    /// Represents a user profile in the Kanban Board system.
    /// </summary>
    internal class User
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string Nickname { get; private set; }
        public string Email { get; }
        public string Password { get; private set; }
        public bool Logged_in { get; private set; }
        public DalUser DalUser { get; private set { DalUser. } }

        /// <summary>
        /// A public constructor that creates a new user and initializes all of its fields.
        /// </summary>
        /// <param name="email">The email the user will be created with.</param>
        /// <param name="password">The password that will be used to sign in the user.</param>
        /// <param name="nickname">The nickname to be associated with this user.</param>
        public User (string email, string password, string nickname) {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
            Logged_in = false;
            DalUser = new DalUser(email, password, nickname);
            DalUser.
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
        /// <returns>Returns a Data Access Layer User.</returns>
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
