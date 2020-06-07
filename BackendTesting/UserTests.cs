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
        

        [Test]
        public void RegisterTest()
        {
            Response r1 = service.Register("1@mashu.com", "Aa1234", "1");
            NUnit.Framework.Assert.IsFalse(r1.ErrorOccured);
        }

        public void CheckAssosiatedBoard()
        {
            User u1;
            u1 = service.Login("1@mashu.com", "Aa1234").Value;
            NUnit.Framework.Assert.True(u1.Asso)
        }

        [Test]
        public void RegisterTest_AlreadyRegisterd()
        {
            Response r1 = service.Register("1@mashu.com", "Aa1234", "2");
            NUnit.Framework.Assert.IsTrue(r1.ErrorOccured);
        }

        [Test]
        public void RegisterTest_JoinBoard()
        {
            Response r1 = service.Register("2@mashu.com", "Aa1234", "2","1@mashu.com");
            NUnit.Framework.Assert.IsFalse(r1.ErrorOccured);
        }


        [Test]
        public void RegisterTest_JoinBoard_NotExisting()
        {
            Response r1 = service.Register("2@mashu.com", "Aa1234", "2", "4@mashu.com");
            NUnit.Framework.Assert.IsTrue(r1.ErrorOccured);
        }


        [OneTimeTearDown]
        public void CleanUp()
        {
            service.DeleteData();
            string path = @"D:\UProject\IntroToSE\milestones-2-tpee\BackendTesting\bin\Debug\KanbanDB.db";
            FileInfo dBFile = new FileInfo(path);
            if (dBFile.Exists)
            {
                dBFile.Delete();
            }
        }
    }
}
