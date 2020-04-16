using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class LoadDataTest
    {
        ServiceLayer.Service _service;

        public LoadDataTest()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }

            _service = new ServiceLayer.Service();

        }

        public void RunTest()
        {
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("LoadDataTest #"+i);
                Console.WriteLine("Input: no input necessary.");
                Console.WriteLine("Runtime outcome: " + _service.LoadData().ErrorMessage);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}
