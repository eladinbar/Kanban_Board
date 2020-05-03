using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class ColumnDalController : DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        internal const string ColumnTableName = "Columns";

        public ColumnDalController() : base(ColumnTableName) { }

        public List<DalColumn> SelectAllColumns(string email)
        {
            List<DalColumn> columnList = Select(email).Cast<DalColumn>().ToList();
            return columnList;
        }

        public bool Insert(DalColumn column)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} " +
                        $"({DalColumn.EmailColumnName}, , {DalColumn.ColumnOrdinalColumnName}, {DalColumn.ColumnNameColumnName}, {DalColumn.ColumnLimitColumnName})" +
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
                }
            }
            return res > 0;
        }

        public bool Delete(DalColumn column)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DALETE FROM {ColumnTableName} WHERE email={column.Email} AND ordinal={column.Ordinal}"
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
            DalColumn result = new DalColumn(reader.GetString(0), (int)reader.GetValue(1), reader.GetString(2), (int)reader.GetValue(3));
            return result;
        }

        internal override void CreateTable()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {ColumnTableName} (" +
                    $"{DalColumn.EmailColumnName} TEXT NOT NULL," +
                    $"{DalColumn.ColumnNameColumnName} TEXT NOT NULL," +
                    $"{DalColumn.ColumnOrdinalColumnName} INTEGER NOT NULL UNIQUE" +
                    $"{DalColumn.ColumnLimitColumnName} INTEFER NOT NULL" +
                    $"PRIMERY KEY({DalColumn.EmailColumnName}, {DalColumn.ColumnNameColumnName})" +
                    $"FOREIGN KEY({DalColumn.EmailColumnName})" +
                    $"  REFERANCE {BoardDalController.BoardTableName} ({DalColumn.EmailColumnName})" +                   
                    $");";
                try
                {
                    connection.Open();
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
}
