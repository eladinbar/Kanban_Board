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
        public string BASE_PATH = Path.GetFullPath(@"..\..\") + "Logs\\";

        public static log4net.ILog getLogger([CallerFilePath]string filename = "")
        {
            return log4net.LogManager.GetLogger(filename);
        }
    }
}
