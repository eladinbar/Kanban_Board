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
            Console.ReadKey();
        }        
    }
}
