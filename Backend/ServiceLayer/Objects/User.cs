using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct User
    {
        public readonly string Email;
        public readonly string Nickname;
        public readonly string AssociatedBoard;

        internal User(string email, string nickname, string associatedBoard)
        {
            this.Email = email;
            this.Nickname = nickname;
            this.AssociatedBoard = associatedBoard;
        }
        // You can add code here
    }
}
