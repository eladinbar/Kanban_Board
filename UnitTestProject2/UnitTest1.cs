using System;
using IntroSE.Kanban.Backend.ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Service service = new Service();
            log4net.Config.XmlConfigurator.Configure();

        }
    }
}
