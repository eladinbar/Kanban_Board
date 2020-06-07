using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class UserModel: NotifiableModelObject
    {
        public string Email { get; private set; }
        public string Nickname { get; private set; }
 
        public UserModel(BackendController controller, string email, string nickname): base(controller)
        {
            this.Email = email;
            this.Nickname = nickname;
        }
    }
}
