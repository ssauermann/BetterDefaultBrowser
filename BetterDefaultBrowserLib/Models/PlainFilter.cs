﻿using System;
using BetterDefaultBrowser.Lib.Helpers;
using YAXLib;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a plain filter.
    /// </summary>
    public class PlainFilter : Filter
    {
        /// <summary>
        /// Regex string
        /// </summary>
        private string regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainFilter" /> class.
        /// </summary>
        public PlainFilter() : base()
        {
        }

        /// <summary>
        /// Gets or sets the regex string.
        /// </summary>
        [YAXSerializeAs("Regex")]
        public string Regex
        {
            get
            {
                return this.regex;
            }

            set
            {
                if (RegexHelper.IsValid(value))
                {
                    this.regex = value;
                }
                else
                {
                    throw new ArgumentException("Invalid regex");
                }
            }
        }

        /// <summary>
        /// Gets or sets the assigned browser.
        /// </summary>
        [YAXSerializeAs("Browser")]
        public Browser Browser { get; set; }
    }
}