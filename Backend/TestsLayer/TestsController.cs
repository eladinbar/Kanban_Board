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







            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
            Console.ReadKey();
        }        
    }
}
