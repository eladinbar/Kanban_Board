using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace IntroSE.Kanban.Backend.KanbanTests
{
    class UserTests
    {
        private ServiceLayer.Service service;
        private List<ServiceLayer.User> randomUsers;
        private string uniPassword;

        public UserTests(int numOfDemandedUsers)
        {
            

            service = new ServiceLayer.Service();
            service.LoadData();
            randomUsers = new UserForTestCreator(numOfDemandedUsers).users;
            uniPassword = "123Abc";
        }



        public void RunAllTests()
        {
            this.Register();
            this.BadPasswordRegister();
            this.ExistingEmailRegister();
            this.NonAcceptableEmailRegister();

            this.Login();
            this.IncorrectEmailLogin();
            this.IncorrectPasswordLogin();
            this.OtherUserAlreadyLoggedIn();
            this.LogoutOfLoggedInUser();
            this.LogoutOfNotCurrentUser();
        }

        public void Register()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("RegisterTest");
            Console.WriteLine("Demanded input: proper new user data.");
            foreach (ServiceLayer.User tempUser in randomUsers)
            {
                string message = service.Register(tempUser.Email, uniPassword, tempUser.Nickname).ErrorMessage;
                Console.WriteLine("Runtime outcome: " + message);
                if (message == null)
                    Console.Write("User was registered successfully!");
            }
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void BadPasswordRegister()
        {
            string[] badPasswords = { "12345", "123abc", "123abcс", "!@#$%^Abc1", "1Ab", "123Abcsadwaeadfgdgssdfgsdfgdsfgdsgdsfgsdgwa" };
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("BadPasswordRegisterTest");
            Console.WriteLine("Input: new user data with non acceptable password.");
            for (int i = 0; i < badPasswords.Length; i++)
            {
                string message = service.Register("badPass" + randomUsers[i].Email, badPasswords[i], randomUsers[i].Nickname).ErrorMessage;
                Console.WriteLine("Runtime outcome: " + message);
                if (message == null)
                    Console.Write("BadPasswordRegister succeeded but was expected to fail");
            }
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void ExistingEmailRegister()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ExistingEmailRegisterTest");
            Console.WriteLine("Input: new user data with existing email.");
            foreach (ServiceLayer.User tempUser in randomUsers)
            {
                string message = service.Register(tempUser.Email, uniPassword, tempUser.Nickname).ErrorMessage;
                Console.WriteLine("Runtime outcome: " + message);
                if (message == null)
                    Console.Write("ExistingEmailRegister succeeded but was expected to fail");
            }
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void NonAcceptableEmailRegister()
        {
            String[] badEmails = { "ads@@mashu.com", "ads@mashucom", "ads@mashu.", "adsmashu.com", "@mashu.com" };
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("NonAcceptableEmailRegisterTest");
            Console.WriteLine("Input: new user data with non acceptable email.");
            for (int i = 0; i < badEmails.Length; i++)
            {
                string message = service.Register(badEmails[i], uniPassword, "nickOfNonAcceptEmail").ErrorMessage;
                Console.WriteLine("Runtime outcome: " + message);
                if (message == null)
                    Console.Write("NonAcceptableEmailRegister succeeded but was expected to fail");
            }
            Console.WriteLine("---------------------------------------------------------------");

        }

        public void Login()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest");
            Console.WriteLine("Input: proper existing user data.");
            string message = service.Login(randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome(succeed if empty): " + message);
            if (message == null)
                Console.Write("Login successful!");
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectPasswordLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectPasswordLoginTest");
            Console.WriteLine("Input: user data with incorrect password.");
            string message = service.Login(randomUsers.ElementAt(0).Email, uniPassword + "Bad").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("IncorrectPasswordLogin succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectEmailLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectEmailLoginTest");
            Console.WriteLine("Input: user data with incorrect email.");
            string message = service.Login("BadEmail_" + randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("IncorrectEmailLogin succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void OtherUserAlreadyLoggedIn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("OtherUserAlreadyLoggedInTest");
            Console.WriteLine("Input: user data.");
            string message1 = service.Login(randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome (same user as logged in): " + message1);
            if (message1 == null)
                Console.Write("SameUserAlreadyLoggedIn succeeded but was expected to fail");
            string message2 = service.Login("Other_" + randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome (different user): " + message2);
            if (message2 == null)
                Console.Write("OtherUserAlreadyLoggedIn succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfLoggedInUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfLoggedInUserTest");
            Console.WriteLine("Input: logged in user data.");
            string message = service.Logout(randomUsers.ElementAt(0).Email).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("LogoutofLoggedInUser was successful!");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfNotCurrentUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfNotCurrentUserTest");
            Console.WriteLine("Input: not logged in user data.");
            service.Register("LogoutOfNotCurrentUserTestMethod@UsersTests.com", uniPassword, "tempNickName");
            string message = service.Logout("LogoutOfNotCurrentUserTestMethod@UsersTests.com").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("LogoutOfNotCurrentUser succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }

    }
}