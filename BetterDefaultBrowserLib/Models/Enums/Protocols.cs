using System;

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
        HTTP = 1 << 0,

        /// <summary>
        /// HTTPS protocol
        /// </summary>
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
}
