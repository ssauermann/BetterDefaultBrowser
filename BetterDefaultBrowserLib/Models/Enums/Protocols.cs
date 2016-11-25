using System;
using System.ComponentModel;

namespace BetterDefaultBrowser.Lib.Models.Enums
{
    /// <summary>
    /// Supported Protocols.
    /// Used by managed filters.
    /// </summary>
    [Flags]
    public enum Protocols
    {
        /// <summary>
        /// HTTP protocol
        /// </summary>
        [Description("http")]
        HTTP = 1 << 0,

        /// <summary>
        /// HTTPS protocol
        /// </summary>
        [Description("https")]
        HTTPS = 1 << 1
    }

    /// <summary>
    /// Extension Methods for Protocols enum.
    /// </summary>
    public static class ProtocolsExtensions
    {
        /// <summary>
        /// Gets the regex for this protocol.
        /// </summary>
        /// <param name="prots">Protocol (Must not be multiple flags)</param>
        /// <returns>Regex string</returns>
        public static string Regex(this Protocols prots)
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
}
