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
        /// The board model constructor. Pulls all the data (columns and tasks) of the current board from the database.
        /// </summary>
        /// <param name="controller">The controller this model uses to communicate with the backend.</param>
        /// <param name="creatorEmail">The email of the board creator.</param>
        /// <param name="currentUser">The current logged in user.</param>
        public BoardModel(BackendController controller, string creatorEmail, UserModel currentUser) : base(controller)
        {
            this.CurrentUser = currentUser;
            this.CreatorEmail = creatorEmail;
            this.Columns = this.CreateColumns(creatorEmail);
        }

        /// <summary>
        /// Pulls all the required data (columns and tasks) from the database.
        /// </summary>
        /// <param name="creatorEmail">The email of the board creator.</param>
        /// <returns>Returns an ObservableCollection of ColumnModels.</returns>
        private ObservableCollection<ColumnModel> CreateColumns(string creatorEmail) //receives SL.Columns and its list of Tasks and transform them into PL.Columns - move this logic to viewModel 
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
        /// Advances the selected task to the next column.
        /// </summary>
        /// <param name="taskToAdvance">The task that the user desires to advance.</param>
        /// <param name="columnOrdinal">The index of the column containing the selected task.</param>
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
        /// Adds a new task to the board. 
        /// </summary>
        /// <param name="newTask">The new task to add. Contains the new task information.</param>
        internal void AddNewTask(TaskModel newTask)
        {
            this.Columns.ElementAt(0).Tasks.Add(newTask);
            this.Columns.ElementAt(0).RaiseProperty("CurrentAmountOfTasks");
            this.Columns.ElementAt(0).RaiseProperty("TasksToView");
        }

        /// <summary>
        /// Adds a new column to the current board at the given index.
        /// </summary>
        /// <param name="newColumnOrdinal">The index to add the new column at.</param>
        /// <param name="newColumnName">The name of the new column.</param>
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
        /// Removes the selected task from the board.
        /// </summary>
        /// <param name="taskToRemove">The task to remove.</param>
        internal void RemoveTask(TaskModel taskToRemove)
        {
            this.Columns.ElementAt(taskToRemove.ColumnOrdinal).Tasks.Remove(taskToRemove);
        }

        /// <summary>
        /// Moves the selected column to its left.
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
        /// Moves the selected column to its right.
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
        /// Updates the GUI content of the board (specifically).
        /// </summary>
        internal void UpdateColumns()
        {
            this.Columns = CreateColumns(this.CreatorEmail);
            RaisePropertyChanged("Columns");
        }
    }
}
