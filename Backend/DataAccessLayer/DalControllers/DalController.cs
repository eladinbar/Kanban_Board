using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    /// <summary>
    /// an abstract class for conneting to database for writing and reading
    /// </summary>
    public abstract class DalController<T> where T:DalObject<T>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        protected readonly string _connectionString;
        protected readonly string _tableName;

        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
            CreateTable();
        }

        //abstract methods
        /// <summary>
        /// converts the reader to a DalObject
        /// </summary>
        /// <param name="reader">SQLite reader to convert</param>
        /// <returns>A DalObject that extands DalObject<T></returns>
        internal abstract T ConvertReaderToObject(SQLiteDataReader reader);
        public abstract bool Insert(T dalObject);
        public abstract bool Delete(T dalObject);
        /// <summary>
        /// Creates a Database table with the name _tableName.
        /// </summary>
        internal abstract void CreateTable();

        //implemented methods
        /// <summary>
        /// select commeand for User table and Board table.
        /// </summary>
        /// <returns>List of DalObject read from the database</returns>
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
                    log.Info("opening connection to DataBase");
                    dataReader = command.ExecuteReader();


                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }
            }

            return fromDB;
        }
        /// <summary>        
        /// select commeand for Columns table of a spesific Board.       
        /// </summary>
        /// <param name="email">the board to select</param>
        /// <returns>List of DalObject read from the database</returns>
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
                    log.Info("opening connection to DataBase");
                    dataReader = command.ExecuteReader();


                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }
            }

            return fromDB;
        }
        /// <summary>
        /// select commeand for Task table of a spesific column in a spacific board.
        /// </summary>
        /// <param name="email"> the board to select</param>
        /// <param name="columnName">the column to select</param>
        /// <returns>List of DalObject read from the database</returns>
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
                    log.Info("opening connection to DataBase");
                    dataReader = command.ExecuteReader();


                    while (dataReader.Read())
                    {
                        fromDB.Add(ConvertReaderToObject(dataReader));
                    }
                    dataReader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }
            }

            return fromDB;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="attributeName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug("Executing update to data base with key " + email);
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="attributeName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug("Executing update to data base with key " + email);
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed."); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email and columnName arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="columnName">primary key</param>
        /// <param name="attributeName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}",email, columnName));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email and columnName arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="columnName">primary key</param>
        /// <param name="attribluteName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}", email, columnName));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email, columnName and taskID arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="columnName">primary key</param>
        /// <param name="taskID">primary key</param>
        /// <param name="attributeName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}",email, columnName ,taskID));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// update the column in the database accosiated with attributeName and set it as attributeValue according to the email, columnName and teskID arguments.
        /// </summary>
        /// <param name="email">primary key</param>
        /// <param name="columnName">primary key</param>
        /// <param name="taskID">primary key</param>
        /// <param name="attributeName">column name to update</param>
        /// <param name="attributeValue">Value to insert to the table</param>
        /// <returns>true if one or more rows where updated</returns>
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}", email, columnName, taskID));
                    res = command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close(); log.Info("connection closed.");
                }

            }

            return res > 0;
        }
        /// <summary>
        /// Creates .db file.
        /// </summary>
        protected void CreateDBFile()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo dBFile = new FileInfo(path);
            if (!dBFile.Exists)
            {
                SQLiteConnection.CreateFile("KanbanDB.db");
            }
        }

        //private methods
        /// <summary>
        /// Creates the SQLite CommandText for the Select methods
        /// </summary>
        /// <param name="keyArgs">keyArgs[0] is for email key, keyArgs[1] is for ColumnName key, KeyArgs[2] is for taskID key</param>
        /// <returns>string of the SQL command-Select</returns>
        private string CommandTextSelect(params string[] keyArgs)
        {
            string command = $"SELECT * FROM {_tableName}";
            
            switch (keyArgs.Length)
            {
                case 1:
                    return command + $" WHERE email=\"{keyArgs[0]}\"";
                case 2:
                    return command + $" WHERE email=\"{keyArgs[0]}\" AND Name=\"{keyArgs[1]}\"";
                case 3:
                    return command + $" WHERE email=\"{keyArgs[0]}\" AND ColumnName=\"{keyArgs[1]}\" AND ID={keyArgs[2]}";
                default:
                    return command;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="keyArgs">keyArgs[0] is for email key, keyArgs[1] is for ColumnName key, KeyArgs[2] is for taskID key</param>
        /// <returns></returns>
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
                    return command + $" AND {DalColumn.ColumnNameColumnName}=\"{keyArgs[1]}\" AND {DalTask.TaskIDColumnName}={keyArgs[3]}";
                default:
                    log.Error("the amount of keyArgs does not meet the requirements");
                    return "";
            }
        }

    }
}
