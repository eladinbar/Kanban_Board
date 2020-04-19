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
        public string BASE_PATH = Path.GetFullPath(@"..\..\") + "\\Logs\\logFile";

        public static log4net.ILog getLogger([CallerFilePath]string filename = "")
        {
            string path = Path.GetFullPath(@"..\..\") + "\\Logs\\" + "logFile";
            ChangeFilePath("RollingFileAppender", path);
            return log4net.LogManager.GetLogger(filename);
        }

        private static void ChangeFilePath(string appenderName, string newFilename)
        {
            log4net.Repository.ILoggerRepository repository = log4net.LogManager.GetRepository();
            foreach (log4net.Appender.IAppender appender in repository.GetAppenders())
            {
                if (appender is log4net.Appender.RollingFileAppender)
                {
                    log4net.Appender.RollingFileAppender rollingFileAppender = (log4net.Appender.RollingFileAppender)appender;
                    rollingFileAppender.File = newFilename;
                    rollingFileAppender.ActivateOptions();
                }
            }
        }
    }
}
