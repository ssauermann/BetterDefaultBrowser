using System;
using System.ComponentModel;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    class PlainFilterViewModel : CloseableViewModel, IDataErrorInfo
    {
        #region Fields

        private readonly PlainFilter _filter;
        private readonly ISettingsGateway _settings;
        private readonly IBrowserGateway _browsers;
        private RelayCommand _saveCommand;

        #endregion

        #region Constructor

        public PlainFilterViewModel(PlainFilter filter, ISettingsGateway settings, IBrowserGateway browsers)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _filter = filter;
            _settings = settings;
            _browsers = browsers;

            // Subscribe to browser list changed event.
            _browsers.PropertyChanged += (sender, args) => UpdateBrowsers();

            // Initialize browser list
            UpdateBrowsers();
        }
        #endregion

        #region PlainFilter Properties

        public string Regex
        {
            get { return _filter.Regex; }
            set
            {
                if (value == _filter.Regex)
                {
                    return;
                }
                _filter.Regex = value;
                OnPropertyChanged("Regex");
            }
        }

        public BrowserStorage Browser
        {
            get { return _filter.Browser; }
            set
            {
                if (value.Equals(_filter.Browser))
                {
                    return;
                }
                _filter.Browser = value;
                OnPropertyChanged("Browser");
            }
        }

        #endregion


        #region Presentation Properties

        public BindingList<BrowserStorage> AvailableBrowsers { get; } = new BindingList<BrowserStorage>();

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(
                           param => Save(),
                           param => CanSave
                       ));
            }
        }

        #endregion

        #region Public Methods

        public void Save()
        {
            var browser = _browsers.GetBrowser(Browser.BrowserKey);
            if (!_filter.IsValid || browser == null)
            {
                throw new InvalidOperationException("Can't save filter while it is invalid.");
            }

            // Update browser name with real name
            _filter.Browser.BrowserName = browser.Name;
            // Store filter
            _settings.UpdateOrAddFilter(_filter);
        }

        #endregion

        #region Private Methods

        private void UpdateBrowsers()
        {
            _browsers.InstalledBrowsers.ForEach(
                b => AvailableBrowsers.Add(
                    new BrowserStorage { BrowserKey = b.Key, BrowserName = b.Name }
                ));
            // OnPropertyChanged("AvailableBrowsers"); // Should not be necessary
        }

        private bool CanSave => ValidateBrowser() == null && _filter.IsValid;

        #endregion

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error => (_filter as IDataErrorInfo).Error;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                var error = propertyName == "Browser" ? ValidateBrowser() : (_filter as IDataErrorInfo)[propertyName];

                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        string ValidateBrowser()
        {
            var browser = _browsers.GetBrowser(Browser.BrowserKey);
            var error = browser == null ? null : "Selected browser is not available.";
            return error;
        }

        #endregion
    }
}
