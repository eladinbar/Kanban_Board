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



            //new service declare
            Service service = new Service();

            //loading data
            Response d = service.LoadData();

            //registering
            Response reg = service.Register("valid@example.com", "123Abc", "3");
            
            //logging in
            Response<User> u = service.Login("valid@example.com", "123Abc");
           
            //getting board
            Response<Board> b = service.GetBoard("valid@example.com");

            //adding a valid task and retrieving the updated column
            Response<ServiceLayer.Task> valid1 = service.AddTask("valid@example.com", "valid title", null, new DateTime(2020, 12, 31));
            Response<Column> column = service.GetColumn("valid@example.com", 0);

            //adding an invalid task and retrieving the updated column
            Response<ServiceLayer.Task> invalid = service.AddTask("valid@example.com", "", "valid description", new DateTime(2020, 12, 31)); //(Invalid title)
            column = service.GetColumn("valid@example.com", "backlog");

            //adding another valid task and retrieving the updated column
            Response<ServiceLayer.Task> valid2 = service.AddTask("valid@example.com", "valid title2", "valid description", new DateTime(2021, 11, 19));
            column = service.GetColumn("valid@example.com", "backlog");





            ////adding task
            //Response<ServiceLayer.Task> t1 = service.AddTask("3@mashu.com", "Title","" ,new DateTime(2020,12,31));
            //Response<ServiceLayer.Column> c1 = service.GetColumn("3@mashu.com", 0);
            //Console.WriteLine(t1.Value.ToString());

            //// limiting column to valid limit
            //Response limColumn0 = service.LimitColumnTasks("3@mashu.com", 0, 2);
            //c1 = service.GetColumn("3@mashu.com", 0);

            ////adding task
            //Response<ServiceLayer.Task>t2 = service.AddTask("3@mashu.com", "Title2", "", new DateTime(2020, 12, 31));
            //c1 = service.GetColumn("3@mashu.com", 0);
            //Console.WriteLine(t2.Value.ToString());

            ////adding task
            //Response<ServiceLayer.Task> t3 = service.AddTask("3@mashu.com", "Title3", "", new DateTime(2020, 12, 31));
            //c1 = service.GetColumn("3@mashu.com", 0);
            //Console.WriteLine(t3.Value.ToString());

            //// limiting column to valid limit
            //limColumn0 = service.LimitColumnTasks("3@mashu.com", 0, 3);
            //c1 = service.GetColumn("3@mashu.com", 0);

            ////adding task
            //t3 = service.AddTask("3@mashu.com", "Title3", "", new DateTime(2020, 12, 31));
            //c1 = service.GetColumn("3@mashu.com", 0);
            //Console.WriteLine(t3.Value.ToString());

            //// updating description
            //Response upDesc = service.UpdateTaskDescription("3@mashu.com", 0, t1.Value.Id, "description was updated");
            // c1 = service.GetColumn("3@mashu.com", 0);

            //// updating duedate
            //Response upDuedate = service.UpdateTaskDueDate("3@mashu.com", 0, t1.Value.Id, new DateTime(2021, 12, 31));
            //c1 = service.GetColumn("3@mashu.com", 0);
            //Response upTitle = service.UpdateTaskTitle("3@mashu.com", 0, t1.Value.Id, "hi");
            //c1 = service.GetColumn("3@mashu.com", 0);

            ////advancing task to in progress
            //Response advance = service.AdvanceTask("3@mashu.com", 0, t1.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 1);

            //// limiting column to valid limit
            //Response limColumn1 = service.LimitColumnTasks("3@mashu.com", 1, 2);
            //c1 = service.GetColumn("3@mashu.com", 1);

            ////advancing task to in progress
            //Response advance2 = service.AdvanceTask("3@mashu.com", 0, t2.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 1);

            ////advancing task to in progress
            //Response advance3 = service.AdvanceTask("3@mashu.com", 0, t3.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 1);

            //// limiting column to valid limit
            //limColumn1 = service.LimitColumnTasks("3@mashu.com", 1, 3);
            //c1 = service.GetColumn("3@mashu.com", 1);

            ////advancing task to in progress
            //advance3 = service.AdvanceTask("3@mashu.com", 0, t3.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 1);

            ////adding task
            //Response<ServiceLayer.Task> t4 = service.AddTask("3@mashu.com", "Title4", "", new DateTime(2020, 12, 31));
            //c1 = service.GetColumn("3@mashu.com", 0);
            //Console.WriteLine(t4.Value.ToString());

            ////advancing task to in progress
            //Response advance4 = service.AdvanceTask("3@mashu.com", 0, t4.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 1);

            //// advancing to done
            //Response advanceD = service.AdvanceTask("3@mashu.com", 1, t1.Value.Id);
            //c1 = service.GetColumn("3@mashu.com", 2);

            ////trying to edit in done
            //Response update2 = service.UpdateTaskDescription("3@mashu.com", 2, t1.Value.Id, "description was updated");
            //Console.WriteLine(update2.ErrorMessage);

            ////trying to advance from done
            //Response advanceD2 = service.AdvanceTask("3@mashu.com", 2, t1.Value.Id);
            //Console.WriteLine(advance3.ErrorMessage);

            ////}
            ////else
            ////{
            ////    Console.WriteLine("press any key to exit the console...");
            ////    Console.ReadKey();
            ////}


            //////System.Environment.Exit(0);

            //Stopwatch timer = new Stopwatch();
            //timer.Start();

            ////LoadData tests
            ////LoadDataTest loadDataTest = new LoadDataTest();
            ////loadDataTest.RunTest();


            ////Register and Login tests                      
            ////UserTests userTests = new UserTests(7);
            ////userTests.RunAllTests();

            //////GetBoard tests
            ////GetBoardTest getBoardTest = new GetBoardTest();
            ////getBoardTest.RunAllTests();

            //////ColumnInvolvedTests
            ////ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests();
            ////columnInvolvedTests.RunAllTests();


            //////TaskInvolvedTests
            ////TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests();
            ////taskInvolvedTests.RunAllTests();

            ////timer.Stop();
            ////Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            //////Console.ForegroundColor = ConsoleColor.Red;

            //////Console.WriteLine("**********************************************************************");
            //////Console.WriteLine("**********************************************************************");
            //////Console.WriteLine("**********************************************************************");
            //////Console.WriteLine("**********************************************************************");
            //////Console.WriteLine("Normal usage state test starts here.");

            ////timer.Restart();

            ////NormalUsageStateTest normalUsageStateTest = new NormalUsageStateTest();
            ////normalUsageStateTest.RunTheTest();

            ////timer.Stop();

            ////Console.ForegroundColor = ConsoleColor.Green;

            ////Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            //Console.ForegroundColor = ConsoleColor.White;

            //Console.Write("do you want to clear the 'data' folder? (y/n):");
            //string choice = Console.ReadLine();
            //if (choice == "y")
            //{
            //    DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            //    DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            //    if (dir2.Exists)
            //    {
            //        dir1.Delete(true);
            //        Console.Write("'data' folder was deleted. Thank you for using Tests.");
            //    }
            //    Console.ReadKey();
            //}
            //else
            //{
            //    Console.ReadKey();
            //}


        }
    }
}
