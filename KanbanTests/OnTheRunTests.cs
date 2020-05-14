using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    public class OnTheRunTests
    {
        public static void RunTests()
        {
            Service service = new Service();
            //loading data
            Response d = service.LoadData();

            //registering
            Response reg = service.Register("3@mashu.com", "123Abc", "3");

            //logging in
            Response<User> u = service.Login("3@mashu.com", "123Abc");

            //getting board
            Response<Board> b = service.GetBoard("3@mashu.com");

            //adding task
            Response<ServiceLayer.Task> t1 = service.AddTask("3@mashu.com", "", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Column> c1 = service.GetColumn("3@mashu.com", 0);
            Console.WriteLine(t1.Value.ToString());

            Response upDesc = service.UpdateTaskDescription("3@mashu.com", 0, t1.Value.Id, "description was updated");
            c1 = service.GetColumn("3@mashu.com", 0);

            // updating duedate
            Response upDuedate = service.UpdateTaskDueDate("3@mashu.com", 0, t1.Value.Id, new DateTime(2021, 12, 31));
            c1 = service.GetColumn("3@mashu.com", 0);
            Response upTitle = service.UpdateTaskTitle("3@mashu.com", 0, t1.Value.Id, "hi");
            c1 = service.GetColumn("3@mashu.com", 0);
            //advancing task to in progress
            Response advance = service.AdvanceTask("3@mashu.com", 0, t1.Value.Id);
            c1 = service.GetColumn("3@mashu.com", 1);

            // updating description

            // advancing to done
            Response advance2 = service.AdvanceTask("3@mashu.com", 1, t1.Value.Id);
            c1 = service.GetColumn("3@mashu.com", 2);
            //trying to edit in done
            Response update2 = service.UpdateTaskDescription("3@mashu.com", 2, t1.Value.Id, "description was updated");
            Console.WriteLine(update2.ErrorMessage);

            //trying to advance from done
            Response advance3 = service.AdvanceTask("3@mashu.com", 2, t1.Value.Id);
            Console.WriteLine(advance3.ErrorMessage);


        }
    }
}
