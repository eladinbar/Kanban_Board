using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class TestsController
    {

        static void Main(string[] args)
        {
            //LogHelper.Setup();

            Console.WriteLine("Do you want to run all regular tests or some tests on the run? : (r/o)");
            string choice = Console.ReadLine();
            if (choice == "o")
                OnTheRunTests.RunTests();
            else
            {
                Console.Write("Do you want to perform a restart of the program with user currentUser@TaskInvolvedTests.com?");
                Console.Write("(available only if the last tests performed were the regular tests): (y/n)");

                string choice2 = Console.ReadLine();
                if (choice2 == "y")
                {
                    Service service = new Service();
                    service.LoadData();
                    service.Login("currentUser@TaskInvolvedTests.com", "123Abc");
                    Console.WriteLine(service.GetColumn("currentUser@TaskInvolvedTests.com", "Backlog").Value.Tasks.ElementAt(0));
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("press any key to exit the console...");
                    Console.ReadKey();
                }


                //System.Environment.Exit(0);

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

                Console.Write("do you want to clear the database file? (y/n):");
                string choice3 = Console.ReadLine();
                if (choice3 == "y")
                {
                    {
                        DeleteDataTest deleteDataTest = new DeleteDataTest();
                        deleteDataTest.RunTest();
                        Console.Write("Database was deleted. Thank you for using Tests.");
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
}