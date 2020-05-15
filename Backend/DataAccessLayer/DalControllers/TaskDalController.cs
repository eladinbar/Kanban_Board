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
    internal class TaskDalController : DalController<DalTask>
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        private const string TaskTableName = "Tasks";

        public TaskDalController(): base(TaskTableName) { }

        /// <summary>
        /// Select command for all task of a specific board of  a spacific column.
        /// </summary>
        /// <param name="email">the board to get the task from</param>
        /// <param name="columnName">the column to gat the task from</param>
        /// <returns>List of DalTask of the Specified column</returns>
        public List<DalTask> SelectAllTasks(string email, string columnName)
        {
            List<DalTask> taskList = Select(email, columnName).Cast<DalTask>().ToList();
            return taskList;
        }

        /// <summary>
        /// Insert command for task to Database.
        /// </summary>
        /// <param name="task">Dal instance to insert to the Database</param>
        /// <returns>True is the method changed more then 0 rows</returns>
        public override bool Insert(DalTask task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("opening connection to DataBase");
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} " +
                        $"({DalTask.EmailColumnName}, {DalTask.ContainingTaskColumnNameColumnName}, {DalTask.TaskIDColumnName}, {DalTask.TaskTitleColumnName}, {DalTask.TaskDescriptionColumnName}, {DalTask.TaskDueDateColumnName},{DalTask.TaskCreationDateColumnName}, {DalTask.TaskLastChangedDateColumnName})" +
                        $"VALUES (@emailVal, @ordinalVal, @idVal, @titleVal, @descriptionVal, @dueDateVal, @creationDateVal, @lastChangedDateVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.Email);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", task.ColumnName);
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.TaskId);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter CreationDateParam = new SQLiteParameter(@"creationDateVal", task.CreationDate);
                    SQLiteParameter lastChangedDateParam = new SQLiteParameter(@"lastChangedDateVal", task.LastChangedDate);

                    command.Parameters.Add(emailParam);                    
                    command.Parameters.Add(ordinalParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(CreationDateParam);
                    command.Parameters.Add(lastChangedDateParam);

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
        /// Delete command for user to the Database.
        /// </summary>
        /// <param name="task">Dal instance to delete from the Database</param>
        /// <returns>True if the method changed more then 0 rows</returns>
        public override bool Delete(DalTask task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {TaskTableName} WHERE email=\"{task.Email}\" AND Name=\"{task.ColumnName}\" AND ID={task.TaskId}"
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

        /// <inhecitdoc cref="DalController{T}"/>
        internal override DalTask ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalTask result = new DalTask(reader.GetString(0), reader.GetString(1), (int)reader.GetValue(2), reader.GetString(3), reader.GetString(4), reader.GetDateTime(5), reader.GetDateTime(6), reader.GetDateTime(7));
            return result;
        }

        /// <summary>
        /// Creates the Tasks table in the Kanban.db.
        /// </summary>
        internal override void CreateTable()
        {       
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"CREATE TABLE {TaskTableName} (" +
                    $"{DalTask.EmailColumnName} TEXT NOT NULL," +
                    $"{DalTask.ContainingTaskColumnNameColumnName} TEXT NOT NULL," +
                    $"{DalTask.TaskIDColumnName} INTEGER NOT NULL," +
                    $"{DalTask.TaskTitleColumnName} TEXT NOT NULL," +
                    $"{DalTask.TaskDescriptionColumnName} TEXT NOT NULL," +
                    $"{DalTask.TaskDueDateColumnName} INTEGER NOT NULL," +
                    $"{DalTask.TaskCreationDateColumnName} INTEGER NOT NULL," +
                    $"{DalTask.TaskLastChangedDateColumnName} INTEGER NOT NULL," +
                    $"PRIMARY KEY({DalTask.EmailColumnName}, {DalTask.ContainingTaskColumnNameColumnName},{DalTask.TaskIDColumnName})" +
                    $"FOREIGN KEY({DalTask.EmailColumnName})" +
                    $"  REFERENCES {ColumnDalController.ColumnTableName} ({DalColumn.EmailColumnName})" +
                     $"FOREIGN KEY({DalTask.ContainingTaskColumnNameColumnName})" +
                    $"  REFERENCES {ColumnDalController.ColumnTableName} ({DalColumn.ColumnNameColumnName})" +
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
