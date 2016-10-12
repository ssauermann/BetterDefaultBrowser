using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a browser.
    /// </summary>
    public class Browser
    {
        /// <summary>
        /// Gets the browsers key identifying it.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Gets the program id for the connected program registration and url associations.
        /// </summary>
        public string ProgId { get; internal set; }

        /// <summary>
        /// Gets the browsers name for display.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the path to this browsers icon.
        /// </summary>
        public string IconPath { get; internal set; }

        /// <summary>
        /// Gets the path to this browsers executable.
        /// </summary>
        public string ApplicationPath { get; internal set; }

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

            return this.Key == other.Key;
        }

        /// <summary>
        /// Generate a hash code for this object.
        /// </summary>
        /// <returns>Calculated hash code</returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(this.Key) ? 0 : this.Key.GetHashCode();
        }
    }
}
