using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TestsController
    {
      static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //declaring propper users and their passwords
            ServiceLayer.User newProperUser1 = new ServiceLayer.User("propUser1@mashu.com", "properNick1");
            string propUser1Password = "abC123";

            ServiceLayer.User newProperUser2 = new ServiceLayer.User("propUser2@mashu.com", "properNick2");
            string propUser2Password = "abC123";

            ServiceLayer.User newProperUser3 = new ServiceLayer.User("propUser3@mashu.com", "properNick3");
            string propUser3Password = "abC123";


            //declaring propper tasks
            DateTime dueDate = new DateTime(26 / 03 / 2035);
            ServiceLayer.Task properTask1 = new ServiceLayer.Task(123, DateTime.Now, "firstTask", "lorem ipsum1", dueDate);

            ServiceLayer.Task properTask2 = new ServiceLayer.Task(123, DateTime.Now, "secondTask", "lorem ipsum2", dueDate);

            ServiceLayer.Task properTask3 = new ServiceLayer.Task(123, DateTime.Now, "thirdTask", "lorem ipsum3", dueDate);

            //declaring new Service.cs appearance.
            ServiceLayer.Service service = new ServiceLayer.Service();


            //LoadData()
            ServiceLayer.Response respLoadData = service.LoadData();
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("First LoadData() call. Expected: succeed. Runtime outcome:");
            Console.WriteLine(respLoadData.ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

            ServiceLayer.Response respLoadData2 = service.LoadData();
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Second LoadData() call. Expected: failed. Runtime outcome:");
            Console.WriteLine(respLoadData2.ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

            LoginTest loginTest = new LoginTest(service);
            loginTest.AllGood(newProperUser1, propUser1Password);




            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
