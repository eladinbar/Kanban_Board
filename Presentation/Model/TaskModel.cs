using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class TaskModel
    {
        public int Id;
        public string Title;
        public string Description;
        public DateTime CreationTime;
        public DateTime DueDate;
        public DateTime LastChangedDate;
        public string AssigneeEmail;

        public TaskModel(int id, string title, string description, DateTime creationTime, DateTime dueDate, string assigneeEmail)
        {

        }
    }
}
