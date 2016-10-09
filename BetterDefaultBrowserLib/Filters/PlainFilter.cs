using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetterDefaultBrowser.Lib.Filters
{
    public class PlainFilter : Filter
    {
        private Regex regex;
        private Browser assignedBrowser;

        public PlainFilter()
        {
            this.Type = FType.PLAIN;
        }

        #region Properties
        /// <summary>
        /// Set a regular expression for filtering requests.
        /// </summary>
        public virtual String RegEx
        {
            get
            {
                return regex.ToString();
            }
            set
            {
                if (IsValidRegex(value))
                {
                    if (regex == null || !regex.Equals(value))
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
                if (assignedBrowser == null || !assignedBrowser.Equals(value))
                {
                    this.assignedBrowser = value;
                    base.OnPropertyChanged("AssignedBrowser");
                }
            }
        }
        #endregion

        #region Methods       

        /// <summary>
        /// Match this filter to an url.
        /// </summary>
        /// <param name="url">URL to match</param>
        /// <param name="matchingResult">Matching browser or null</param>
        /// <returns>Does the filter match the url?</returns>
        public override bool Match(string url, out Browser matchingResult)
        {
            if (regex.IsMatch(url))
            {
                matchingResult = AssignedBrowser;
                return true;
            }
            matchingResult = null;
            return false;
        }

        /// <summary>
        /// Convert this filter to an XML representation.
        /// </summary>
        /// <returns>XML Element</returns>
        internal override XElement ToXML()
        {
            var e = base.ToXML();
            e.Add(new XElement("regex", RegEx),
                new XElement("browser", AssignedBrowser)
                  );
            return e;
        }

        /// <summary>
        /// Loads values for this filter from an XML representation.
        /// <param name="e">XML element</param>
        /// </summary>
        internal override void FromXML(XElement e)
        {
            base.FromXML(e);

            RegEx = e.Element("regex").Value;

            try
            {
                AssignedBrowser = new Browser(e.Element("browser").Value);
            }
            catch (Exception ex) when (ex is ArgumentException | ex is ArgumentNullException)
            {
                throw new FilterInvalidException("Browser creation failed.", ex);
            }
        }

        #endregion


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
