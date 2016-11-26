using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Logic
{
    public class UrlParser
    {
        private string _url;

        public string Protocol { get; private set; }
        public string Sd { get; private set; }
        public string Domain { get; private set; }
        public string Tld { get; private set; }
        public string Page { get; private set; }
        public string Parameter { get; private set; }
        public string Port { get; private set; }

        private static class RegexStrings
        {
            internal static readonly RegexOptions Options = RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase;

            internal static readonly Regex ProtocolRegex = new Regex(@"^(?<replace>(?<protocol>[a-zA-Z]+?)\:\/\/\/?)",
                Options);
        }


        public UrlParser(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            _url = input;
        }

        public bool Parse()
        {
            try
            {
                string url = _url;
                // If any parsing fails it will return false and the short circuit 'and' will stop evaluating.
                return ParseProtocol(url, out url);
                // && ParseParameter(url, out url) && ParsePage(url, out url) && ParseDomains(url, out url);
            }
            catch
            {
                return false;
            }
        }

        private bool ParseProtocol(string url, out string urlOut)
        {
            var m = RegexStrings.ProtocolRegex.Match(url);
            if (!m.Success)
            {
                urlOut = url;
                return false;
            }

            Protocol = m.Groups["protocol"].Value.ToLower();

            urlOut = RegexStrings.ProtocolRegex.Replace(url, "$'");

            return true;
        }
    }
}
