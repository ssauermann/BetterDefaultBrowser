using System;
using YAXLib;

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
        }

        /// <summary>
        /// Gets or sets the matched protocols.
        /// </summary>
        [YAXSerializeAs("Protocols")]
        public Protocols Protocols { get; set; }

        /// <summary>
        /// Gets or sets the matched url.
        /// </summary>
        [YAXSerializeAs("URL")]
        public string URL { get; set; }

        /// <summary>
        /// Gets or sets the ignored url parts.
        /// </summary>
        [YAXSerializeAs("Flags")]
        public Ignore Flags { get; set; }
    }
}
