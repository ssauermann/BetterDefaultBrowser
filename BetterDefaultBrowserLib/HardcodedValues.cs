using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Centralized collection of all hardcoded values.
    /// </summary>
    public static class HardcodedValues
    {
        /// <summary>
        /// App name for usage in display, paths or registry.
        /// </summary>
        public const string APP_NAME = "Better Default Browser";

        /// <summary>
        /// Program id for registry.
        /// </summary>
        public const string PROG_ID = "BetterDefaultBrowserHTML";

        /// <summary>
        /// Application description
        /// </summary>
        public const string APP_DESC =
                "This is the fake browser entry for the 'Better Default Browser' proxy, so links can be opened in different browsers based on filters.";

        /// <summary>
        /// Path to the folder in which settings and log files are stored. Ending with \.
        /// </summary>
        public static readonly string DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + APP_NAME + @"\";

        /// <summary>
        /// Error codes to communicate between proxy and main client.
        /// </summary>
        public enum PROXY_ERROR_CODE
        {
            /// <summary>
            /// Unknown error
            /// </summary>
            UNKOWN,

            /// <summary>
            /// Default browser is not set error
            /// </summary>
            DEFAULT_BROWSER,

            /// <summary>
            /// Default browser is BDB
            /// </summary>
            LOOP
        }

        /// <summary>
        /// Error codes to communicate between helper and main client.
        /// </summary>
        public enum HELPER_ERROR_CODE
        {
            /// <summary>
            /// Unknown error
            /// </summary>
            UNKOWN
        }
    }
}
