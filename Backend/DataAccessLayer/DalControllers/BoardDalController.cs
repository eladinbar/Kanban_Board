using System.Collections.Generic;
using System.Data.SQLite;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class BoardDalController : DalController<DalBoard>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string BoardTableName = "Boards";

        /// <summary>
        /// A public constructor that initializes the database path and the connection string accordingly. Initializes the 'Boards' table name and creates it in the database.
        /// </summary>
        public BoardDalController() : base(BoardTableName) { }

        /// <summary>
        /// Retrieves all board data from the database.
        /// </summary>
        /// <returns>Returns a list of all DalBoard objects in the database.</returns>
        public List<DalBoard> SelectAllBoards()
        {
            log.Info("Loading all Boards from data base");
            List<DalBoard> boardList = Select();
            ColumnDalController columnController = new ColumnDalController();
            foreach(DalBoard b in boardList)
            {
                log.Debug("Loading all columns of board " + b.Email);
                b.Columns = columnController.SelectAllColumns(b.Email);
            }
            return boardList;
        }

        /// <summary>
        /// Creates the 'Boards' table in the database.
        /// </summary>
        internal override void CreateTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {BoardTableName} (" +
                    $"{DalBoard.EmailColumnName} TEXT NOT NULL," +
                    $"{DalBoard.BoardTaskCountName} INTEGER NOT NULL," +
                    $"PRIMARY KEY({DalBoard.EmailColumnName})" +
                    $"FOREIGN KEY({DalBoard.EmailColumnName})" +
                    $"  REFERENCES {UserDalController.UserTableName} ({DalBoard.EmailColumnName})" +
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
        /// Converts an SQLiteDataReader to a DalBoard.
        /// </summary>
        /// <param name="reader">The SQLite reader to convert.</param>
        /// <returns>Returns a DalBoard.</returns>
        internal override DalBoard ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalBoard result = new DalBoard(reader.GetString(0), reader.GetInt32(1));
            return result;
        }

        /// <summary>
        /// Inserts the given board into the 'Boards' table in the database.
        /// </summary>
        /// <param name="board">The data access layer board instance to insert into the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Insert(DalBoard board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Opening a connection to the database.");
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({DalBoard.EmailColumnName}, {DalBoard.BoardTaskCountName})" +
                        $"VALUES (@emailVal, @taskCounterVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", board.Email);
                    SQLiteParameter taskCounterParam = new SQLiteParameter(@"taskCounterVal", board.TaskCounter);                    

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(taskCounterParam);
                    
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
                    log.Info("Te connection was closed.");
                }
            }
            return res > 0;
        }

        /// <summary>
        /// Deletes the given board from the 'Boards' table in the database.
        /// </summary>
        /// <param name="board">The data access layer board instance to delete from the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Delete(DalBoard board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {BoardTableName} WHERE email=\"{board.Email}\""
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
