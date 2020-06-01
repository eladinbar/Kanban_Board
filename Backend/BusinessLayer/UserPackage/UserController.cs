using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        private const int MAXIMAL_PASSWORD_LENGTH = 25;
        private const int MINIMAL_PASSWORD_LENGTH = 5;


        private Dictionary<string, User> Users;

        /// <summary>
        /// The user controller constructor. Initializes the 'Users' field by loading all existing data from memory, if no data exists, creates an empty dictionary.
        /// </summary>
        public UserController() {
            Users = new Dictionary<string, User>();
            UserDalController dalC = new UserDalController();
            List<DataAccessLayer.DALOs.DalUser> DALusers = dalC.SelectAllUsers();
            foreach (DataAccessLayer.DALOs.DalUser DALuser in DALusers) {
                User savedUser = new User(DALuser.Email, DALuser.Password, DALuser.Nickname, DALuser.AssociatedBoard);
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
            if (nickname.Length == 0)
                throw new ArgumentException("Cannot register with an empty nickname, please try again.");
            if (!Users.ContainsKey(email)) {
               ValidatePassword(password);
               ValidateEmail(email);
               User newUser = new User(email, password, nickname, email);
               newUser.Save();
               Users.Add(email, newUser);
            }
            else
               throw new ArgumentException("A user with " + email + " E-mail address already exists, please re-evaluate your information and try again.");
        }

        /// <summary>
        /// Registers a new user to another board.
        /// </summary>
        /// <param name="email">The email address of the user to register.</param>
        /// <param name="password">The password of the user to register.</param>
        /// <param name="nickname">The nickname of the user to register.</param>
        /// <param name="boardId">The board that the user will be Associated with.</param>
        /// <exception cref="ArgumentException">Thrown when the e-mail address given is already taken by another user.</exception>
        public void Register(string email, string password, string nickname, string boardId)
        {
            log.Debug("Register Attempt");
            if (nickname.Length == 0)
                throw new ArgumentException("Cannot register with an empty nickname, please try again.");
            if (!Users.ContainsKey(email))
            {
                ValidatePassword(password);
                ValidateEmail(email);
                User newUser = new User(email, password, nickname, boardId);
                newUser.Save();
                Users.Add(email, newUser);
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
            }
            else
                throw new ArgumentException("Old password does not match the current password. Please try again.");
        }

        /// <summary>
        /// Checks if a given password fits certain criteria:
        /// 1. Checks that the password length is within the appropriate bounds.
        /// 2. Checks if the password contains at least 1 digit, lowercase letter and uppercase letter
        /// </summary>
        /// <param name="password">The password the user would like to use.</param>
        /// <exception cref="ArgumentException">Thrown when the password given doesn't fit the criteria.</exception>
        private void ValidatePassword (string password) {
            if (password.Length < MINIMAL_PASSWORD_LENGTH | password.Length > MAXIMAL_PASSWORD_LENGTH ||
            !password.Any(char.IsDigit) | !password.Any(char.IsLower) | !password.Any(char.IsUpper))
                throw new ArgumentException("A user password must be in length of 5 to 25 characters and must include at least one uppercase letter, one lowercase letter and a number.");
        }

        /// <summary>
        /// Checks if a given e-mail address fits certain criteria.
        /// </summary>
        /// <param name="email">The email address the user intends to be indetified with.</param>
        /// <exception cref="ArgumentException">Thrown when the email address given doesn't fit the criteria.</exception>
        private void ValidateEmail (string email) {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(email + " is invalid, please consult the following form *example@gmail.com*.");
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }

            if (!Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                throw new ArgumentException(email + " is invalid, please use only alphanumerical characters and consult the following form *example@gmail.com*.");
        }
    }
}
