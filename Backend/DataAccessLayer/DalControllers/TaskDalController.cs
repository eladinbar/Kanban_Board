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
        private const string TaskTableName = "Tasks";

        public TaskDalController(): base(TaskTableName) { }

        public List<DalTask> SelectAllTasks(string email, int ordinal)
        {
            List<DalTask> taskList = Select(email,ordinal).Cast<DalTask>().ToList();
            return taskList;
        }

        internal override DalObject ConvertReaderToObject(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
