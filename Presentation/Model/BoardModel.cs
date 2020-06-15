using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Presentation.Model
{
    public class BoardModel : NotifiableModelObject
    {

        public string CreatorEmail { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }
        private UserModel CurrentUser;

        public BoardModel(BackendController controller, string creatorEmail, UserModel currentUser) : base(controller)
        {
            this.CurrentUser = currentUser;
            this.CreatorEmail = creatorEmail;
            this.Columns = this.CreateColumns(creatorEmail);
        }

        private ObservableCollection<ColumnModel> CreateColumns(string creatorEmail)  //receives SL.Columns and its list of Tasks and transform them into PL.Columns - move this logic to viewModel 
        {
            ObservableCollection<ColumnModel> tempColumns = new ObservableCollection<ColumnModel>();
            int i = 0;
            foreach (string cName in this.Controller.GetBoard(creatorEmail).ColumnsNames.ToList())
            {
                var c = this.Controller.GetColumn(creatorEmail, cName);
                ObservableCollection<TaskModel> tasks = new ObservableCollection<TaskModel>();
                foreach (var t in c.Tasks)
                {
                    tasks.Add(new TaskModel(Controller,t.Id, t.Title, t.Description, t.CreationTime, t.DueDate, t.CreationTime, t.emailAssignee, i, this.CurrentUser.Email));
                }
                tempColumns.Add(new ColumnModel(Controller, tasks, c.Limit, c.Name, i, CreatorEmail));
                i++;
            }
            return tempColumns;
        }

        public void AdvanceTask(TaskModel taskToAdvance, int columnOrdinal)
        {
            this.Columns.ElementAt(columnOrdinal).Tasks.Remove(taskToAdvance);
            this.Columns.ElementAt(columnOrdinal + 1).Tasks.Add(taskToAdvance);
            taskToAdvance.ColumnOrdinal = taskToAdvance.ColumnOrdinal + 1;
        }

        internal void AddNewTask(TaskModel newTask)
        {
            this.Columns.ElementAt(0).Tasks.Add(newTask);
        }

        internal void AddColumn(int newColumnOrdinal, string newColumnName)
        {
            ColumnModel newColumn = new ColumnModel(this.Controller, new ObservableCollection<TaskModel>(), 100, newColumnName, newColumnOrdinal, CreatorEmail);
            this.Columns.Insert(newColumnOrdinal, newColumn);
            for (int i = newColumnOrdinal + 1; i < this.Columns.Count; i++)
            {
                ColumnModel cm = this.Columns.ElementAt(i);
                cm.Ordinal = i + 1;
                foreach (TaskModel tm in cm.Tasks) tm.ColumnOrdinal = i + 1;
            }
            RaisePropertyChanged("Columns");
        }

        internal void MoveColumnLeft(int columnOrdinal)
        {
            ColumnModel columnToMove = this.Columns.ElementAt(columnOrdinal);
            ColumnModel columnToUpdateOrdinal = this.Columns.ElementAt(columnOrdinal - 1);
            this.Columns.RemoveAt(columnOrdinal);
            this.Columns.Insert(columnOrdinal - 1, columnToMove);
            columnToMove.Ordinal = columnOrdinal - 1;
            columnToUpdateOrdinal.Ordinal = columnOrdinal;
            foreach (TaskModel t in columnToMove.Tasks) t.ColumnOrdinal = columnToMove.Ordinal;
            foreach (TaskModel t in columnToUpdateOrdinal.Tasks) t.ColumnOrdinal = columnToUpdateOrdinal.Ordinal;
        }

        internal void MoveColumnRight(int columnOrdinal)
        {
            ColumnModel columnToMove = this.Columns.ElementAt(columnOrdinal);
            ColumnModel columnToUpdateOrdinal = this.Columns.ElementAt(columnOrdinal+1);
            this.Columns.RemoveAt(columnOrdinal);
            this.Columns.Insert(columnOrdinal + 1, columnToMove);
            columnToMove.Ordinal = columnOrdinal + 1;
            columnToUpdateOrdinal.Ordinal = columnOrdinal;
            foreach (TaskModel t in columnToMove.Tasks) t.ColumnOrdinal = columnToMove.Ordinal;
            foreach (TaskModel t in columnToUpdateOrdinal.Tasks) t.ColumnOrdinal = columnToUpdateOrdinal.Ordinal;
        }

        internal void UpdateColumns()
        {
            this.Columns = CreateColumns(this.CreatorEmail);
            RaisePropertyChanged("Columns");
        }
    }
}
