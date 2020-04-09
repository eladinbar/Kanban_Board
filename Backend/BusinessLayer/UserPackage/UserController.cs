﻿using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.UserPackage
{
    class UserController
    {
        private Dictionary<string, User> Users;

        public UserController() {
            DalController dalC = new DalController();
            this.Users = dalC.LoadAllUsers();
        }

        internal Dictionary<string, User> users
        {
            get { return Users; }
        }

        public void Register(string email, string password, string nickname) {
            if (!Users.ContainsKey(email)) {
               User newUser = new User(email, password, nickname);
               Users.Add(email, newUser);
            }
            else
               throw new ArgumentException("A user with this E-mail address already exists, please re-evaluate your information and try again.");
        }

        public void Login (string email, string password) {
            if (!Users.ContainsKey(email))
                throw new ArgumentException("The E-mail given does not exist in the database, please register and try again.");
            else if (!Users[email].password.Equals(password))
                throw new ArgumentException("Incorrect password. Please try again.");
            else
                Users[email].Login();
        }

        public void Logout (string email) {
            Users[email].Logout();
        }

        public void ChangePassword (string email, string oldPassword, string newPassword) {
            if (Users[email].password.Equals(oldPassword)) {
                ValidatePassword(newPassword);
                Users[email].ChangePassword(newPassword);
            }
            else
                throw new ArgumentException("Old password does not match the current password. Please try again.");
        }

        private void ValidatePassword (string password) {
            if (password.Length < 4 | password.Length > 20 | !Regex.IsMatch(password, "^[a-zA-Z0-9]*$") | !password.Any(char.IsDigit) | !password.Any(char.IsLower) | !password.Any(char.IsUpper))
                throw new ArgumentException("A user password must be in length of 4 to 20 characters and must include at least one uppercase letter, one lowercase letter and a number.");
        }
    }
}
