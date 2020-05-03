using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    public abstract class DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        protected readonly string _connectionString;
        protected readonly string _tableName;

        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            _connectionString = $"Data Source={path}; Version=3;";
            _tableName = tableName;
        }

        internal abstract DalObject ConvertReaderToObject(SQLiteDataReader reader);

        //No spasific key
        public List<DalObject> Select()
        {
            List<DalObject> fromDB = new List<DalObject>();
            using(var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"select * from {_tableName}"
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

            return fromDB;
        }
        //email key select
        public List<DalObject> Select(string email)
        {
            List<DalObject> fromDB = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"select * from {_tableName} where email={email}"
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

            return fromDB;
        }
        //email key and ordinal key select select
        public List<DalObject> Select(string email, int ordinal)
        {
            List<DalObject> fromDB = new List<DalObject>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"SELECT FROM {_tableName} WHERE email={email} and Ordinal={ordinal}"
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

            return fromDB;
        }

        //primery key update. accepts a single key parmeter
        public bool Update(string email, string attribluteName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
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
                    connection.Close();
                }

            }

            return res > 0;
        }
        //primery key update. accepts a single key parmeter
        public bool Update(string email, string attribluteName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
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
                    connection.Close();
                }

            }

            return res > 0;
        }
        //combine key of 2 keys update
        public bool Update(string email, string name, string attribluteName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email} and Name={name}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}",email, name));
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
        //combine key of 2 keys update
        public bool Update(string email, string name, string attribluteName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email} and Name={name}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1}", email, name));
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
        //combine key of 3 keys update
        public bool Update(string email, string name, int taskID, string attribluteName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email} AND Name={name} AND ID={taskID}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}",email, name ,taskID));
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
        //combine key of 3 keys update
        public bool Update(string email, string name, int taskID, string attribluteName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"UPDATE {_tableName} SET [{attribluteName}] = @{attribluteName} WHERE email={email} AND Name={name} AND ID={taskID}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attribluteName, attributeValue));
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    log.Debug(("Executing update to data base with key {0} name {1} ID {2}", email, name, taskID));
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

        internal abstract void CreateTable();
    }
}
