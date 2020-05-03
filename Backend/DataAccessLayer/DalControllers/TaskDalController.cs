using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DalControllers
{
    internal class TaskDalController : DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();
        private const string TaskTableName = "Tasks";

        public TaskDalController(): base(TaskTableName) { }

        public List<DalTask> SelectAllTasks(string email, int ordinal)
        {
            List<DalTask> taskList = Select(email, ordinal).Cast<DalTask>().ToList();
            return taskList;
        }


        public bool Insert(DalTask task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} " +
                        $"({DalTask.EmailColumnName}, {DalTask.TaskOrdinalColumnName}, {DalTask.TaskIDColumnName}, {DalTask.TaskTitleColumnName}, {DalTask.TaskDescriptionColumnName}, {DalTask.TaskDueDateColumnName},{DalTask.TaskCreationDateColumnName}, {DalTask.TaskLastChangedDateColumnName})" +
                        $"VALUES (@emailVal, @ordinalVal, @idVal, @titleVal, @descriptionVal, @dueDateVal, @creationDateVal, @lastChangedDateVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", task.Email);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", task.Ordinal);
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
                }
            }
            return res > 0;
        }

        public bool Delete(DalTask task)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DALETE FROM {TaskTableName} WHERE email={task.Email} AND Ordinal={task.Ordinal} AND ID={task.TaskId}"
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
            DalTask result = new DalTask(reader.GetString(0), (int)reader.GetValue(1), (int)reader.GetValue(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7));
            return result;
        }
    }
}
