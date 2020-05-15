using IntroSE.Kanban.Backend.ServiceLayer;
using NUnit.Framework;
using log4net;
using log4net.Appender;
using log4net.Config;
using IntroSE.Kanban.Backend;
using log4net.Repository;
using log4net.Repository.Hierarchy;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Service service = new Service();
            log4net.Config.XmlConfigurator.Configure();


        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}