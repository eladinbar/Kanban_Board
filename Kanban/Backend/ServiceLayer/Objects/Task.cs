﻿using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly DateTime DueDate;
        public readonly string Title;
        public readonly string Description;
        
        internal Task(int id, DateTime creationTime, DateTime dueDate, string title, string description)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
        }
        // You can add code here
    
    
        public override string ToString()
        {
            return "#" + Id +
                "\nTitle: " + Title +
                "\nDescription: " + Description +
                "\nDue Date: " + DueDate.ToLongDateString() +
                "\nCreation Time: " + CreationTime.ToLongDateString();
        }
    }
}
