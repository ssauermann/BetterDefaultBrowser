using System;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a plain filter.
    /// </summary>
    public class PlainFilter : Filter
    {
        #region Fields
        /// <summary>
        /// Gets or sets the regex string.
        /// </summary>
        [YAXSerializeAs("Regex")]
        public string Regex { get; set; }

        /// <summary>
        /// Gets or sets the the assigned browser.
        /// </summary>
        [YAXSerializeAs("Browser")]
        public BrowserStorage Browser { get; set; }
        #endregion

        #region Validation

        private static readonly string[] ValidatedProperties =
        {
            "Regex",
        };

        protected override string GetValidationError(string propertyName)
        {
            string parentError = base.GetValidationError(propertyName);
            if (parentError != null)
            {
                return parentError;
            }

            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Regex":
                    error = ValidateRegex();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on PlainFilter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateRegex()
        {
            if (Validator.IsStringMissing(Regex))
            {
                return "Regex must not be empty. Use .* to match any url.";
            }
            if (!RegexHelper.IsValid(Regex))
            {
                return "Regex is invalid.";
            }
            return null;
        }

        #endregion
    }
}
