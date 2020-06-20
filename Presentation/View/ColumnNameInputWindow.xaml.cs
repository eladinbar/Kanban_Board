using System;
using System.Windows;

namespace Presentation.View
{
    /// <summary>
    /// Represents InputDialog window to receive an input from the user.
    /// </summary>
    public partial class InputDialog : Window
    {

        public string Answer
        {
            get { return txtAnswer.Text; }
        }

        /// <summary>
        /// InputDialog constructor. Invokes current window.
        /// </summary>
        /// <param name="question">An message represented to the user.</param>
        /// <param name="defaultAnswer">The default answer in the input box.</param>
        public InputDialog(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
        }

        /// <summary>
        /// Tracks the interaction with the 'Ok' button of this window.
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        /// <summary>
        /// Allows the user to instantly overwrite the answer provided by default
        /// </summary>
        /// <param name="sender">The object that invoked the event and fired the event handler.</param>
        /// <param name="e">Contains state information and event data associated with the event.</param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

    }
}