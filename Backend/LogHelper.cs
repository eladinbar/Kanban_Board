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
        private static string BASE_PATH = Path.GetFullPath(@"..\..\") + "Logs\\logFile.txt";

        /// <summary>
        /// gets the logger of log4net with the class file name to log in the logFile.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static log4net.ILog getLogger([CallerFilePath]string filename = "")
        {
            if (!setted)
            {
                Setup();
                setted = true;
            }
            return log4net.LogManager.GetLogger(filename);
        }

      
        /// <summary>
        /// Sets up the rollingfileappender to save to the logs to "Logs" folder in the original directory of the program using
        /// IntroSE.Kanban.Backend.dll
        /// </summary>
        /// <remarks>
        /// the logger saves a log file once per program execution.
        /// </remarks>
        public static void Setup()        {            //defines how we want to log to the LogFile.txt            PatternLayout patternLayout = new PatternLayout();            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} %level - (%type: %method - %line)%newline %message%newline%exception";            patternLayout.ActivateOptions();            //creates and defines a RollingFileAppender for file logging.            RollingFileAppender roller = new RollingFileAppender();            roller.AppendToFile = true;            roller.File = BASE_PATH;            roller.Layout = patternLayout;            roller.MaxSizeRollBackups = 20;            roller.MaximumFileSize = "10MB";            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.StaticLogFileName = true;            //sets the RollingFileAppender as the logger appender.            log4net.Config.BasicConfigurator.Configure(roller);            roller.ActivateOptions();                  }

    }
}
