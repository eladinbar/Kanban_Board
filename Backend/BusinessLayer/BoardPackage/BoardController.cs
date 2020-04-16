using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer.BoardPackage
{
    /// <summary>
    /// The BoardController is the class that controls the functionality of the BoardPackage.
    /// Contains the methods for adding and modifying the content of a board.
    /// </summary>
    public class BoardController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

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
         
        /// <summary>
        /// Gets the Board woth the key equals to email.
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <returns> 
        /// The Board of the user with that email.
        /// </returns>
        /// <exception cref="ArgumentException.ArgumentException(string)">
        /// Thrown when there is no key that equals to email in Boards dictionary.
        /// </exception>
        public Board GetBoard(string email)
        {
            Board tempBoard;
            if (Boards.TryGetValue(email, out tempBoard))
                return tempBoard;
            else
                throw new ArgumentException("board not exist with this email");
        }
        /// <summary>
        /// Get the column in the board with the email and the specified columnName.
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnName">The name of the column in the board.</param>
        /// <returns>
        /// return Column with the spesified name in the board with the key email.
        /// </returns>
        public Column GetColumn(string email, string columnName)
        {
            Board newBoard = GetBoard(email);
            return newBoard.GetColumn(columnName);
        }
        /// <summary>
        /// Get the column in the board with the key email and the specified columnOrdinal .
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <returns>
        /// return Column with the spesified columnOrdinal in the board with the key email.
        /// </returns>
        public Column GetColumn(string email, int columnOrdinal)
        {
            Board b = GetBoard(email);
            return b.GetColumn(columnOrdinal);
        }

        /// <summary>
        /// limit the number of taks that the columns with columnOrdinal can hold.
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <param name="limit">The number of maximum task that the column will hold.</param>
        /// <exception cref="ArgumentException">theown when tring to set limit to s number less or equal to 0.</exception>
        public void LimitColumnTask(string email, int columnOrdinal, int limit)
        {
            Board b = GetBoard(email);
            Column c = b.GetColumn(columnOrdinal);
            c.LimitColumnTasks(limit);
            c.Save("Boards\\" + email + "\\");
           
        }
        /// <summary>
        /// Advence a task from specified column to the next one.
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <param name="taskId">The task ID to advance.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Trown if trying to advance from Done column or is the next column is full.
        /// </exception>
        public void AdvanceTask(string email, int columnOrdinal,int taskId)
        {
            Board b = GetBoard(email);
            if (b.GetColumn(columnOrdinal).Name.Equals("Done"))
            {
                log.Error("attempt to advance task in Done column");
                throw new ArgumentOutOfRangeException("cannot advance task at Done Column");
            }
            else if (!b.GetColumn(columnOrdinal + 1).CheckLimit())
            {
                log.Error("attempt to advance task to a full column");
                throw new ArgumentOutOfRangeException("Next column is full");
            }
            else
            {
                Column c = b.GetColumn(columnOrdinal);
                Task toAdvance = c.RemoveTask(taskId);
                Column targetColumn = b.GetColumn(columnOrdinal + 1);
                targetColumn.InsertTask(toAdvance);
                toAdvance.Save("Boards\\" + email + "\\" + targetColumn.Name + "\\");
                toAdvance.Delete(toAdvance.Id + "", "Boards\\" + email + "\\" + c.Name + "\\");
                log.Debug("task " + taskId + " was advanced");
            }
        }
        /// <summary>
        /// Adds a new task to "Backlog" column.
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="title">Title of the task.</param>
        /// <param name="description">Description of the new task.</param>
        /// <param name="dueDate">Duedate of the new task.</param>
        /// <returns>
        /// return the new task that was created.
        /// </returns>
        /// <exception cref="Exception">
        /// thrown is "Backlog" column is full.
        /// </exception>
        public Task AddTask(string email, string title, string description, DateTime dueDate)
        {
            Column c = GetColumn(email, "Backlog");
            if (!c.CheckLimit())
            {
                log.Error("attemp to add task when backlog is full");
                throw new Exception("backlog column is full");
            }
            Task newTask = new Task(title, description, dueDate, GetBoard(email).TaskCounter);
            GetBoard(email).TaskCounter = GetBoard(email).TaskCounter + 1;


            c.InsertTask(newTask);
            newTask.Save("Boards\\" + email + "\\" + c.Name + "\\");
            log.Debug("new task was added to Backlog Column");
            return newTask;            
        }
        /// <summary>
        /// update the title of the task with taskId 
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <param name="taskId">The task ID to update.</param>
        /// <param name="newTitle">the new title to insert</param>
        public void UpdateTaskTitle(string email, int columnOrdinal, int taskId, string newTitle)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskTitle(newTitle);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
            log.Debug("Task title was updated");
        }

        /// <summary>
        /// update the description of the task with taskId 
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <param name="taskId">The task ID to update.</param>
        /// <param name="newTitle">the new desctiption to insert</param>
        public void UpdateTaskDescription(string email, int columnOrdinal, int taskId, string newDescription)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskDescription(newDescription);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
            log.Debug("Task description was updated");
        }

        /// <summary>
        /// update the duedate of the task with taskId 
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        /// <param name="columnOrdinal">the column number of the board.</param>
        /// <param name="taskId">The task ID to update.</param>
        /// <param name="newTitle">the new duedate to insert</param>
        public void UpdateTaskDueDate(string email, int columnOrdinal, int taskId, DateTime newDueDate)
        {
            Task toUpdate = GetColumn(email, columnOrdinal).GetTask(taskId);
            toUpdate.UpdateTaskDuedate(newDueDate);
            toUpdate.Save("Boards\\" + email + "\\" + GetColumn(email, columnOrdinal).Name + "\\");
            log.Debug("Task doudate was updated");
        }

        /// <summary>
        /// Adds a new board to the Boards dictionary
        /// </summary>
        /// <param name="email">the email of the user that the board belong.</param>
        public void AddNewBoard(string email)
        {
            Board newBoard = new Board(email);
            Boards.Add(email, newBoard);
            newBoard.Save("Boards\\");
            log.Info("New board was added with kay " + email);
        }
    }
}
