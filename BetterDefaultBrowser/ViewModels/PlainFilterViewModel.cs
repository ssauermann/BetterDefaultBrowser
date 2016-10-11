using BetterDefaultBrowser.Commands;
using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BetterDefaultBrowser.ViewModels
{
    class PlainFilterViewModel : FilterViewModelBase
    {
        private PlainFilter pFilter;
        private Browser browser;
        private BindingList<Browser> browserList;
        private bool validRegex = true;
        private String regex = ".*";

        private OpenFilter oFilter = null;
        private bool IsSubfilter = false;

        public PlainFilterViewModel() : this(new PlainFilter { Name = "Unnamed filter", RegEx = ".*" })
        {
        }

        /// <summary>
        /// Create a new PlainFilterViewModel.
        /// When Using a managed Filter this constructor should be invoked.
        /// </summary>
        public PlainFilterViewModel(PlainFilter f) : base(f)
        {
            pFilter = (PlainFilter)filter;
            browser = pFilter.AssignedBrowser;
            browserList = AllBrowsers.InstalledBrowsers;
        }

        public PlainFilterViewModel(PlainFilter f, OpenFilter o) : base(f)
        {
            pFilter = (PlainFilter)filter;
            browserList = AllBrowsers.InstalledBrowsers;
            oFilter = o;
            IsSubfilter = true;
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
        public Browser Browser
        {
            get
            {
                return browser;
            }
            set
            {
                if (browser == null || !browser.Equals(value))
                {
                    this.browser = value;
                    OnPropertyChanged("Browser");
                }
            }
        }

        public BindingList<Browser> BrowserList
        {
            get
            {
                return browserList;
            }
        }


        #endregion


        #region Commands
        protected virtual void StoreFilterExecute()
        {
            //Assuming browser is valid
            pFilter.AssignedBrowser = browser;
            if (!IsSubfilter)
            {
                pFilter.Store();
            }
            else
            {
                oFilter.InnerFilter = pFilter;
                oFilter.Store();
            }
            MyVisibility = Visibility.Hidden;
        }

        protected virtual bool CanStoreFilterExecute()
        {
            return Name != "" && IsValidRegEx && Browser != null;
        }

        public ICommand StoreFilter { get { return new RelayCommand(StoreFilterExecute, CanStoreFilterExecute); } }
        #endregion
    }
}
