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
                Console.WriteLine("Runtime outcome: " + ((message==null) ?  "User was registered successfully!" : message));
            }
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void BadPasswordRegister()
        {
            string[] badPasswords = { "12345", "123abc", "123abcс", "1Ab", "123Abcsadwaeadfgdgssdfgsdfgdsfgdsgdsfgsdgwa" };
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("BadPasswordRegisterTest");
            Console.WriteLine("Input: new user data with non acceptable password.");
            for (int i = 0; i < badPasswords.Length; i++)
            {
                string message = service.Register("badPass" + randomUsers[i].Email, badPasswords[i], randomUsers[i].Nickname).ErrorMessage;
                Console.WriteLine("Runtime outcome: " + ((message == null) ? "BadPasswordRegister succeeded but was expected to fail" : message));
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
                Console.WriteLine("Runtime outcome: " + ((message == null) ? "ExistingEmailRegister succeeded but was expected to fail" : message));
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
                Console.WriteLine("Runtime outcome: " + ((message == null) ? "NonAcceptableEmailRegister succeeded but was expected to fail" : message));
            }
            Console.WriteLine("---------------------------------------------------------------");

        }

        public void Login()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest");
            Console.WriteLine("Input: proper existing user data.");
            string message = service.Login(randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome(succeed if empty): " + ((message == null) ? "Login successful!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectPasswordLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectPasswordLoginTest");
            Console.WriteLine("Input: user data with incorrect password.");
            string message = service.Login(randomUsers.ElementAt(0).Email, uniPassword + "Bad").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "IncorrectPasswordLogin succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectEmailLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectEmailLoginTest");
            Console.WriteLine("Input: user data with incorrect email.");
            string message = service.Login("BadEmail_" + randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "IncorrectEmailLogin succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void OtherUserAlreadyLoggedIn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("OtherUserAlreadyLoggedInTest");
            Console.WriteLine("Input: user data.");
            string message1 = service.Login(randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome (same user as logged in): " + ((message1 == null) ? "SameUserAlreadyLoggedIn succeeded but was expected to fail" : message1));
            string message2 = service.Login("Other_" + randomUsers.ElementAt(0).Email, uniPassword).ErrorMessage;
            Console.WriteLine("Runtime outcome (different user): " + ((message2 == null) ? "OtherUserAlreadyLoggedIn succeeded but was expected to fail" : message2));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfLoggedInUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfLoggedInUserTest");
            Console.WriteLine("Input: logged in user data.");
            string message = service.Logout(randomUsers.ElementAt(0).Email).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "LogoutofLoggedInUser was successful!" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfNotCurrentUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfNotCurrentUserTest");
            Console.WriteLine("Input: not logged in user data.");
            service.Register("LogoutOfNotCurrentUserTestMethod@UsersTests.com", uniPassword, "tempNickName");
            string message = service.Logout("LogoutOfNotCurrentUserTestMethod@UsersTests.com").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + ((message == null) ? "LogoutOfNotCurrentUser succeeded but was expected to fail" : message));
            Console.WriteLine("---------------------------------------------------------------");
        }

    }
}