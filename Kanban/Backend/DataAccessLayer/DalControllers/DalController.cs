using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    /// <summary>
    /// An abstract class used as the basis to form connections with the database for reading and writing purposes.
    /// </summary>
    public abstract class DalController<T> where T:DalObject<T>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        protected readonly string _connectionString;
        protected readonly string _tableName;

        /// <summary>
        /// A public constructor, initializes the database path and the connection string accordingly. Initializes the respective table name and creates it in the database.
        /// </summary>
        /// <param name="tableName">The table name of the object this controller represents.</param>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
            CreateTable();
        }

        //abstract methods

        /// <summary>
        /// Creates a database table with the name initialized in the field '_tableName'.
        /// </summary>
        internal abstract void CreateTable();

        /// <summary>
        /// Inserts the given object into its respective table in the database.
        /// </summary>
        /// <param name="dalObject">The data access layer object instance to insert into the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public abstract bool Insert(T dalObject);

        /// <summary>
        /// Deletes the given object from its respective table in the database.
        /// </summary>
        /// <param name="dalObject">The data access layer object instance to delete from the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public abstract bool Delete(T dalObject);

        /// <summary>
        /// Converts an SQLiteDataReader to a DalObject.
        /// </summary>
        /// <param name="reader">The SQLite reader to convert.</param>
        /// <returns>Returns a DalObject that extends DalObject<T>.</returns>
        internal abstract T ConvertReaderToObject(SQLiteDataReader reader);

        //implemented methods

        /// <summary>
        /// A select command function for the Users and Boards tables.
        /// </summary>
        /// <returns>Returns a DalObject<T> List as read from the database.</returns>
        public List<T> Select()
        {
            List<T> fromDB = new List<T>();
            using(var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextSelect()
                };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    log.Info("Opening a connection to the database.");
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("The connection was closed.");
                }
            }
            return fromDB;
        }

        /// <summary>        
        /// A select command function for the Columns table of a specific board.
        /// </summary>
        /// <param name="email">The board to select columns from.</param>
        /// <returns>Returns a DalObject<T> List as read from the database.</returns>
        public List<T> Select(string email)
        {
            List<T> fromDB = new List<T>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextSelect(email)
                };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    log.Info("Opening a connection to the database");
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("The connection was closed.");
                }
            }
            return fromDB;
        }

        /// <summary>
        /// A select command for the Tasks table of a specific column in a specific board.
        /// </summary>
        /// <param name="email">The board to select columns from.</param>
        /// <param name="columnName">The column to select tasks from.</param>
        /// <returns>Returns a DalObject<T> List as read from the database.</returns>
        public List<T> Select(string email, string columnName)
        {
            List<T> fromDB = new List<T>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextSelect(email, columnName)
                };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    log.Info("Opening a connection to the database.");
                    dataReader = command.ExecuteReader();


                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured.", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("The connection was closed.");
                }
            }
            return fromDB;
        }

        /// <summary>
        /// Updates the column in the database associated with the given email and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attributeName, email)
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attributeName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug("Executing update to database with key " + email);
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
        /// Updates the column in the database associated with the given email and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attributeName, email)
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attributeName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug("Executing update to database with key " + email);
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
        /// Updates the column in the database associated with the given email and columnName and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="columnName">The columnName the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string columnName, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attributeName,email,columnName)
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attributeName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}",email, columnName));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exception occured", e);
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
        /// Updates the column in the database associated with the given email and columnName and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="columnName">The columnName the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string columnName, string attribluteName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attribluteName, email, columnName)
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attribluteName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}", email, columnName));
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
        /// Updates the column in the database associated with the given email, columnName and taskID and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="columnName">The columnName the column to update is associated with.</param>
        /// <param name="taskID">The taskID the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string columnName, int taskID, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attributeName, email, columnName, taskID.ToString())
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attributeName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}",email, columnName ,taskID));
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
        /// Updates the column in the database associated with the given email, columnName and taskID and sets the value of its 'attributeName' to the given 'attributeValue'.
        /// </summary>
        /// <param name="email">The email the column to update is associated with.</param>
        /// <param name="columnName">The columnName the column to update is associated with.</param>
        /// <param name="taskID">The taskID the column to update is associated with.</param>
        /// <param name="attributeName">The name of the column to update</param>
        /// <param name="attributeValue">The value to update in the table.</param>
        /// <returns>Returns true if one or more rows were updated.</returns>
        public bool Update(string email, string columnName, int taskID, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = CommandTextUpdate(attributeName, email, columnName, taskID.ToString())
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@""+attributeName, attributeValue));
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}", email, columnName, taskID));
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
        /// Creates the database .db file.
        /// </summary>
        protected void CreateDBFile()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo DBFile = new FileInfo(path);
            if (!DBFile.Exists)
            {
                SQLiteConnection.CreateFile("KanbanDB.db");
            }
        }

        //private methods

        /// <summary>
        /// Creates the SQLite CommandText for the various 'Select' methods.
        /// </summary>
        /// <param name="keyArgs">keyArgs[0] represents the email key, keyArgs[1] represents the ColumnName key and KeyArgs[2] represents the taskID key.</param>
        /// <returns>Returns the respective string of the desired SQL command.</returns>
        private string CommandTextSelect(params string[] keyArgs)
        {
            string command = $"SELECT * FROM {_tableName}";
            
            switch (keyArgs.Length)
            {
                case 1:
                    return command + $" WHERE email=\"{keyArgs[0]}\" ORDER BY email, Ordinal ASC";
                case 2:
                    return command + $" WHERE email=\"{keyArgs[0]}\" AND ColumnName=\"{keyArgs[1]}\"";
                case 3:
                    return command + $" WHERE email=\"{keyArgs[0]}\" AND ColumnName=\"{keyArgs[1]}\" AND ID={keyArgs[2]}";
                default:
                    log.Error("keyArgs contains values in slots [0]-[2], given index was out of bounds.");
                    return command;
            }
        }

        /// <summary>
        /// Creates the SQLite CommandText for the various 'Update' methods.
        /// </summary>
        /// <param name="attributeName">The name of the column to update.</param>
        /// <param name="keyArgs">keyArgs[0] represents the email key, keyArgs[1] represents the ColumnName key and KeyArgs[2] represents the taskID key.</param>
        /// <returns>Returns the respective string of the desired SQL command.</returns>
        private string CommandTextUpdate(string attributeName, params string[] keyArgs)
        {
            string command = $"UPDATE {_tableName} SET [{attributeName}] = @{attributeName} WHERE {DalObject<T>.EmailColumnName}=\"{keyArgs[0]}\"";

            switch (keyArgs.Length)
            {
                case 1:
                    return command;
                case 2:
                    return command + $" AND {DalColumn.ColumnNameColumnName}=\"{keyArgs[1]}\"";
                case 3:
                    return command + $" AND {DalColumn.ColumnNameColumnName}=\"{keyArgs[1]}\" AND {DalTask.TaskIDColumnName}={keyArgs[2]}";
                default:
                    log.Error("keyArgs contains values in slots [0]-[2], given index was out of bounds.");
                    return "";
            }
        }
    }
}
