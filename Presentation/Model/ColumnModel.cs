using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    class ColumnModel
    {
        public ObservableCollection<TaskModel> Tasks;
        public ObservableCollection<SimpleTaskModel> SimpleTasks; //minimalistic tasks models list
        public int Limit;
        public string Name;
        public int Ordinal;

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
