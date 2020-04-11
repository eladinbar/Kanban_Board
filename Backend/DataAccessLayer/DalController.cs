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
        private readonly string BASE_PATH = Path.GetFullPath(@"..\..\") + "data\\";

        public void WriteToFile (string fileName, string content) {
            File.WriteAllText(BASE_PATH + fileName + ".json", content);//
        }

        public string ReadFromFile (string fileName) {
            return File.ReadAllText(BASE_PATH + fileName + ".json");//
        }

        public List<User> LoadAllUsers() {
            List<User> users = new List<User>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Users\\");
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
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\");
            if (dir.Exists) {
                foreach (FileInfo board in dir.GetFiles("*.json")) {
                    Board savedBoard = new Board();
                    savedBoard = savedBoard.FromJson(board.Name);
                    LoadAllColumns(board.Name);
                    boards.Add(savedBoard);
                }
            }
            else
                dir.Create();
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
                    savedColumn = savedColumn.FromJson(column.Name);
                    LoadAllTasks(boardName, column.Name);
                    columns.Add(savedColumn);
                }
            }
            else
                dir.Create();
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
                    savedTask = savedTask.FromJson(task.Name);
                    tasks.Add(savedTask);
                }
            }
            else
                dir.Create();
            return tasks;
        }
    }
}
