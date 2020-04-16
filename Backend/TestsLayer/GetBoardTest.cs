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
        private ServiceLayer.User _currentUser;
        private string _uniPassword;

        public GetBoardTest(ServiceLayer.Service srv)
        {

            _service = new ServiceLayer.Service();
            _currentUser = new ServiceLayer.User("currentUser@GetBoardTest.com", "currentUser@GetBoardTest");
            _uniPassword = "123Abc";
            _service.Register(_currentUser.Email, _uniPassword, _currentUser.Nickname);
            _service.Login(_currentUser.Email, _uniPassword);
        }

        public void RunAllTests()
        {
            this.GetBoard();
            this.GetBoardOfNotLoggedInUser();
            this.GetBoardWithNonExistingEmail();
        }

        public void GetBoard()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardTest");
            Console.WriteLine("Input: proper user's email.");
            Console.WriteLine("Runtime outcome: " + _service.GetBoard(_currentUser.Email).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetBoardWithNonExistingEmail()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardWithNonExistingEmailTest");
            Console.WriteLine("Input: non existing email.");
            Console.WriteLine("Runtime outcome: " + _service.GetBoard("nonExistingEmail@GetBoardWithNonExistingEmailMethod.com").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetBoardOfNotLoggedInUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardOfNotLoggedInUserTest");
            Console.WriteLine("Input: not logged in user's email.");
            ServiceLayer.User tempUser = new ServiceLayer.User("notLoggedInEmail@GetBoardOfNotLoggedInUserMethod.com", "tempGetBoardOfNotLoggedInUserNickName");
            _service.Register(tempUser.Email, _uniPassword, tempUser.Nickname);
            Console.WriteLine("Runtime outcome: " + _service.GetBoard(tempUser.Email).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }
    }
}
