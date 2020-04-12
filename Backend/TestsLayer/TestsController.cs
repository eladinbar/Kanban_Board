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


            timer.Stop();
            Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));
        }        
    }
}
