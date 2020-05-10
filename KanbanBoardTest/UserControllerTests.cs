using IntroSE.Kanban.Backend.BusinessLayer.UserPackage;
using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;
using log4net;
using KanbanBoardTest;

namespace Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Service service;

        [SetUp]
        public void SetUp() {
            log.Info("bla bla");
            service = new Service();
        }

        [Test]
        public void check() {
            Assert.IsTrue(true);
        }
    }
}