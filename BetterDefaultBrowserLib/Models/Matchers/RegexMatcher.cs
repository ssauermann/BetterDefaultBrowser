using System;
using System.Text.RegularExpressions;
using BetterDefaultBrowser.Lib.Helpers;
using Serilog;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models.Matchers
{
    /// <summary>
    /// This matcher uses a regex expression to decide if it matches the url.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class RegexMatcher : Matcher
    {
        /// <summary>
        /// Gets or sets the regex expression to validate.
        /// </summary>
        [YAXSerializeAs("Pattern")]
        [YAXSerializableField]
        public string Pattern { get; set; }

        /// <summary>
        /// Checks if an url is matched by this matcher.
        /// </summary>
        /// <param name="url">Url to match</param>
        /// <returns>Does url match?</returns>
        public override bool IsMatch(string url)
        {
            try
            {
                return Regex.IsMatch(url, Pattern);
            }
            catch (Exception ex)
            {
                Log.Logger.Information("Matching {url} with {Pattern} resulted in an exception:\n {ex}", url, Pattern, ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the validation error for a given column name or null if no error exists.
        /// </summary>
        /// <param name="columnName">Field name</param>
        /// <returns>Null or the error string</returns>
        public override string GetValidationError(string columnName)
        {
            if (columnName != nameof(Pattern))
                return null;

            return RegexHelper.IsValid(Pattern) ? null : "Regex pattern is invalid.";
        }
    }
}
