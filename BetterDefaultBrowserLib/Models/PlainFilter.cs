using System;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a plain filter.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class PlainFilter : Filter
    {
        #region Fields
        /// <summary>
        /// Gets or sets the regex string.
        /// </summary>
        [YAXSerializeAs("Regex")]
        [YAXSerializableField]
        public string Regex { get; set; }

        /// <summary>
        /// Gets or sets the the assigned browser.
        /// </summary>
        [YAXSerializeAs("Browser")]
        [YAXSerializableField]
        public BrowserStorage Browser { get; set; }
        #endregion

        #region Validation

        private static readonly string[] ValidatedProperties =
        {
            nameof(Regex),
            nameof(Browser)
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
                case nameof(Regex):
                    error = ValidateRegex();
                    break;
                case nameof(Browser):
                    error = ValidateBrowser();
                    break;
                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on PlainFilter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateBrowser()
        {
            if (Browser == null)
            {
                return "No browser selected.";
            }
            return null;
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

        public override bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return base.IsValid;
            }
        }

        #endregion
    }
}
