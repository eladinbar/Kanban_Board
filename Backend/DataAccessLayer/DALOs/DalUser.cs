using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// The data access layer representation of a User.
    /// </summary>
    internal class DalUser : DalObject<DalUser>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public const string UserPasswordColumnName = "Password";
        public const string UserNicknameColumnName = "Nickname";

        private string _password;
        public string Password { get => _password; set { _password = value; _controller.Update(Email,UserPasswordColumnName, value); } }
        private string _nickname;
        public string Nickname { get => _nickname; set { _nickname = value; _controller.Update(Email, UserNicknameColumnName, value); } }

        /// <summary>
        /// A public constructor that initializes all necessary fields to be persisted.
        /// </summary>
        /// <param name="email">The email that is to be associated with the new DalUser.</param>
        /// <param name="password">The password of the user to be persisted.</param>
        /// <param name="nickname">The nickname of the user to be persisted.</param>
        public DalUser(string email, string password, string nickname) : base(new UserDalController())
        {
            Email = email;
            _password = password;
            _nickname = nickname;
        }
    }
}
