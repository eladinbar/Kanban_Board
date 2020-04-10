using IntroSE.Kanban.Backend.DataAccessLayer;
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
        

        public BoardController()
        {
            DalController dalC = new DalController();
            _boards = dalC.LoadAllboards();            
        }
              
        public Board GetBoard(string email)
        {
            Board tempBoard;
            if (_boards.TryGetValue(email, out tempBoard))
                return tempBoard;
            else
                throw new ArgumentException("board not exist with this email");
        }

        public Column GetColumn(string email, string columnName)
        {
            Board newBoard = GetBoard(email);
            return newBoard.GetColumn(columnName);
        }

        public Column GetColumn(string email, int columnOrdinal)
        {
            Board b = GetBoard(email);
            return b.GetColumn(columnOrdinal);
        }

        public void LimitColumnTask(string email, int columnOrdinal, int limit)
        {
            Board b = GetBoard(email);
            Column c = b.GetColumn(columnOrdinal);
            if (limit > 0)
            {
                c.LimitColumnTasks(limit);
                c.Save();
            }

            else
                throw new ArgumentException("limit must be a ntural non zero number");
        }

        public void AdvanceTask(string email, int columnOrdinal,int taskId)
        {
            Board b = GetBoard(email);
            if (b.GetColumn(columnOrdinal).Name.Equals("Done"))
                throw new ArgumentOutOfRangeException("cannot advance task at Done Column");
            else
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toAdvance = c.RemoveTask(taskId);
                Column targetColumn = b.GetColumn(columnOrdinal + 1); 
                c.InsertTask(toAdvance);
                c.Save();
            }
        }

        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            Column c = GetColumn(email, "Backlog");
            if (!c.CheckLimit())
                throw new Exception("backlog column is full");

            int taskCounter = GetBoard(email).TaskCounter;
            Task newTask = new Task(title, description, dueDate, taskCounter);
            taskCounter++;
            
            c.InsertTask(newTask);
            c.Save();
            return newTask;            
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle)
        {
            GetColumn(email, columnOrdinal).GetTask(taskId).UpdateTaskTitle(newTitle);
        }

        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription)
        {
            GetColumn(email, columnOrdinal).GetTask(taskId).UpdateTaskDescription(newDescription);
        }

        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            GetColumn(email, columnOrdinal).GetTask(taskId).UpdateTaskDuedate(newDueDate);
        }

      
       

       

    }
}
