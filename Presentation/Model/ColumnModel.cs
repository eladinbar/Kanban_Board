using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }
        public string CreatorEmail;
        public int Limit { get; private set; } //maybe make it a property which will appear at the bottom part of a columnModel and will be a changable box????????????????
        public string _name;      
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                this.Controller.ChangeColumnName(CreatorEmail, Ordinal, _name);
            }
        }
        public int Ordinal { get; private set; }

        public ColumnModel(BackendController controller, ObservableCollection<TaskModel> tasks, int limit, string name, int ordinal, string creatorEmail) : base(controller)
        {
            this.Tasks = tasks;
            this.Limit = limit;
            this._name = name;
            this.Ordinal = ordinal;
            this.CreatorEmail = creatorEmail;
        }
    }
}
