using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class User : DalObject<User>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }

        public User(string email, string password, string nickname)
        {
            Email = email;
            Password = password;
            Nickname = nickname;
        }

        public User() { Email = null; Password = null; Nickname = null; }

        /// <summary>
        /// The method in the DataAccessLayer to save an object to the persistent layer.
        /// </summary>
        /// <param name="path">The path the object will be saved to.</param>
        public override void Save(string path) {
            DalController dc = new DalController();
            DirectoryInfo dir = new DirectoryInfo(dc.BASE_PATH + "Users\\");
            if (!dir.Exists) 
                dir.Create();
            dc.WriteToFile(Email, ToJson(), "Users\\");
        }
        
    }
}
