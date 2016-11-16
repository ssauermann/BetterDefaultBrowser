using System;
using System.Linq;
using System.Text.RegularExpressions;
using DNL = DomainName.Library;

namespace BetterDefaultBrowser.Lib.Logic
{
    using Models;

    /// <summary>
    /// Helper methods for regex creation.
    /// </summary>
    public static class RegexBuilder
    {
        /// <summary>
        /// Create a regex from a managed filter.
        /// </summary>
        /// <param name="filter">Filter to process</param>
        /// <returns>Created regex</returns>
        public static string Build(ManagedFilter filter)
        {
            string finalUrl = filter.Url;

            // Remove all protocols
            finalUrl = Regex.Replace(finalUrl, @"(.*?\:\/\/)", string.Empty);

            var domainUrl = finalUrl;

            // Domain part of url is case insensitive
            // Find first slash or ? or line end and insert case insensitive matching end tag before it
            // Additional add a start tag at the begining of the string
            var cireg = new Regex(@"(\/|\?|\z)");
            finalUrl = @"<_>" + cireg.Replace(finalUrl, m => $@"<->{m.Value}", 1);

            // Process flags and replace ignored parts
            var flags = Enum.GetValues(typeof(Ignore)).Cast<Ignore>();
            var filterFlags = filter.Flags;

            foreach (var flag in flags)
            {
                if (filterFlags.HasFlag(flag))
                {
                    if (flag == Ignore.SD)
                    {
                        // Use external Lib to get SD
                        var sd = GetSD(domainUrl);
                        if (sd != string.Empty)
                        {
                            var regSd = new Regex(Regex.Escape(sd + "."));
                            finalUrl = regSd.Replace(finalUrl, "<>", 1);
                        }
                    }
                    else if (flag == Ignore.TLD)
                    {
                        // Use external Lib to get TLD
                        var tld = GetTLD(domainUrl);
                        var regTld = new Regex(Regex.Escape("." + tld));
                        finalUrl = regTld.Replace(finalUrl, "<>", 1);
                    }
                    else
                    {
                        // Remove the part from the url
                        finalUrl = Regex.Replace(finalUrl, flag.Regex(), "<>");
                    }
                }
            }

            // Escape remaining url
            finalUrl = Regex.Escape(finalUrl);

            // Add protocolls to start of the new regex
            var protocols = Enum.GetValues(typeof(Protocols)).Cast<Protocols>();
            var filterProtocols = filter.Protocols;
            var protocolRegex = "<_>(" + string.Join("|", from p in protocols where filterProtocols.HasFlag(p) select p.Regex()) + ")<->";
            finalUrl = protocolRegex + finalUrl;

            // Set replaced parts to an valid regex expression
            finalUrl = Regex.Replace(finalUrl, "(<>)+", "(.*?)");
            finalUrl = Regex.Replace(finalUrl, "(<->)(<_>)+", string.Empty);
            finalUrl = Regex.Replace(finalUrl, "(<_>)+", "(?i)");
            finalUrl = Regex.Replace(finalUrl, "(<->)+", "(?-i)");

            return finalUrl;
        }

        /// <summary>
        /// Tests if an url is valid.
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <returns>Validity of url</returns>
        public static bool URLIsValid(string url)
        {
            var domainUrl = Regex.Replace(url, Ignore.Page.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Parameter.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Port.Regex(), string.Empty);
            DNL.DomainName domainOut;
            return DNL.DomainName.TryParse(domainUrl, out domainOut);
        }

        /// <summary>
        /// Get the top level domain.
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <returns>top level domain</returns>
        private static String GetTLD(string url)
        {
            var domainUrl = Regex.Replace(url, Ignore.Page.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Parameter.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Port.Regex(), string.Empty);

            DNL.DomainName domainOut;
            if (!DNL.DomainName.TryParse(domainUrl, out domainOut))
            {
                throw new ArgumentException("URL invalid");
            }

            return domainOut.TLD;
        }

        /// <summary>
        /// Get the sub domain.
        /// </summary>
        /// <param name="url">URL to process.</param>
        /// <returns>sub domain</returns>
        private static String GetSD(string url)
        {
            var domainUrl = Regex.Replace(url, Ignore.Page.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Parameter.Regex(), string.Empty);
            domainUrl = Regex.Replace(domainUrl, Ignore.Port.Regex(), string.Empty);

            DNL.DomainName domainOut;
            if (!DNL.DomainName.TryParse(domainUrl, out domainOut))
            {
                throw new ArgumentException("URL invalid");
            }

            return domainOut.SubDomain;
        }
    }
}
