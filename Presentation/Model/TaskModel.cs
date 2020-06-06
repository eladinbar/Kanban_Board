using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    public class TaskModel : NotifiableModelObject
    {
        public int ID;
        public string Title;
        public string Description;
        public DateTime CreationTime;
        public DateTime DueDate;
        public DateTime LastChangedDate;
        public string AssigneeEmail;

        public TaskModel(BackendController Controller, int ID, string Title, string Description, DateTime CreationTime, DateTime DueDate, 
        DateTime LastChangedDate, string AssigneeEmail) : base(Controller) {
            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.CreationTime = CreationTime;
            this.DueDate = DueDate;
            this.LastChangedDate = LastChangedDate;
            this.AssigneeEmail = AssigneeEmail;
        }
    }
}
