using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Helpers;
using Serilog;
using Serilog.Core;

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

        // https://regex101.com/r/GH7ywc/1
        // https://regex101.com/delete/URJdqtzvwphkCjbyVKS9ivLl

        internal static readonly RegexOptions Options = RegexOptions.ExplicitCapture;

        internal static readonly Regex RegexMatcher = new Regex(@"^((?<protocol>[a-zA-Z]+?)\:\/\/\/?)?(?<domain>([^\?\:\#\/\n\s\(\)\[\]\{\}]+)|(\[[0-9a-fA-F:]+\]))(\:(?<port>[0-9]+))?(\/(?<page>[^\?\n]*))?(\?(?<parameter>.*))?$", Options);


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

                var match = RegexMatcher.Match(url);
                if (!match.Success)
                {
                    return false;
                }

                Protocol = match.Groups["protocol"].Value.ToLower();
                Port = match.Groups["port"].Value;
                Parameter = match.Groups["parameter"].Value;
                Page = match.Groups["page"].Value;
                Domain = match.Groups["domain"].Value;

                return true;
            }
            catch (Exception ex)
            {
                Log.Debug(ex, "Url parsing failed when parsing {url}", _url);
                return false;
            }
        }
    }
}
