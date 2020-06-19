using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class NotifiableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
                
        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        protected void RaisePropertyChanging(string property)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(property));
        }

    }
}
