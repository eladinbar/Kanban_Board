using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TestsController
    {
      static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Console.WriteLine(Path.GetFullPath(@"..\..\") + "data\\");
            
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
            foreach (ServiceLayer.User tempUser in randomUsers)
                regTest.AllGood(tempUser, uniPassword);

            
            //Login Test - first login
            LoginTest loginTest = new LoginTest(service);
            loginTest.AllGood(randomUsers.ElementAt(0), uniPassword);










            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
