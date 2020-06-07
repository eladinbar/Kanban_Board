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
        public int Limit { get; private set; }
        public string Name { get; private set; }
        public int Ordinal { get; private set; }

        public ColumnModel(ObservableCollection<TaskModel> tasks, int limit, string name, int ordinal, ObservableCollection<SimpleTaskModel> simpleTasks)
        {
            this.Tasks = tasks;
            this.Limit = limit;
            this.Name = name;
            this.Ordinal = ordinal;
            this.SimpleTasks = simpleTasks;
        }
    }
}
