using System;
using System.ComponentModel;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Abstract model of a filter.
    /// </summary>
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public abstract class Filter : IDataErrorInfo
    {
        protected Filter()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Name")]
        [YAXSerializableField]
        public string Name { get; set; }

        /// <summary>
        /// Gets the internal id.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("ID")]
        [YAXSerializableField]
        public string Id { get; protected set; }

        /// <summary>
        /// Gets or sets the filters priority.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Priority")]
        [YAXSerializableField]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets this filter enabled.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Enabled")]
        [YAXSerializableField]
        public bool IsEnabled { get; set; }

        #region IDataErrorInfo
        public string Error => string.Empty;

        public string this[string columnName] => GetValidationError(columnName);
        #endregion

        #region Object Methods

        protected bool Equals(Filter other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Filter)obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            var id = Id;
            return id?.GetHashCode() ?? 0;
        }

        #endregion

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
                    error = ValidateName();
                    break;

                case "Priority":
                    error = ValidatePriority();
                    break;

                default:
                    System.Diagnostics.Debug.Fail("Unexpected property being validated on Filter: " + propertyName);
                    break;
            }

            return error;
        }

        private string ValidateName()
        {
            if (Validator.IsStringMissing(Name))
            {
                return "Name must not be empty.";
            }
            return null;
        }

        private string ValidatePriority()
        {
            if (Priority < 0)
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
