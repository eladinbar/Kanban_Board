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
        private ServiceLayer.Service _service;
        private List<ServiceLayer.User> _randomUsers;
        private string _uniPassword;

        public UserTests(int numOfDemandedUsers)
        {
            

            _service = new ServiceLayer.Service();
            _service.LoadData();
            _randomUsers = new UserForTestCreator(numOfDemandedUsers)._users;
            _uniPassword = "123Abc";
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
            foreach (ServiceLayer.User tempUser in _randomUsers)
                Console.WriteLine("Runtime outcome: " + _service.Register(tempUser.Email, _uniPassword, tempUser.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void BadPasswordRegister()
        {
            String[] badPsswords = { "12345", "123abc", "123abcс", "!@#$%^Abc1" };
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("BadPasswordRegisterTest");
            Console.WriteLine("Input: new user data with non acceptable password.");
            for (int i = 0; i < badPsswords.Length; i++)
                Console.WriteLine("Runtime outcome: " + _service.Register("badPass" + _randomUsers[i].Email, badPsswords[i], _randomUsers[i].Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void ExistingEmailRegister()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ExistingEmailRegisterTest");
            Console.WriteLine("Input: new user data with existing email.");
            foreach (ServiceLayer.User tempUser in _randomUsers)
                Console.WriteLine("Runtime outcome: " + _service.Register(tempUser.Email, _uniPassword, tempUser.Nickname).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }


        public void NonAcceptableEmailRegister()
        {
            String[] badEmails = { "ads@@mashu.com", "ads@mashucom", "ads@mashu.", "adsmashu.com", "@mashu.com" };
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("NonAcceptableEmailRegisterTest");
            Console.WriteLine("Input: new user data with non acceptable email.");
            for (int i = 0; i < badEmails.Length; i++)
                Console.WriteLine("Runtime outcome: " + _service.Register(badEmails[i], _uniPassword, "nickOfNonAcceptEmail").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }

        public void Login()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LoginTest");
            Console.WriteLine("Input: proper existing user data.");
            Console.WriteLine("Runtime outcome(succeed if empty): " + _service.Login(_randomUsers.ElementAt(0).Email, _uniPassword).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectPasswordLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectPasswordLoginTest");
            Console.WriteLine("Input: user data with incorrect password.");
            Console.WriteLine("Runtime outcome: " + _service.Login(_randomUsers.ElementAt(0).Email, _uniPassword + "Bad").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void IncorrectEmailLogin()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("IncorrectEmailLoginTest");
            Console.WriteLine("Input: user data with incorrect email.");
            Console.WriteLine("Runtime outcome: " + _service.Login("BadEmail_" + _randomUsers.ElementAt(0).Email, _uniPassword).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void OtherUserAlreadyLoggedIn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("OtherUserAlreadyLoggedInTest");
            Console.WriteLine("Input: user data.");
            Console.WriteLine("Runtime outcome (same user as logged in): " + _service.Login(_randomUsers.ElementAt(0).Email, _uniPassword).ErrorMessage);
            Console.WriteLine("Runtime outcome (different user): " + _service.Login("Other_" + _randomUsers.ElementAt(0).Email, _uniPassword).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfLoggedInUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfLoggedInUserTest");
            Console.WriteLine("Input: logged in user data.");
            Console.WriteLine("Runtime outcome: " + _service.Logout(_randomUsers.ElementAt(0).Email).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LogoutOfNotCurrentUser()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LogoutOfNotCurrentUserTest");
            Console.WriteLine("Input: not logged in user data.");
            _service.Register("LogoutOfNotCurrentUserTestMethod@UsersTests.com", _uniPassword, "tempNickName");
            Console.WriteLine("Runtime outcome: " + _service.Logout("LogoutOfNotCurrentUserTestMethod@UsersTests.com").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

    }
}