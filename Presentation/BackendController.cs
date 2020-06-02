using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Presentation
{
    public class BackendController
    {
        private IService Service { get; set; }
        public BackendController(IService service)
        {
            Service = service;
        }
    }
}
