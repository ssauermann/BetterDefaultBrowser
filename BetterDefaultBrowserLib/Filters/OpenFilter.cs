using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetterDefaultBrowser.Lib.Filters
{
    /// <summary>
    /// Filter which will use currently running browsers first before opening a new one.
    /// </summary>
    public class OpenFilter : Filter
    {
        private bool onlyOpen;
        private BindingList<Browser> browsers = new BindingList<Browser>();
        private Filter inner;

        /// <summary>
        /// Create a new OpenFilter.
        /// </summary>
        public OpenFilter() : base()
        {
            this.Type = FType.OPEN;
        }

        /// <summary>
        /// Do not start a new browsers, only use open browsers from 'Browsers' list, else do not match.
        /// </summary>
        public Boolean OnlyOpen
        {
            get
            {
                return onlyOpen;
            }
            set
            {
                if (onlyOpen != value)
                {
                    onlyOpen = value;
                    OnPropertyChanged("OnlyOpen");
                }
            }
        }

        /// <summary>
        /// Priority list what browser to use if opened.
        /// </summary>
        public BindingList<Browser> Browsers
        {
            get
            {
                return browsers;
            }
        }

        public Filter InnerFilter
        {
            get
            {
                return inner;
            }
            set
            {
                inner = value;
                OnPropertyChanged("InnerFilter");

            }
        }

        /// <summary>
        /// Convert this filter to an XML representation.
        /// </summary>
        /// <returns>XML Element</returns>
        internal override XElement ToXML()
        {
            var e = base.ToXML();

            XElement[] bElements = new XElement[Browsers.Count];

            int i = 0;
            foreach (var b in Browsers)
            {
                bElements[i] = new XElement("browser", b.KeyName);
                i++;
            }

            e.Add(new XElement("onlyOpen", OnlyOpen),
                new XElement("browsers", bElements),
                new XElement("inner", InnerFilter.ToXML())
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
            bool onop;
            Boolean.TryParse(e.Element("onlyOpen").Value, out onop);
            OnlyOpen = onop;

            var inner = e.Element("inner").FirstNode as XElement;

            if (inner != null)
            {
                InnerFilter = Settings.FilterFromElement(inner);
            }

            var browsers = e.Element("browsers");
            foreach (var b in browsers.Elements())
            {
                this.browsers.Add(new Browser(b.Value));
            }

        }

        /// <summary>
        /// Match this filter to an url.
        /// </summary>
        /// <param name="url">URL to match</param>
        /// <param name="matchingResult">Matching browser or null</param>
        /// <returns>Does the filter match the url?</returns>
        public override bool Match(string url, out Browser matchingResult)
        {
            matchingResult = null;

            //No match without inner filter
            if (InnerFilter == null)
                return false;

            Browser innerMatch;

            var isMatch = inner.Match(url, out innerMatch);

            //Inner filter does not match
            if (!isMatch)
                return false;

            //Inner filter does match
            foreach (Browser b in Browsers)
            {
                if (isBrowserOpen(b))
                {
                    matchingResult = b;
                    return true;
                }
            }

            //Browser list does not match -> check if OnlyOpen is set
            if (OnlyOpen)
            {
                //Did not match
                return false;
            }

            //Return inner browser
            matchingResult = innerMatch;
            return true;
        }

        /// <summary>
        /// Checks if a brother is currently opened.
        /// </summary>
        /// <param name="browser">Browser to check</param>
        /// <returns>Is running?</returns>
        public static bool isBrowserOpen(Browser browser)
        {
            var path = browser.ApplicationPath;

            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    String pPath = p.MainModule.FileName;
                    int pathEql = String.Compare(
                                    Path.GetFullPath(pPath).TrimEnd('\\'),
                                    Path.GetFullPath(path).TrimEnd('\\'),
                                    StringComparison.InvariantCultureIgnoreCase);
                    if (pathEql == 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) when (ex is NotSupportedException | ex is InvalidOperationException)
                {
                    //Ignore: Some processes (System / Idle) can not be inspected
                }
                catch (Win32Exception)
                {
                    //Win32 Applications can't inspect 64bit processes
                }
            }

            return false;
        }
    }
}
