using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a open filter.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class OpenFilter : Filter
    {
        #region Fields
        /// <summary>
        /// Gets or sets a value indicating whether only currently running browsers should be used
        /// or a new browser should be opened if no running one matches.
        /// </summary>
        [YAXSerializeAs("OnlyOpen")]
        [YAXSerializableField]
        public bool OnlyOpen { get; set; }

        /// <summary>
        /// Gets the list of the browser priority list.
        /// </summary>
        [YAXSerializeAs("Browsers")]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Browser")]
        [YAXSerializableField]
        public List<BrowserStorage> Browsers { get; } = new List<BrowserStorage>();

        /// <summary>
        /// Gets or sets the inner filter.
        /// </summary>
        [YAXSerializeAs("InnerFilter")]
        [YAXSerializableField]
        public Filter InnerFilter { get; set; }

        #endregion
        #region Validation

        private static readonly string[] ValidatedProperties =
        {
            "Browsers",
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
                case "Browsers":
                    error = this.ValidateBrowsers();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on OpenFilter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateBrowsers()
        {
            if (Browsers.Count == 0)
            {
                return "Browser list contains invalid browsers.";
            }
            return null;
        }

        #endregion
    }
}
