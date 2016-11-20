using System;
using System.ComponentModel;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a browser.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class Browser
    {
        /// <summary>
        /// String builder helper
        /// </summary>
        private static readonly Chillitom.ToStringBuilder<Browser> StringBuilder = new Chillitom.ToStringBuilder<Browser>().IncludeAllPublic().OrderAlphabetically(true).MultiLine(true).Compile();

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser" /> class.
        /// </summary>
        /// <param name="key">Browser key</param>
        public Browser(string key)
        {
            Key = key;
        }

        #region Fields
        /// <summary>
        /// Gets the browsers key identifying it.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Key")]
        [YAXSerializableField]
        public string Key { get; }

        /// <summary>
        /// Gets the program id for the connected program registration and url associations.
        /// </summary>
        public string ProgId { get; internal set; }

        /// <summary>
        /// Gets the browsers name for display.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Name")]
        [YAXSerializableField]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the path to this browsers icon.
        /// </summary>
        public string IconPath { get; internal set; }

        /// <summary>
        /// Gets the path to this browsers executable.
        /// </summary>
        public string ApplicationPath { get; internal set; }
        #endregion

        #region Object Methods

        /// <summary>
        /// Create a string of this object.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return StringBuilder.Stringify(this);
        }

        /// <summary>
        /// Test equality of two objects.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Are they equal?</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Browser;

            if (other == null)
            {
                return false;
            }

            return Key == other.Key;
        }

        /// <summary>
        /// Generate a hash code for this object.
        /// </summary>
        /// <returns>Calculated hash code</returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Key) ? 0 : Key.GetHashCode();
        }

        #endregion

        #region Validation

        private static readonly string[] ValidatedProperties =
        {
            "Key",
        };

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Key":
                    error = ValidateKey();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on OpenFilter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateKey()
        {
            if (Validator.IsStringMissing(Key) || BrowserGateway.Instance.GetBrowser(Key) == null)
            {
                return "Browser is not installed.";
            }
            return null;
        }

        /// <summary>
        /// Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get
            {
                foreach (string property in ValidatedProperties)
                    if (GetValidationError(property) != null)
                        return false;

                return true;
            }
        }

        #endregion
    }
}
