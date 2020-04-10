using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class BoardController
    {
        private Dictionary<String, Board> _boards;
        int _TaskCounter;

        public BoardController()
        {
            this._boards = null;
            this._TaskCounter = 0;
        }

        public Board GetBoard(string email)
        {
            Board newBoard;
            if (_boards.TryGetValue(email, out newBoard))
                return newBoard;
            else
                throw new ArgumentException("board not exist with this email");
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

        private Dictionary<string, Board> Boards { get; set; }
        private int TaskCounter { get; set; }

    }
}
