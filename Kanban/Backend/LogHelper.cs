using log4net.Appender;
using log4net.Layout;
using System.IO;
using System.Runtime.CompilerServices;

namespace IntroSE.Kanban.Backend
{
    public class LogHelper
    {
        private static bool setted = false;
        private static string BASE_PATH = @"Logs\logFile.txt";

        /// <summary>
        /// Gets the logger of log4net with the class file name to log in the logFile.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Returns the requested logger.</returns>
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
        /// Sets up the RollingFileAppender to save the logs to the "Logs" folder in the original directory the program is using.
        /// </summary>
        /// <remarks>
        /// The logger saves a log file once per program execution.
        /// </remarks>
        public static void Setup()        {            //Defines how we want to log to the LogFile.txt            PatternLayout patternLayout = new PatternLayout();            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} %level - (%type: %method - %line)%newline %message%newline%exception";            patternLayout.ActivateOptions();            //Creates and defines a RollingFileAppender for file logging.            RollingFileAppender roller = new RollingFileAppender();            roller.AppendToFile = true;            roller.File = BASE_PATH;            roller.Layout = patternLayout;            roller.MaxSizeRollBackups = 20;            roller.MaximumFileSize = "10MB";            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.StaticLogFileName = true;            //Sets the RollingFileAppender as the logger appender.            log4net.Config.BasicConfigurator.Configure(roller);            roller.ActivateOptions();                }
    }
}
