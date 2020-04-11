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
        private readonly string Nickname;
        private readonly string Email;
        private readonly string Password;

        public User(string email, string password, string nickname)
        {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
        }

        public User() { }

        public string email {
            get { return Email; }
        }

        public string password {
            get { return Password; }
        }

        public string nickname {
            get { return Nickname; }
        }

        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + "Users\\");
            if (!dir.Exists) 
                dir.Create();
            dc.WriteToFile(Email, ToJson(), "Users\\");
        }
    }
}
