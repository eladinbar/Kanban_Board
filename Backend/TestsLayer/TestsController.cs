using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TestsController
    {
      static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }
            



            //declaring random users
            UserForTestCreator userForTestCreator = new UserForTestCreator(5);
            List<ServiceLayer.User> randomUsers = userForTestCreator._users;
            userForTestCreator.PrintUsers();

            string uniPassword = "abC123";

            //declaring random tasks
            TaskForTestCreator taskForTestCreator = new TaskForTestCreator(5);
            List<ServiceLayer.Task> randomTasks = taskForTestCreator._tasks;
            taskForTestCreator.PrintTasks();


            //declaring new Service.cs appearance.
            ServiceLayer.Service service = new ServiceLayer.Service();


            //LoadData tests
            LoadDataTest ldTest = new LoadDataTest();
            ldTest.LoadData(service);
            ldTest.LoadData(service);
            ldTest.LoadData(service);


            //Register tests - random users registration
            
            
            RegisterTest regTest = new RegisterTest(service);
            regTest.BadPassword(randomUsers.ElementAt(0), "12345");
            regTest.BadPassword(randomUsers.ElementAt(0), "123abc");
            regTest.BadPassword(randomUsers.ElementAt(0), "123abcс");
            regTest.BadPassword(randomUsers.ElementAt(0), "!@#$%^Abc1");
            regTest.NonAcceptableEmail("ads@@mashu.com", uniPassword);
            regTest.NonAcceptableEmail("ads@mashucom", uniPassword);
            regTest.NonAcceptableEmail("ads@mashu.", uniPassword);
            regTest.NonAcceptableEmail("adsmashu.com", uniPassword);
            regTest.NonAcceptableEmail("@mashu.com", uniPassword);





            foreach (ServiceLayer.User tempUser in randomUsers)
                regTest.AllGood(tempUser, uniPassword);
            
            //Login Tests
            LoginTest loginTest = new LoginTest(service);
            loginTest.IncorrectPassword(randomUsers.ElementAt(0), "1234Abcd");
            loginTest.IncorrectEmail(new ServiceLayer.User("123", "123"), uniPassword);
            loginTest.AllGood(randomUsers.ElementAt(0), uniPassword);
            loginTest.AlreadyLoggedIn(randomUsers.ElementAt(0), uniPassword);
            loginTest.LogoutOfLoggedInUser(randomUsers.ElementAt(0));
            loginTest.AllGood(randomUsers.ElementAt(0),uniPassword);
            loginTest.LogoutOfOtherUser(randomUsers.ElementAt(1));
            loginTest.AllGood(randomUsers.ElementAt(0), uniPassword);



            //GetBoard tests
            GetBoardTest getBoardTest = new GetBoardTest(service);
            getBoardTest.AllGood(randomUsers.ElementAt(1));
            getBoardTest.NonExistingEmail(new ServiceLayer.User("nonexistingemail@mashu.com", "123"));
            getBoardTest.NotLoggedInUser(new ServiceLayer.User("nonexistingemail@mashu.com", "123"));

            //TaskInvolvedTests
            TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests(service);
            int i = 0;
            foreach(ServiceLayer.User tempUser in randomUsers)
            {
                taskInvolvedTests.AddTaskAllGood(tempUser, randomTasks.ElementAt(i));
                i++;
            }
           

            //ColumnInvolvedTests
            ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests(service);



            









            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
