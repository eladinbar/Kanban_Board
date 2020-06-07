using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    public class ColumnModel
    {
        public ObservableCollection<TaskModel> Tasks;
        public ObservableCollection<SimpleTaskModel> SimpleTasks; //minimalistic tasks models list
        public int Limit { get; private set; } //maybe make it a property which will appear at the bottom part of a columnModel and will be a changable box????????????????
        public string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Ordinal { get; private set; }

        public ColumnModel(ObservableCollection<TaskModel> tasks, int limit, string name, int ordinal, ObservableCollection<SimpleTaskModel> simpleTasks)
        {
            this.Tasks = tasks;
            this.Limit = limit;
            this._name = name;
            this.Ordinal = ordinal;
            this.SimpleTasks = simpleTasks;
        }
    }
}
