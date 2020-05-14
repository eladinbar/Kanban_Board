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
    internal class BoardDalController : DalController<DalBoard>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string BoardTableName = "Boards";

        public BoardDalController() : base(BoardTableName) { }

        public List<DalBoard> SelectAllBoards()
        {
            log.Info("loading all Boards from data base");
            List<DalBoard> boardList = Select().Cast<DalBoard>().ToList();
            ColumnDalController columnController = new ColumnDalController();
            foreach(DalBoard b in boardList)
            {
                log.Debug("loading all columns of " + b.Email);
                b.Columns = columnController.SelectAllColumns(b.Email);
            }
            return boardList;
        }

        public override bool Insert(DalBoard board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("opening connection to DataBase");
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

        public bool Delete(DalBoard board)
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


        internal override DalBoard ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalBoard result = new DalBoard(reader.GetString(0), (int)reader.GetValue(1));
            return result;
        }

        internal override void CreateTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {BoardTableName} (" +
                    $"{DalBoard.EmailColumnName} TEXT NOT NULL," +
                    $"{DalBoard.BoardTaskCountName} InTEGER NOT NULL," +
                    $"PRIMARY KEY({DalBoard.EmailColumnName})" +
                    $"FOREIGN KEY({DalBoard.EmailColumnName})" +
                    $"  REFERENCES {UserDalController.UserTableName} ({DalBoard.EmailColumnName})" +
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
                }
            }
        }



    }

    public enum BoardDalEnumController
    {
        
    }
}
