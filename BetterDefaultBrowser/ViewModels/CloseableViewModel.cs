using System;
using System.Windows.Input;

namespace BetterDefaultBrowser.ViewModels
{
    public abstract class CloseableViewModel : ViewModelBase
    {
        #region Fields

        RelayCommand _closeCommand;

        #endregion

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(param => OnRequestClose())); }
        }

        #endregion

        #region RequestClose [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    }
}
