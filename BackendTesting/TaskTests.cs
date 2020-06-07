using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;

namespace BackendTesting
{
    [TestClass]
    public class TaskTests
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
            LoggedUser = service.Login("2@mashu.com", "Aa1234").Value;
        }


        [Test]
        public void AddTaskTest_ValidInput()
        {
            string expectedTitle = "my title";
            string expectedDescription = "my Description";
            string assigneeEmail = "1@mashu.com";

            Response<Task> r1 = service.AddTask(LoggedUser.AssociatedBoard, expectedTitle, expectedDescription, new DateTime(2021,3,5));
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(expectedTitle, r1.Value.Title, r1.ErrorMessage);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(expectedDescription, r1.Value.Description, r1.ErrorMessage);
            NUnit.Framework.StringAssert.AreEqualIgnoringCase(assigneeEmail, r1.Value.emailAssignee, r1.ErrorMessage);

        }

        [OneTimeTearDown]
        public void Clean()
        {
            service.DeleteData();
        }

    }
}
