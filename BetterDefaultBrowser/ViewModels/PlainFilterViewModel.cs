using System;
using System.ComponentModel;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    class PlainFilterViewModel : FilterViewModel<PlainFilter>, IDataErrorInfo
    {
        #region Constructor

        public PlainFilterViewModel(PlainFilter filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway) : base(filter, settingsGateway, browserGateway)
        {
            // Subscribe to browser list changed event.
            BrowserGateway.PropertyChanged += (sender, args) => UpdateBrowsers();

            // Initialize browser list
            UpdateBrowsers();
        }
        #endregion

        #region PlainFilter Properties

        public string Regex
        {
            get { return Filter.Regex; }
            set
            {
                if (value == Filter.Regex)
                {
                    return;
                }
                Filter.Regex = value;
                OnPropertyChanged("Regex");
            }
        }

        public BrowserStorage Browser
        {
            get { return Filter.Browser; }
            set
            {
                if (value.Equals(Filter.Browser))
                {
                    return;
                }
                Filter.Browser = value;
                OnPropertyChanged("Browser");
            }
        }

        #endregion


        #region Presentation Properties

        public BindingList<BrowserStorage> AvailableBrowsers { get; } = new BindingList<BrowserStorage>();

        #endregion

        #region Public Methods

        public override void Save()
        {
            var browser = BrowserGateway.GetBrowser(Browser.BrowserKey);
            if (!Filter.IsValid || browser == null)
            {
                throw new InvalidOperationException("Can't save filter while it is invalid.");
            }

            // Update browser name with real name
            Filter.Browser.BrowserName = browser.Name;
            // Store filter
            SettingsGateway.UpdateOrAddFilter(Filter);
        }

        #endregion

        #region Private Methods

        private void UpdateBrowsers()
        {
            BrowserGateway.InstalledBrowsers.ForEach(
                b => AvailableBrowsers.Add(
                    new BrowserStorage { BrowserKey = b.Key, BrowserName = b.Name }
                ));
            // OnPropertyChanged("AvailableBrowsers"); // Should not be necessary
        }

        protected override bool CanSave => ValidateBrowser() == null && Filter.IsValid;

        #endregion

        #region IDataErrorInfo Members
        string IDataErrorInfo.Error => (Filter as IDataErrorInfo).Error;

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                var error = propertyName == "Browser" ? ValidateBrowser() : (Filter as IDataErrorInfo)[propertyName];

                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }

        string ValidateBrowser()
        {
            var browser = BrowserGateway.GetBrowser(Browser.BrowserKey);
            var error = browser == null ? null : "Selected browser is not available.";
            return error;
        }

        #endregion
    }
}
