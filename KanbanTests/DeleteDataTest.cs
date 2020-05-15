using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    public class DeleteDataTest
    {
        Service service;

        public DeleteDataTest()
        {
            service = new Service();
            UserService userService = new UserService(new BusinessLayer.SecurityController());
        }

        public void RunTest()
        {
            for (int i = 1; i < 4; i++)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("DeleteDataTest #" + i);
                Console.WriteLine("Input: no input necessary.");
                string message = service.DeleteData().ErrorMessage;
                Console.WriteLine("Runtime outcome: " + ((message == null) ? "DeleteData run successfully!" : message));
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}
