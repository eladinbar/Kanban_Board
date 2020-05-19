using log4net.Appender;
using log4net.Layout;
using System;
using System.Data;
using System.Data.SQLite;
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

        /// <summary>
        /// sets up appenders for logging.
        /// </summary>
        /// <param name="appenders">
        /// an array of appenders to setup basic configuration.
        /// </param>
        public static void SetUp(params IAppender[] appenders)
        {
            //Sets the RollingFileAppender as the logger appender.
            log4net.Config.BasicConfigurator.Configure(appenders);
        }

        /// <summary>
        /// Sets up the RollingFileAppender to save the logs to the "Logs" folder in the original directory the program is using.
        /// </summary>
        /// <returns>
        /// return the rolling file appender.
        /// </returns>
        /// <remarks>
        /// The logger saves a log file once per program execution.
        /// </remarks>
        public static IAppender SetupRoliingFileAppender()
        {
            //Defines how we want to log to the LogFile.txt
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss} %level - (%type: %method - %line)%newline %message%newline%exception";
            patternLayout.ActivateOptions();

            //Creates and defines a RollingFileAppender for file logging.
            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.File = BASE_PATH;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 20;
            roller.MaximumFileSize = "10MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Once;
            roller.StaticLogFileName = true;

            roller.ActivateOptions();
            return roller;

        }
        /// <summary>
        /// Sets up AdoNetAppender to SQLite Database with the following columns (LogId, Date, Level, Type, Method, LineNo, Massage, Exception).
        /// </summary>
        /// <returns>
        /// Returns the sqlite Appender.
        /// </returns>
        public static IAppender SetUpSQLiteAppender()
        {
            //define the path to the database file
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanLoggerDB.db"));
            FileInfo fileDB = new FileInfo(path);
            //creates the Database file if not exists
            if (!fileDB.Exists)  CreateLoggerDB();
            
            //creates the appender
            AdoNetAppender appender = new AdoNetAppender
            {
                ConnectionType = "System.Data.SQLite.SQLiteConnection, System.Data.SQLite",
                ConnectionString = $"Data Source={path}; Version=3;",
                CommandText =$"INSERT INTO {LogTableName} (Date, Level, Type, Method, LineNo, Massage, Exception) " +
                $"Value  (@Date, @Level, @Type, @Method, @LineNo, @Massage, @Exception)"     
            };


            //define all parameters to log in the data base
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
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%level"))
            };
            AdoNetAppenderParameter typeParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Type",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%type"))
            };
            AdoNetAppenderParameter methodParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Method",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%method"))
            };
            AdoNetAppenderParameter lineNoParam = new AdoNetAppenderParameter
            {
                ParameterName = "@LineNo",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%line"))
            };
            AdoNetAppenderParameter massageParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Message",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%massage"))
            };
            AdoNetAppenderParameter exceptionParam = new AdoNetAppenderParameter
            {
                ParameterName = "@Exception",
                DbType = DbType.String,
                Layout = new Layout2RawLayoutAdapter(new PatternLayout("%exeption"))
            };

            //adds the parameters to the appender
            appender.AddParameter(dateParam);
            appender.AddParameter(levelParam);
            appender.AddParameter(typeParam);
            appender.AddParameter(methodParam);
            appender.AddParameter(lineNoParam);
            appender.AddParameter(massageParam);
            appender.AddParameter(exceptionParam);
            //after 20 log lines the appender writes to the Database
            appender.BufferSize = 20;

            //activates the appender settings that we added priviusly and return the appender.
            appender.ActivateOptions();                                          
            return appender;
        }

        /// <summary>
        /// create a .db file to appender the log events to a SQLite Database
        /// </summary>
        private static void CreateLoggerDB()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanLoggerDB.db"));
            using (var connection = new SQLiteConnection())
            {
               
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"CREATE TABLE {LogTableName} (" +
                                      $"\"LogId\"     INTEGER NOT NULL PRIMARY KEY," +
                                      $"Date      TEXT NOT NULL," +
                                      $"Level     TEXT NOT NULL," +
                                      $"Type      TEXT NOT NULL," +
                                      $"Method    TEXT NOT NULL," +
                                      $"LineNo    INTEGER NOT NULL," +
                                      $"Massage   TEXT NOT NULL," +
                                      $"Exception TEXT NOT NULL" +
                                      $"); "
                };
                connection.ConnectionString = $"Data Source={path}; Version=3;";
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                } catch(SQLiteException e)
                {
                    getLogger().Error("SQlite Erxception occured", e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    getLogger().Info("connection closed.");
                }

            }
        }
    }
}
