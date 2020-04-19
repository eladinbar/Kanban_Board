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
            _currentUser = new ServiceLayer.User("currentUser@TaskInvolvedTeasts.com", "currentUser@TaskInvolvedTeasts.com");
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
            this.AdvanceOrEditTaskDoneColumn();
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
                Console.WriteLine(_service.AddTask(_currentUser.Email, tempTask.Title,
                    tempTask.Description, tempTask.DueDate).ErrorMessage);
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
                Console.WriteLine(_service.AddTask(_currentUser.Email, "OverColumnLimit" + tempTask.Title + "#" + i,
                    tempTask.Description, tempTask.DueDate).ErrorMessage);
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
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, 0, 1, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateNonExistOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateNonExistOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, int.MaxValue,1, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateBadOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateBadOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal - task exists but in different column.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(_currentUser.Email, 1, 1, newDueDate).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }



        //UpdateTaskTitle
        public void UpdateTaskTitle()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskTitleTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskTitle(_currentUser.Email, 0,1, "UpdateTaskTitleTest").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        //UpdateTaskDescription
        public void UpdateTaskDescription()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDescriptioneTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDescription(_currentUser.Email, 0,1, "UpdateTaskDescriptioneTest").ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }


        //AdvanceTask
        public void AdvanceTask()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskTest");
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(_currentUser.Email, 0, 1).ErrorMessage);
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceOrEditTaskDoneColumn()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceOrEditTaskDoneColumn");
            Console.WriteLine("Input: proper task details with 'done' column ordinal.");
            _service.AdvanceTask(_currentUser.Email, 1, 1);
            Console.WriteLine("Runtime outcome for advancing: " + _service.AdvanceTask(_currentUser.Email, 2,1).ErrorMessage);
            Console.WriteLine("Runtime outcome for editing: " + _service.UpdateTaskDescription(_currentUser.Email, 2, 1, "if you see this in description - it's bad").ErrorMessage);
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

