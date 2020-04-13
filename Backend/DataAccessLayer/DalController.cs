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
        private static readonly log4net.ILog log = LogHelper.getLogger();

        private readonly string _BASE_PATH = Path.GetFullPath(@"..\..\") + "data\\";

        public void WriteToFile (string fileName, string content, string path) {
            File.WriteAllText(BASE_PATH + path + fileName + ".json", content);
        }

        public string ReadFromFile (string fileName, string path) {
            return File.ReadAllText(BASE_PATH + path + fileName + ".json");
        }

        public void RemoveFromFile (string fileName, string path) {
            File.Delete(BASE_PATH + path + fileName + ".json");
        }

        public List<User> LoadAllUsers() {
            List<User> users = new List<User>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Users\\");
            if (dir.Exists) //Checks that the directory exists and loads all user data from it
            {
                foreach (FileInfo user in dir.GetFiles("*.json"))
                {
                    User savedUser = new User();
                    savedUser = savedUser.FromJson(ReadFromFile(user.Name, "Users\\"));
                    users.Add(savedUser);
                }
            }
            return users;
        }

        public List<Board> LoadAllBoards() {
            List<Board> boards = new List<Board>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\");
            if (dir.Exists) {
                foreach (FileInfo board in dir.GetFiles("*.json")) {
                    Board savedBoard = new Board();
                    savedBoard = savedBoard.FromJson(ReadFromFile(board.Name, "Boards\\"));
                    savedBoard = new Board(savedBoard.UserEmail, savedBoard.TaskCounter, LoadAllColumns(board.Name));
                    boards.Add(savedBoard);
                }
            }
            return boards;
        }

        private List<Column> LoadAllColumns(string boardName)
        {
            List<Column> columns = new List<Column>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\" + boardName + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo column in dir.GetFiles("*.json"))
                {
                    Column savedColumn = new Column();
                    savedColumn = savedColumn.FromJson(ReadFromFile(column.Name, "Boards\\" + boardName));
                    savedColumn = new Column(savedColumn.Name, savedColumn.Limit, LoadAllTasks(boardName, column.Name));
                    columns.Add(savedColumn);
                }
            }
            return columns;
        }

        private List<Task> LoadAllTasks (string boardName, string columnName) {
            List<Task> tasks = new List<Task>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\" + boardName + "\\" + columnName + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo task in dir.GetFiles("*.json"))
                {
                    Task savedTask = new Task();
                    savedTask = savedTask.FromJson(ReadFromFile(task.Name, "Boards\\" + boardName + "\\" + columnName + "\\"));
                    tasks.Add(savedTask);
                }
            }
            return tasks;
        }

        //getter
        public string BASE_PATH { get; }
    }
}
