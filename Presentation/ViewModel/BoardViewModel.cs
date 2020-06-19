using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using System.Collections.ObjectModel;
using Presentation.View;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;

namespace Presentation.ViewModel
{
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
        public bool notCreator { get => !IsCreator; }

        private string ColumnNameToolTip()
        {
            if (IsCreator) return "Column name";
            else return "Column name - can be changed only by board creator";
        }



        //public TaskViewModel CurrentTask { get; private set; } //not needed????????????????????????????????????????
        //private bool _isSelected = false;
        //public bool IsSelected
        //{
        //    get => _isSelected;
        //    set
        //    {
        //        _isSelected = value;
        //        RaisePropertyChanged("IsSelected");
        //    }
        //}

        public BoardViewModel(BackendController controller, UserModel currentUser, string creatorEmail)
        {
            this.Controller = controller;
            this.CurrentUser = currentUser;
            this.Board = new BoardModel(controller, creatorEmail, currentUser);
            this.IsCreator = true;  //(this.CurrentUser.Email.Equals(this.Board.CreatorEmail));
            ChangeColumnNameToolTip = this.ColumnNameToolTip();
            this.dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3); //change to greater period (maybe a minute) !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            dispatcherTimer.Start();
        }

        public void EditTask(TaskModel taskToEdit)
        {
            if (taskToEdit == null) return;
            TaskWindow taskEditWindow = new TaskWindow(this.Controller, taskToEdit, (taskToEdit.AssigneeEmail == this.CurrentUser.Email));
            taskEditWindow.ShowDialog();
        }


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

        public void RemoveColumn(string email, int columnOrdinal)
        {
            try
            {
                this.Controller.RemoveColumn(email, columnOrdinal);
                this.Board.UpdateColumns(); //this or update manually???????????????????????????
                MessageBox.Show("Column has been removed successfully.", "Remove Column");

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error occured");
            }
        }


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

        public void Logout()
        {
            MessageBox.Show(this.Controller.Logout(CurrentUser.Email), "Logout", MessageBoxButton.OK);            
        }


        internal void ChangePassword()
        {
            MessageBox.Show(this.Controller.Logout(CurrentUser.Email), "Change password", MessageBoxButton.OK);
        }


        internal void AddTask()
        {
            TaskWindow taskAddWindow = new TaskWindow(this.Controller, this.CurrentUser.Email);
            taskAddWindow.ShowDialog();
            string lastButton = taskAddWindow.LastClickedButton;
            if (lastButton.Equals("Save Task"))
            {
                var tempTask = this.Controller.GetColumn(this.Board.CreatorEmail, 0).Tasks.Last();
                TaskModel newTask = new TaskModel(this.Controller, tempTask.Id, tempTask.Title, tempTask.Description, tempTask.CreationTime, tempTask.DueDate, tempTask.CreationTime, CurrentUser.Email, 0);
                this.Board.AddNewTask(newTask);                
            }
        }

        internal void SortTasksByDueDate(int columnOrdinal)
        {
            ObservableCollection<TaskModel> tasks = this.Board.Columns.ElementAt(columnOrdinal).Tasks;
            ObservableCollection<TaskModel> tempTasksCollection = new ObservableCollection<TaskModel>(tasks.OrderBy(t => t.DueDate));
            tasks.Clear();
            foreach (TaskModel t in tempTasksCollection) tasks.Add(t);
        }

        internal void SearchBox_TextChanged(string senderText)
        {
            foreach (ColumnModel cm in this.Board.Columns)
            {
                cm.SearchBox_TextChanged(senderText);
            }
        }
    }
}
