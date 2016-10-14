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
            this.Key = key;
        }

        /// <summary>
        /// Gets the browsers key identifying it.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Key")]
        [YAXSerializableField]
        public string Key { get; private set; }

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

        /// <summary>
        /// Create a string of this object.
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return Browser.StringBuilder.Stringify(this);
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
