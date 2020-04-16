using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend;

namespace Tests
{
    [TestClass]
    class LoadDataTest
    {
        IntroSE.Kanban.Backend.ServiceLayer.Service _service = new IntroSE.Kanban.Backend.ServiceLayer.Service();

        public LoadDataTest()
        {
        }

        [TestMethod]
        public void LoadData()
        {
            IntroSE.Kanban.Backend.ServiceLayer.Response rsp =_service.LoadData();
            Assert.AreEqual("The data was loaded successfully.", rsp.ErrorMessage);
        }
    }
}
