using log4net.Appender;
using log4net.Layout;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;

namespace IntroSE.Kanban.Backend
{
    public class LogHelper
    {
        private static bool setted = false;
        private const string BASE_PATH = @"Logs\logFile.txt";
        private const string LogTableName = "Log";

        /// <summary>
        /// Gets the logger of log4net with the class file name to log in the logFile.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Returns the requested logger.</returns>
        public static log4net.ILog getLogger([CallerFilePath]string filename = "")
        {
            if (!setted)
            {
                SetUp(SetupRoliingFileAppender(), SetUpSQLiteAppender());
                setted = true;
            }
            return log4net.LogManager.GetLogger(filename);
        }


        public static void SetUp(params AppenderSkeleton[] appenders)
        {
            //Sets the RollingFileAppender as the logger appender.
            log4net.Config.BasicConfigurator.Configure(appenders);
            foreach(AppenderSkeleton appender in appenders)
                appender.ActivateOptions();
        }

        /// <summary>
        /// Sets up the RollingFileAppender to save the logs to the "Logs" folder in the original directory the program is using.
        /// </summary>
        /// <remarks>
        /// The logger saves a log file once per program execution.
        /// </remarks>
        public static AppenderSkeleton SetupRoliingFileAppender()        {            //Defines how we want to log to the LogFile.txt            PatternLayout patternLayout = new PatternLayout();            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} %level - (%type: %method - %line)%newline %message%newline%exception";            patternLayout.ActivateOptions();            //Creates and defines a RollingFileAppender for file logging.            RollingFileAppender roller = new RollingFileAppender();            roller.AppendToFile = true;            roller.File = BASE_PATH;            roller.Layout = patternLayout;            roller.MaxSizeRollBackups = 20;            roller.MaximumFileSize = "10MB";            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.StaticLogFileName = true;            return roller;        }

        public static AppenderSkeleton SetUpSQLiteAppender()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanLoggerDB.db"));
            AdoNetAppender appender = new AdoNetAppender
            {
                ConnectionType = "System.Data.SQLite.SQLiteConnection, System.Data.SQLite",
                ConnectionString = $"Data Source={path}; Version=3;",
                CommandText =$"INSERT INTO {LogTableName} (Date, Level, Type, Method, LineNo, Massage, Exception) " +
                $"Value  (@Date, @Level, @Type, @Method, @LineNo, @Massage, @Exception)"
                
            };

            AdoNetAppenderParameter dateParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Date",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawTimeStampLayout()
            };

            AdoNetAppenderParameter levelParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Level",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Level"}
            };


            AdoNetAppenderParameter typeParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Type",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Type" }
            };


            AdoNetAppenderParameter methodParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Method",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Method" }
            };

            AdoNetAppenderParameter lineNoParam = new AdoNetAppenderParameter
            {
                ParameterName = "@LineNo",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Level" }
            };


            AdoNetAppenderParameter massageParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Message",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Massage" }
            };

            AdoNetAppenderParameter exceptionParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Exception",
                DbType = DbType.String,
                Layout = new log4net.Layout.RawPropertyLayout { Key = "Exception" }
            };

            appender.AddParameter(dateParam);
            appender.AddParameter(levelParam);
            appender.AddParameter(typeParam);
            appender.AddParameter(methodParam);
            appender.AddParameter(lineNoParam);
            appender.AddParameter(massageParam);
            appender.AddParameter(exceptionParam);

            appender.BufferSize = 20;
            appender.ActivateOptions();
                                          
            return appender;
        }
    }
}
