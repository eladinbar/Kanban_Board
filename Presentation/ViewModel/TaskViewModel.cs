using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    internal class TaskViewModel : NotifiableObject
    {
        public int ID { get => ID; set { ID = value; RaisePropertyChanged("ID"); } }
        public string Title { get => Title; set { Title = value; RaisePropertyChanged("Title"); } }
        public string Description { get => Description; set { Description = value; RaisePropertyChanged("Description"); } }
        public string Message { get => Message; set { Message = value; RaisePropertyChanged("Message"); } }
        public DateTime CreationTime { get; }
        public DateTime DueDate { get => DueDate; set {
            Message = "";
            try {
                    Controller.UpdateTaskDueDate();
                    RaisePropertyChanged("DueDate");
            } 
            catch(Exception ex) {
                    Message = ex.Message;
            }
        } }
        public DateTime LastChangedDate { get => ID; set { ID = value; RaisePropertyChanged("ID"); } }
        public string TaskAssignee { get => ID; set { ID = value; RaisePropertyChanged("ID"); } }
        public string TaskAsigneeNickname { get => ID; set { ID = value; RaisePropertyChanged("ID"); } }
        private BackendController Controller;
        private TaskModel Task;
        public SolidColorBrush BackgroundColor { get {return new SolidColorBrush(Task.DueDate.CompareTo(DateTime.Now) ? Colors.Blue : Colors.Red);} }

        public TaskViewModel(TaskModel Task) {
             this.Controller = Task.Controller;
             this.Task = Task;
             this.ID = Task.ID;
             this.Title = Task.Title;
             this.Description = Task.Description;
             this.CreationTime = Task.CreationTime;
             this.DueDate = Task.DueDate;
             this.LastChangedDate = Task.LastChangedDate;
             this.TaskAssignee = Task.TaskAssignee.Email;
             this.TaskAsigneeNickname = Task.TaskAsignee.Nickname;
        }


    }
}
