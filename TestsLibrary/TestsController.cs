using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using log4net;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TestsController
    {

        static void Main(string[] args)
        {



            //Console.Write("Do you want to perform a restart of the program? (y/n)");
            //string choice2 = Console.ReadLine();
            //if (choice2 == "y")
            //{
            ServiceLayer.Service service = new ServiceLayer.Service();
            //loading data
            Response d = service.LoadData();
           
            //registering
            Response reg = service.Register("3@mashu.com", "123Abc", "3");
           
            
           // Response reg2 = service.Register("4@mas.hu.com", "123Abc", "4");
        
            //logging in
            Response<User> u = service.Login("3@mashu.com", "123Abc");
           
            //getting board
            Response<Board> b = service.GetBoard("3@mashu.com");
           
            //adding task
            Response<ServiceLayer.Task> t1 = service.AddTask("3@mashu.com", "",null ,new DateTime(2020,12,31));
            Response<ServiceLayer.Column> c1 = service.GetColumn("3@mashu.com", 0);
            Console.WriteLine(t1.Value.ToString());
         
            Response upDesc = service.UpdateTaskDescription("3@mashu.com", 0, t1.Value.Id, "description was updated");
             c1 = service.GetColumn("3@mashu.com", 0);

            // updating duedate
            Response upDuedate = service.UpdateTaskDueDate("3@mashu.com", 0, t1.Value.Id, new DateTime(2021, 12, 31));
            c1 = service.GetColumn("3@mashu.com", 0);
            Response upTitle = service.UpdateTaskTitle("3@mashu.com", 0, t1.Value.Id, "hi");
            c1 = service.GetColumn("3@mashu.com", 0);
            //advancing task to in progress
            Response advance = service.AdvanceTask("3@mashu.com", 0, t1.Value.Id);
            c1 = service.GetColumn("3@mashu.com", 1);

            // updating description

            // advancing to done
            //Response advance2 = service.AdvanceTask("3@mashu.com", 1, t1.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 2);
            //trying to edit in done
            Response update2 = service.UpdateTaskDescription("3@mashu.com", 2, t1.Value.Id, "description was updated");
            Console.WriteLine(update2.ErrorMessage);
           
            //trying to advance from done
            Response advance3 = service.AdvanceTask("3@mashu.com", 2, t1.Value.Id);
            Console.WriteLine(advance3.ErrorMessage);
            
            //}
            //else
            //{
            //    Console.WriteLine("press any key to exit the console...");
            //    Console.ReadKey();
            //}


            ////System.Environment.Exit(0);

            Stopwatch timer = new Stopwatch();
            timer.Start();

            //LoadData tests
            //LoadDataTest loadDataTest = new LoadDataTest();
            //loadDataTest.RunTest();


            //Register and Login tests                      
            //UserTests userTests = new UserTests(7);
            //userTests.RunAllTests();

            ////GetBoard tests
            //GetBoardTest getBoardTest = new GetBoardTest();
            //getBoardTest.RunAllTests();

            ////ColumnInvolvedTests
            //ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests();
            //columnInvolvedTests.RunAllTests();


            ////TaskInvolvedTests
            //TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests();
            //taskInvolvedTests.RunAllTests();

            //timer.Stop();
            //Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            ////Console.ForegroundColor = ConsoleColor.Red;

            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("Normal usage state test starts here.");

            //timer.Restart();

            //NormalUsageStateTest normalUsageStateTest = new NormalUsageStateTest();
            //normalUsageStateTest.RunTheTest();

            //timer.Stop();

            //Console.ForegroundColor = ConsoleColor.Green;

            //Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("do you want to clear the 'data' folder? (y/n):");
            string choice = Console.ReadLine();
            if (choice == "y")
            {
                DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
                DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
                if (dir2.Exists)
                {
                    dir1.Delete(true);
                    Console.Write("'data' folder was deleted. Thank you for using Tests.");
                }
                Console.ReadKey();
            }
            else
            {
                Console.ReadKey();
            }


        }
    }
}
