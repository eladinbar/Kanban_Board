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
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - LimitColumnAllGood().");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, columnOrdinal, columnLimit));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitColumnBadColumnOrdinal(ServiceLayer.User user, int badColumnOrdinal, int columnLimit)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - LimitColumnBadColumnOrdinal().");
            Console.WriteLine("Input: data with non existing column ordinal.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, badColumnOrdinal, columnLimit));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitColumnLesserColumnLimit(ServiceLayer.User user, int columnOrdinal, int lesserColumnLimit)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - LimitColumnLesserColumnLimit().");
            Console.WriteLine("Input: data with lesser column limit than current limit.");
            Console.WriteLine("Expected: failed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(user.Email, columnOrdinal, lesserColumnLimit));
            Console.WriteLine("---------------------------------------------------------------");
        }

        //GetColumnByName
        public void GetColumnByNameAllGood(ServiceLayer.User user, string columnName)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - GetColumnByNameAllGood().");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, columnName));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistName(ServiceLayer.User user, string badColumnName)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - GetColumnByNonExistName().");
            Console.WriteLine("Input: non existing column name.");
            Console.WriteLine("Expected: failed - column with that name doesnt exist.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnName));
            Console.WriteLine("---------------------------------------------------------------");
        }


        //GetColumnByOrdinal
        public void GetColumnByOrdinalAllGood(ServiceLayer.User user, int badColumnOrdinal)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - GetColumnByOrdinalAllGood().");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnOrdinal));
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistOrdinal(ServiceLayer.User user, int badColumnOrdinal)
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("ColumnInvolvedTests - GetColumnByNonExistOrdinal().");
            Console.WriteLine("Input: non existing column name.");
            Console.WriteLine("Expected: failed - column with that ordinal doesnt exist.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(user.Email, badColumnOrdinal));
            Console.WriteLine("---------------------------------------------------------------");
        }


    }
}

