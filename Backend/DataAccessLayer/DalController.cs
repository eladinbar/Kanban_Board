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
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH);
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
            DirectoryInfo dir = new DirectoryInfo(BASE_PATH + "");
        }
    }
}
