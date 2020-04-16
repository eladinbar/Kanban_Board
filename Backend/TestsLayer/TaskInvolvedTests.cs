using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TaskInvolvedTests
    {
        private ServiceLayer.Service _service;
        private ServiceLayer.User _currentUser;
        private List<ServiceLayer.Task> _randomTasks;

        public TaskInvolvedTests()
        {
            DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
            DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
            if (dir2.Exists)
            {
                dir1.Delete(true);
            }

            _randomTasks = new TaskForTestCreator(10)._tasks;
            _service = new ServiceLayer.Service();
            _service.LoadData();
            _currentUser = new ServiceLayer.User("currentUser@ColumnInvolvedTests.com", "currentUser@ColumnInvolvedTests");
            _service.Register(_currentUser.Email, "123Abc", _currentUser.Nickname);
            _service.Login(_currentUser.Email, "123Abc");
        }

        public void RunAllTests()
        {
            this.AddTasks();
            this.AddTaskOverColumnLimit();

            this.UpdateTaskDueDate();
            this.UpdateTaskDueDateBadOrdinal();
            this.UpdateTaskDueDateNonExistOrdinal();

            this.UpdateTaskTitle();
            this.UpdateTaskDescription();

            this.AdvanceTask();
            this.AdvanceTaskDoneColumn();
            this.AdvanceTaskIdNotExist();
        }


        //AddTask
        public void AddTasks()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddTasksTest");
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Runtime outcome: ");
            foreach (ServiceLayer.Task tempTask in _randomTasks)
                Console.WriteLine(_service.AddTask(_currentUser.Email, _randomTasks.ElementAt(0).Title,
                    _randomTasks.ElementAt(0).Description, _randomTasks.ElementAt(0).DueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AddTaskOverColumnLimit()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddTaskOverColumnLimitTest");
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Runtime outcome: ");
            _service.LimitColumnTasks(_currentUser.Email, 0, 10);
            int i = 1;
            foreach (ServiceLayer.Task tempTask in _randomTasks)
            {
                Console.WriteLine(_service.AddTask(_currentUser.Email, "OverColumnLimit" + _randomTasks.ElementAt(0).Title + "#" + i,
                    _randomTasks.ElementAt(0).Description, _randomTasks.ElementAt(0).DueDate).ErrorMessage);
                i++;
            }
            Console.WriteLine("---------------------------------------------------------------");
        }



        //UpdateTaskDueDate
        public void UpdateTaskDueDate()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateTest");
            Console.WriteLine("Input: proper details.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, 0, _randomTasks.ElementAt(0).Id, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateNonExistOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateNonExistOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, int.MaxValue, _randomTasks.ElementAt(0).Id, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateBadOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateBadOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal - task exists bit in different column.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, 2, _randomTasks.ElementAt(0).Id, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }



        //UpdateTaskTitle
        public void UpdateTaskTitle()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskTitleTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskTitle(_currentUser.Email, 0, _randomTasks.ElementAt(0).Id, "UpdateTaskTitleTest").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        //UpdateTaskDescription
        public void UpdateTaskDescription()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDescriptioneTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDescription(_currentUser.Email, 0, _randomTasks.ElementAt(0).Id, "UpdateTaskDescriptioneTest").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        //AdvanceTask
        public void AdvanceTask()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(_currentUser.Email, 0, _randomTasks.ElementAt(0).Id).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceTaskDoneColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskDoneColumnTest");
            Console.WriteLine("Input: proper task details with 'done' column ordinal.");
            _service.AdvanceTask(_currentUser.Email, 1, _randomTasks.ElementAt(0).Id);
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(_currentUser.Email, 2, _randomTasks.ElementAt(0).Id).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceTaskIdNotExist()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskIdNotExistTest");
            Console.WriteLine("Input: non existing task id.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(_currentUser.Email, 0, -34543543).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");

        }


    }
}

