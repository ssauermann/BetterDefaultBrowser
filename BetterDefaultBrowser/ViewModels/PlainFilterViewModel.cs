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
    class PlainFilterViewModel : FilterViewModelBase
    {
        private PlainFilter pFilter;
        private BrowserViewModel browser;
        private BrowserListViewModel browserList;
        private bool validRegex = true;
        private String regex = ".*";

        public PlainFilterViewModel() : base(new PlainFilter { Name = "Unnamed filter", RegEx = ".*" })
        {
            pFilter = (PlainFilter)filter;
            browser = new BrowserViewModel("");
            browserList = new BrowserListViewModel();
        }

        /// <summary>
        /// When Using a managed Filter this constructor should be invoked
        /// </summary>
        public PlainFilterViewModel(ManagedFilter mFilter) : base(mFilter)
        {
            browser = new BrowserViewModel("");
            browserList = new BrowserListViewModel();
        }


        #region Properties


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
                        pFilter.RegEx = value;
                        IsValidRegEx = true;
                    }
                    else
                    {
                        IsValidRegEx = false;
                    }
                    OnPropertyChanged("RegEx");
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
                    OnPropertyChanged("IsValidRegEx");
                    OnPropertyChanged("RegExBackground");
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
                    OnPropertyChanged("Browser");
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


        #region Commands
        void StoreFilterExecute()
        {
            //Assuming browser is valid
            pFilter.AssignedBrowser = browser.Browser;
            pFilter.Store();
        }

        bool CanStoreFilterExecute()
        {
            return Name != "" && IsValidRegEx && Browser.IsAvailable;
        }

        public ICommand StoreFilter { get { return new RelayCommand(StoreFilterExecute, CanStoreFilterExecute); } }
        #endregion
    }
}
