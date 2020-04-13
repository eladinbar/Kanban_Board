using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    public class BoardController
    {
        private Dictionary<String, Board> Boards;
        
        public BoardController()
        {
            DalController dalC = new DalController();
            Boards = new Dictionary<string, Board>();
            List<DataAccessLayer.Board> DALboards = dalC.LoadAllBoards();
            foreach (DataAccessLayer.Board DALboard in DALboards) {
                List<Column> columns = new List<Column>();
                foreach (DataAccessLayer.Column DALcolumn in DALboard.Columns) {
                    List<Task> tasks = new List<Task>();
                    foreach (DataAccessLayer.Task DALtask in DALcolumn.Tasks) {
                        tasks.Add(new Task(DALtask.Title, DALtask.Description, DALtask.DueDate, DALtask.Id, DALtask.CreationTime, DALtask.LastChangedDate));
                    }
                    columns.Add(new Column(DALcolumn.Name, tasks, DALcolumn.Limit));
                }
                Boards.Add(DALboard.UserEmail, new Board(DALboard.UserEmail, DALboard.TaskCounter, columns));
            }
        }
              
        public Board GetBoard(string email)
        {
            Board tempBoard;
            if (Boards.TryGetValue(email, out tempBoard))
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
                c.Save("Boards\\" + email + "\\");
            }

            else
                throw new ArgumentException("limit must be a natural non zero number");
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
                targetColumn.InsertTask(toAdvance);
                toAdvance.Save("Boards\\" + email + "\\" + targetColumn.Name + "\\");
                toAdvance.Delete(toAdvance.Id + "", "Boards\\" + email + "\\" + c.Name + "\\");
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
            newTask.Save("Boards\\" + email + "\\" + c.Name + "\\");
            return newTask;            
        }

        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskTitle(newTitle);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
        }

        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskDescription(newDescription);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
        }

        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskDuedate(newDueDate);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
        }

        public void AddNewBoard(string email)
        {
            Boards.Add(email, new Board(email));
        }
    }
}
