using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class NormalUsageStateTest
    {
        private ServiceLayer.Service _service;
        private List<ServiceLayer.User> _randomUsers;
        private List<ServiceLayer.Task> _randomTasks;
        private string _uniPassword;

        public NormalUsageStateTest()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }

            _service = new ServiceLayer.Service();
            _service.LoadData();
            _randomUsers = new UserForTestCreator(10)._users;
            _randomTasks = new TaskForTestCreator(10)._tasks;
            _uniPassword = "123Abc";
        }

        public void RunTheTest()
        {
            this.RegisterAll();
            for (int k = 0; k < 100; k++)
            {
                int i = new Random().Next(1,7);
                switch (i)
                {
                    case 1:
                        Register();
                        Console.WriteLine("#"+i+" Register() executed");
                        break;

                    case 2:
                        Console.WriteLine("#" + i + " Login() executed");
                        Login();
                        break;

                    case 3:
                        Console.WriteLine("#" + i + " Logout() executed");
                        Logout();
                        break;

                    case 4:
                        Console.WriteLine("#" + i + " AddAllTasks() executed");
                        AddAllTasks();
                        break;

                    case 5:
                        Console.WriteLine("#" + i + " AdvanceTask() executed");
                        AdvanceTask();
                        break;

                    case 6:
                        Console.WriteLine("#" + i + " EditTask() executed");
                        EditTask();
                        break;

                    case 7:
                        Console.WriteLine("#" + i + " LimitColumn() executed");
                        LimitColumn();
                        break;

                    default: break;
                }

            }

            /*
             * 1 = Register();
             * 2 = Login();
             * 3 = Logout();
             * 4 = AddAllTasks();
             * 5 = AdvanceTask();
             * 6 = EditTask();
             * 7 = LimitColumn();
             */
        }


        private void RegisterAll()
        {
            foreach (ServiceLayer.User tempUser in _randomUsers) _service.Register(tempUser.Email, _uniPassword, tempUser.Nickname);
        }

        private void Register()
        {
            int rand = new Random().Next(1,10000);
            string email = rand.ToString() + "@mashu.com";
            string password = rand.ToString() + "Aa";
            string nickname = rand.ToString();
            _service.Register(email, password, nickname);
        }

        private void Login()
        {
            int rand = new Random().Next(1,10);
            _service.Login(_randomUsers.ElementAt(rand).Email, _uniPassword);

        }

        private void Logout()
        {
            int rand = new Random().Next(1, 10);
            _service.Logout(_randomUsers.ElementAt(rand).Email);
        }

        private void AddAllTasks()
        {
            int rand = new Random().Next(1, 10);
            foreach (ServiceLayer.Task tempTask in _randomTasks) _service.AddTask(_randomUsers.ElementAt(rand).Email, tempTask.Title, tempTask.Description, tempTask.DueDate);
        }

        private void AdvanceTask()
        {
            int randUser = new Random().Next(1, 10);
            int randTaskId = new Random().Next(1, 100);
            int randOrdinal = new Random().Next(1, 5);
            _service.AdvanceTask(_randomUsers.ElementAt(randUser).Email, randOrdinal, randTaskId);
        }

        private void EditTask()
        {
            int randTaskId = new Random().Next(1, 100);
            int randUser = new Random().Next(1, 10);
            int randOrdinal = new Random().Next(1, 5);
            int randTest = new Random().Next(0, 2);
            DateTime dueDate = new DateTime(2035, 03, 26);
            if (randTaskId == 0)
                _service.UpdateTaskDescription(_randomUsers.ElementAt(randUser).Email, randOrdinal, randTaskId, "this description was updated by NoramlUsageTest");
            if (randTaskId == 1)
                _service.UpdateTaskDueDate(_randomUsers.ElementAt(randUser).Email, randOrdinal, randTaskId, dueDate);
            if (randTaskId == 2)
                _service.UpdateTaskTitle(_randomUsers.ElementAt(randUser).Email, randOrdinal, randTaskId, "this title was updated by NoramlUsageTest");

        }

        private void LimitColumn()
        {
            int randUser = new Random().Next(1, 10);
            int randOrdinal = new Random().Next(1, 5);
            _service.LimitColumnTasks(_randomUsers.ElementAt(randUser).Email,randOrdinal, 2);
        }
    }
}
