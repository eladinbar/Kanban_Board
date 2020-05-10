using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DalControllers;
using IntroSE.Kanban.Backend.DataAccessLayer.DALOs;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Service service = new Service();
            DalUser user = new DalUser("example@gmail.com", "123Abc", "example");
            user.Save();
        }
    }
}
