﻿using System.Collections.Generic;
using System.ComponentModel;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a open filter.
    /// </summary>
    public class OpenFilter : Filter
    {
        /// <summary>
        /// Priority list of assigned browsers.
        /// </summary>
        private LinkedList<Browser> browsers = new LinkedList<Browser>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFilter" /> class.
        /// </summary>
        public OpenFilter() : base()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether only currently running browsers should be used
        /// or a new browser should be opened if no running one matches.
        /// </summary>
        [YAXSerializeAs("OnlyOpen")]
        public bool OnlyOpen { get; set; }

        /// <summary>
        /// Gets the list of the browser priority list.
        /// </summary>
        [YAXSerializeAs("Browsers")]
        [YAXCollection(YAXCollectionSerializationTypes.Recursive, EachElementName = "Browser")]
        public LinkedList<Browser> Browsers
        {
            get
            {
                return this.browsers;
            }
        }

        /// <summary>
        /// Gets or sets the inner filter.
        /// </summary>
        [YAXSerializeAs("InnerFilter")]
        public Filter InnerFilter { get; set; }
    }
}