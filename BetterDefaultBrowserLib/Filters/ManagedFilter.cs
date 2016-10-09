using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetterDefaultBrowser.Lib.Filters
{
    /// <summary>
    /// Filter which is managed with the GUI
    /// </summary>
    public class ManagedFilter : PlainFilter
    {
        private Protocols protocol;
        private String url = "";
        private Ignore flags;

        /// <summary>
        /// Create a managed filter for an assigned browser.
        /// </summary>
        /// <param name="browser">Assigned browser</param>
        public ManagedFilter() : base()
        {
            this.Type = FType.MANAGED;
        }


        #region Properties
        /// <summary>
        /// The matching protocols.
        /// </summary>
        public Protocols Protocols
        {
            get
            {
                return protocol;
            }
            set
            {
                if (!protocol.Equals(value))
                {
                    protocol = value;
                    RebuildRegex();
                    OnPropertyChanged("Protocols");
                }
            }
        }

        /// <summary>
        /// The matching url. This must be reduced by the set flags before it can be used in a regular expression.
        /// </summary>
        public String URL
        {
            get
            {
                return url;
            }
            set
            {
                if (!url.Equals(value))
                {
                    url = value;
                    RebuildRegex();
                    OnPropertyChanged("URL");
                }
            }
        }

        /// <summary>
        /// The set flags.
        /// </summary>
        public Ignore Flags
        {
            get
            {
                return flags;
            }
            set
            {
                if (!flags.Equals(value))
                {
                    flags = value;
                    RebuildRegex();
                    OnPropertyChanged("Flags");
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convert this filter to an XML representation.
        /// </summary>
        /// <returns>XML Element</returns>
        internal override XElement ToXML()
        {
            var e = base.ToXML();

            e.Add(new XElement("protocols", Protocols),
                new XElement("flags", Flags),
                new XElement("url", URL)
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

            URL = e.Element("url").Value;

            int x = -1;
            if (Int32.TryParse(e.Element("flags").Value, out x))
            {
                Flags = (Ignore)x;
            }
            else
                throw new FilterInvalidException("XML parsing error. Invalid flags.");


            if (Int32.TryParse(e.Element("protocols").Value, out x))
            {
                Protocols = (Protocols)x;
            }
            else
                throw new FilterInvalidException("XML parsing error. Invalid protocols.");

        }

        private void RebuildRegex()
        {
            this.RegEx = RegexBuilder.build(this);
        }
        #endregion

        /// <summary>
        /// Ignore flags used by managed filters.
        /// </summary>
        [Flags]
        public enum Ignore
        {
            //These have to be in order of occurence in an url.
            SD = 1 << 0,
            TLD = 1 << 1,
            Port = 1 << 2,
            Page = 1 << 3,
            Parameter = 1 << 4
        }

    }

    /// <summary>
    /// Supported Protocols.
    /// Used by managed filters.
    /// </summary>
    [Flags]
    public enum Protocols
    {
        HTTP = 1 << 0,
        HTTPS = 1 << 1
    }

    /// <summary>
    /// Extension Methods for Protocols enum
    /// </summary>
    public static class ProtocolsExtensions
    {
        /// <summary>
        /// Gets the regex for this protocol
        /// </summary>
        /// <param name="prots">Protocol</param>
        /// <returns>Regex</returns>
        public static String Regex(this Protocols prots)
        {
            switch (prots)
            {
                case Protocols.HTTP:
                    return @"(http\:\/\/)";
                case Protocols.HTTPS:
                    return @"(https\:\/\/)";
                default:
                    throw new NotImplementedException("Missing implementation for a protocol");
            }
        }
    }


    /// <summary>
    /// Extension Methods for Ignore enum
    /// </summary>
    public static class IgnoreExtensions
    {
        /// <summary>
        /// Gets the regex for this flag.
        /// </summary>
        /// <param name="flag">Flag</param>
        /// <returns>Regex</returns>
        public static String Regex(this ManagedFilter.Ignore flag)
        {
            switch (flag)
            {
                case ManagedFilter.Ignore.Parameter:
                    return @"(\?.*)";
                case ManagedFilter.Ignore.Page:
                    return @"(\/(?:[^?])+)";
                case ManagedFilter.Ignore.Port:
                    return @"(\:[0-9]+)";
                case ManagedFilter.Ignore.TLD:
                    throw new NotSupportedException("Regex has to be created by DomainNameLib.");
                case ManagedFilter.Ignore.SD:
                    throw new NotSupportedException("Regex has to be created by DomainNameLib.");
                default:
                    throw new NotImplementedException("Missing implementation for a protocol");
            }
        }
    }
}
