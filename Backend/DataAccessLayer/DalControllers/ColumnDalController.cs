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
    internal class ColumnDalController : DalController<DalColumn>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string ColumnTableName = "Columns";

        /// <summary>
        /// A public constructor that initializes the database path and the connection string accordingly. Initializes the 'Columns' table name and creates it in the database.
        /// </summary>
        public ColumnDalController() : base(ColumnTableName) { }

        /// <summary>
        /// Retrieves all column data of a specific board from the database.
        /// </summary>
        /// <param name="email">The board to retrieve columns from.</param>
        /// <returns>Returns a list of all DalColumn objects associated with the specified board in the database.</returns>
        public List<DalColumn> SelectAllColumns(string email)
        {
            log.Info("Loading all columns from the database.");
            List<DalColumn> columnList = Select(email);
            TaskDalController taskController = new TaskDalController();
            foreach(DalColumn c in columnList)
            {
                log.Debug("Loading all tasks of column " + c.Name + " from board " + email);
                c.Tasks = taskController.SelectAllTasks(c.Email, c.Name);
            }

            return columnList;
        }

        /// <summary>
        /// Creates the 'Columns' table in the database.
        /// </summary>
        internal override void CreateTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {ColumnTableName} (" +
                    $"{DalColumn.EmailColumnName} TEXT NOT NULL," +
                    $"{DalColumn.ColumnNameColumnName} TEXT NOT NULL," +
                    $"{DalColumn.ColumnOrdinalColumnName} INTEGER NOT NULL," +
                    $"{DalColumn.ColumnLimitColumnName} INTEGER NOT NULL," +
                    $"PRIMARY KEY({DalColumn.EmailColumnName}, {DalColumn.ColumnNameColumnName})" +
                    $"FOREIGN KEY({DalColumn.EmailColumnName})" +
                    $"  REFERENCES {BoardDalController.BoardTableName} ({DalColumn.EmailColumnName})" +
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
        /// Converts an SQLiteDataReader to a DalColumn.
        /// </summary>
        /// <param name="reader">The SQLite reader to convert.</param>
        /// <returns>Returns a DalColumn.</returns>
        internal override DalColumn ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalColumn result = new DalColumn(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3));
            return result;
        }

        /// <summary>
        /// Inserts the given column into the 'Columns' table in the database.
        /// </summary>
        /// <param name="column">The data access layer column instance to insert into the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Insert(DalColumn column)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Opening a connection to the database");
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} " +
                        $"({DalColumn.EmailColumnName}, {DalColumn.ColumnOrdinalColumnName}, {DalColumn.ColumnNameColumnName}, {DalColumn.ColumnLimitColumnName})" +
                        $"VALUES (@emailVal, @ordinalVal, @nameVal, @limitVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", column.Email);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", column.Name);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", column.Ordinal);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.Limit);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(limitParam);

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
        /// Deletes the given column from the 'Columns' table in the database.
        /// </summary>
        /// <param name="column">The data access layer column instance to delete from the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Delete(DalColumn column)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {ColumnTableName} WHERE email=\"{column.Email}\" AND Name=\"{column.Name}\""
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
                    log.Info("The connection was closed.");
                }
            }
            return res > 0;
        }        
    }    
}
