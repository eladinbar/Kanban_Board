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

        /// <summary>
        /// A Board window constructor for an existing board. Initializes the Data Context view model.
        /// </summary>
        /// <param name="controller">The controller this constructor uses to initialize the Data Context with it.</param>
        /// <param name="currentUser">The current loged in user.</param>
        /// <param name="creatorEmail">An email of current board creator.</param>
        public BoardWindow(BackendController controller, UserModel currentUser, string creatorEmail) 
        {
            InitializeComponent();
            this.viewModel = new BoardViewModel(controller, currentUser, creatorEmail);
            this.DataContext = this.viewModel;
            this.CreatorEmail = creatorEmail;
            this.CurrentUser = currentUser;
            this.CanChangeSearchBox = false;
        }

        /// <summary>
        /// Allows to sort the tasks by due date in the selected column.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void SortTasksByDueDate_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SortTasksByDueDate((int)((Button)sender).Tag);
        }

        /// <summary>
        /// Allows to add new task to the current board.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void AddTask_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.AddTask();                        
        }

        /// <summary>
        /// Allows to edit the selected task.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void EditTask_Click(object sender, RoutedEventArgs e) 
        {
            TaskModel taskToEdit = ((MenuItem)sender).DataContext as TaskModel;
            this.viewModel.EditTask(taskToEdit);
            this.DataContext = null; 
            this.DataContext = this.viewModel;
        }

        /// <summary>
        /// Allows to remove the selected task. Validates user's request with an Yes/No message box.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
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


        /// <summary>
        /// Allows to advance the selected task. Validates user request with an Yes/No message box.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void AdvanceTask_Click(object sender, RoutedEventArgs e) 
        {
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

        /// <summary>
        /// Allows to logout current user from the system. Validates user request with an Yes/No message box.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
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

        /// <summary>
        /// Allows to move the selected column to its left.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void MoveColumnLeftClick(object sender, RoutedEventArgs e)
        {
            int columnOrdinal = ((int)((Button)sender).Tag);
            this.viewModel.MoveColumnLeft(CreatorEmail, columnOrdinal);            
        }


        /// <summary>
        /// Allows to move the selected column to its right.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        public void MoveColumnRightClick(object sender, RoutedEventArgs e)
        {
            int columnOrdinal = ((int)((Button)sender).Tag);
            this.viewModel.MoveColumnRight(CreatorEmail, columnOrdinal);
        }


        /// <summary>
        /// Allows to remove the selected column from current board without removing its content (tasks). Validates user request with an Yes/No message box.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
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


        /// <summary>
        /// Tracks which keys were pressed while the focus was on 'ColumnName' header property (TextBox). 
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a key event.</param>
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

        /// <summary>
        /// Tracks the focus changes on 'ColumnName' property (TextBox). 
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a key event.</param>
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


        /// <summary>
        /// Allows to add new column to the current board at the demanded index.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void AddColumnToTheRightMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem currentMenuItem = ((MenuItem)sender);
            int columnOrdinal = (int)currentMenuItem.Tag;
            this.viewModel.AddColumn(CreatorEmail, columnOrdinal + 1);
        }


        /// <summary>
        /// Allows to add new column to the current board at the demanded index.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void AddColumnToTheLeftMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem currentMenuItem = ((MenuItem)sender);
            int columnOrdinal = (int)currentMenuItem.Tag;
            this.viewModel.AddColumn(CreatorEmail, columnOrdinal);            
        }


        /// <summary>
        /// Part of the 'search for task' logic: transfers the input from the search box to the Data Context.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Provides data for the TextBoxBase.TextChanged event.</param>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.viewModel != null & this.CanChangeSearchBox) this.viewModel.SearchBox_TextChanged(((TextBox)sender).Text);
        }


        /// <summary>
        /// Part of the 'search for task' logic: track the focus changes on 'SearchBox' property (TextBox).
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Text = "";
            this.SearchBox.Foreground = Brushes.Black;
            this.CanChangeSearchBox = true;
        }


        /// <summary>
        /// Part of the 'search for task' logic: track the focus changes on 'SearchBox' property (TextBox).
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.viewModel.SearchBox_TextChanged("");
            this.CanChangeSearchBox = false;
            this.SearchBox.Foreground = Brushes.Gray;
            this.SearchBox.Text = "Search for a task...";
        }


        /// <summary>
        /// Part of changing the column limit logic: tracks which keys were pressed while the focus was on 'ColumnLimit' property (TextBox).
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a key event.</param>
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


        /// <summary>
        /// Part of changing the column limit logic: track the focus changes on 'ColumnLimit' property (TextBox).
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
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
    }
}
