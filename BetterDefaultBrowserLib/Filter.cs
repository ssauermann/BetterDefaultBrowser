using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowserLib
{
    public class Filter
    {
        private Regex regex;
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

        public Browser AssignedBrowser;


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

        public Filter(String regex, Browser browser)
        {
            this.RegEx = regex;
            this.AssignedBrowser = browser;
        }

        public bool match(string url)
        {
            return regex.IsMatch(url);
        }

        public Filter(String regex, String browser)
        {
            this.RegEx = regex;
            this.AssignedBrowser = new Browser(browser);
        }

        public class FilterInvalidException: Exception
        {
            public FilterInvalidException()
            {

            }

            public FilterInvalidException(String msg)
                :base(msg)
            {

            }
        }

        public override string ToString()
        {
            return "Filter: " + AssignedBrowser + " <-> " + RegEx;
        }
    }
}
