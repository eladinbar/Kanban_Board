using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace KanbanBoardTest
{
    [TestFixture]
    public class LoggingTests
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LoggingTests()
        {
            FileInfo fileInfo = new FileInfo(@"C:\Users\Elad\Source\Repos\BGU-SE\milestones-2-tpee\KanbanBoardTest\Test.config");
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void BasicLogTest()
        {
            log.Error("Hmm, Write my log entry already");
        }

        [Test]
        public void DatabaseLogTest()
        {

        }
    }
}
