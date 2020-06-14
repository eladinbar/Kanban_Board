using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using System.Collections.ObjectModel;
using Presentation.View;
using System.Windows;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
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
            this.Board = new BoardModel(controller, creatorEmail);
            this.IsCreator = true;  //(this.CurrentUser.Email.Equals(this.Board.CreatorEmail));
            ChangeColumnNameToolTip = this.ColumnNameToolTip();

        }

        public void EditTask(TaskModel taskToEdit)
        {
            if (taskToEdit == null) return;
            TaskWindow taskEditWindow = new TaskWindow(this.Controller, taskToEdit, (taskToEdit.AssigneeEmail == this.CurrentUser.Email));
            taskEditWindow.ShowDialog();
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
            if (!lastButton.Equals("Cancel"))
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

    }
}
