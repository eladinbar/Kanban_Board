using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        private readonly int MAX_COLUMN_NAME_LENGTH = 15;
        public int MaxColumnNameLength { get => MAX_COLUMN_NAME_LENGTH; }
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public ObservableCollection<TaskModel> TasksToView { get; private set; }
        public string CreatorEmail;        
        public int CurrentAmountOfTasks { get => this.Tasks.Count; }

        private int _ordinal;
        public int Ordinal
        {
            get => _ordinal;
            set
            {
                _ordinal = value;
                RaisePropertyChanged("Ordinal");
            }
        }

        private int _limit; 
        public string Limit
        {
            get => _limit.ToString();
            set
            {
                this.Controller.LimitColumnTasks(CreatorEmail, Ordinal, Int32.Parse(value));
                this._limit = Int32.Parse(value);
                RaisePropertyChanged("Limit");

            }
        }

        private string _name;      
        public string Name
        {
            get => _name;
            set
            {
                this.Controller.ChangeColumnName(CreatorEmail, Ordinal, value);
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public ColumnModel(BackendController controller, ObservableCollection<TaskModel> tasks, int limit, string name, int ordinal, string creatorEmail) : base(controller)
        {
            this.Tasks = tasks;
            this._limit = limit;
            this._name = name;
            this._ordinal = ordinal;
            this.CreatorEmail = creatorEmail;
            this.TasksToView = this.Tasks;
            this.Tasks.CollectionChanged += HandleChange;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.TasksToView = new ObservableCollection<TaskModel>(this.Tasks);
            RaisePropertyChanged("TasksToView");
        }

        public void RaiseProperty(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }
        

        public bool OnKeyDownHandlerName(object sender, KeyEventArgs e) //column name changes
        {
            if (e.Key == Key.Return)
            {
                try
                {
                    Name = ((TextBox)sender).Text;
                    MessageBox.Show("Column name changed successfully", "Info");
                    return true;
                }
                catch (Exception ex)
                {                    
                    MessageBox.Show(ex.Message, "Invalid Action");
                    ((TextBox)sender).Undo();
                    return false;
                }
            }
            return false;                       
        }

        
        internal bool OnKeyDownHandlerLimit(object sender, KeyEventArgs e)
        {
            //if (e.Key > 57 | e.Key < 48) maybe restrict letters input?????????????????????????????????????
            if (e.Key == Key.Return)
            {
                try
                {
                    this.Limit = ((TextBox)sender).Text;
                    MessageBox.Show("Column limit changed successfully", "Info");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Invalid Action");
                    ((TextBox)sender).Undo();
                    return false;
                }
            }
            return false;
        }


        internal void SearchBox_TextChanged(string senderText)
        {
            string txtOrig = senderText;
            string upper = txtOrig.ToUpper();
            string lower = txtOrig.ToLower();
            var tskFiltered = from Tsk in this.Tasks
                              let tTitle = Tsk.Title
                              let tDescription = Tsk.Description
                              where
                               tTitle.StartsWith(lower)
                               || tTitle.StartsWith(upper)
                               || tTitle.Contains(txtOrig)
                               || tDescription.StartsWith(upper)
                               || tDescription.StartsWith(lower)
                               || tDescription.Contains(txtOrig)
                              select Tsk;

            this.TasksToView = new ObservableCollection<TaskModel>(tskFiltered); //update tasks source
            RaisePropertyChanged("TasksToView");


        }

    }
}
