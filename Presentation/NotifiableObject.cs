using System.ComponentModel;

namespace Presentation
{
    public class NotifiableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Notifies the specified property of changes made to it.
        /// </summary>
        /// <param name="property">The name of the property that is specified as changed.</param>
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
