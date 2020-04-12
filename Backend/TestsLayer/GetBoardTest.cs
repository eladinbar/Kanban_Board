using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class GetBoardTest
    {
        private ServiceLayer.Service _service;

        public GetBoardTest(ServiceLayer.Service srv)
        {
            _service = srv;
        }

        public void AllGood(ServiceLayer.User user)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardTest - AllGood().");
            Console.WriteLine("Input: proper user's email.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetBoard(user.Email));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void NonExistingEmail(ServiceLayer.User user)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardTest - NonExistingEmail().");
            Console.WriteLine("Input: non existing email.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetBoard(user.Email));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void NotLoggedInUser(ServiceLayer.User user)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardTest - NotLoggedInUser().");
            Console.WriteLine("Input: not logged in user's email.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetBoard(user.Email));
            Console.WriteLine("---------------------------------------------------------------");

        }
    }
}
