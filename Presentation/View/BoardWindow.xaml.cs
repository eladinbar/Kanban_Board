using Presentation.ViewModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Presentation.Model;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
       
        private BoardViewModel viewModel;        
        public string CreatorEmail { get; private set; }
        private UserModel CurrentUser;
        private bool CanChangeSearchBox;


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! burn after reading ==>
        //private static BackendController controller = new BackendController();
        //private static string tempUser1Email = "maze1@mapo.com";
        //private static string tempPass = "123Abc";
        //private static string tempUser1Nick = "maze1Nick";
        //private static UserModel tempUserModel1 = new UserModel(controller, tempUser1Email, tempUser1Nick);

        //private static IService service = controller.Service;
        //private void CreateData() {
        //    MessageBoxResult result = MessageBox.Show("Wipe dataBase?", "Clear DataBase", MessageBoxButton.YesNo);
        //    switch (result)
        //    {
        //        case MessageBoxResult.Yes:
        //            service.DeleteData();
        //            break;
        //        case MessageBoxResult.No:
        //            break;
        //    }
        //    service.Register(tempUser1Email, tempPass, tempUser1Nick);
        //    string tempUser2Email = "maze2@mapo.com";
        //    service.Register(tempUser2Email, tempPass, tempUser1Nick, tempUser1Email);

        //    service.Login(tempUser1Email, tempPass);
        //    DateTime dTime = new DateTime(2030, 03, 26);
        //    Console.WriteLine("this is not the droids: {0}", service.AddTask(tempUser1Email, "title1", "desc1", dTime).ErrorOccured);
        //    service.AddTask(tempUser1Email, "title2", "desc2", dTime);
        //    service.AddTask(tempUser1Email, "title3", "desc3", dTime);
        //    service.AddTask(tempUser1Email, "title4", "desc4", dTime);
        //    service.AddTask(tempUser1Email, "title5", "desc5", dTime);
        //    service.AddTask(tempUser1Email, "title6", "desc6", dTime);

        //    service.AdvanceTask(tempUser1Email, 0, 1);
        //    service.AdvanceTask(tempUser1Email, 1, 1);

        //    service.AdvanceTask(tempUser1Email, 0, 2);
        //    service.AdvanceTask(tempUser1Email, 1, 2);

        //    service.AdvanceTask(tempUser1Email, 0, 3);
        //    service.AdvanceTask(tempUser1Email, 0, 4);
        //    service.AddColumn(tempUser1Email, 3, "added1");
        //    service.AddColumn(tempUser1Email, 4, "added2");
        //    service.AddColumn(tempUser1Email, 5, "added3");


        //}
        //  <== burn after reading!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!




        //public BoardWindow() // tha whats starting before merging with login window 
        //{
        //    this.CreateData();
        //    InitializeComponent();
        //    this.viewModel = new BoardViewModel(controller, tempUserModel1, tempUser1Email);
        //    this.DataContext = this.viewModel;
        //    this.CreatorEmail = tempUser1Email;
        //    this.CurrentUser = tempUserModel1;
        //    this.CanChangeSearchBox = false;
        //}

        public BoardWindow(BackendController controller, UserModel currentUser, string creatorEmail) 
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(controller, currentUser, creatorEmail);
            this.DataContext = this.viewModel;
            this.CreatorEmail = creatorEmail;
            this.CurrentUser = currentUser;
            this.CanChangeSearchBox = false;
        }


        public void SortTasksByDueDate_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SortTasksByDueDate((int)((Button)sender).Tag);
        }


        public void AddTask_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.AddTask();                        
        }


        public void EditTask_Click(object sender, RoutedEventArgs e) 
        {
            TaskModel taskToEdit = ((MenuItem)sender).DataContext as TaskModel;
            this.viewModel.EditTask(taskToEdit);

            //not so pretty!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            this.DataContext = null; //maybe to change the color of a border
            this.DataContext = this.viewModel;
        }


        public void RemoveTaskMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("               Remove this task? \n                    (Irreversible!)", "Remove Task", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    TaskModel taskToRemove = ((MenuItem)sender).DataContext as TaskModel;
                    this.viewModel.RemoveTask(taskToRemove);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }


        public void AdvanceTask_Click(object sender, RoutedEventArgs e) 
        {
            //not so pretty!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            MessageBoxResult result = MessageBox.Show("               Advance this task? \n                    (Irreversible!)", "Advance Task", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    TaskModel taskToAdvance = ((Button)sender).DataContext as TaskModel;
                    this.viewModel.AdvanceTask(taskToAdvance);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }


        public void LogoutVerificationMessageBox(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to leave? :( ", "Logout", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    this.viewModel.Logout();
                    MainWindow mw = new MainWindow(CurrentUser.Controller);
                    mw.Show();
                    this.Close();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }


        public void MoveColumnLeftClick(object sender, RoutedEventArgs e)
        {
            int columnOrdinal = ((int)((Button)sender).Tag);
            this.viewModel.MoveColumnLeft(CreatorEmail, columnOrdinal);            
        }


        public void MoveColumnRightClick(object sender, RoutedEventArgs e)
        {
            int columnOrdinal = ((int)((Button)sender).Tag);
            this.viewModel.MoveColumnRight(CreatorEmail, columnOrdinal);
        }


        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Remove this column?", "Remove Column", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Button currentButton = ((Button)sender);
                    int columnOrdinal = (int)currentButton.Tag;
                    this.viewModel.RemoveColumn(CreatorEmail, columnOrdinal);
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }


        private void ColumnName_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox currentTextBox = ((TextBox)sender);
            int columnOrdinal = (int)currentTextBox.Tag;
            if (this.viewModel.Board.Columns.ElementAt(columnOrdinal).OnKeyDownHandlerName(sender, e))
            {
                currentTextBox.IsUndoEnabled = false;
                currentTextBox.IsUndoEnabled = true;
                Keyboard.ClearFocus();
            }
        }


        private void ColumnName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = ((TextBox)sender);
            if (currentTextBox.CanUndo == true)
            {
                currentTextBox.Undo();
                currentTextBox.IsUndoEnabled = false;
                currentTextBox.IsUndoEnabled = true;
            }
        }


        private void AddColumnToTheRightMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem currentMenuItem = ((MenuItem)sender);
            int columnOrdinal = (int)currentMenuItem.Tag;
            this.viewModel.AddColumn(CreatorEmail, columnOrdinal + 1);
        }


        private void AddColumnToTheLeftMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem currentMenuItem = ((MenuItem)sender);
            int columnOrdinal = (int)currentMenuItem.Tag;
            this.viewModel.AddColumn(CreatorEmail, columnOrdinal);            
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.viewModel != null & this.CanChangeSearchBox) this.viewModel.SearchBox_TextChanged(((TextBox)sender).Text);
        }


        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Text = "";
            this.SearchBox.Foreground = Brushes.Black;
            this.CanChangeSearchBox = true;
        }


        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.viewModel.SearchBox_TextChanged("");
            this.CanChangeSearchBox = false;
            this.SearchBox.Foreground = Brushes.Gray;
            this.SearchBox.Text = "Search for a task...";
        }

        private void LimitOfTasks_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox currentTextBox = ((TextBox)sender);
            int columnOrdinal = (int)currentTextBox.Tag;
            if (this.viewModel.Board.Columns.ElementAt(columnOrdinal).OnKeyDownHandlerLimit(sender, e))
            {
                currentTextBox.IsUndoEnabled = false;
                currentTextBox.IsUndoEnabled = true;
                Keyboard.ClearFocus();
            }
        }

        private void LimitOfTasks_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = ((TextBox)sender);
            if (currentTextBox.CanUndo == true)
            {
                currentTextBox.Undo();
                currentTextBox.IsUndoEnabled = false;
                currentTextBox.IsUndoEnabled = true;
            }
        }






        /*private void EditTaskButton_Click((object sender, RoutedEventArgs e){
          TaskWindow tW = new TaskWindow (controller, currentUser, creatorEmail);
          tW.ShowDialog();
        }
        */
    }
}
