using System.ComponentModel;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
