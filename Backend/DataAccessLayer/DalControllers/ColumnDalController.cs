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

        public ColumnDalController() : base(ColumnTableName) { }

        /// <summary>
        /// Select command for all column of a specific board.
        /// </summary>
        /// <param name="email">the specified board to select</param>
        /// <returns>List of DalColumn of the Specified Board</returns>
        public List<DalColumn> SelectAllColumns(string email)
        {
            List<DalColumn> columnList = Select(email).Cast<DalColumn>().ToList();
            TaskDalController taskController = new TaskDalController();
            foreach(DalColumn c in columnList)
            {
                c.Tasks = taskController.SelectAllTasks(c.Email, c.Name);
            }

            return columnList;
        }
        /// <inhecitdoc>
        /// cref="DalController{T}"
        /// </inhecitdoc>
        internal override DalColumn ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalColumn result = new DalColumn(reader.GetString(0), (int)reader.GetValue(1), reader.GetString(2), (int)reader.GetValue(3));
            return result;
        }
        /// <summary>
        /// Insert command for column to Database.
        /// </summary>
        /// <param name="column">Dal instance to insert to the Database</param>
        /// <returns>True is the method changed more then 0 rows</returns>
        public override bool Insert(DalColumn column)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("opening connection to DataBase");
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
                    log.Error("SQLite exeption occured", e);
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
        /// Delete command for column to the Database.
        /// </summary>
        /// <param name="column">Dal instance to delete from the Database</param>
        /// <returns>True if the method changed more then 0 rows</returns>
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
                    log.Info("connection closed.");
                }
            }
            return res > 0;
        }
        /// <summary>
        /// Creates the Columns table in the Kanban.db.
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
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    SQLiteDataReader reader = tableExistence.ExecuteReader();
                    if (!reader.Read())
                        command.ExecuteNonQuery();
                    reader.Close();
                }
                catch (SQLiteException e)
                {
                    log.Error("SQLite exeption occured", e);
                }
                finally
                {
                    tableExistence.Dispose();
                    command.Dispose();
                    connection.Close();
                    log.Info("connection closed.");
                }
            }
        }
    }
    
}
