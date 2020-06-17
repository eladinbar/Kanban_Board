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
                try
                {
                    this.Controller.LimitColumnTasks(CreatorEmail, Ordinal, Int32.Parse(value));
                    this._limit = Int32.Parse(value);
                    RaisePropertyChanged("Limit");
                    MessageBox.Show("Column limit changed successfully", "Info");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Invalid Action");
                }

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
        }




        //doesn work properklyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy!~!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public bool OnKeyUpHandler(object sender, KeyEventArgs e) //column name changes
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
                    return false;
                }
            }
            return false;                       
        }
    }
}
