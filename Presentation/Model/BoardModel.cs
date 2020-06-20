using System.Linq;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    /// <summary>
    /// The model the Board window is designed after.
    /// </summary>
    public class BoardModel : NotifiableModelObject
    {

        public string CreatorEmail { get; set; }
        public ObservableCollection<ColumnModel> Columns { get; set; }
        private UserModel CurrentUser;

        /// <summary>
        /// Board model constructor. Pulles all the data (columns and tasks) of the current board from the Data Base.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="creatorEmail"></param>
        /// <param name="currentUser"></param>
        /// <param name="controller">The controller this model uses to communicate with the backend.</param>
        /// <param name="currentUser">The current loged in user.</param>
        /// <param name="creatorEmail">An email of current board creator.</param>
        public BoardModel(BackendController controller, string creatorEmail, UserModel currentUser) : base(controller)
        {
            this.CurrentUser = currentUser;
            this.CreatorEmail = creatorEmail;
            this.Columns = this.CreateColumns(creatorEmail);
        }

        /// <summary>
        /// Pulls all the required data (columns and tasks) from the Data Base.
        /// </summary>
        /// <param name="creatorEmail">Current board creator email.</param>
        /// <returns>Returns ObservableCollection of ColumnModels.</returns>
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
                    tasks.Add(new TaskModel(Controller,t.Id, t.Title, t.Description, t.CreationTime, t.DueDate, t.CreationTime, t.emailAssignee, i, this.CurrentUser));
                }
                tempColumns.Add(new ColumnModel(Controller, tasks, c.Limit, c.Name, i, CreatorEmail));
                i++;
            }
            return tempColumns;
        }

        /// <summary>
        /// Allows to advance the selected task to the next column.
        /// </summary>
        /// <param name="taskToAdvance">A task that user demanded to advance.</param>
        /// <param name="columnOrdinal">The index of the column that the selected task placed in it.</param>
        public void AdvanceTask(TaskModel taskToAdvance, int columnOrdinal)
        {
            ColumnModel sourceColumn = this.Columns.ElementAt(columnOrdinal);
            sourceColumn.Tasks.Remove(taskToAdvance);
            sourceColumn.RaiseProperty("CurrentAmountOfTasks");
            ColumnModel targetColumn = this.Columns.ElementAt(columnOrdinal + 1);
            targetColumn.Tasks.Add(taskToAdvance);
            targetColumn.RaiseProperty("CurrentAmountOfTasks");
            taskToAdvance.ColumnOrdinal = taskToAdvance.ColumnOrdinal + 1;        
        }

        /// <summary>
        /// Allows to add new task to the current board. 
        /// </summary>
        /// <param name="newTask">The new task to add. Contains the new task information.</param>
        internal void AddNewTask(TaskModel newTask)
        {
            this.Columns.ElementAt(0).Tasks.Add(newTask);
            this.Columns.ElementAt(0).RaiseProperty("CurrentAmountOfTasks");
            this.Columns.ElementAt(0).RaiseProperty("TasksToView");
        }


        /// <summary>
        /// Allows to add new column to the current board at the demanded index.
        /// </summary>
        /// <param name="newColumnOrdinal">The index to add the new column at it.</param>
        /// <param name="newColumnName">The name of the new column. Received from the user.</param>
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

        /// <summary>
        /// Allows to remove the selected task from the board.
        /// </summary>
        /// <param name="taskToRemove">A task that was selected to be removed.</param>
        internal void RemoveTask(TaskModel taskToRemove)
        {
            this.Columns.ElementAt(taskToRemove.ColumnOrdinal).Tasks.Remove(taskToRemove);
        }

        /// <summary>
        /// Allows to move a selected column to its left.
        /// </summary>
        /// <param name="columnOrdinal">The index of the selected column.</param>
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

        /// <summary>
        /// Allows to move a selected column to its right.
        /// </summary>
        /// <param name="columnOrdinal">The index of the selected column.</param>
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

        /// <summary>
        /// An help-method that allows to update GUI content of the board (specifically).
        /// </summary>
        internal void UpdateColumns()
        {
            this.Columns = CreateColumns(this.CreatorEmail);
            RaisePropertyChanged("Columns");
        }
    }
}
