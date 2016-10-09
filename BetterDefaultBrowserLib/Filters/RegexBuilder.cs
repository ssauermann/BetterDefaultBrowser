using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Filters
{
    public class RegexBuilder
    {

        public String build(ManagedFilter filter)
        {

            var protocols = Enum.GetValues(typeof(Protocols)).Cast<Protocols>();
            var filterProtocols = filter.Protocols;

            var protocolRegex = "((?i)" + String.Join("|", from p in protocols where filterProtocols.HasFlag(p) select p.Regex()) + "(?-i))";

            String finalUrl = filter.URL;

            Console.Write("Original url:\t");
            Console.WriteLine(finalUrl);


            finalUrl = Regex.Replace(finalUrl, @"(.*?\:\/\/)", "");


            Console.Write("Without protocol:\t");
            Console.WriteLine(finalUrl);


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
                else
                {
                    //Part will not be removed, should it be matched case insensitive?
                    //TODO
                }
            }


            finalUrl = protocolRegex + finalUrl;

            Console.Write("With selected protocols:\t");
            Console.WriteLine(finalUrl);


            finalUrl = Regex.Replace(finalUrl, "(<>)+", "(.*?)");

            Console.Write("As Regex:\t");
            Console.WriteLine(finalUrl);

            return finalUrl;


            //TODO: Match TLD and SD as case insensitive
        }

    }
}
