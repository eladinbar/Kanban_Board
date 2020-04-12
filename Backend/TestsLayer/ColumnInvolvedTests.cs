using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class ColumnInvolvedTests
    {
        private ServiceLayer.Service _service;

        public ColumnInvolvedTests(ServiceLayer.Service srv)
        {
            _service = srv;
        }


        //LimitColumn
        public void LimitColumnAllGood(ServiceLayer.User user, int columnOrdinal, int columnLimit)
        {
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, columnOrdinal, columnLimit));
        }

        public void LimitColumnBadColumnOrdinal(ServiceLayer.User user, int badColumnOrdinal, int columnLimit)
        {
            Console.WriteLine("Input: data with non existing column ordinal.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, badColumnOrdinal, columnLimit));
        }

        public void LimitColumnLesserColumnLimit(ServiceLayer.User user, int columnOrdinal, int lesserColumnLimit)
        {
            Console.WriteLine("Input: data with lesser column limit than current limit.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, columnOrdinal, lesserColumnLimit));
        }

        //GetColumnByName
        public void GetColumnByNameAllGood(ServiceLayer.User user, string columnName)
        {
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, columnName));
        }

        public void GetColumnByNonExistName(ServiceLayer.User user, string badColumnName)
        {
            Console.WriteLine("Input: non existing column name.");
            Console.WriteLine("Expected: failed - column with that name doesnt exist.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnName));
        }


        //GetColumnByOrdinal
        public void GetColumnByOrdinalAllGood(ServiceLayer.User user, int badColumnOrdinal)
        {
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnOrdinal));
        }

        public void GetColumnByNonExistOrdinal(ServiceLayer.User user, int badColumnOrdinal)
        {
            Console.WriteLine("Input: non existing column name.");
            Console.WriteLine("Expected: failed - column with that ordinal doesnt exist.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnOrdinal));
        }


    }
}

