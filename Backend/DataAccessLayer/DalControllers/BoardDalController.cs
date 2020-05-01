using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class BoardDalController : DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        private const string BoardTableName = "Boards";

        public BoardDalController() : base(BoardTableName) { }

        public List<DalBoard> SelectAllBoards()
        {
            List<DalBoard> boardList = Select().Cast<DalBoard>().ToList();
            return boardList;
        }

        public bool Insert(DalBoard board)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
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
                    CommandText = $"DALETE FROM {BoardTableName} WHERE email={board.Email}"
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
            DalBoard result = new DalBoard(reader.GetString(0), (int)reader.GetValue(1));
            return result;
        }
    }
}
