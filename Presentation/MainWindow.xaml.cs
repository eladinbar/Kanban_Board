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
        
        //Mock MainWindow for testing only
        public MainWindow()
        {
            InitializeComponent();
            UserModel user = new UserModel("username", "password", "nickname");
            window = new TaskWindow(new TaskModel(new BackendController(new Service()), 1, "title", "description",
            DateTime.Now, DateTime.UtcNow, DateTime.Now, user), user);


        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            window.ShowDialog();
        }
    }
}
