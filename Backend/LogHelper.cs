using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend
{
    public class LogHelper
    {
        private static bool setted = false;
        public static string BASE_PATH = Path.GetFullPath(@"..\..\") + "Logs\\logFile";

        public static log4net.ILog getLogger([CallerFilePath]string filename = "")
        {

            if (!setted)
            {
                Setup();
                setted = true;
            }
            //string path = Path.GetFullPath(@"..\..\") + "\\Logs\\" + "logFile";
            //ChangeFilePath("RollingFileAppender", path);
            return log4net.LogManager.GetLogger(filename);
        }

        //private static void ChangeFilePath(string appenderName, string newFilename)
        //{
        //    log4net.Repository.ILoggerRepository repository = log4net.LogManager.GetRepository();
        //    foreach (log4net.Appender.IAppender appender in repository.GetAppenders())
        //    {
        //        if (appender is log4net.Appender.RollingFileAppender)
        //        {
        //            log4net.Appender.RollingFileAppender rollingFileAppender = (log4net.Appender.RollingFileAppender)appender;
        //            rollingFileAppender.File = newFilename;
        //            rollingFileAppender.ActivateOptions();
        //        }
        //    }
        //}

        public static void Setup()        {            PatternLayout patternLayout = new PatternLayout();            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} %level - (%type: %method - %line)%newline %message%newline%exception";            patternLayout.ActivateOptions();            RollingFileAppender roller = new RollingFileAppender();            roller.AppendToFile = true;            roller.File = BASE_PATH;            roller.Layout = patternLayout;            roller.MaxSizeRollBackups = 5;            roller.MaximumFileSize = "10MB";            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.StaticLogFileName = true;            log4net.Config.BasicConfigurator.Configure(roller);            roller.ActivateOptions();                  }

    }
}
