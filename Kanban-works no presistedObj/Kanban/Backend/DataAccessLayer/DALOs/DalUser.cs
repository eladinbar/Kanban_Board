using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    /// <summary>
    /// Dal access layer representation  of a User
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

        public DalUser(string email, string password, string nickname) : base(new UserDalController())
        {
            Email = email;
            _password = password;
            _nickname = nickname;
        }
    }
}
