using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class BoardViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        public BoardViewModel(Model.UserModel currentUser)
        {
            this.Controller = new BackendController(); //need to receive from MainWindow (= LoginWindow)
        }
    }
}
