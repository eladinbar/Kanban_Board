using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class NormalUsageStateTest
    {
        private Service service;
        private List<User> randomUsers;
        private List<ServiceLayer.Task> randomTasks;
        private string uniPassword;
        private BusinessLayer.UserPackage.User currentUser;
        private int userCounter;

        public NormalUsageStateTest()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }

            service = new ServiceLayer.Service();
            service.LoadData();
            randomUsers = new UserForTestCreator(10)._users;
            randomTasks = new TaskForTestCreator(10)._tasks;
            uniPassword = "123Abc";
            currentUser = null;
            userCounter = 0;
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
                    Console.WriteLine(j + "<------------------------");
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
            foreach (User tempUser in randomUsers)
            {
                service.Register(tempUser.Email, uniPassword, tempUser.Nickname);
                userCounter++;
            }
        }

        private void Register()
        {
            if (userCounter > 15) return;
            int rand = new Random().Next(1, 10000);
            string email = rand.ToString() + "@mashu.com";
            string nickname = rand.ToString();
            service.Register(email, uniPassword, nickname);
            randomUsers.Add(new ServiceLayer.User(email, nickname));
            userCounter++;
        }

        private void Login()
        {
            int randUser = new Random().Next(8, randomUsers.Count - 1);
            service.Login(randomUsers.ElementAt(randUser).Email, uniPassword);
            if (currentUser == null)
                currentUser = new BusinessLayer.UserPackage.User(randomUsers.ElementAt(randUser).Email, uniPassword, randomUsers.ElementAt(randUser).Nickname);

        }

        private void Logout()
        {
            int randUser = new Random().Next(0, randomUsers.Count - 1);
            service.Logout(randomUsers.ElementAt(randUser).Email);
            if (currentUser != null && randomUsers.ElementAt(randUser).Email.Equals(currentUser.Email))
                currentUser = null;
        }

        private void AddAllTasks()
        {
            if (currentUser != null)
                foreach (ServiceLayer.Task tempTask in randomTasks) service.AddTask(currentUser.Email, tempTask.Title, tempTask.Description, tempTask.DueDate);

        }

        private void AdvanceTask()
        {
            if (currentUser != null)
            {
                int randTaskId = new Random().Next(1, 11);
                int randOrdinal = new Random().Next(0, 2);
                service.AdvanceTask(currentUser.Email, randOrdinal, randTaskId);
            }
        }

        private void EditTask()
        {
            if (currentUser != null)
            {
                int randTaskId = new Random().Next(1, 11);
                int randOrdinal = new Random().Next(0, 2);
                int randTest = new Random().Next(0, 2);
                DateTime dueDate = new DateTime(2035, 03, 26);
                if (randTaskId == 0)
                    service.UpdateTaskDescription(currentUser.Email, randOrdinal, randTaskId, "this description was updated by NoramlUsageTest");
                if (randTaskId == 1)
                    service.UpdateTaskDueDate(currentUser.Email, randOrdinal, randTaskId, dueDate);
                if (randTaskId == 2)
                    service.UpdateTaskTitle(currentUser.Email, randOrdinal, randTaskId, "this title was updated by NoramlUsageTest");
            }

        }

        private void LimitColumn()
        {
            if (currentUser != null)
            {
                int randOrdinal = new Random().Next(0, 2);
                service.LimitColumnTasks(currentUser.Email, randOrdinal, 5);
            }
        }
    }
}