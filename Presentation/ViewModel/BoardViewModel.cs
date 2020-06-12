using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        private BackendController Controller;
        public UserModel CurrentUser { get; private set; }
        public BoardModel Board { get; private set; }
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get =>  _selectedTask;
            set
            {
                _selectedTask = value;
                IsSelected = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public BoardViewModel(BackendController controller, UserModel currentUser, string creatorEmail)
        {
            this.Controller = controller;
            this.CurrentUser = currentUser;
            this.Board = new BoardModel(controller, creatorEmail);
            this._selectedTask = null;
        }

        public void AdvanceTask(int columnOrdinal, int taskId)
        {
            try
            {
                this.Controller.AdvanceTask(Board.CreatorEmail, columnOrdinal, taskId);
                this.Board.AdvanceTask(this.SelectedTask, columnOrdinal);
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
    }
}
