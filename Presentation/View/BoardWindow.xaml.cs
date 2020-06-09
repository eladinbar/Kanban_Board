using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.IO;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardViewModel viewModel;

        public string CreatorEmail { get; private set; }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! burn after reading ==>
        private static BackendController controller = new BackendController();
        private static string tempUser1Email = "maze1@mapo.com";
        private static string tempPass = "123Abc";
        private static string tempUser1Nick = "maze1Nick";
        private static UserModel tempUserModel1 = new UserModel(controller, tempUser1Email, tempUser1Nick);

        private static IService service = controller.Service;
        private void CreateData() {
            MessageBoxResult result = MessageBox.Show("Wipe dataBase?", "Clear DataBase", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    service.DeleteData();
                    break;
                case MessageBoxResult.No:
                    break;
            }
            service.Register(tempUser1Email, tempPass, tempUser1Nick);
            service.Login(tempUser1Email, tempPass);
            DateTime dTime = new DateTime(2030, 03, 26);
            Console.WriteLine("this is not the droids: {0}", service.AddTask(tempUser1Email, "title1", "desc1", dTime).ErrorOccured);
            service.AddTask(tempUser1Email, "title2", "desc2", dTime);
            service.AddTask(tempUser1Email, "title3", "desc3", dTime);
            service.AddTask(tempUser1Email, "title4", "desc4", dTime);
            service.AddTask(tempUser1Email, "title5", "desc5", dTime);
            service.AddTask(tempUser1Email, "title6", "desc6", dTime);

            service.AdvanceTask(tempUser1Email, 0, 1);
            service.AdvanceTask(tempUser1Email, 1, 1);

            service.AdvanceTask(tempUser1Email, 0, 2);
            service.AdvanceTask(tempUser1Email, 1, 2);

            service.AdvanceTask(tempUser1Email, 0, 3);
            service.AdvanceTask(tempUser1Email, 0, 4);
        }
        //  <== burn after reading!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!




        public BoardWindow() 
        {
            this.CreateData();
            InitializeComponent();
            this.viewModel = new BoardViewModel(controller, tempUserModel1, tempUser1Email);
            this.DataContext = this.viewModel;
            this.CreatorEmail = tempUser1Email;
        }

        public BoardWindow(BackendController controller, UserModel currentUser, string creatorEmail) //need to receive from login window those parameters
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(controller, currentUser, creatorEmail);
            this.DataContext = this.viewModel;
            this.CreatorEmail = creatorEmail;            
        }


        public void LogoutVerificationMessageBox(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to leave? :( ", "Logout", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.viewModel.Logout();
                    //LoginWindow invoke()
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        public void ChangePassword(object sender, RoutedEventArgs e)
        {
            this.viewModel.ChangePassword();
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }

        /*private void EditTaskButton_Click((object sender, RoutedEventArgs e){
          TaskWindow tW = new TaskWindow (controller, currentUser, creatorEmail);
          tW.ShowDialog();
        }
        */
    }
}
