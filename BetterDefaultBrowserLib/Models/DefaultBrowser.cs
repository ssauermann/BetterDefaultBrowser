using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    public class BrowserStorage
    {
        #region Fields

        /// <summary>
        /// Key of the browser.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Key")]
        public string BrowserKey { get; set; }

        /// <summary>
        /// Name of the browser.
        /// This can be used as a fallback for displaying the name if the browser key can't be found in the registry.
        /// </summary>
        [YAXAttributeForClass]
        [YAXSerializeAs("Name")]
        public string BrowserName { get; set; }

        protected bool Equals(BrowserStorage other)
        {
            return string.Equals(BrowserKey, other.BrowserKey) && string.Equals(BrowserName, other.BrowserName);
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
            unchecked
            {
                return ((BrowserKey?.GetHashCode() ?? 0) * 397) ^ (BrowserName?.GetHashCode() ?? 0);
            }
        }

        #endregion
    }
}
