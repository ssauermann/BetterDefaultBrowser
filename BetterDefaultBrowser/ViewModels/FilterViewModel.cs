using System;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    abstract class FilterViewModel<T> : CloseableViewModel where T : Filter
    {
        #region Fields
        protected readonly T Filter;
        protected readonly ISettingsGateway SettingsGateway;
        protected readonly IBrowserGateway BrowserGateway;
        protected RelayCommand SaveCmd;
        #endregion

        #region Constructor
        protected FilterViewModel(T filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            if (settingsGateway == null)
            {
                throw new ArgumentNullException(nameof(settingsGateway));
            }
            if (browserGateway == null)
            {
                throw new ArgumentNullException(nameof(browserGateway));
            }

            Filter = filter;
            SettingsGateway = settingsGateway;
            BrowserGateway = browserGateway;
        }
        #endregion

        #region Filter properties

        public string Name
        {
            get { return Filter.Name; }
            set
            {
                if (value.Equals(Filter.Name))
                {
                    return;
                }
                Filter.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Priority
        {
            get { return Filter.Priority; }
            set
            {
                if (value.Equals(Filter.Priority))
                {
                    return;
                }
                Filter.Priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public bool IsEnabled
        {
            get { return Filter.IsEnabled; }
            set
            {
                if (value.Equals(Filter.IsEnabled))
                {
                    return;
                }
                Filter.IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        #endregion

        #region Command
        public ICommand SaveCommand
        {
            get
            {
                return SaveCmd ?? (SaveCmd = new RelayCommand(
                           param => Save(),
                           param => CanSave
                       ));
            }
        }

        public abstract void Save();
        protected abstract bool CanSave { get; }
        #endregion
    }
}
