using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Filter for request handling. If the filter is matched, the assigned browser will be used to open the url.
    /// </summary>
    public class Filter : INotifyPropertyChanged
    {
        #region Attributes & Constructors
        //Default value: Regex matching nothing
        private Regex regex = new Regex("(?!x)x");
        private Browser assignedBrowser;
        private FType ftype;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Create a new filter with an assigned browser.
        /// </summary>
        /// <param name="browser">Assigned browser</param>
        public Filter(Browser browser)
        {
            this.AssignedBrowser = browser;
        }

        /// <summary>
        /// Create a new filter with an assigned browser.
        /// </summary>
        /// <param name="browser">Key name of the assigned browser</param>
        public Filter(String browser) : this(new Browser(browser)) { }

        #endregion

        #region Properties
        /// <summary>
        /// Set a regular expression for filtering requests.
        /// </summary>
        public String RegEx
        {
            get
            {
                return regex.ToString();
            }
            set
            {
                if (IsValidRegex(value))
                {
                    if (!regex.Equals(value))
                    {
                        regex = new Regex(value);
                        OnPropertyChanged("RegEx");
                    }
                }
                else
                {
                    throw new FilterInvalidException("Not a valid regex!");
                }
            }
        }

        /// <summary>
        /// Assigned browser for this filter.
        /// </summary>
        public Browser AssignedBrowser
        {
            get
            {
                return this.assignedBrowser;
            }
            set
            {
                if (!assignedBrowser.Equals(value))
                {
                    this.assignedBrowser = value;
                    OnPropertyChanged("AssignedBrowser");
                }
            }
        }

        /// <summary>
        /// Filter type
        /// </summary>
        public FType Type
        {
            get
            {
                return this.ftype;
            }
            set
            {
                if (!ftype.Equals(value))
                {
                    this.ftype = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        /// <summary>
        /// ID for reference in saved file
        /// </summary>
        internal String ID { get; set; }

        #endregion

        #region Methods
        /// <summary>
        /// Match this filter to an url.
        /// </summary>
        /// <param name="url">URL to match</param>
        /// <returns>Does the filter match?</returns>
        public bool match(string url)
        {
            return regex.IsMatch(url);
        }

        /// <summary>
        /// Stores the filter into the save file.
        /// Has to be called after an filters was edited or added.
        /// </summary>
        public void store()
        {
            Settings.saveFilter(this);
        }

        public override string ToString()
        {
            return "Filter: " + AssignedBrowser + " <-> " + RegEx;
        }

        #endregion

        #region Inner Classes
        /// <summary>
        /// Filter has invalid settings.
        /// </summary>
        public class FilterInvalidException : Exception
        {
            public FilterInvalidException()
            {

            }

            public FilterInvalidException(String msg)
                : base(msg)
            {

            }
        }

        /// <summary>
        /// Filter types
        /// </summary>
        public enum FType
        {
            /// <summary>
            /// Just plain regex
            /// </summary>
            PLAIN,
            /// <summary>
            /// Uses first open browser
            /// </summary>
            OPEN,
            /// <summary>
            /// Fancy GUI for regex creation
            /// </summary>
            MANAGED
        }
        #endregion

        #region Static Methods
        private static bool IsValidRegex(string pattern)
        {
            if (string.IsNullOrEmpty(pattern)) return false;

            try
            {
                Regex.IsMatch("", pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
