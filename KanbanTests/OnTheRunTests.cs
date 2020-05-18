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

            //add columns
            Response<Column> nc1 = service.AddColumn("3@mashu.com", 0, "column-1");
            b = service.GetBoard("3@mashu.com");
            Response<Column> nc2 = service.AddColumn("3@mashu.com", 2, "column-2");
            b = service.GetBoard("3@mashu.com");
            Response<Column> nc3 = service.AddColumn("3@mashu.com", 5, "column-3");
            b = service.GetBoard("3@mashu.com");
            Response<Column> nc4 = service.AddColumn("3@mashu.com", 6, "column-4");
            b = service.GetBoard("3@mashu.com");
            Response<Column> bnc = service.AddColumn("4@mashu.com", 0, "column-1");
            Response<Column> bnc1 = service.AddColumn("3@mashu.com", 0, "");

            //adding task
            Response<ServiceLayer.Task> t1 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Column> c1 = service.GetColumn("3@mashu.com", 0);
            Console.WriteLine(t1.ErrorMessage);
            Response<ServiceLayer.Task> t2 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t3 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t4 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t5 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t6 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t7 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t8 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));
            Response<ServiceLayer.Task> t9 = service.AddTask("3@mashu.com", "edanTitleYaavodHalak", null, new DateTime(2020, 12, 31));

            Response upDesc = service.UpdateTaskDescription("3@mashu.com", 0, t1.Value.Id, "description was updated");
            c1 = service.GetColumn("3@mashu.com", 0);

            // updating duedate
            Response upDuedate = service.UpdateTaskDueDate("3@mashu.com", 0, t1.Value.Id, new DateTime(2021, 12, 31));
            c1 = service.GetColumn("3@mashu.com", 0);
            Response upTitle = service.UpdateTaskTitle("3@mashu.com", 0, t1.Value.Id, "hi");
            c1 = service.GetColumn("3@mashu.com", 0);

            //advancing task to in progress
            Response advance1 = service.AdvanceTask("3@mashu.com", 0, t1.Value.Id);
            c1 = service.GetColumn("3@mashu.com", 1);
            Response advance2 = service.AdvanceTask("3@mashu.com", 1, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance3 = service.AdvanceTask("3@mashu.com", 2, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance4 = service.AdvanceTask("3@mashu.com", 3, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance5 = service.AdvanceTask("3@mashu.com", 4, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance6 = service.AdvanceTask("3@mashu.com", 5, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance7 = service.AdvanceTask("3@mashu.com", 6, t1.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance8 = service.AdvanceTask("3@mashu.com", 0, t2.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance9 = service.AdvanceTask("3@mashu.com", 0, t3.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance10 = service.AdvanceTask("3@mashu.com", 1, t3.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance11 = service.AdvanceTask("3@mashu.com", 0, t4.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance12 = service.AdvanceTask("3@mashu.com", 1, t4.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance13 = service.AdvanceTask("3@mashu.com", 2, t4.Value.Id);
            b = service.GetBoard("3@mashu.com");
            Response advance14 = service.AdvanceTask("3@mashu.com", 0, t5.Value.Id);
            b = service.GetBoard("3@mashu.com");

            //limit columns
            Response lim = service.LimitColumnTasks("3@mashu.com", 0, 5);

            //removing columns
            Response rc = service.RemoveColumn("3@mashu.com", 1);
            b = service.GetBoard("3@mashu.com");
            Response rc2 = service.RemoveColumn("3@mashu.com", 0);
            b = service.GetBoard("3@mashu.com");

            //Moving columns left
            Response<Column> ml1 = service.MoveColumnLeft("3@mashu.com", 4);
            b = service.GetBoard("3@mashu.com");
            Response<Column> ml2 = service.MoveColumnLeft("3@mashu.com", 6);
            b = service.GetBoard("3@mashu.com");
            Response<Column> ml3 = service.MoveColumnLeft("3@mashu.com", 5);
            b = service.GetBoard("3@mashu.com");
            Response<Column> ml4 = service.MoveColumnLeft("3@mashu.com", 3);
            b = service.GetBoard("3@mashu.com");
            Response<Column> ml5 = service.MoveColumnLeft("3@mashu.com", 0);
            b = service.GetBoard("3@mashu.com");

            //Moving columns right
            Response<Column> mr1 = service.MoveColumnRight("3@mashu.com", 2);
            b = service.GetBoard("3@mashu.com");
            Response<Column> mr2 = service.MoveColumnRight("3@mashu.com", 0);
            b = service.GetBoard("3@mashu.com");
            Response<Column> mr3 = service.MoveColumnRight("3@mashu.com", 4);
            b = service.GetBoard("3@mashu.com");
            Response<Column> mr4 = service.MoveColumnRight("3@mashu.com", 6);
            b = service.GetBoard("3@mashu.com");

            // advancing to last column
            Response advance21 = service.AdvanceTask("3@mashu.com", 1, t1.Value.Id);
            c1 = service.GetColumn("3@mashu.com", 2);

            //trying to edit in last column
            Response update2 = service.UpdateTaskDescription("3@mashu.com", 2, t1.Value.Id, "description was updated");
            Console.WriteLine(update2.ErrorMessage);

            //trying to advance from last column
            Response advance31 = service.AdvanceTask("3@mashu.com", 2, t1.Value.Id);
            Console.WriteLine(advance3.ErrorMessage);

            //trying to delete data
            Response advance32 = service.DeleteData();
            Console.WriteLine(advance32.ErrorMessage);
        }
    }
}
