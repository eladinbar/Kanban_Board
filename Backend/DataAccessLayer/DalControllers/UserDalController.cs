using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class UserDalController : DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        private const string UserTableName = "Users";

        public UserDalController() : base(UserTableName) { }

        public List<DalUser> SelectAllUsers()
        {
            List<DalUser> userList = Select().Cast<DalUser>().ToList();
            return userList;
        }

        internal override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }
        
    }
}
