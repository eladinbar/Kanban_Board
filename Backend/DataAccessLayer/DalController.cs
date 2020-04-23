using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// The DalController governs all data access activity.
    /// </summary>
    internal class DalController
    {
        private static readonly log4net.ILog log = LogHelper.getLogger();

        public string BASE_PATH { get; }

        public DalController () {
            //BASE_PATH = Path.GetFullPath(@"..\..\") + "data\\";
            BASE_PATH = Directory.GetCurrentDirectory() + "\\data\\";
            log.Info("DalController created");
        }

        /// <summary>
        /// Writes a a file containing string content to a specific path on the disk.
        /// </summary>
        /// <param name="fileName">The name the file will be saved with.</param>
        /// <param name="content">The content the file will contain when writing to the disk.</param>
        /// <param name="path">The path the file will be written to.</param>
        public void WriteToFile (string fileName, string content, string path) {
            try
            {
                File.WriteAllText(BASE_PATH + path + fileName + ".json", content);
            } catch(Exception ex)
            {
                log.Fatal(ex);
            }
        }

        /// <summary>
        /// Reads a file from a specific path on the disk.
        /// </summary>
        /// <param name="fileName">The name of the file to read from the disk.</param>
        /// <param name="path">The path the file will be read from.</param>
        /// <returns>Returns the string representing the text written in the file.</returns>
        public string ReadFromFile (string fileName, string path) {
            try
            {
                string fromMemory = File.ReadAllText(BASE_PATH + path + fileName);
                return fromMemory;
            } catch(Exception ex)
            {
                log.Fatal(ex);
                return "";
            }
        }

        /// <summary>
        /// Removes a file from a specific path from the disk.
        /// </summary>
        /// <param name="fileName">The name the file to be removed.</param>
        /// <param name="path">The path the file will be removed from.</param>
        public void RemoveFromFile (string fileName, string path) {
            try
            {
                File.Delete(BASE_PATH + path + fileName + ".json");
            } catch(Exception ex)
            {
                log.Fatal(ex);
            }
        }

        /// <summary>
        /// Loads all user data previously saved on the disk.
        /// </summary>
        /// <returns>Returns a DataAccessLayer List of all the users that were saved to the disk.</returns>
        public List<User> LoadAllUsers() {
            log.Debug("Loading all users");
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

        /// <summary>
        /// Loads all board data previously saved on the disk.
        /// </summary>
        /// <returns>Returns a DataAccessLayer List of all the boards that were saved to the disk.</returns>
        public List<Board> LoadAllBoards() {
            log.Debug("Loading all Boards");
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

        /// <summary>
        /// Loads all column data previously saved on the disk.
        /// </summary>
        /// <returns>Returns a DataAccessLayer List of all the columns that were saved to the disk.</returns>
        private List<Column> LoadAllColumns(string boardName)
        {
            log.Debug("Loading all columns of " + boardName);
            List<Column> columns = new List<Column>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\" + boardName.Substring(0, boardName.Length - 5) + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo column in dir.GetFiles("*.json"))
                {
                    Column savedColumn = new Column();
                    savedColumn = savedColumn.FromJson(ReadFromFile(column.Name, "Boards\\" + boardName.Substring(0, boardName.Length - 5)+"\\"));
                    savedColumn = new Column(savedColumn.Name, savedColumn.Limit, LoadAllTasks(boardName, column.Name));
                    columns.Add(savedColumn);
                }
            }
            return columns;
        }

        /// <summary>
        /// Loads all user data previously saved on the disk.
        /// </summary>
        /// <returns>Returns a DataAccessLayer List of all the tasks that were saved to the disk.</returns>
        private List<Task> LoadAllTasks (string boardName, string columnName) {
            log.Debug("Loading all tasks in " + columnName + " of Board" + boardName);
            List<Task> tasks = new List<Task>();
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "Boards\\" + boardName.Substring(0, boardName.Length - 5) + "\\" + columnName.Substring(0, columnName.Length - 5) + "\\");
            if (dir.Exists)
            {
                foreach (FileInfo task in dir.GetFiles("*.json"))
                {
                    Task savedTask = new Task();
                    savedTask = savedTask.FromJson(ReadFromFile(task.Name, "Boards\\" + boardName.Substring(0, boardName.Length - 5) + "\\" + columnName.Substring(0, columnName.Length - 5) + "\\"));
                    tasks.Add(savedTask);
                }
            }
            return tasks;
        }
    }
}
