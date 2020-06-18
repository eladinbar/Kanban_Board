using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using Presentation.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TaskWindow window;
        BackendController Controller;
        
        //Mock MainWindow for testing only
        public MainWindow()
        {
            InitializeComponent();
            this.Controller = new BackendController();
            UserModel user = new UserModel(Controller, "username", "nickname");
            window = null;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            //if (window.Closed())
            BackendController controller = new BackendController();
            TaskModel task = new TaskModel(controller, 1, "title", "description", DateTime.Now, new DateTime(2021, 2, 1), DateTime.Now, "email", 0);
            //window = new TaskWindow(controller, task, 0, true); //edit
            window = new TaskWindow(controller, "email"); //add
            window.ShowDialog();
        }
    }
}
