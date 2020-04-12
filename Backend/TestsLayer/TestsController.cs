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

            
            ServiceLayer.Service service = new ServiceLayer.Service();

            //LoadData()
            ServiceLayer.Response respLoadData = service.LoadData();
            Console.WriteLine(respLoadData.ErrorMessage);
        
            //LoadData() secondly. Expected: failed. Return: response.
            ServiceLayer.Response respLoadData2 = service.LoadData();
            Console.WriteLine(respLoadData2.ErrorMessage);


            //declaring propper users and their passwords
            ServiceLayer.User newProperUser1 = new ServiceLayer.User("propUser1@mashu.com", "properNick1");
            string propUser1Password = "abC123";

            ServiceLayer.User newProperUser2 = new ServiceLayer.User("propUser2@mashu.com", "properNick2");
            string propUser2Password = "abC123";

            ServiceLayer.User newProperUser3 = new ServiceLayer.User("propUser3@mashu.com", "properNick3");
            string propUser3Password = "abC123";


            //declaring propper tasks




            







            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
