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
        /// <summary>
        /// Regex string
        /// </summary>
        private string _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainFilter" /> class.
        /// </summary>
        public PlainFilter() : base()
        {
        }

        /// <summary>
        /// Gets or sets the regex string.
        /// </summary>
        [YAXSerializeAs("Regex")]
        public string Regex
        {
            get
            {
                return _regex;
            }

            set
            {
                if (RegexHelper.IsValid(value))
                {
                    _regex = value;
                }
                else
                {
                    throw new ArgumentException("Invalid regex");
                }
            }
        }

        /// <summary>
        /// Gets or sets the assigned browser.
        /// </summary>
        [YAXSerializeAs("Browser")]
        public Browser Browser { get; set; }

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
