using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class GetBoardTest
    {
        private Service service;
        private User currentUser;
        private string uniPassword;

        public GetBoardTest()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo DBFile = new FileInfo(path);
            if (DBFile.Exists)
                DBFile.Delete();

            service = new Service();
            service.LoadData();
            currentUser = new User("currentUser@GetBoardTest.com", "currentUser@GetBoardTest");
            uniPassword = "123Abc";
            service.Register(currentUser.Email, uniPassword, currentUser.Nickname);
            service.Login(currentUser.Email, uniPassword);
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
            string message = service.GetBoard(currentUser.Email).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "Board was retrieved successfully!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetBoardWithNonExistingEmail()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardWithNonExistingEmailTest");
            Console.WriteLine("Input: non existing email.");
            string message = service.GetBoard("nonExistingEmail@GetBoardWithNonExistingEmailMethod.com").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetBoardWithNonExistingEmail succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetBoardOfNotLoggedInUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetBoardOfNotLoggedInUserTest");
            Console.WriteLine("Input: not logged in user's email.");
            User tempUser = new User("notLoggedInEmail@GetBoardOfNotLoggedInUserMethod.com", "tempGetBoardOfNotLoggedInUserNickName");
            service.Register(tempUser.Email, uniPassword, tempUser.Nickname);
            string message = service.GetBoard(tempUser.Email).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "GetBoardOfNotLoggedInUser succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");

        }
    }
}