using System;
using System.ComponentModel;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    public abstract class FilterViewModel<T> : CloseableViewModel, IDataErrorInfo where T : Filter
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
            DisplayName = Name;
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
        public sealed override string DisplayName
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
            PrepareSave();
            SettingsGateway.UpdateOrAddFilter(Filter);
            UnsavedChanges = false;
        }

        public abstract void PrepareSave();
        protected abstract bool CanSave { get; }
        #endregion

        #region IDataErrorInfo Members
        string IDataErrorInfo.Error => (Filter as IDataErrorInfo).Error;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                var error = ValidateMe(propertyName);
                if (error != null)
                {
                    return error;
                }

                error = (Filter as IDataErrorInfo)[propertyName];

                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        protected abstract string ValidateMe(string property);

        #endregion
    }
}
