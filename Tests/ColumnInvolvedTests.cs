using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend;

namespace Tests
{
    [TestClass]
    public class ColumnInvolvedTests
    {
        private IntroSE.Kanban.Backend.ServiceLayer.Service _service;
        /*
IntroSE.Kanban.Backend
         */

        public ColumnInvolvedTests(IntroSE.Kanban.Backend.ServiceLayer.Service srv)
        {
            _service = srv;
        }
        public ColumnInvolvedTests()
        {

        }

        [TestMethod]
        public void LimitColumnAllGood(IntroSE.Kanban.Backend.ServiceLayer.User user, int columnOrdinal, int columnLimit)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - LimitColumnAllGood().");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, columnOrdinal, columnLimit).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }
    }
}


