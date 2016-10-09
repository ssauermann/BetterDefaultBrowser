using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DNL = DomainName.Library;

namespace BetterDefaultBrowser.Lib.Filters
{
    /// <summary>
    /// Helper methods for regex creation.
    /// </summary>
    public static class RegexBuilder
    {

        /// <summary>
        /// Create a regex from a managed filter.
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Regex</returns>
        public static String build(ManagedFilter filter)
        {

            String finalUrl = filter.URL;

            //Remove all protocols
            finalUrl = Regex.Replace(finalUrl, @"(.*?\:\/\/)", "");

            var domainURL = finalUrl;


            //Domain part of url is case insensitive
            //Find first slash or ? or line end and insert case insensitive matching end tag before it
            //Additional add a start tag at the begining of the string
            var cireg = new Regex(@"(\/|\?|\z)");
            finalUrl = @"<_>" + cireg.Replace(finalUrl, m => string.Format(@"<->{0}", m.Value), 1);


            //Process flags and replace ignored parts
            var flags = Enum.GetValues(typeof(ManagedFilter.Ignore)).Cast<ManagedFilter.Ignore>();
            var filterFlags = filter.Flags;

            foreach (var flag in flags)
            {
                if (filterFlags.HasFlag(flag))
                {
                    if (flag == ManagedFilter.Ignore.SD)
                    {
                        //Use external Lib to get SD
                        var sd = getSD(domainURL);
                        var regSD = new Regex(Regex.Escape(sd + "."));
                        finalUrl = regSD.Replace(finalUrl, "<>", 1);
                    }
                    else if (flag == ManagedFilter.Ignore.TLD)
                    {
                        //Use external Lib to get TLD
                        var tld = getTLD(domainURL);
                        var regTLD = new Regex(Regex.Escape("." + tld));
                        finalUrl = regTLD.Replace(finalUrl, "<>", 1);
                    }
                    else
                    {
                        //Remove the part from the url
                        finalUrl = Regex.Replace(finalUrl, flag.Regex(), "<>");

                    }
                }
            }

            //Escape remaining url
            finalUrl = Regex.Escape(finalUrl);

            //Add protocolls to start of the new regex
            var protocols = Enum.GetValues(typeof(Protocols)).Cast<Protocols>();
            var filterProtocols = filter.Protocols;
            var protocolRegex = "<_>(" + String.Join("|", from p in protocols where filterProtocols.HasFlag(p) select p.Regex()) + ")<->";
            finalUrl = protocolRegex + finalUrl;


            //Set replaced parts to an valid regex expression
            finalUrl = Regex.Replace(finalUrl, "(<>)+", "(.*?)");
            finalUrl = Regex.Replace(finalUrl, "(<->)(<_>)+", "");
            finalUrl = Regex.Replace(finalUrl, "(<_>)+", "(?i)");
            finalUrl = Regex.Replace(finalUrl, "(<->)+", "(?-i)");

            return finalUrl;
        }

        private static String getTLD(string url)
        {
            var domainURL = Regex.Replace(url, ManagedFilter.Ignore.Page.Regex(), "");
            domainURL = Regex.Replace(domainURL, ManagedFilter.Ignore.Parameter.Regex(), "");

            DNL.DomainName dnOut;
            if (!DNL.DomainName.TryParse(domainURL, out dnOut))
            {
                throw new Filter.FilterInvalidException("URL invalid");
            }
            return dnOut.TLD;
        }

        private static String getSD(string url)
        {
            var domainURL = Regex.Replace(url, ManagedFilter.Ignore.Page.Regex(), "");
            domainURL = Regex.Replace(domainURL, ManagedFilter.Ignore.Parameter.Regex(), "");

            DNL.DomainName dnOut;
            if (!DNL.DomainName.TryParse(domainURL, out dnOut))
            {
                throw new Filter.FilterInvalidException("URL invalid");
            }
            return dnOut.SubDomain;
        }
    }
}
