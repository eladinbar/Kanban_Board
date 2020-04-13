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
        public string Email { get; }
        public string Password { get; }
        public string Nickname { get; }

        public User(string email, string password, string nickname)
        {
            Email = email;
            Password = password;
            Nickname = nickname;
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
        
    }
}
