using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    class MainWindowViewModel : CloseableViewModel
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
            Tabs.Add(new PlainFilterViewModel(new PlainFilter(), _settingsGateway, _browserGateway));
            Tabs.Add(new PlainFilterViewModel(new PlainFilter(), _settingsGateway, _browserGateway));
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
