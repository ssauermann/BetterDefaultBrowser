using BetterDefaultBrowser.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.ViewModels
{
    class BrowserViewModel : INotifyPropertyChanged
    {
        #region Construction
        public BrowserViewModel(String key)
        {
            try
            {
                browser = new Browser(key);
                isAvailable = true;
                isDefault = browser.Equals(AllBrowsers.Default);
                Name = browser.Name;
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException)
            {
                isAvailable = false;
                isDefault = false;
                Name = "Invalid browser";
            }
        }
        #endregion

        #region Members
        private Browser browser;
        private bool isAvailable;
        private bool isDefault;
        #endregion

        #region Properties
        /// <summary>
        /// Browser name for display
        /// </summary>
        public String Name { get; }


        /// <summary>
        /// Is this browser currently the system default browser?
        /// </summary>
        public bool IsDefault
        {
            get
            {
                if (isAvailable)
                    return isDefault;
                return false;
            }
            internal set
            {
                if (isAvailable)
                    if (isDefault != value)
                    {
                        isDefault = value;
                        RaisePropertyChanged("IsDefault");
                    }
            }
        }

        public String IsDefaultString
        {
            get
            {
                if (IsDefault)
                    return "(default)";
                return "";
            }
        }

        internal Browser Browser
        {
            get
            {
                return browser;
            }
        }

        internal bool IsAvailable
        {
            get
            {
                return isAvailable;
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

        public override bool Equals(object obj)
        {
            if (!(obj is BrowserViewModel))
                return false;
            var other = obj as BrowserViewModel;

            if (this.isAvailable != other.isAvailable)
                return false;

            if (!this.isAvailable && !other.isAvailable)
                return true;

            return this.browser.Equals(other.browser);
        }
        #endregion

    }
}
