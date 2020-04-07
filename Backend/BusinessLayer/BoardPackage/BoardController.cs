using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class BoardController
    {
        private Dictionary<String, Board> Boards;
        int TaskCounter;

        public BoardController()
        {
            this.Boards = null;
            this.TaskCounter = 0;
        }

        public Board GetBoard(string email)
        {
            throw new NotImplementedException();
        }

        public Column GetColumn(string email, string columnName)
        {
            throw new NotImplementedException();
        }

        public Column GetColumn(string email, int columnOrdinal)
        {
            throw new NotImplementedException();
        }

        public void LimitColumnTask(string email, int columnOrdinal, int limit)
        {
            throw new NotImplementedException();
        }

        public void AdvanceTask(string email, int columnOrdinal,int TaskId)
        {
            throw new NotImplementedException();
        }

        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, int newTitle)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, int newDescription)
        {
            throw new NotImplementedException();
        }

        public void UpdateDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            throw new NotImplementedException();
        }

    }
}
