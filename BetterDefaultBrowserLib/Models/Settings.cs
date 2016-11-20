using System.Collections.Generic;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Settings class which will be serialized into the settings file.
    /// </summary>
    internal class Settings
    {
        [YAXSerializeAs("Default")]
        internal BrowserStorage DefaultBrowser { get; set; }

        [YAXSerializeAs("Filters")]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Filter")]
        internal List<Filter> Filters { get; } = new List<Filter>();
    }
}
