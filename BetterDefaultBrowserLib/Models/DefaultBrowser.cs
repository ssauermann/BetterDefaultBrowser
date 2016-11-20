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

        #endregion
    }
}
