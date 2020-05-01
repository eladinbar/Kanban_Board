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

        public bool Insert(DalUser user)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();   
                    command.CommandText = $"INSERT INTO {UserTableName} ({DalUser.EmailColumnName}, {DalUser.UserPasswordColumnName},{DalUser.UserNicknameColumnName})" +
                        $"VALUES (@emailVal, @passwordVal,@NicknameVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    SQLiteParameter nicknameParam = new SQLiteParameter(@"nicknameVal", user.Nickname);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(nicknameParam);

                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res>0;
        }
    
        public bool Delete(DalUser user)
        {
            int res = -1;
            using(var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DALETE FROM {UserTableName} WHERE email={user.Email}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }


        internal override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalUser result = new DalUser(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            return result;
        }
    }   
   
}
