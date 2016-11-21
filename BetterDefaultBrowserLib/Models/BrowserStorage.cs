using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class BrowserStorage
    {
        #region Fields

        /// <summary>
        /// Gets or sets the key of the browser.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Key")]
        [YAXSerializableField]
        public string BrowserKey { get; set; }

        /// <summary>
        /// Gets or sets the name of the browser.
        /// This can be used as a fallback for displaying the name if the browser key can't be found in the registry.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Name")]
        [YAXSerializableField]
        public string BrowserName { get; set; }

        protected bool Equals(BrowserStorage other)
        {
            return string.Equals(BrowserKey, other.BrowserKey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BrowserStorage)obj);
        }

        public override int GetHashCode()
        {
            return BrowserKey?.GetHashCode() ?? 0;
        }

        #endregion
    }
}
