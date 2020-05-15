using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.KanbanTests
{
    class TaskForTestCreator
    {
        public List<ServiceLayer.Task> tasks { get; }

        public TaskForTestCreator(int numOfTasks)
        {
            tasks = new List<ServiceLayer.Task>();
            Random rand = new Random();
            DateTime dueDate = new DateTime(2035 , 03 , 26);
            for (int i = 0; i < numOfTasks; i++)
            {
                int id = rand.Next(numOfTasks) + 1;
                ServiceLayer.Task tempTask = new ServiceLayer.Task(id, DateTime.Now, dueDate, "task " + id + " title", "lorem ipsum " + (i + 1));
                tasks.Add(tempTask);
            }
        }

        public void PrintTasks()
        {
            Console.WriteLine("Current list of tasks: ");
            int i = 1;
            foreach (ServiceLayer.Task tempTask in tasks)
            {
                Console.WriteLine(i + ") Id: " + tempTask.Id + ". Title: \"" + tempTask.Title + "\" (description: " + tempTask.Description + ")." + " Created on: " + tempTask.CreationTime + ", until: " + tempTask.DueDate + ".");
                i++;
            }
        }
    }
}