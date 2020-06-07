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
            window = new TaskWindow(new TaskModel(new BackendController(), 1, "title", "description",
            DateTime.Now, DateTime.UtcNow, DateTime.Now, "email", 0), 0, true);
            window.ShowDialog();
        }
    }
}
