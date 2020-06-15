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

        private int _limit; //maybe make it a property which will appear at the bottom part of a columnModel and will be a changable box????????????????
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
                try
                {
                    this.Controller.ChangeColumnName(CreatorEmail, Ordinal, value);
                    _name = value;
                    RaisePropertyChanged("Name");
                    MessageBox.Show("Column name changed successfully", "Info");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Invalid Action");
                }
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


        public bool OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Name = ((TextBox)sender).Text;
                return true;
            }
            return false;
        }
    }
}
