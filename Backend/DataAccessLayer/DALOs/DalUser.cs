using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DALOs
{
    internal class DalUser : DalObject
    {
        public const string UserPasswordColumnName = "Password";
        public const string UserNicknameColumnName = "Nickname";

        private string _password;
        public string Password { get => _password; set { _password = value; _controller.Update(Email,UserPasswordColumnName, value); } }
        private string _nickname;
        public string nickname { get => _nickname; set { _nickname = value; _controller.Update(Email, UserPasswordColumnName, value); } }

        public DalUser(string email, string password, string nickname) : base(new UserDalController())
        {
            Email = email;
            _password = password;
            _nickname = nickname;
        }
    }
}
