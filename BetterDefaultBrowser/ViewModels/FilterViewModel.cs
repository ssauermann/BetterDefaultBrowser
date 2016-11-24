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

            // Unsaved changes * when property changed:
            PropertyChanged += (sender, args) => { if (args.PropertyName != nameof(DisplayName)) UnsavedChanges = true; };
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
                DisplayName = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(DisplayName));
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
                OnPropertyChanged(nameof(Priority));
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
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        #endregion

        #region Unsaved changes

        private bool _unsavedChanges;
        private bool UnsavedChanges
        {
            get { return _unsavedChanges; }
            set
            {
                if (_unsavedChanges != value)
                {
                    _unsavedChanges = value;
                    OnPropertyChanged(nameof(DisplayName));
                }
            }
        }

        // Override display name so no converter from bool to * is needed.
        public override string DisplayName
        {
            get
            {
                if (UnsavedChanges)
                {
                    return base.DisplayName + "*";
                }
                return base.DisplayName;
            }
            protected set { base.DisplayName = value; }
        }

        #endregion

        #region Command
        public ICommand SaveCommand
        {
            get
            {
                return SaveCmd ?? (SaveCmd = new RelayCommand(
                           param => DoSave(),
                           param => CanSave
                       ));
            }
        }

        private void DoSave()
        {
            Save();
            UnsavedChanges = false;
        }

        public abstract void Save();
        protected abstract bool CanSave { get; }
        #endregion
    }
}
