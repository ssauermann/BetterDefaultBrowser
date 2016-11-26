﻿using System;

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
    }

    /// <summary>
    /// Extension Methods for Ignore enum.
    /// </summary>
    public static class IgnoreExtensions
    {
        /// <summary>
        /// Gets the regex for this flag.
        /// </summary>
        /// <param name="flag">Ignore flag</param>
        /// <returns>Regex string</returns>
        public static String Regex(this Ignore flag)
        {
            switch (flag)
            {
                case Ignore.Parameter:
                    return @"(\?.*)";
                case Ignore.Page:
                    return @"(\/(?:[^?])+)";
                case Ignore.Port:
                    return @"(\:[0-9]+)";
                case Ignore.Protocol:
                    throw new NotImplementedException("Needs rewrite!"); //TODO
                case Ignore.TopLevelDomain:
                    throw new NotSupportedException("Regex has to be created by DomainNameLib.");
                case Ignore.SubDomain:
                    throw new NotSupportedException("Regex has to be created by DomainNameLib.");
                default:
                    throw new NotImplementedException("Missing implementation for a flag");
            }
        }
    }
}
