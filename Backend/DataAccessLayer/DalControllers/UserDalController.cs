using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class UserDalController : DalController<DalUser>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string UserTableName = "Users";

        public UserDalController() : base(UserTableName) { }

        /// <summary>
        /// gets all user data from Database.
        /// </summary>
        /// <returns></returns>
        public List<DalUser> SelectAllUsers()
        {
            List<DalUser> userList = Select().Cast<DalUser>().ToList();
            return userList;
        }

        /// <summary>
        /// Insert command for user to Database.
        /// </summary>
        /// <param name="user">Dal instance to insert to the Database</param>
        /// <returns>True is the method changed more then 0 rows</returns>
        public override bool Insert(DalUser user)
        {
            
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("opening connection to DataBase");
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

        /// <summary>
        /// Delete command for user to the Database.
        /// </summary>
        /// <param name="user">Dal instance to delete from the Database</param>
        /// <returns>True if the method changed more then 0 rows</returns>
        public bool Delete(DalUser user)
        {
            int res = -1;
            using(var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {UserTableName} WHERE email=\"{user.Email}\""
                };
                try
                {
                    log.Info("opening connection to DataBase");
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

        /// <summary>
        /// Deletes the Database file for a clean start of the program.
        /// </summary>
        public void DeleteDatabase()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo dBFile = new FileInfo(path);
            if (dBFile.Exists)
            {
                dBFile.Delete();
            }
        }

        /// <inhecitdoc cref="DalController{T}"/>
        internal override DalUser ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalUser result = new DalUser(reader.GetString(0), reader.GetString(1), reader.GetString(2));
            return result;
        }

        /// <summary>
        /// Creates the Users table in the Kanban.db.
        /// </summary>
        internal override void CreateTable()
        {

            CreateDBFile();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {UserTableName} (" +
                    $"{DalUser.EmailColumnName} TEXT NOT NULL," +
                    $"{DalUser.UserPasswordColumnName} TEXT NOT NULL," +
                    $"{DalUser.UserNicknameColumnName} TEXT NOT NULL," +
                    $"PRIMARY KEY({DalUser.EmailColumnName})" +
                    $");";
                SQLiteCommand tableExistence = new SQLiteCommand(null, connection);
                tableExistence.CommandText = $"SELECT name FROM sqlite_master WHERE type=\"table\" AND name=\"{_tableName}\"";

                try
                {
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    SQLiteDataReader reader = tableExistence.ExecuteReader();
                    if (!reader.Read())
                        command.ExecuteNonQuery();
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
        }
    }   
   
}
