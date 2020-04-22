using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    /// <summary>
    /// The UserController is the class that controls the functionality of the UserPackage.
    /// Contains the methods for adding and modifying the content of a user.
    /// </summary>
    internal class UserController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private Dictionary<string, User> Users;

        /// <summary>
        /// The user controller constructor. Initializes the 'Users' field by loading all existing data from memory, if no data exists, creates an empty dictionary.
        /// </summary>
        public UserController() {
            Users = new Dictionary<string, User>();
            DalController dalC = new DalController();
            List<DataAccessLayer.User> DALusers = dalC.LoadAllUsers();
            foreach (DataAccessLayer.User DALuser in DALusers) {
                User savedUser = new User(DALuser.Email, DALuser.Password, DALuser.Nickname);
                Users.Add(savedUser.Email, savedUser);
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="email">The email address of the user to register.</param>
        /// <param name="password">The password of the user to register.</param>
        /// <param name="nickname">The nickname of the user to register.</param>
        /// <exception cref="ArgumentException">Thrown when the e-mail address given is already taken by another user.</exception>
        public void Register(string email, string password, string nickname) {
            log.Debug("Register Attempt");
            if (!Users.ContainsKey(email)) {
               ValidatePassword(password);
               ValidateEmail(email);
               User newUser = new User(email, password, nickname);
               Users.Add(email, newUser);
               newUser.Save("Users\\");
            }
            else
               throw new ArgumentException("A user with " + email + " E-mail address already exists, please re-evaluate your information and try again.");
        }

        /// <summary>
        /// Log in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login.</param>
        /// <param name="password">The password of the user to login.</param>
        /// <returns>The User object that logged into the system.</returns>
        /// <exception cref="ArgumentException">Thrown when the e-mail address given is already taken by another user or when the password is incorrect.</exception>
        public User Login (string email, string password) {
            log.Warn("Login Attempt with " + email);
            if (!Users.ContainsKey(email))
            {
                log.Error("Not registered user login attempt");
                throw new ArgumentException(email + " does not exist in the database, please register and try again.");
            }
            else if (!Users[email].Password.Equals(password))
            {
                log.Warn("Login attempt with wrong Password");
                throw new ArgumentException("Incorrect password. Please try again.");
            }
            else
                Users[email].Login();
            return Users[email];
        }

        /// <summary>        
        /// Log out a logged in user.
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        public void Logout (string email) {
            Users[email].Logout();
        }

        /// <summary>
        /// Changes the user's password with a new one if the old password given matches with his current one.
        /// </summary>
        /// <param name="email">The email address of the user requesting a password change.</param>
        /// <param name="oldPassword">The password the user is willing to change.</param>
        /// <param name="newPassword">The password the user is willing to change to.</param>
        /// <exception cref="ArgumentException">Thrown when the old password does not match the current password.</exception>
        public void ChangePassword (string email, string oldPassword, string newPassword) {
            if (Users[email].Password.Equals(oldPassword)) {
                ValidatePassword(newPassword);
                Users[email].ChangePassword(newPassword);
                Users[email].Save("Users\\");
            }
            else
                throw new ArgumentException("Old password does not match the current password. Please try again.");
        }

        /// <summary>
        /// Checks if a given password fits certain criteria:
        /// 1. Checks that the password length is within the appropriate bounds.
        /// 2. Checks that the password consists of alphanumerical characters only.
        /// 3. Checks if the password contains at least 1 digit, lowercase letter and uppercase letter
        /// </summary>
        /// <param name="password">The password the user would like to use.</param>
        /// <exception cref="ArgumentException">Thrown when the password given doesn't fit the criteria.</exception>
        private void ValidatePassword (string password) {
            if (password.Length < 4 | password.Length > 20 ||
            !Regex.IsMatch(password, "^[a-zA-Z0-9]*$") |
            !password.Any(char.IsDigit) | !password.Any(char.IsLower) | !password.Any(char.IsUpper))
                throw new ArgumentException("A user password must be in length of 4 to 20 characters and must include at least one uppercase letter, one lowercase letter and a number.");
        }

        /// <summary>
        /// Checks if a given e-mail address fits certain criteria by dividing it into 5 parts:
        /// 1. The local part - checks that the user inputs only alphanumerical characters and/or the "._-" characters.
        /// 2. The second part consists of the literal @ character.
        /// 3. The third part is the domain part - all characters to provide a domain name are available.
        /// 4. The fourth part is the dot character.
        /// 5. The fifth and final part is the top level domain: allowed to consist of 2 to 18 characters and multiple parts.
        /// </summary>
        /// <param name="email">The email address the user intends to be indetified with.</param>
        /// <exception cref="ArgumentException">Thrown when the email address given doesn't fit the criteria.</exception>
        private void ValidateEmail (string email) {
            if (!Regex.IsMatch(email, @"[a-zA-Z0-9._-]+@[a-zA-Z0-9-]+\.[a-zA-Z.]{2,18}"))
                throw new ArgumentException(email + " is invalid, please use only alphanumerical characters and consult the following form *example@gmail.com*");
        }
    }
}
