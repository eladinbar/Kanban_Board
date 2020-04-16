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
    class LoginTest
    {
        private ServiceLayer.Service _service;

        public LoginTest(ServiceLayer.Service srv)
        {
            _service = srv;
        }

        [TestMethod]
        public void AllGood(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - AllGood().");
            Console.WriteLine("Input: proper existing user data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: User "+user.Email+" loggedIn. System message: " + _service.Login(user.Email, password).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        [TestMethod]
        public void IncorrectPassword(ServiceLayer.User user, string incorrectPassword)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - IncorrectPassword().");
            Console.WriteLine("Input: user data with incorrect password.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Login(user.Email, incorrectPassword).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        [TestMethod]
        public void IncorrectEmail(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - IncorrectEmail().");
            Console.WriteLine("Input: user data with incorrect email.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Login(user.Email, password).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        [TestMethod]
        public void AlreadyLoggedIn(ServiceLayer.User user, string password)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - AlreadyLoggedIn().");
            Console.WriteLine("Input: user data.");
            Console.WriteLine("Expected: failed - other user already logged in.");
            Console.WriteLine("Runtime outcome: " + _service.Login(user.Email, password).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        [TestMethod]
        public void LogoutOfLoggedInUser(ServiceLayer.User user)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - LogoutOfLoggedInUser().");
            Console.WriteLine("Input: logged in user data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.Logout(user.Email).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        [TestMethod]
        public void LogoutOfOtherUser(ServiceLayer.User user)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest - LogoutOfOtherUser().");
            Console.WriteLine("Input: not logged in user data.");
            Console.WriteLine("Expected: failled - other user is logged in.");
            Console.WriteLine("Runtime outcome: " + _service.Logout(user.Email).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }

    }
}
