using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class LoadDataTest
    {
        Service service;

        public LoadDataTest()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo DBFile = new FileInfo(path);
            if (DBFile.Exists)
                DBFile.Delete();

            service = new Service();

        }

        public void RunTest()
        {
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("LoadDataTest #" + i);
                Console.WriteLine("Input: no input necessary.");
                Console.WriteLine("Runtime outcome: " + service.LoadData().ErrorMessage);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}