using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class User : DalObject<User>
    {
        private string Nickname;
        private string Email;
        private string Password;

        public User () { }

        public User(string email, string password, string nickname)
        {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
        }

        public string email {
            get { return Email; }
        }

        public string password {
            get { return Password; }
        }

        public string nickname {
            get { return Nickname; }
        }

        public override string ToJson() {
            return JsonSerializer.Serialize(this); //Returns the DAL User instance in Json (string) form
        }

        public override User FromJson(string json) {
            DalController dc = new DalController();
            User savedUser = JsonSerializer.Deserialize<User>(dc.ReadFromFile(json));
            return savedUser; //Returns the user 
        }

        public override void Save()
        {
            DalController dc = new DalController();
            dc.WriteToFile(Email, ToJson());
        }
    }
}
