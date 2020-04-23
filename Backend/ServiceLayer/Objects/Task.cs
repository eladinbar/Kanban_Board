using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly DateTime LastChangedDate;
        internal Task(int id, DateTime creationTime, string title, string description, DateTime dueDate)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = dueDate;
            this.LastChangedDate = creationTime;
        }
        // You can add code here

        public override string ToString()
        {
            return "#" + Id +
                "\nTitle: " + Title +
                "\nDescription: " + Description +
                "\nDue Date: " + DueDate.ToLongDateString() +
                "\nCreation Time: " + CreationTime.ToLongDateString() +
                "\nLast Changed Date: " + LastChangedDate.ToLongDateString();
        }
    }
}
