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

        /// <summary>
        /// A public constructor that initializes the database path and the connection string accordingly. Initializes the 'Tasks' table name and creates it in the database.
        /// </summary>
        public TaskDalController(): base(TaskTableName) { }

        /// <summary>
        /// Retrieves all task data of a specific board and column from the database.
        /// </summary>
        /// <param name="email">The board to retrieve tasks from.</param>
        /// <param name="columnName">The column to retrieve tasks from.</param>
        /// <returns>Returns a list of all DalTask objects associated with the specified board and column from the database.</returns>
        public List<DalTask> SelectAllTasks(string email, string columnName)
        {
            log.Info("Loading all tasks from the database.");
            List<DalTask> taskList = Select(email, columnName).Cast<DalTask>().ToList();
            return taskList;
        }

        /// <summary>
        /// Creates the 'Tasks' table in the database.
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
        /// Converts an SQLiteDataReader to a DalTask.
        /// </summary>
        /// <param name="reader">The SQLite reader to convert.</param>
        /// <returns>Returns a DalTask.</returns>
        internal override DalTask ConvertReaderToObject(SQLiteDataReader reader)
        {
            DalTask result = new DalTask(reader.GetString(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), 
                reader.GetString(4), DateTime.Parse(reader.GetString(5)), DateTime.Parse(reader.GetString(6)), DateTime.Parse(reader.GetString(7)));
            return result;
        }

        /// <summary>
        /// Inserts the given task into the 'Tasks' table in the database.
        /// </summary>
        /// <param name="task">The data access layer task instance to insert into the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
        public override bool Insert(DalTask task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    log.Info("Opening a connection to the database");
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} " +
                        $"({DalTask.EmailColumnName}, {DalTask.ContainingTaskColumnNameColumnName}, {DalTask.TaskIDColumnName}, {DalTask.TaskTitleColumnName}," +
                        $" {DalTask.TaskDescriptionColumnName}, {DalTask.TaskDueDateColumnName},{DalTask.TaskCreationDateColumnName}, {DalTask.TaskLastChangedDateColumnName})" +
                        $"VALUES (@emailVal, @ordinalVal, @idVal, @titleVal, @descriptionVal, @dueDateVal, @creationDateVal, @lastChangedDateVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.Email);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", task.ColumnName);
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.TaskId);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate.ToString());
                    SQLiteParameter CreationDateParam = new SQLiteParameter(@"creationDateVal", task.CreationDate.ToString());
                    SQLiteParameter lastChangedDateParam = new SQLiteParameter(@"lastChangedDateVal", task.LastChangedDate.ToString());

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
        /// Deletes the given task from the 'Tasks' table in the database.
        /// </summary>
        /// <param name="task">The data access layer task instance to delete from the database.</param>
        /// <returns>Returns true if the method changed more than 0 rows.</returns>
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
