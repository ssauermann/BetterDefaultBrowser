using System;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Models.Enums;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a managed filter.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class ManagedFilter : PlainFilter
    {
        #region Fields
        /// <summary>
        /// Gets or sets the matched url.
        /// </summary>
        [YAXSerializeAs("URL")]
        [YAXSerializableField]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ignored url parts.
        /// </summary>
        [YAXSerializeAs("Flags")]
        [YAXSerializableField]
        public Ignore Flags { get; set; }

        #endregion
        #region Validation
        private static readonly string[] ValidatedProperties =
        {
            nameof(Url),
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
                case nameof(Url):
                    error = ValidateUrl();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on ManagedFilter: " + propertyName);
                    break;
            }

            return error;
        }


        private string ValidateUrl()
        {
            return null; //TODO
            //if (!RegexBuilder.URLIsValid(Url))
            //{
            //    return "You must provide a valid url.";
            //}
            //return null;
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
