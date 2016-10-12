using System;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a managed filter.
    /// </summary>
    public class ManagedFilter : PlainFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedFilter" /> class.
        /// </summary>
        public ManagedFilter() : base()
        {
            this.Type = FilterTypes.MANAGED;
        }

        /// <summary>
        /// Gets or sets the matched protocols.
        /// </summary>
        public Protocols Protocols { get; set; }

        /// <summary>
        /// Gets or sets the matched url.
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the ignored url parts.
        /// </summary>
        public Ignore Flags { get; set; }
    }
}
