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
            //service.DeleteData();
            //string path = @"D:\UProject\IntroToSE\milestones-2-tpee\BackendTesting\bin\Debug\KanbanDB.db";
            //FileInfo dBFile = new FileInfo(path);
            //if (dBFile.Exists)
            //{
            //    dBFile.Delete();
            //}

            service = new Service();
            service.Register("1@mashu.com", "Aa1234", "1");
            service.Register("2@mashu.com", "Aa1234", "2", "1@mashu.com");
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


        
        //[Test]
        //public void CheckAssosiatedBoard(string user)
        //{
        //    User u1 = service.Login("1@mashu.com", "Aa1234").Value;
        //    NUnit.Framework.Assert.True(u1.AssociatedBoard.Equals("1@mashu.com"));
        //    service.Logout(user);
        //}

        //[Test]
        //public void CheckAssosiatedBoard2(string user)
        //{
        //    User u1;
        //    u1 = service.Login("2@mashu.com", "Aa1234").Value;
        //    NUnit.Framework.Assert.True(u1.AssociatedBoard.Equals("1@mashu.com"));
        //    service.Logout(user);
        //}


       
    }
}
