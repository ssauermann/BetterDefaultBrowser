using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Gateways
{
    /// <summary>
    /// Base class for implementing the INotifyPropertyChanged interface.
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises a property changed event with the given property name.
        /// </summary>
        /// <param name="propertyName">Property that has changed</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler prop = this.PropertyChanged;
            if (prop != null)
            {
                prop(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
