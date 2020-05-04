using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class DalTesting
    {
        static void Main(string[] args)
        {

            UserDalController userC = new UserDalController();
            userC.CreateTable();
            BoardDalController boardC = new BoardDalController();
            boardC.CreateTable();
            ColumnDalController c = new ColumnDalController();
            c.CreateTable();
            TaskDalController t = new TaskDalController();
            t.CreateTable();


            DalUser u1 = new DalUser("e@gmail.com", "Aa123", "The bug E");
            userC.Insert(u1);
            u1.Password = "Bb1234";
            DalUser u2 = new DalUser("b@gmail.com", "Aa123", "The big b");
            userC.Insert(u2);
            DalBoard b1 = new DalBoard("e@gmail.com", 0);
            boardC.Insert(b1);
            DalBoard b2 = new DalBoard("b@gmail.com", 0);
            boardC.Insert(b2);
            DalColumn c1 = new DalColumn("b@gmail.com", 0, "backlog", -1);
            c.Insert(c1);
            DalColumn c2 = new DalColumn("b@gmail.com", 1, "in prograss", -1);
            c.Insert(c2);
            c1.Limit = 15;
            DalTask t1 = new DalTask("b@gmail.com", c1.Name, 1, "this is a title", "this is a description", DateTime.MaxValue, DateTime.Now, DateTime.Now);
            t.Insert(t1);
            t1.Title = "this is a new title";
            t1.Description = "this is a new description";
            t1.LastChangedDate = DateTime.MaxValue;
            Console.Read();
        }

    }
}
