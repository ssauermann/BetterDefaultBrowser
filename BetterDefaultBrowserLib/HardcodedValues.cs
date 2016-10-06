using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowserLib
{
    public static class HardcodedValues
    {
        /// <summary>
        /// App name for usage in paths.
        /// </summary>
        public static String APP_NAME_PATH = "BetterDefaultBrowser";

        /// <summary>
        /// App name for usage in display.
        /// </summary>
        public static String APP_NAME  = "Better Default Browser";

        /// <summary>
        /// Path to the folder in which settings and log files are stored. Ending with \.
        /// </summary>
        public static String DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + APP_NAME_PATH + @"\";

        /// <summary>
        /// Browser name as it will be set in the registry.
        /// </summary>
        public static String BROWSER_NAME = "BDB Proxy";
    }
}
