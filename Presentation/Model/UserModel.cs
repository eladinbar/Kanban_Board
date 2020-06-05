using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Nickname { get; private set; }
        
        public UserModel(string username, string password, string nickname)
        {
            Username = username;
            Password = password;
            Nickname = nickname;
        }
    }
}