using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Filter which is managed with the GUI
    /// </summary>
    public class ManagedFilter : Filter
    {
        private Protocols protocol;
        private String url;
        private Ignore flags;

        /// <summary>
        /// Create a managed filter for an assigned browser.
        /// </summary>
        /// <param name="browser">Assigned browser</param>
        public ManagedFilter(Browser browser) : base(browser)
        {
            this.Type = FType.MANAGED;
        }

        /// <summary>
        /// The matching protocols.
        /// </summary>
        public Protocols Protocol
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
                    OnPropertyChanged("Protocol");
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
                    OnPropertyChanged("Flags");
                }
            }
        }

        /// <summary>
        /// Ignore flags used by managed filters.
        /// </summary>
        [Flags]
        public enum Ignore
        {
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
}
