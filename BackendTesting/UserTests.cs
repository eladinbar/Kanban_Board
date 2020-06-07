using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.IO;

namespace BackendTesting
{
    [TestClass]
    public class UserTests
    {
        IService service = new Service();

        [OneTimeSetUp]
        public void SetUp()
        {
           
            service = new Service();
            service.Register("1@mashu.com", "Aa1234", "1");
            service.Register("2@mashu.com", "Aa1234", "2", "1@mashu.com");
            service.Register("3@mashu.com", "Aa1234", "3", "1@mashu.com");
            service.Register("4@mashu.com", "Aa1234", "4", "1@mashu.com");
            service.Register("5@mashu.com", "Aa1234", "5", "1@mashu.com");
            service.Register("6@mashu.com", "Aa1234", "6", "1@mashu.com");
            service.Register("7@mashu.com", "Aa1234", "7", "1@mashu.com");
        }
        

        [Test]
        public void RegisterTest_AlreadyRegisterd()
        {
            Response r1 = service.Register("1@mashu.com", "Aa1234", "2");
            NUnit.Framework.Assert.IsTrue(r1.ErrorOccured);
        }

        [Test]
        public void RegisterTest_JoinBoard_NotExisting()
        {
            Response r1 = service.Register("3@mashu.com", "Aa1234", "2", "4@mashu.com");
            NUnit.Framework.Assert.IsTrue(r1.ErrorOccured);
        }

        [TestCase("1@mashu.com")]
        [TestCase("2@mashu.com")]
        public void LoginTest_ExcistingUser(string userTochack)
        {
            string expectedUser = userTochack;
           
            Response<User> r1 = service.Login(userTochack, "Aa1234");
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(expectedUser, r1.Value.Email, r1.ErrorMessage);
            service.Logout(userTochack);

        }

        [OneTimeTearDown]
        public void Clean()
        {
            service.DeleteData();
        }
    }
}
