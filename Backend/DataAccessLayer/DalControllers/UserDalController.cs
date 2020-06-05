using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class UserDalController : DalController<DalUser>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string UserTableName = "Users";

        /// <summary>
        /// A public constructor that initializes the database path and the connection string accordingly. Initializes the 'Users' table name and creates it in the database.
        /// </summary>
        public UserDalController() : base(UserTableName) { }

        /// <summary>
        /// Retrieves all user data from the database.
        /// </summary>
        /// <returns>Returns a list of all DalUser objects in the database.</returns>
        public List<DalUser> SelectAllUsers()
        {
            log.Info("Loading all users from the database.");
            List<DalUser> userList = Select();
            return userList;
        }

        public List<DalUser> SelectAllUsersOfBoard(string email)
        {
            log.Info("Loading all users from the database.");
            List<DalUser> userList = Select(email);
            return userList;
        }

        /// <summary>
        /// Creates the 'Users' table in the database.
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
                    $"{DalUser.UserAssociatedBoardColumnName} TEXT NOT NULL," +
                    $"PRIMARY KEY({DalUser.EmailColumnName})" +
                    $");";
                SQLiteCommand tableExistence = new SQLiteCommand(null, connection);
                tableExistence.CommandText = $"SELECT name FROM sqlite_master WHERE type=\"table\" AND name=\"{_tableName}\"";
                try
                {
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    SQLiteDataReader reader = tableExistence.ExecuteReader();
                    if (!reader.Read())
                        command.ExecuteNonQuery();
                    reader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    tableExistence.Dispose();
                    command.Dispose();
                    connection.Close();
                    log.Info("The connection was closed.");
                }
            }
        }

        /// <summary>
        /// Converts an SQLiteDataReader to a DalUser.
        /// </summary>
        /// <param name="reader">The SQLite reader to convert.</param>
        /// <returns>Returns a DalUser.</returns>
        internal override DalUser ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalUser result = new DalUser(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
            return result;
        }

        /// <summary>
        /// Inserts the given user into the 'Users' table in the database.
        /// </summary>
        /// <param name="user">The data access layer user instance to insert into the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Insert(DalUser user)
        {
            
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Opening a connection to the database.");
                    connection.Open();   
                    command.CommandText = $"INSERT INTO {UserTableName} ({DalUser.EmailColumnName}, {DalUser.UserPasswordColumnName},{DalUser.UserNicknameColumnName},{DalUser.UserAssociatedBoardColumnName})" +
                        $"VALUES (@emailVal, @passwordVal, @nicknameVal, @associatedBoardVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    SQLiteParameter nicknameParam = new SQLiteParameter(@"nicknameVal", user.Nickname);
                    SQLiteParameter boardParam = new SQLiteParameter(@"associatedBoardVal", user.AssociatedBoard);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(nicknameParam);
                    command.Parameters.Add(boardParam);

                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("The connection was closed.");
                }
            }
            return res > 0;
        }

        /// <summary>
        /// Deletes the given user from the 'Users' table in the database.
        /// </summary>
        /// <param name="user">The data access layer user instance to delete from the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Delete(DalUser user)
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
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    log.Info("connection closed.");
                }
            }
            return res > 0;
        }

        /// <summary>
        /// Deletes the database file for a clean start of the program.
        /// </summary>
        public void DeleteDatabase()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), _databaseName));
            FileInfo dBFile = new FileInfo(path);
            if (dBFile.Exists)
            {
                dBFile.Delete();
            }
        }
    }      
}
