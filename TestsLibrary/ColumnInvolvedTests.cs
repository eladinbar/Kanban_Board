using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class ColumnInvolvedTests
    {
        private ServiceLayer.Service _service;
        private ServiceLayer.User _currentUser;

        public ColumnInvolvedTests()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }

            _service = new ServiceLayer.Service();
            _service.LoadData();
            _currentUser = new ServiceLayer.User("currentUser@ColumnInvolvedTests.com", "currentUser@ColumnInvolvedTests");
            _service.Register(_currentUser.Email, "123Abc", _currentUser.Nickname);
            _service.Login(_currentUser.Email, "123Abc");
        }

        public void RunAllTests()
        {
            this.LimitColumn();
            this.LimitColumnBadColumnOrdinal();
            this.LimitLesserThanTaskNum();

            this.GetColumnByName();
            this.GetColumnByOrdinal();
            this.GetColumnByNonExistName();
            this.GetColumnByNonExistOrdinal();
        }

        public void LimitColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitColumnTest");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(_currentUser.Email, 10, 1).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitColumnBadColumnOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitColumnBadColumnOrdinalTest");
            Console.WriteLine("Input: data with non existing column ordinal.");
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(_currentUser.Email, int.MaxValue, 1).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void LimitLesserThanTaskNum()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("LimitLesserThanTaskNumTest");
            Console.WriteLine("Input: data with smaller column limit than current number of tasks in that column.");
            TaskForTestCreator taskForTestCreator = new TaskForTestCreator(4);
            List<ServiceLayer.Task> _randomTasks = taskForTestCreator._tasks;
            _service.AddTask(_currentUser.Email, _randomTasks.ElementAt(0).Title, _randomTasks.ElementAt(0).Description, _randomTasks.ElementAt(0).DueDate);
            _service.AddTask(_currentUser.Email, _randomTasks.ElementAt(1).Title, _randomTasks.ElementAt(1).Description, _randomTasks.ElementAt(1).DueDate);
            _service.AddTask(_currentUser.Email, _randomTasks.ElementAt(2).Title, _randomTasks.ElementAt(2).Description, _randomTasks.ElementAt(2).DueDate);
            Console.WriteLine("Runtime outcome: " + _service.LimitColumnTasks(_currentUser.Email, 0, 1).ErrorMessage);
            _service.LimitColumnTasks(_currentUser.Email, 0, 10); //reseting the limit to 10
            Console.WriteLine("End of the test: current limit was resetted back to 10.");
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void GetColumnByName()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNameTest");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(_currentUser.Email, "Done").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistName()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNonExistNameTest");
            Console.WriteLine("Input: non existing column name.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(_currentUser.Email, "NonExistingNameOfColumn").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        public void GetColumnByOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByOrdinalTest");
            Console.WriteLine("Input: proper data.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(_currentUser.Email, 2).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void GetColumnByNonExistOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("GetColumnByNonExistOrdinalTest");
            Console.WriteLine("Input: non existing column ordinal.");
            Console.WriteLine("Runtime outcome: " + _service.GetColumn(_currentUser.Email, int.MaxValue).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


    }
}

