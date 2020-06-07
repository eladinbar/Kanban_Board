using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using Presentation.View;
namespace Presentation
{
    class WannaBeLoginWindow
    {

        [STAThread]
        public static void Main(string[] args)
        {
            BackendController controller = new BackendController();

            string tempUser1Email = "maze1@mapo.com";
            string tempPass = "123Abc";
            string tempUser1Nick = "maze1Nick";

            IService service = controller.Service;
            service.Register(tempUser1Email, tempPass, tempUser1Nick);
            service.Login(tempUser1Email, tempPass);
            DateTime dTime = new DateTime(2035, 03, 26);
            Console.WriteLine("this is not the droids: {0}", service.AddTask(tempUser1Email, "title1", "desc1", dTime).ErrorOccured);
            service.AddTask(tempUser1Email, "title2", "desc2", dTime);
            service.AddTask(tempUser1Email, "title3", "desc3", dTime);
            service.AddTask(tempUser1Email, "title4", "desc4", dTime);
            service.AddTask(tempUser1Email, "title5", "desc5", dTime);
            service.AddTask(tempUser1Email, "title6", "desc6", dTime);
            //service.AdvanceTask(tempUser1Email, 0,);


            UserModel tempUserModel1 = new UserModel(controller, tempUser1Email, tempUser1Nick);
            BoardWindow boardWindow = new BoardWindow(controller, tempUserModel1, tempUser1Email);
            boardWindow.ShowDialog();

        }

    }
}
