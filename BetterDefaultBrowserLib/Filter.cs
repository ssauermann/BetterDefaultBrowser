using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Filter for request handling. If the filter is matched, the assigned browser will be used to open the url.
    /// </summary>
    public class Filter
    {
        private Regex regex;

        /// <summary>
        /// Create a new filter with a regular expression and an assigned browser.
        /// </summary>
        /// <param name="regex">Regular Expression</param>
        /// <param name="browser">Assigned browser</param>
        public Filter(String regex, Browser browser)
        {
            this.RegEx = regex;
            this.AssignedBrowser = browser;
        }
        
        /// <summary>
        /// Create a new filter with a regular expression and an assigned browser.
        /// </summary>
        /// <param name="regex">Regular Expression</param>
        /// <param name="browser">Key name of the assigned browser</param>
        public Filter(String regex, String browser)
        {
            this.RegEx = regex;
            this.AssignedBrowser = new Browser(browser);
        }

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
                    regex = new Regex(value);
                }else
                {
                    throw new FilterInvalidException("Not valid regex!");
                }
            }
        }

        /// <summary>
        /// Assigned browser for this filter.
        /// </summary>
        public Browser AssignedBrowser { get; set; }

        /// <summary>
        /// Match this filter to an url.
        /// </summary>
        /// <param name="url">URL to match</param>
        /// <returns>Does the filter match?</returns>
        public bool match(string url)
        {
            return regex.IsMatch(url);
        }


        public override string ToString()
        {
            return "Filter: " + AssignedBrowser + " <-> " + RegEx;
        }

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
    }
}
