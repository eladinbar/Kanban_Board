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
        private BusinessLayer.UserPackage.User _currentUser;
        private int _userCounter;

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
            _currentUser = null;
            _userCounter = 0;
        }

        public void RunTheTest()
        {
            int counter = 1;
            this.RegisterAll();
            for (int i = 1; i <= 10000000; i++)
            {
                if (i % 1000 == 0)
                {
                    Console.Write("#" + counter + " ");
                    int j = new Random().Next(1, 27);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(j+"<------------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    switch (j)
                    {
                        case 2:
                            {
                                Register();
                                Console.WriteLine(" Register() executed");
                                break;
                            }

                        case 1:
                        case 3:
                        case 4:
                        case 23:
                        case 24:
                            {
                                Console.WriteLine("Logout() and Login() executed");
                                Logout();
                                Login();
                                break;
                            }

                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 19:
                        case 20:
                        case 26:
                        case 5:
                            {
                                Console.WriteLine("AddAllTasks() executed");
                                AddAllTasks();
                                break;
                            }

                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 21:
                        case 25:
                            {
                                Console.WriteLine("AdvanceTask() executed");
                                AdvanceTask();
                                break;
                            }

                        case 16:
                        case 17:
                        case 22:
                            {
                                Console.WriteLine("EditTask() executed");
                                EditTask();
                                break;
                            }

                        case 18:
                            {
                                Console.WriteLine("LimitColumn() executed");
                                LimitColumn();
                                break;
                            }
                    }
                    counter++;
                }
            }
                        
        }


        private void RegisterAll()
        {
            foreach (ServiceLayer.User tempUser in _randomUsers)
            {
                _service.Register(tempUser.Email, _uniPassword, tempUser.Nickname);
                _userCounter++;
            }
        }

        private void Register()
        {
            if (_userCounter > 15) return;
            int rand = new Random().Next(1,10000);
            string email = rand.ToString() + "@mashu.com";
            string nickname = rand.ToString();
            _service.Register(email, _uniPassword, nickname);
            _randomUsers.Add(new ServiceLayer.User(email, nickname));
            _userCounter++;
        }

        private void Login()
        {
            int randUser = new Random().Next(8,_randomUsers.Count-1);
            _service.Login(_randomUsers.ElementAt(randUser).Email, _uniPassword);
            if (_currentUser==null)
                _currentUser = new BusinessLayer.UserPackage.User(_randomUsers.ElementAt(randUser).Email, _uniPassword, _randomUsers.ElementAt(randUser).Nickname);

        }

        private void Logout()
        {
            int randUser = new Random().Next(0, _randomUsers.Count - 1);
            _service.Logout(_randomUsers.ElementAt(randUser).Email);
            if (_currentUser != null && _randomUsers.ElementAt(randUser).Email.Equals(_currentUser.Email))
                _currentUser = null;
        }

        private void AddAllTasks()
        {
            if (_currentUser!=null)
                foreach (ServiceLayer.Task tempTask in _randomTasks) _service.AddTask(_currentUser.Email, tempTask.Title, tempTask.Description, tempTask.DueDate);
        
        }

        private void AdvanceTask()
        {
            if (_currentUser != null)
            {
                int randTaskId = new Random().Next(1, 11);
                int randOrdinal = new Random().Next(0, 2);
                _service.AdvanceTask(_currentUser.Email, randOrdinal, randTaskId);
            }
        }

        private void EditTask()
        {
            if (_currentUser != null)
            {
                int randTaskId = new Random().Next(1, 11);
                int randOrdinal = new Random().Next(0, 2);
                int randTest = new Random().Next(0, 2);
                DateTime dueDate = new DateTime(2035, 03, 26);
                if (randTaskId == 0)
                    _service.UpdateTaskDescription(_currentUser.Email, randOrdinal, randTaskId, "this description was updated by NoramlUsageTest");
                if (randTaskId == 1)
                    _service.UpdateTaskDueDate(_currentUser.Email, randOrdinal, randTaskId, dueDate);
                if (randTaskId == 2)
                    _service.UpdateTaskTitle(_currentUser.Email, randOrdinal, randTaskId, "this title was updated by NoramlUsageTest");
            }

        }

        private void LimitColumn()
        {
            if (_currentUser != null)
            {
                int randOrdinal = new Random().Next(0, 2);
                _service.LimitColumnTasks(_currentUser.Email, randOrdinal, 5);
            }
        }
    }
}
