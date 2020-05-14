using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class TaskInvolvedTests
    {
        private Service service;
        private User currentUser;
        private List<ServiceLayer.Task> randomTasks;

        public TaskInvolvedTests()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "KanbanDB.db"));
            FileInfo DBFile = new FileInfo(path);
            if (DBFile.Exists)
                DBFile.Delete();

            randomTasks = new TaskForTestCreator(10).tasks;
            service = new Service();
            service.LoadData();
            currentUser = new User("currentUser@TaskInvolvedTests.com", "currentUser@TaskInvolvedTests.com");
            service.Register(currentUser.Email, "123Abc", currentUser.Nickname);
            service.Login(currentUser.Email, "123Abc");
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
            this.AdvanceOrEditTaskFinalColumn();
            this.AdvanceTaskIdNotExist();
        }


        //AddTask
        public void AddTasks()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddTasksTest");
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Runtime outcome: ");
            foreach (ServiceLayer.Task tempTask in randomTasks)
            {
                string message = service.AddTask(currentUser.Email, tempTask.Title, tempTask.Description, tempTask.DueDate).ErrorMessage;
                Console.WriteLine(message);
                if (message == null)
                    Console.Write("AddTask was executed successfully!");
            }
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AddTaskOverColumnLimit()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AddTaskOverColumnLimitTest");
            Console.WriteLine("Input: proper new task details with existing email.");
            Console.WriteLine("Runtime outcome: ");
            service.LimitColumnTasks(currentUser.Email, 0, 10);
            int i = 1;
            foreach (ServiceLayer.Task tempTask in randomTasks)
            {
                string message = service.AddTask(currentUser.Email, "OverColumnLimit" + tempTask.Title + "#" + i, tempTask.Description, tempTask.DueDate).ErrorMessage;
                Console.WriteLine(message);
                if (message == null & i>10)
                    Console.Write("AddTaskOverColumnLimit succeeded but was expected to fail");
                else
                    Console.Write("AddTask run successfully!");
                i++;
            }
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDate()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateTest");
            Console.WriteLine("Input: proper details.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            string message = service.UpdateTaskDueDate(currentUser.Email, 0, 1, newDueDate).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("UpdateTaskDueDate run successfully!");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateNonExistOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateNonExistOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            string message = service.UpdateTaskDueDate(currentUser.Email, int.MaxValue, 1, newDueDate).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("UpdateTaskDueDateNonExistOrdinal succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDueDateBadOrdinal()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDueDateBadOrdinalTest");
            Console.WriteLine("Input: proper details except columnOrdinal - task exists but in different column.");
            DateTime newDueDate = new DateTime(2035, 7, 24);
            string message = service.UpdateTaskDueDate(currentUser.Email, 1, 1, newDueDate).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("UpdateTaskDueDateBadOrdinal succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskTitle()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskTitleTest");
            Console.WriteLine("Input: proper task details.");
            string message = service.UpdateTaskTitle(currentUser.Email, 0, 1, "UpdateTaskTitleTest").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("UpdateTaskTitle run successfully!");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void UpdateTaskDescription()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("UpdateTaskDescriptioneTest");
            Console.WriteLine("Input: proper task details.");
            string message = service.UpdateTaskDescription(currentUser.Email, 0, 1, "UpdateTaskDescriptioneTest").ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("UpdateTaskDescription run successfully!");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceTask()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskTest");
            Console.WriteLine("Input: proper task details.");
            string message = service.AdvanceTask(currentUser.Email, 0, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("AdvanceTask run successfully!");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceOrEditTaskFinalColumn()////////////////////////////////////
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceOrEditTaskDoneColumn");
            Console.WriteLine("Input: proper task details with 'done' column ordinal.");
            service.AdvanceTask(currentUser.Email, 1, 1);
            string message1 = service.AdvanceTask(currentUser.Email, 2, 1).ErrorMessage;
            Console.WriteLine("Runtime outcome for advancing: " + message1);
            if (message1 == null)
                Console.WriteLine("AdvanceTaskFinalColumn succeeded but was expected to fail");
            string message2 = service.UpdateTaskDescription(currentUser.Email, 2, 1, "if you see this in description - it's bad").ErrorMessage;
            Console.WriteLine("Runtime outcome for editing: " + message2);
            if (message2 == null)
                Console.Write("EditTaskFinalColumn succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");
        }

        public void AdvanceTaskIdNotExist()
        {
            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("AdvanceTaskIdNotExistTest");
            Console.WriteLine("Input: non existing task id.");
            string message = service.AdvanceTask(currentUser.Email, 0, -34543543).ErrorMessage;
            Console.WriteLine("Runtime outcome: " + message);
            if (message == null)
                Console.Write("AdvanceTaskIdNotExist succeeded but was expected to fail");
            Console.WriteLine("---------------------------------------------------------------");

        }
    }
}