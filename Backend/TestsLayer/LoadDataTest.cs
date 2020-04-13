using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class LoadDataTest
    {
        private Boolean _dataLoaded;

        public LoadDataTest()
        {
            _dataLoaded = false;
        }

        public void LoadData(ServiceLayer.Service service)
        {
            if (!_dataLoaded)
            {
                _dataLoaded = true;
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("LoadDataTest - Data loaded for the first time.");
                Console.WriteLine("Input: no input necessary.");
                Console.WriteLine("Expected: succeed - data loaded successfully.");
                Console.WriteLine("Runtime outcome: " + service.LoadData().ErrorMessage);
                Console.WriteLine("---------------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("LoadDataTest - Data loaded not for the first time.");
                Console.WriteLine("Input: no input necessary.");
                Console.WriteLine("Expected: failed - data is already loaded.");
                Console.WriteLine("Runtime outcome: " + service.LoadData().ErrorMessage);
                Console.WriteLine("---------------------------------------------------------------");
            }
        }
    }
}
