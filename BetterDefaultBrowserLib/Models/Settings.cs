﻿using System.Collections.Generic;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Settings class which will be serialized into the settings file.
    /// </summary>
    [YAXComment("Settings file for the Better Default Browser application. Do not edit this file manually.")]
    [YAXSerializableType(FieldsToSerialize = YAXSerializationFields.AttributedFieldsOnly)]
    internal class Settings
    {
        [YAXSerializeAs("Version")]
        [YAXAttributeForClass]
        [YAXErrorIfMissed(YAXExceptionTypes.Warning, DefaultValue = 1)]
        [YAXSerializableField]
        internal int Version { get; set; }

        [YAXSerializeAs("Default")]
        [YAXErrorIfMissed(YAXExceptionTypes.Warning, DefaultValue = null)]
        [YAXSerializableField]
        internal BrowserStorage DefaultBrowser { get; set; }

        [YAXSerializeAs("Filters")]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Filter")]
        [YAXErrorIfMissed(YAXExceptionTypes.Warning, DefaultValue = null)]
        [YAXSerializableField]
        internal List<Filter> Filters { get; set; }
    }
}
