using BetterDefaultBrowser.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.ViewModels
{
    class BrowserListViewModel : INotifyPropertyChanged
    {
        #region Construction
        public BrowserListViewModel()
        {
            //TEST
            browsers.Clear();
            foreach (var b in AllBrowsers.InstalledBrowsers)
            {
                browsers.Add(new BrowserViewModel(b.KeyName));
            }
        }
        #endregion

        #region Members
        private static BindingList<BrowserViewModel> browsers = new BindingList<BrowserViewModel>();
        #endregion

        #region Properties
        public BindingList<BrowserViewModel> Browsers
        {
            get
            {
                return browsers;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
