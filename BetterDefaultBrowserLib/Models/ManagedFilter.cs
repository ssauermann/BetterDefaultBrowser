using System;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Models.Enums;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a managed filter.
    /// </summary>
    public class ManagedFilter : PlainFilter
    {
        #region Fields
        /// <summary>
        /// Gets or sets the matched protocols.
        /// </summary>
        [YAXSerializeAs("Protocols")]
        public Protocols Protocols { get; set; }

        /// <summary>
        /// Gets or sets the matched url.
        /// </summary>
        [YAXSerializeAs("URL")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ignored url parts.
        /// </summary>
        [YAXSerializeAs("Flags")]
        public Ignore Flags { get; set; }

        #endregion
        #region Validation
        private static readonly string[] ValidatedProperties =
        {
            "Protocols",
            "URL",
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
                case "Protocols":
                    error = ValidateProtocols();
                    break;

                case "URL":
                    error = ValidateUrl();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on ManagedFilter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateProtocols()
        {
            // Zero == none selected
            if (Protocols == 0)
            {
                return "You must select at least one protocol.";
            }
            return null;
        }

        private string ValidateUrl()
        {
            if (!RegexBuilder.URLIsValid(Url))
            {
                return "You must provide a valid url.";
            }
            return null;
        }

        #endregion
    }
}
