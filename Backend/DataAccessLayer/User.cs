using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class User : DalObject<User>
    {
        private string Nickname;
        private string Email;
        private string Password;

        public User(string email, string password, string nickname)
        {
            this.Email = email;
            this.Password = password;
            this.Nickname = nickname;
        }

        public void Save() {
            throw new NotImplementedException();
        }

        public string ToJson() {
            throw new NotImplementedException();
        }

        public User FromJson() {
            throw new NotImplementedException();
        }
    }
}
