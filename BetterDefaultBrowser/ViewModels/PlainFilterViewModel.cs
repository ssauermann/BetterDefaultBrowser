using BetterDefaultBrowser.Commands;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BetterDefaultBrowser.ViewModels
{
    class PlainFilterViewModel
    {
        private PlainFilter filter;
        private BrowserViewModel browser;
        private BrowserListViewModel browserList;
        private bool validRegex = true;
        private String regex = ".*";

        public PlainFilterViewModel()
        {
            this.filter = new PlainFilter { Name = "Unnamed filter", RegEx = ".*" };
            browser = new BrowserViewModel("");
            browserList = new BrowserListViewModel();
        }


        #region Properties
        public String Name
        {
            get
            {
                return filter.Name;
            }
            set
            {
                if (filter.Name != value)
                {
                    filter.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Set a regular expression for filtering requests.
        /// </summary>
        public String RegEx
        {
            get
            {
                return regex;
            }
            set
            {
                if (regex != value)
                {
                    regex = value;
                    if (PlainFilter.IsValidRegex(value))
                    {
                        filter.RegEx = value;
                        IsValidRegEx = true;
                    }
                    else
                    {
                        IsValidRegEx = false;
                    }
                    RaisePropertyChanged("RegEx");
                }
            }
        }

        public bool IsValidRegEx
        {
            get
            {
                return validRegex;
            }
            set
            {
                if (IsValidRegEx != value)
                {
                    validRegex = value;
                    RaisePropertyChanged("IsValidRegEx");
                    RaisePropertyChanged("RegExBackground");
                }
            }
        }

        public SolidColorBrush RegExBackground
        {
            get
            {
                if (IsValidRegEx)
                {
                    return Brushes.White;
                }
                else
                {
                    return Brushes.IndianRed;
                }
            }
        }


        /// <summary>
        /// Assigned browser for this filter.
        /// </summary>
        public BrowserViewModel Browser
        {
            get
            {
                return browser;
            }
            set
            {
                if (!browser.Equals(value))
                {
                    this.browser = value;
                    RaisePropertyChanged("Browser");
                }
            }
        }

        public BindingList<BrowserViewModel> BrowserList
        {
            get
            {
                return browserList.Browsers;
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


        #region Commands
        void StoreFilterExecute()
        {
            //Assuming browser is valid
            filter.AssignedBrowser = browser.Browser;
            filter.Store();
        }

        bool CanStoreFilterExecute()
        {
            return Name != "" && IsValidRegEx && Browser.IsAvailable;
        }

        public ICommand StoreFilter { get { return new RelayCommand(StoreFilterExecute, CanStoreFilterExecute); } }
        #endregion
    }
}
