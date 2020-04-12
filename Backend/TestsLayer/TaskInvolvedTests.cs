using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TaskInvolvedTests
    {
        private ServiceLayer.Service _service;

        public TaskInvolvedTests(ServiceLayer.Service srv)
        {
            _service = srv;
        }


        //AddTask
        public void AddTaskAllGood(ServiceLayer.User user, ServiceLayer.Task task)
        {
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.AddTask(user.Email, task.Title, task.Description, task.DueDate));
        }

        public void AddTaskOverColumnLimit(ServiceLayer.User user, ServiceLayer.Task task)
        {
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Expected: failed - reached column limit.");
            Console.WriteLine("Runtime outcome: " + _service.AddTask(user.Email, task.Title, task.Description, task.DueDate));
        }
        
        

        //UpdateTaskDueDate
        public void UpdateTaskDueDateAllGood(ServiceLayer.User user, ServiceLayer.Task task, int columnOrdinal, DateTime newDueDate)
        {
            Console.WriteLine("Input: proper details.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(user.Email, columnOrdinal, task.Id, newDueDate));
        }
        
        public void UpdateTaskDueDateNonExistOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, DateTime newDueDate)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - non existing column ordinal.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(user.Email, badColumnOrdinal, task.Id, newDueDate));
        }

        public void UpdateTaskDueDateBadOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, DateTime newDueDate)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - task in other column.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDueDate(user.Email, badColumnOrdinal, task.Id, newDueDate));
        }



        //UpdateTaskTitle
        public void UpdateTaskTitleAllGood(ServiceLayer.User user, ServiceLayer.Task task, int columnOrdinal, string newTitle)
        {
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskTitle(user.Email, columnOrdinal, task.Id, newTitle));
        }

        public void UpdateTaskTitleNonExistOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, string newTitle)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - non existing column ordinal.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskTitle(user.Email, badColumnOrdinal, task.Id, newTitle));
        }

        public void UpdateTaskTitleBadOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, string newTitle)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - task in other column.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskTitle(user.Email, badColumnOrdinal, task.Id, newTitle));
        }


        //UpdateTaskDescription
        public void UpdateTaskDescriptioneAllGood(ServiceLayer.User user, ServiceLayer.Task task, int columnOrdinal, string newDescription)
        {
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDescription(user.Email, columnOrdinal, task.Id, newDescription));
        }

        public void UpdateTaskDescriptioneNonExistOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, string newDescription)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - non existing column ordinal.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDescription(user.Email, badColumnOrdinal, task.Id, newDescription));
        }

        public void UpdateTaskDescriptionBadOrdinal(ServiceLayer.User user, ServiceLayer.Task task, int badColumnOrdinal, string newDescription)
        {
            Console.WriteLine("Input: proper details except columnOrdinal.");
            Console.WriteLine("Expected: failed - task in other column.");
            Console.WriteLine("Runtime outcome: " + _service.UpdateTaskDescription(user.Email, badColumnOrdinal, task.Id, newDescription));
        }


        //AdvanceTask
        public void AdvanceTaskAllGood(ServiceLayer.User user, ServiceLayer.Task task, int columnOrdinal)
        {
            Console.WriteLine("Input: proper task details.");
            Console.WriteLine("Expected: succeed - proper response.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(user.Email, columnOrdinal, task.Id));
        }

        public void AdvanceTaskDoneColumn(ServiceLayer.User user, ServiceLayer.Task task, int doneColumnOrdinal)
        {
            Console.WriteLine("Input: proper task details with 'done' column ordinal.");
            Console.WriteLine("Expected: failed - can't advance a task from 'done' column.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(user.Email, doneColumnOrdinal, task.Id));
        }

        public void AdvanceTaskIdNotExist(ServiceLayer.User user, ServiceLayer.Task task, int columnOrdinal)
        {
            Console.WriteLine("Input: non existing task id.");
            Console.WriteLine("Expected: failed - task doesn't exist.");
            Console.WriteLine("Runtime outcome: " + _service.AdvanceTask(user.Email, columnOrdinal, task.Id));
        }


    }
}

