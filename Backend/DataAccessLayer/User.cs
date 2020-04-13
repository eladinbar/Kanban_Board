using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class User : DalObject<User>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private readonly string _email;
        private readonly string _password;
        private readonly string _nickname;

        public User(string email, string password, string nickname)
        {
            _email = email;
            _password = password;
            _nickname = nickname;
        }

        public User() { }

        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + "Users\\");
            if (!dir.Exists) 
                dir.Create();
            dc.WriteToFile(Email, ToJson(), "Users\\");
        }

        //getters
        public string Email { get; }
        public string Password { get; }
        public string Nickname { get; }
    }
}
