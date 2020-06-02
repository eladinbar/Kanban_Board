using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Modal
{
    public class NotifiableModalObject : NotifiableObject
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModalObject(BackendController controller)
        {
            Controller = controller;
        }
    }
}
