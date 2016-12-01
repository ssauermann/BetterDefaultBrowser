using System;

namespace BetterDefaultBrowser.Lib.Models.Enums
{
    /// <summary>
    /// Ignore flags used by managed filters.
    /// </summary>
    [Flags]
    public enum Ignore
    {
        /// <summary>
        /// Ignore subdomain.
        /// </summary>
        SubDomain = 1 << 0,

        /// <summary>
        /// Ignore top-level-domain.
        /// </summary>
        TopLevelDomain = 1 << 1,

        /// <summary>
        /// Ignore port.
        /// </summary>
        Port = 1 << 2,

        /// <summary>
        /// Ignore page.
        /// </summary>
        Page = 1 << 3,

        /// <summary>
        /// Ignore parameters.
        /// </summary>
        Parameter = 1 << 4,

        /// <summary>
        /// Ignore protocols.
        /// </summary>
        Protocol = 1 << 5,

        /// <summary>
        /// Ignore domain.
        /// </summary>
        Domain = 1 << 6,

        /// <summary>
        /// Ignore fragment.
        /// </summary>
        Fragment = 1 << 7,
    }
}
