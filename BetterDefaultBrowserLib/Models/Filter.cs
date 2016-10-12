using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Abstract model of a filter.
    /// </summary>
    public abstract class Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter" /> class.
        /// <para> Sets the type of the child filter.</para>
        /// </summary>
        /// <param name="type">Type of filter</param>
        public Filter(FilterTypes type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the internal id.
        /// </summary>
        public string ID { get; internal set; }

        /// <summary>
        /// Gets or sets the filter type.
        /// </summary>
        public FilterTypes Type { get; protected set; }

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
    }
}
