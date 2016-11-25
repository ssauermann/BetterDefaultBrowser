using System;
using System.ComponentModel;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    public class PlainFilterViewModelT<T> : FilterViewModel<T> where T : PlainFilter
    {
        #region Constructor

        public PlainFilterViewModelT(T filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway) : base(filter, settingsGateway, browserGateway)
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
                OnPropertyChanged(nameof(Regex));
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
                OnPropertyChanged(nameof(Browser));
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
            // OnPropertyChanged(nameof(AvailableBrowsers)); // Should not be necessary
        }

        protected override bool CanSave => ValidateBrowser() == null && Filter.IsValid;

        #endregion

        #region Validation

        protected override string ValidateMe(string property)
        {
            return property == "Browser" ? ValidateBrowser() : null;
        }

        string ValidateBrowser()
        {
            //Validate browser storage not null
            var innerError = (Filter as IDataErrorInfo)[nameof(Filter.Browser)];
            if (innerError != null)
            {
                return innerError;
            }

            // Validate browser is installed
            var browser = BrowserGateway.GetBrowser(Browser?.BrowserKey);
            var error = browser != null ? null : "Selected browser is not available on this system.";
            return error;
        }

        #endregion
    }
}
