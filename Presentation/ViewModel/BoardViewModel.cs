using System;
using System.Linq;
using Presentation.Model;
using System.Collections.ObjectModel;
using Presentation.View;
using System.Windows;
using System.Windows.Threading;

namespace Presentation.ViewModel
{
    /// <summary>
    /// The Data Context of the Board window.
    /// </summary>
    class BoardViewModel : NotifiableObject
    {
        private DispatcherTimer dispatcherTimer;
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach (ColumnModel c in this.Board.Columns)
                foreach (TaskModel t in c.Tasks)
                    t.RaiseProperty("TaskBackgroundColor");
        }

        private BackendController Controller;
        public UserModel CurrentUser { get; private set; }
        public bool IsCreator { get; private set; } 
        public BoardModel Board { get; private set; }
        public string ChangeColumnNameToolTip { get; private set; }
        public bool NotCreator { get => !IsCreator; }

        private string ColumnNameToolTip()
        {
            if (IsCreator) return "Column name";
            else return "Column name - can only be changed by the board creator.";
        }

        /// <summary>
        /// A BoardViewModel constructor for an existing board. Initializes an DispatcherTimer for runtime content update.
        /// </summary>
        /// <param name="controller">The controller this view model uses to communicate with the backend.</param>
        /// <param name="currentUser">The current loged in user.</param>
        /// <param name="creatorEmail">An email of current board creator.</param>
        public BoardViewModel(BackendController controller, UserModel currentUser, string creatorEmail)
        {
            this.Controller = controller;
            this.CurrentUser = currentUser;
            this.Board = new BoardModel(controller, creatorEmail, currentUser);
            this.IsCreator = (this.CurrentUser.Email.Equals(this.Board.CreatorEmail));
            ChangeColumnNameToolTip = this.ColumnNameToolTip();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3); //changable period of time. Current value: every 3 seconds
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Declares and initializes a TaskWindow with values of the task which user requests to edit.
        /// </summary>
        /// <param name="taskToEdit">An task which exists in DataBase and user requests to edit it.</param>
        public void EditTask(TaskModel taskToEdit)
        {
            if (taskToEdit == null) return;
            TaskWindow taskEditWindow = new TaskWindow(this.Controller, taskToEdit, (taskToEdit.AssigneeEmail == this.CurrentUser.Email), this.CurrentUser);
            taskEditWindow.ShowDialog();
        }

        /// <summary>
        /// Allows to remove an existing task according to user request.
        /// </summary>
        /// <param name="taskToRemove">An task which exists in DataBase and user requests to delete it.</param>
        internal void RemoveTask(TaskModel taskToRemove)
        {
            try
            {
                this.Controller.RemoveTask(this.Board.CreatorEmail, taskToRemove.ColumnOrdinal, taskToRemove.ID);
                this.Board.RemoveTask(taskToRemove);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error occured");
            }
        }

        /// <summary>
        /// Allows to add new column to the current board at the demanded index. Receives the new column name from the user through InputDialog.
        /// </summary>
        /// <param name="email">Board creator email.</param>
        /// <param name="newColumnOrdinal">Future index of the new column.</param>
        public void AddColumn(string email, int newColumnOrdinal)
        {
            try
            {
                InputDialog columnNameDialog = new InputDialog("Enter the new column name:");
                columnNameDialog.ShowDialog();
                string newColumnName = columnNameDialog.Answer;
                this.Controller.AddColumn(email, newColumnOrdinal, newColumnName);
                this.Board.AddColumn(newColumnOrdinal, newColumnName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Add column error");
            }

        }

        /// <summary>
        /// Allows to move the selected column to its left.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Selected column index.</param>
        public void MoveColumnLeft(string email, int columnOrdinal)
        {
            try
            {
                this.Controller.MoveColumnLeft(email, columnOrdinal);
                this.Board.MoveColumnLeft(columnOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error occured");
            }
        }

        /// <summary>
        /// Allows to move the selected column to its right.
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Selected column index.</param>
        public void MoveColumnRight(string email, int columnOrdinal)
        {
            try
            {
                this.Controller.MoveColumnRight(email, columnOrdinal);
                this.Board.MoveColumnRight(columnOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error occured");
            }
        }

        /// <summary>
        /// Allows to remove the selected column from current board without removing its content (tasks).
        /// </summary>
        /// <param name="email">Current board creator email.</param>
        /// <param name="columnOrdinal">Selected column index.</param>
        public void RemoveColumn(string email, int columnOrdinal)
        {
            try
            {
                this.Controller.RemoveColumn(email, columnOrdinal);
                this.Board.UpdateColumns();
                MessageBox.Show("Column has been removed successfully.", "Remove Column");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error occured");
            }
        }

        /// <summary>
        /// Allows to advance the selected task to the next column. 
        /// </summary>
        /// <param name="taskToAdvance">A task that user demanded to advance.</param>
        public void AdvanceTask(TaskModel taskToAdvance)
        {
            try
            {
                if (taskToAdvance == null) return;
                this.Controller.AdvanceTask(Board.CreatorEmail, taskToAdvance.ColumnOrdinal, taskToAdvance.ID);
                this.Board.AdvanceTask(taskToAdvance, taskToAdvance.ColumnOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Shows an logout confirming message.
        /// </summary>
        public void Logout()
        {
            MessageBox.Show(this.Controller.Logout(CurrentUser.Email), "Logout", MessageBoxButton.OK);            
        }

        /// <summary>
        /// Declares and invokes a TaskWindow. Allows to add new task to the current board.
        /// </summary>
        internal void AddTask()
        {
            TaskWindow taskAddWindow = new TaskWindow(this.Controller, this.CurrentUser);
            taskAddWindow.ShowDialog();
            string lastButton = taskAddWindow.LastClickedButton;
            if (lastButton!= null && lastButton.Equals("Save Task"))
            {
                var tempTask = this.Controller.GetColumn(this.Board.CreatorEmail, 0).Tasks.Last();
                TaskModel newTask = new TaskModel(this.Controller, tempTask.Id, tempTask.Title, tempTask.Description, tempTask.CreationTime, tempTask.DueDate, 
                    tempTask.CreationTime, tempTask.emailAssignee, 0, CurrentUser);
                this.Board.AddNewTask(newTask);                
            }
        }

        /// <summary>
        /// Allows to sort tasks by due date in the selected column.
        /// </summary>
        /// <param name="columnOrdinal"></param>
        internal void SortTasksByDueDate(int columnOrdinal)
        {
            ObservableCollection<TaskModel> tasks = this.Board.Columns.ElementAt(columnOrdinal).Tasks;
            ObservableCollection<TaskModel> tempTasksCollection = new ObservableCollection<TaskModel>(tasks.OrderBy(t => t.DueDate));
            tasks.Clear();
            foreach (TaskModel t in tempTasksCollection) tasks.Add(t);
        }

        /// <summary>
        /// Part of the 'search for task' logic: transfers the input from the search box to each column.
        /// </summary>
        /// <param name="senderText">The input (text) entered in the search box.</param>
        internal void SearchBox_TextChanged(string senderText)
        {
            foreach (ColumnModel cm in this.Board.Columns)
            {
                cm.SearchBox_TextChanged(senderText);
            }
        }
    }
}
