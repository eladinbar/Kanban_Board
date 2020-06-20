using System;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Presentation.Model
{
    /// <summary>
    /// The model that represents a column of the Board. Used by the board window.
    /// </summary>
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


        /// <summary>
        /// Column model constructor.
        /// </summary>
        /// <param name="controller">The controller this model uses to communicate with the backend.</param>
        /// <param name="tasks">An ObservableCollection of TaskModels.</param>
        /// <param name="limit">Column limit of the tasks.</param>
        /// <param name="name">Column name.</param>
        /// <param name="ordinal">Column ordinal in the board.</param>
        /// <param name="creatorEmail">Current board creator email.</param>
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

        /// <summary>
        /// Updates the viewable content of the tasks in the GUI.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a change of the collection.</param>
        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.TasksToView = new ObservableCollection<TaskModel>(this.Tasks);
            RaisePropertyChanged("TasksToView");
        }

        /// <summary>
        /// Raises a property of the current class from another class.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        public void RaiseProperty(string propertyName)
        {
            RaisePropertyChanged(propertyName);
        }


        /// <summary>
        /// Tracks which keys were pressed while the focus was on 'ColumnName' header property (TextBox). Updates column name accordingly.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a key event.</param>
        /// <returns>Returns 'true' if the 'Enter' key was pressed. Otherwise returns 'false'.</returns>       
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


        /// <summary>
        /// Tracks which keys were pressed while the focus was on 'ColumnLimit' property (TextBox). Updates column limit accordingly.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a key event.</param>
        /// <returns>Returns 'true' if the 'Enter' key was pressed. Otherwise returns 'false'.</returns>  
        internal bool OnKeyDownHandlerLimit(object sender, KeyEventArgs e)
        {
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

        /// <summary>
        /// Updates the GUI content of the columns accordingly to the search box input.
        /// </summary>
        /// <param name="senderText">The input of the search box.</param>
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
