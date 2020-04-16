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

            //delete data directory before and after each test

            //declaring random tasks
            TaskForTestCreator taskForTestCreator = new TaskForTestCreator(5);
            List<ServiceLayer.Task> randomTasks = taskForTestCreator._tasks;
            taskForTestCreator.PrintTasks();

            //LoadData tests


            //Register and Login tests                      
            UserTests userTests = new UserTests(7);
            userTests.RunAllTests();


            //GetBoard tests
            //GetBoardTest getBoardTest = new GetBoardTest(service);
            //getBoardTest.AllGood(randomUsers.ElementAt(0));
            //getBoardTest.NonExistingEmail(new ServiceLayer.User("nonexistingemail@mashu.com", "123"));
            //getBoardTest.NotLoggedInUser(new ServiceLayer.User("nonexistingemail@mashu.com", "123"));


            //ColumnInvolvedTests
            //ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests(service);
            //columnInvolvedTests.GetColumnByNameAllGood(randomUsers.ElementAt(0), "Done");
            //columnInvolvedTests.GetColumnByNonExistName(randomUsers.ElementAt(0), "Achiya");
            //columnInvolvedTests.GetColumnByOrdinalAllGood(randomUsers.ElementAt(0), 2);
            //columnInvolvedTests.GetColumnByNonExistOrdinal(randomUsers.ElementAt(0), 45);
            //columnInvolvedTests.LimitColumnAllGood(randomUsers.ElementAt(0), 0, 23);
            //columnInvolvedTests.LimitColumnBadColumnOrdinal(randomUsers.ElementAt(0), 8, 67);

            //TaskInvolvedTests
            //TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests(service);
            //foreach (ServiceLayer.Task tempTask in randomTasks)
              //  taskInvolvedTests.AddTaskAllGood(randomUsers.ElementAt(0), tempTask);
            //columnInvolvedTests.LimitColumnAllGood(randomUsers.ElementAt(0), 0, 5);
            //columnInvolvedTests.LimitColumnLesserColumnLimit(randomUsers.ElementAt(0), 0, 3);
            //taskInvolvedTests.AddTaskOverColumnLimit(randomUsers.ElementAt(0), randomTasks.ElementAt(0));


















            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
