using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Abstract model of a filter.
    /// </summary>
    public abstract class Filter : IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter" /> class.
        /// <para> Sets the type of the filter.</para>
        /// </summary>
        protected Filter()
        {
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the internal id.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("ID")]
        public string ID { get; internal set; }

        /// <summary>
        /// Gets or sets the filters priority.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Priority")]
        public int Priority { get; set; }

        #region IDataErrorInfo
        public string Error => string.Empty;

        public string this[string columnName] => this.GetValidationError(columnName);
        #endregion

        /// <summary>
        /// Test equality of two objects.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Are they equal?</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Filter;

            if (other == null)
            {
                return false;
            }

            return this.ID == other.ID;
        }

        /// <summary>
        /// Generate a hash code for this object.
        /// </summary>
        /// <returns>Calculated hash code</returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(this.ID) ? 0 : this.ID.GetHashCode();
        }

        #region Validation

        private static readonly string[] ValidatedProperties =
        {
            "Name",
            "Priority",
        };

        /// <summary>
        /// Validates the given property name and returns an error message if invalid.
        /// Must call the parent implementation when overwritten.
        /// </summary>
        /// <param name="propertyName">Property string</param>
        /// <returns>Error message</returns>
        protected virtual string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
                return null;

            string error = null;

            switch (propertyName)
            {
                case "Name":
                    error = this.ValidateName();
                    break;

                case "Priority":
                    error = this.ValidatePriority();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on Filter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateName()
        {
            if (Validator.IsStringMissing(this.Name))
            {
                return "Name must not be empty.";
            }
            return null;
        }

        private string ValidatePriority()
        {
            if (this.Priority >= 0)
            {
                return "Priority must not be negative.";
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
