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

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        private BoardViewModel viewModel;

        private string CreatorEmail;

        private ObservableCollection<ColumnModel> _columns; //???????????????????????????
        public ObservableCollection<ColumnModel> Columns //???????????????????????????
        {
            get => _columns;
            set => _columns = value;
        }

        public BoardWindow(BackendController controller, UserModel currentUser, string creatorEmail) //need to receive from login window those parameters
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(controller, currentUser, creatorEmail);
            this.DataContext = this.viewModel;
            this.CreatorEmail = creatorEmail;
            //this._columns
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBox_TextChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
