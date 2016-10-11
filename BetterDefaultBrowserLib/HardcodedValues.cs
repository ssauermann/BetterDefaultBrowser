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
        public const String APP_NAME = "Better Default Browser";

        /// <summary>
        /// Prog id for registry.
        /// </summary>
        public const String PROG_ID = "BetterDefaultBrowserHTML";

        /// <summary>
        /// Path to the folder in which settings and log files are stored. Ending with \.
        /// </summary>
        public static readonly String DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + APP_NAME + @"\";

        public const String APP_DESC =
                "This is the fake browser entry for the 'Better Default Browser' proxy, so links can be opened in different browsers based on filters.";

        /// <summary>
        /// Error codes to communicate between proxy and main client.
        /// </summary>
        public enum PROXY_ERROR_CODE
        {
            UNKOWN,
            DEFAULT_BROWSER,
            LOOP
        }

        public enum HELPER_ERROR_CODE
        {
            UNKOWN,

        }
    }
}
