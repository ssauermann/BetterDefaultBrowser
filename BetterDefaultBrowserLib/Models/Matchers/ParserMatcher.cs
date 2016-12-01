using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Models.Enums;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models.Matchers
{
    /// <summary>
    /// This matcher uses the url parsing class to parse the given url into single parts.
    /// It will then compare them with the set parts to decide if it matches the url.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class ParserMatcher : Matcher
    {
        public Tuple<string, bool> Protocol { get; set; }
        public Tuple<string, bool> SubDomain { get; set; }
        public Tuple<string, bool> Domain { get; set; }
        public Tuple<string, bool> TopLevelDomain { get; set; }
        public Tuple<string, bool> Port { get; set; }
        public Tuple<string, bool> Page { get; set; }
        public Tuple<string, bool> Parameters { get; set; }
        public Tuple<string, bool> Fragment { get; set; }

        /// <summary>
        /// Checks if an url is matched by this matcher.
        /// </summary>
        /// <param name="url">Url to match</param>
        /// <returns>Does url match?</returns>
        public override bool IsMatch(string url)
        {
            var parser = new UrlParser(url);
            if (!parser.Parse())
                return false;

            // Check each
            if (!Protocol.Item2)
            {
                if (Protocol.Item1 != parser.Protocol)
                    return false;
            }
            if (!SubDomain.Item2)
            {
                if (SubDomain.Item1 != parser.Sd)
                    return false;
            }
            if (!Domain.Item2)
            {
                if (Domain.Item1 != parser.Domain)
                    return false;
            }
            if (!TopLevelDomain.Item2)
            {
                if (TopLevelDomain.Item1 != parser.Tld)
                    return false;
            }
            if (!Port.Item2)
            {
                if (Port.Item1 != parser.Port)
                    return false;
            }
            if (!Page.Item2)
            {
                if (Page.Item1 != parser.Page)
                    return false;
            }
            if (!Parameters.Item2)
            {
                if (Parameters.Item1 != parser.Parameter)
                    return false;
            }
            if (!Fragment.Item2)
            {
                if (Fragment.Item1 != parser.Fragment)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns the validation error for a given column name or null if no error exists.
        /// </summary>
        /// <param name="columnName">Field name</param>
        /// <returns>Null or the error string</returns>
        public override string GetValidationError(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
