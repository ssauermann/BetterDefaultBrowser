using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Filters
{
    public static class RegexBuilder
    {

        public static String build(ManagedFilter filter)
        {

            String finalUrl = filter.URL;

            Console.Write("Original url:\t");
            Console.WriteLine(finalUrl);

            //Remove all protocols
            finalUrl = Regex.Replace(finalUrl, @"(.*?\:\/\/)", "");


            Console.Write("Without protocol:\t");
            Console.WriteLine(finalUrl);


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
                    //Remove the part from the url
                    finalUrl = Regex.Replace(finalUrl, flag.Regex(), "<>");

                    Console.Write("Without " + flag + ":\t");
                    Console.WriteLine(finalUrl);

                }
            }

            //Escape remaining url
            finalUrl = Regex.Escape(finalUrl);

            //Add protocolls to start of the new regex
            var protocols = Enum.GetValues(typeof(Protocols)).Cast<Protocols>();
            var filterProtocols = filter.Protocols;
            var protocolRegex = "<_>(" + String.Join("|", from p in protocols where filterProtocols.HasFlag(p) select p.Regex()) + ")<->";
            finalUrl = protocolRegex + finalUrl;

            Console.Write("With selected protocols:\t");
            Console.WriteLine(finalUrl);


            //Set replaced parts to an valid regex expression
            finalUrl = Regex.Replace(finalUrl, "(<>)+", "(.*?)");
            finalUrl = Regex.Replace(finalUrl, "(<->)(<_>)+", "");
            finalUrl = Regex.Replace(finalUrl, "(<_>)+", "(?i)");
            finalUrl = Regex.Replace(finalUrl, "(<->)+", "(?-i)");

            Console.Write("As Regex:\t");
            Console.WriteLine(finalUrl);

            return finalUrl;
        }

    }
}
