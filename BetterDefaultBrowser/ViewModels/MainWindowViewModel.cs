using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;

namespace BetterDefaultBrowser.ViewModels
{
    public class MainWindowViewModel : CloseableViewModel
    {
        #region Fields

        private readonly ISettingsGateway _settingsGateway;
        private readonly IBrowserGateway _browserGateway;

        #endregion

        #region Constructor

        public MainWindowViewModel(string settingsFile)
        {
            _settingsGateway = new SettingsGateway(settingsFile);
            _browserGateway = BrowserGateway.Instance;

            // Register event listener when tabs are added or removed
            Tabs.CollectionChanged += OnTabsChanged;

            //TODO: Remove; Just for testing
            foreach (var filter in _settingsGateway.GetFilters())
            {
                if (typeof(ManagedFilter) == filter.GetType())
                {
                    Tabs.Add(new ManagedFilterViewModel((ManagedFilter)filter, _settingsGateway, _browserGateway));
                }
                if (typeof(PlainFilter) == filter.GetType())
                {
                    Tabs.Add(new PlainFilterViewModel((PlainFilter)filter, _settingsGateway, _browserGateway));
                }
                else if (typeof(OpenFilter) == filter.GetType())
                {
                    //Tabs.Add(new OpenFilterViewModel((OpenFilter)filter, _settingsGateway, _browserGateway));
                }
                else
                {
                    Debug.Fail("Implementation missing for a filter type.");
                }
            }

            Tabs.Add(new ManagedFilterViewModel(new ManagedFilter() { Flags = Ignore.Page }, _settingsGateway, _browserGateway));
        }

        #endregion

        #region Display properties



        #endregion

        #region Tabs

        public ObservableCollection<CloseableViewModel> Tabs { get; } =
            new ObservableCollection<CloseableViewModel>();

        private void OnTabsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (CloseableViewModel tab in e.NewItems)
                    tab.RequestClose += this.OnTabRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (CloseableViewModel tab in e.OldItems)
                    tab.RequestClose -= this.OnTabRequestClose;
        }

        void OnTabRequestClose(object sender, EventArgs e)
        {
            CloseableViewModel tab = sender as CloseableViewModel;
            tab?.Dispose();
            Tabs.Remove(tab);
        }
        #endregion


    }
}
