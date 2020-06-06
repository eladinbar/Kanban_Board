using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel : NotifiableModelObject
    {
        public string Username { get; private set; }
        public string Nickname { get; private set; }
        
        public UserModel(BackendController controller, string username, string nickname) : base(controller)
        {
            Username = username;
            Nickname = nickname;
        }
    }
}