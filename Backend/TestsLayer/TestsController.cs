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


            //LoadData tests
            LoadDataTest loadDataTest = new LoadDataTest();
            loadDataTest.RunTest();


            //Register and Login tests                      
            UserTests userTests = new UserTests(7);
            userTests.RunAllTests();

            //GetBoard tests
            GetBoardTest getBoardTest = new GetBoardTest();
            getBoardTest.RunAllTests();


            //ColumnInvolvedTests
            ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests();
            columnInvolvedTests.RunAllTests();


            //TaskInvolvedTests
            TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests();
            taskInvolvedTests.RunAllTests();

            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("**********************************************************************");
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("Normal usage state test starts here.");

            timer.Restart();

            NormalUsageStateTest normalUsageStateTest = new NormalUsageStateTest();
            normalUsageStateTest.RunTheTest();

            timer.Stop();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

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
                }
                Console.Write("'data' folder was deleted. Thank you for using Tests. Press any key to close the console....");
                Console.ReadKey();
            }
            else
            {
                Console.Write("Thank you for using Tests. Press any key to close the console....");
                Console.ReadKey();
            }
        }
    }
}
