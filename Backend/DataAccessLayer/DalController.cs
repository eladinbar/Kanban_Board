using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class DalController
    {
        private readonly string _BASE_PATH = Path.GetFullPath(@"..\..\") + "data\\";

        public void WriteToFile (string fileName, string content, string path) {
            File.WriteAllText(_BASE_PATH + path + fileName + ".json", content);
        }

        public string ReadFromFile (string fileName) {
            return File.ReadAllText(_BASE_PATH + fileName + ".json");
        }

        public void RemoveFromFile (string path) {
            File.Delete(BASE_PATH + path);
        }

        public List<User> LoadAllUsers() {
            List<User> users = new List<User>();
            DirectoryInfo dir = new DirectoryInfo(_BASE_PATH + "Users\\");
            if (dir.Exists) { //If the directory and any user data exists, load all of it
                foreach (FileInfo user in dir.GetFiles("*.json")) {
                    User savedUser = new User();
                    savedUser = savedUser.FromJson(user.Name);
                    users.Add(savedUser);
                }
            }
            else //Otherwise create a new folder to store future data in
                dir.Create();
            return users;
        }

        public List<Board> LoadAllBoards() {
            List<Board> boards = new List<Board>();
            DirectoryInfo dir = new DirectoryInfo(_BASE_PATH + "Boards\\");
            if (dir.Exists) {
                foreach (FileInfo board in dir.GetFiles("*.json")) {
                    Board savedBoard = new Board();
                    savedBoard = savedBoard.FromJson(board.Name);
                    savedBoard = new Board(savedBoard.UserEmail, savedBoard.TaskCounter, LoadAllColumns(board.Name));
                    boards.Add(savedBoard);
                }
            }
            return boards;
        }

        private List<Column> LoadAllColumns(string boardName)
        {
            List<Column> columns = new List<Column>();
            DirectoryInfo dir = new DirectoryInfo(_BASE_PATH + "Boards\\" + boardName + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo column in dir.GetFiles("*.json"))
                {
                    Column savedColumn = new Column();
                    savedColumn = savedColumn.FromJson(column.Name);
                    savedColumn = new Column(savedColumn.Name, savedColumn.Limit, LoadAllTasks(boardName, column.Name));
                    columns.Add(savedColumn);
                }
            }
            return columns;
        }

        private List<Task> LoadAllTasks (string boardName, string columnName) {
            List<Task> tasks = new List<Task>();
            DirectoryInfo dir = new DirectoryInfo(_BASE_PATH + "Boards\\" + boardName + "\\" + columnName + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo task in dir.GetFiles("*.json"))
                {
                    Task savedTask = new Task();
                    savedTask = savedTask.FromJson(task.Name);
                    tasks.Add(savedTask);
                }
            }
            return tasks;
        }

        public string BASE_PATH { get; }
    }
}
