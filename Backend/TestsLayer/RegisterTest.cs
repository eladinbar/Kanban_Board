using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class RegisterTest
    {
        private ServiceLayer.Service _service;

        public RegisterTest(ServiceLayer.Service srv)
        {
            _service = srv;
        }

        public void AllGood(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RegisterTest - AllGood().");
            Console.WriteLine("Input: proper new user data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Register(user.Email, password, user.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void BadPassword(ServiceLayer.User user, string nonAcceptablePassword)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RegisterTest - BadPassword().");
            Console.WriteLine("Input: new user data with non acceptable password.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Register(user.Email, nonAcceptablePassword, user.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void ExistingEmail(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RegisterTest - ExistingEmail().");
            Console.WriteLine("Input: new user data with existing email.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Register(user.Email, password, user.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void ExistingNickname(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RegisterTest - ExistingNickname().");
            Console.WriteLine("Input: new user data with existing nickname.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Register(user.Email, password, user.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }
    }
}
