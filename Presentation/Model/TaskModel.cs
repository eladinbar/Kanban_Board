﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        public int ID { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get; set; }
        public DateTime LastChangedDate{ get; }
        public string AssigneeEmail { get; set; }
        public int ColumnOrdinal { get; set; }

        public TaskModel(BackendController Controller, int ID, string Title, string Description, DateTime CreationTime, DateTime DueDate, 
        DateTime LastChangedDate, string AssigneeEmail, int columnOrdinal) : base(Controller) {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.CreationTime = CreationTime;
            this.DueDate = DueDate;
            this.LastChangedDate = LastChangedDate;
            this.AssigneeEmail = AssigneeEmail;
            this.ColumnOrdinal = columnOrdinal;
        }

        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string title)
        {
            Controller.UpdateTaskTitle(AssigneeEmail, ColumnOrdinal, ID, title);
            this.Title = title;
            RaisePropertyChanged("Title");
        }

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string description)
        {
            Controller.UpdateTaskDescription(AssigneeEmail, ColumnOrdinal, ID, description);
            this.Description = description;
            RaisePropertyChanged("Description");
        }

        /// <summary>
        /// Update the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <param name="taskId">The task to be updated identified task ID.</param>
        /// <param name="dueDate">The new due date of the column.</param>
        /// <returns>A response object. The response should contain an error message in case of an error.</returns>
        public void UpdateTaskDueDate(DateTime dueDate)
        {
            Controller.UpdateTaskDueDate(AssigneeEmail, ColumnOrdinal, ID, dueDate);
            this.DueDate = dueDate;
            RaisePropertyChanged("DueDate");
        }

        /// <summary>
        /// Assigns a task to a user
        /// </summary> 
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        public void AssignTask(string emailAssignee)
        {
            Controller.AssignTask(AssigneeEmail, ColumnOrdinal, ID, emailAssignee);
            this.AssigneeEmail = emailAssignee;
            RaisePropertyChanged("TaskAssignee");
        }
    }
}
