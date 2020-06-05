using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Presentation.Model
{
    class BoardModel : NotifiableModelObject
    {
        private UserModel CurrentUser { get; set; } //readonly???

        public ObservableCollection<ColumnModel> Columns { get; set; }

        public BoardModel(BackendController controller, UserModel currentUser) : base(controller)
        {
            this.CurrentUser = currentUser;
            this.Columns = new ObservableCollection<ColumnModel>(this.Controller.GetColumns())
        }
    }
}
