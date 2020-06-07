using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace BackendTesting
{
    [TestClass]
    public class BoardTests
    {

        public IService service;
        public User LoggedUser;

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
            LoggedUser = service.Login("1@mashu.com", "Aa1234").Value;
            service.AddTask(LoggedUser.AssociatedBoard, "my Title", "my description", new DateTime(2021, 3, 5));
        }

        [Test]
        public void ChangeAssigneeTest_TrueCase_ValidMemberIsAssingee()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("1@mashu.com", "Aa1234").Value;
            Response r =  service.AssignTask("1@mashu.com", 0, 1, "2@mashu.com");
            NUnit.Framework.Assert.IsFalse(r.ErrorOccured,r.ErrorMessage);
        }

        [Test]
        public void ChangeAssigneeTest_FalseCase_ValidMemberNotAssignee()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("3@mashu.com", "Aa1234").Value;
            Response r = service.AssignTask("3@mashu.com", 0, 1, "2@mashu.com");
            NUnit.Framework.Assert.IsTrue(r.ErrorOccured, r.ErrorMessage);
        }

        [Test]
        public void ChangeAssigneeTest_InvalidMember()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("1@mashu.com", "Aa1234").Value;
            Response r = service.AssignTask("3@mashu.com", 0, 1, "79@mashu.com");
            NUnit.Framework.Assert.IsTrue(r.ErrorOccured, r.ErrorMessage);
        }

        [Test]
        public void ChangeColumnName_ValidName_UserIsHost()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("1@mashu.com", "Aa1234").Value;
            Response r = service.ChangeColumnName("1@mashu.com", 0, "newColumnName");
            NUnit.Framework.Assert.IsFalse(r.ErrorOccured, r.ErrorMessage);
        }

        [Test]
        public void ChangeColumnName_InvalidName_UserIsHost()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("1@mashu.com", "Aa1234").Value;
            Response r = service.ChangeColumnName("1@mashu.com", 0, "done");
            NUnit.Framework.Assert.IsTrue(r.ErrorOccured, r.ErrorMessage);
        }

        [Test]
        public void ChangeColumnName_ValidName_NotHost()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("2@mashu.com", "Aa1234").Value;
            Response r = service.ChangeColumnName("1@mashu.com", 0, "Validname2");
            NUnit.Framework.Assert.IsTrue(r.ErrorOccured, r.ErrorMessage);
        }

        [Test]
        public void ChangeColumnName_InvalidName_NotHost()
        {
            service.Logout(LoggedUser.Email);
            LoggedUser = service.Login("2@mashu.com", "Aa1234").Value;
            Response r = service.ChangeColumnName("1@mashu.com", 0, "NewColumnName");
            NUnit.Framework.Assert.IsTrue(r.ErrorOccured, r.ErrorMessage);
        }

        [OneTimeTearDown]
        public void Clean()
        {
            service.DeleteData();
        }
    }
}
